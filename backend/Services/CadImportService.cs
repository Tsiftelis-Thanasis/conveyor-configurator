using System.Text.RegularExpressions;
using ACadSharp;
using ACadSharp.Entities;
using ACadSharp.IO;

namespace ConveyorApi.Services;

public class CadImportService
{
    // ──────────────────────────────────────────────────────────────────
    // Public entry point
    // ──────────────────────────────────────────────────────────────────

    public CadImportResult ImportFile(Stream fileStream, string fileName)
    {
        var extension = Path.GetExtension(fileName).ToLowerInvariant();
        try
        {
            var tempPath = Path.Combine(Path.GetTempPath(), $"cad_import_{Guid.NewGuid()}{extension}");
            using (var fs = File.Create(tempPath))
                fileStream.CopyTo(fs);

            try
            {
                CadDocument doc;
                if (extension == ".dwg")
                {
                    using var reader = new DwgReader(tempPath);
                    doc = reader.Read();
                }
                else if (extension == ".dxf")
                {
                    using var reader = new DxfReader(tempPath);
                    doc = reader.Read();
                }
                else
                {
                    throw new NotSupportedException($"Unsupported format: {extension}");
                }
                return AnalyzeDocument(doc, fileName);
            }
            finally { if (File.Exists(tempPath)) File.Delete(tempPath); }
        }
        catch (Exception ex)
        {
            return new CadImportResult { Success = false, Error = $"Failed to parse CAD file: {ex.Message}" };
        }
    }

    // ──────────────────────────────────────────────────────────────────
    // Document analysis
    // ──────────────────────────────────────────────────────────────────

    private CadImportResult AnalyzeDocument(CadDocument doc, string fileName)
    {
        var ctx = new ParseContext();

        ProcessEntities(doc.ModelSpace.Entities, ctx,
            offsetX: 0, offsetY: 0, scaleX: 1, scaleY: 1, rotation: 0, depth: 0);

        var result = new CadImportResult
        {
            Success = true,
            FileName = fileName,
            Entities = ctx.Entities,
            Texts = ctx.Texts,
            MeshData = new CadMeshData
            {
                Vertices = ctx.Vertices.ToArray(),
                Indices  = ctx.Indices.ToArray()
            }
        };

        // Entity type counts (diagnostics)
        foreach (var e in ctx.Entities)
        {
            result.EntityTypeCounts.TryGetValue(e.Type, out var c);
            result.EntityTypeCounts[e.Type] = c + 1;
        }
        result.EntityTypeCounts["Text"] = ctx.Texts.Count;

        // Track sections
        foreach (var e in ctx.Entities)
        {
            var ts = IdentifyTrackSection(e);
            if (ts != null) result.TrackSections.Add(ts);
        }

        result.TotalTrackLength = result.TrackSections.Where(t => t.Type == "Straight").Sum(t => t.Length);
        result.TotalCurveLength = result.TrackSections.Where(t => t.Type == "Curve").Sum(t => t.Length);
        result.CurveCount       = result.TrackSections.Count(t => t.Type == "Curve");

        if (ctx.Entities.Any())
            result.BoundingBox = CalculateBoundingBox(ctx.Entities);

        result.SuggestedConfig = GenerateSuggestedConfig(result);
        return result;
    }

    // ──────────────────────────────────────────────────────────────────
    // ParseContext — collected during the recursive walk
    // ──────────────────────────────────────────────────────────────────

    private sealed class ParseContext
    {
        public List<CadEntityInfo> Entities { get; } = [];
        public List<TextItem>      Texts    { get; } = [];
        public List<float>         Vertices { get; } = [];
        public List<int>           Indices  { get; } = [];
        public int                 VertexOffset;
    }

    // ──────────────────────────────────────────────────────────────────
    // Recursive entity walker — expands INSERT blocks
    // ──────────────────────────────────────────────────────────────────

    private void ProcessEntities(
        IEnumerable<Entity> entities, ParseContext ctx,
        double offsetX, double offsetY, double scaleX, double scaleY, double rotation,
        int depth)
    {
        if (depth > 8) return;

        foreach (var entity in entities)
        {
            switch (entity)
            {
                // ── Block reference: recurse with composed transform ──
                case Insert insert when insert.Block != null:
                {
                    var (nx, ny, nsx, nsy, nr) = ComposeTransform(
                        offsetX, offsetY, scaleX, scaleY, rotation,
                        insert.InsertPoint.X, insert.InsertPoint.Y,
                        insert.XScale, insert.YScale, insert.Rotation);
                    ProcessEntities(insert.Block.Entities, ctx, nx, ny, nsx, nsy, nr, depth + 1);
                    break;
                }

                // ── LwPolyline with optional bulge segments ──
                case LwPolyline poly:
                    ProcessLwPolyline(poly, ctx, offsetX, offsetY, scaleX, scaleY, rotation);
                    break;

                // ── Polyline2D with optional bulge segments ──
                case Polyline2D poly2d:
                    ProcessPolyline2D(poly2d, ctx, offsetX, offsetY, scaleX, scaleY, rotation);
                    break;

                // ── Single-line text (DBText in DXF; TextEntity in ACadSharp 3.x) ──
                case TextEntity textEnt when !string.IsNullOrWhiteSpace(textEnt.Value):
                {
                    var (tx, ty) = T(textEnt.InsertPoint.X, textEnt.InsertPoint.Y,
                                     offsetX, offsetY, scaleX, scaleY, rotation);
                    ctx.Texts.Add(new TextItem
                    {
                        Content  = textEnt.Value.Trim(),
                        Position = new Point3D(tx, ty, textEnt.InsertPoint.Z),
                        Height   = textEnt.Height * Math.Max(Math.Abs(scaleX), Math.Abs(scaleY)),
                        Rotation = NormDeg(textEnt.Rotation * Rad2Deg + rotation * Rad2Deg),
                        Layer    = entity.Layer?.Name ?? "0",
                        ItemType = "Text"
                    });
                    break;
                }

                // ── Multi-line text ──
                case MText mtext when !string.IsNullOrWhiteSpace(mtext.Value):
                {
                    var (tx, ty) = T(mtext.InsertPoint.X, mtext.InsertPoint.Y,
                                     offsetX, offsetY, scaleX, scaleY, rotation);
                    ctx.Texts.Add(new TextItem
                    {
                        Content  = CleanMText(mtext.Value),
                        Position = new Point3D(tx, ty, mtext.InsertPoint.Z),
                        Height   = mtext.Height * Math.Max(Math.Abs(scaleX), Math.Abs(scaleY)),
                        Rotation = NormDeg(mtext.Rotation * Rad2Deg + rotation * Rad2Deg),
                        Layer    = entity.Layer?.Name ?? "0",
                        ItemType = "MText"
                    });
                    break;
                }

                // ── Dimension annotation ──
                case Dimension dim:
                {
                    ExtractDimensionText(dim, ctx, offsetX, offsetY, scaleX, scaleY, rotation);
                    break;
                }

                // ── Everything else (Line, Arc, Circle, Spline, …) ──
                default:
                {
                    var info = ProcessSingleEntity(entity, ctx,
                        offsetX, offsetY, scaleX, scaleY, rotation);
                    if (info != null)
                        ctx.Entities.Add(info);
                    break;
                }
            }
        }
    }

    // ──────────────────────────────────────────────────────────────────
    // Dimension text extraction
    // ──────────────────────────────────────────────────────────────────

    private void ExtractDimensionText(
        Dimension dim, ParseContext ctx,
        double offsetX, double offsetY, double scaleX, double scaleY, double rotation)
    {
        // Use override text if set; otherwise try to measure from geometry
        string text = dim.Text?.Trim() ?? "";
        if (text == "<>" || text == "") text = "";

        // Compute geometric measurement for common dimension types
        if (string.IsNullOrEmpty(text))
        {
            text = dim switch
            {
                DimensionLinear dl  => $"{MeasureLinear(dl):F0}",
                DimensionAligned da => $"{MeasureAligned(da):F0}",
                _                   => ""
            };
        }

        if (string.IsNullOrEmpty(text)) return;

        // Text sits at TextMiddlePoint
        var tp = dim.TextMiddlePoint;
        var (tx, ty) = T(tp.X, tp.Y, offsetX, offsetY, scaleX, scaleY, rotation);

        ctx.Texts.Add(new TextItem
        {
            Content  = text,
            Position = new Point3D(tx, ty, 0),
            Height   = 150 * Math.Max(Math.Abs(scaleX), Math.Abs(scaleY)), // typical dim text height
            Rotation = 0,
            Layer    = dim.Layer?.Name ?? "0",
            ItemType = "Dimension"
        });

        // Dimension witness/extension lines are intentionally NOT added to ctx.Entities
        // so they don't clutter the 2D preview with confusing extra lines.
    }

    private static double MeasureLinear(DimensionLinear dl)  => dl.Measurement;
    private static double MeasureAligned(DimensionAligned da) => da.Measurement;

    private void AddDimensionGeometry(
        Dimension dim, ParseContext ctx,
        double offsetX, double offsetY, double scaleX, double scaleY, double rotation)
    {
        // Draw definition points connected by lines so dimensions are visible in 2D preview
        List<(double x, double y)> pts = dim switch
        {
            DimensionLinear dl  => [(dl.FirstPoint.X,    dl.FirstPoint.Y),
                                    (dl.SecondPoint.X,   dl.SecondPoint.Y),
                                    (dl.DefinitionPoint.X, dl.DefinitionPoint.Y)],
            DimensionAligned da => [(da.FirstPoint.X,    da.FirstPoint.Y),
                                    (da.SecondPoint.X,   da.SecondPoint.Y),
                                    (da.DefinitionPoint.X, da.DefinitionPoint.Y)],
            _                   => []
        };

        for (int i = 0; i < pts.Count - 1; i++)
        {
            var (ax, ay) = T(pts[i].x,     pts[i].y,     offsetX, offsetY, scaleX, scaleY, rotation);
            var (bx, by) = T(pts[i+1].x,   pts[i+1].y,   offsetX, offsetY, scaleX, scaleY, rotation);
            var p1 = new Point3D(ax, ay, 0);
            var p2 = new Point3D(bx, by, 0);
            var info = new CadEntityInfo
            {
                Type  = "Line",
                Layer = dim.Layer?.Name ?? "0",
                StartPoint = p1, EndPoint = p2,
                Length = Dist(p1, p2)
            };
            AddLineInfoToMesh(info, ctx);
            ctx.Entities.Add(info);
        }
    }

    // ──────────────────────────────────────────────────────────────────
    // LwPolyline with Bulge → Line / Arc segments
    // ──────────────────────────────────────────────────────────────────

    private void ProcessLwPolyline(LwPolyline poly, ParseContext ctx,
        double offsetX, double offsetY, double scaleX, double scaleY, double rotation)
    {
        var verts    = poly.Vertices.ToList();
        int count    = verts.Count;
        int segCount = poly.IsClosed ? count : count - 1;
        string layer = poly.Layer?.Name ?? "0";

        for (int i = 0; i < segCount; i++)
        {
            var v1 = verts[i];
            var v2 = verts[(i + 1) % count];

            var (ax, ay) = T(v1.Location.X, v1.Location.Y, offsetX, offsetY, scaleX, scaleY, rotation);
            var (bx, by) = T(v2.Location.X, v2.Location.Y, offsetX, offsetY, scaleX, scaleY, rotation);

            if (Math.Abs(v1.Bulge) > 1e-10)
            {
                var arc = BulgeToArc(ax, ay, bx, by, v1.Bulge, layer);
                if (arc != null) { AddArcInfoToMesh(arc, ctx); ctx.Entities.Add(arc); }
            }
            else
            {
                var p1   = new Point3D(ax, ay, 0);
                var p2   = new Point3D(bx, by, 0);
                var line = new CadEntityInfo { Type = "Line", Layer = layer, StartPoint = p1, EndPoint = p2, Length = Dist(p1, p2) };
                AddLineInfoToMesh(line, ctx);
                ctx.Entities.Add(line);
            }
        }
    }

    private void ProcessPolyline2D(Polyline2D poly, ParseContext ctx,
        double offsetX, double offsetY, double scaleX, double scaleY, double rotation)
    {
        var verts    = poly.Vertices.ToList();
        int count    = verts.Count;
        int segCount = poly.IsClosed ? count : count - 1;
        string layer = poly.Layer?.Name ?? "0";

        for (int i = 0; i < segCount; i++)
        {
            var v1 = verts[i];
            var v2 = verts[(i + 1) % count];

            var (ax, ay) = T(v1.Location.X, v1.Location.Y, offsetX, offsetY, scaleX, scaleY, rotation);
            var (bx, by) = T(v2.Location.X, v2.Location.Y, offsetX, offsetY, scaleX, scaleY, rotation);

            if (Math.Abs(v1.Bulge) > 1e-10)
            {
                var arc = BulgeToArc(ax, ay, bx, by, v1.Bulge, layer);
                if (arc != null) { AddArcInfoToMesh(arc, ctx); ctx.Entities.Add(arc); }
            }
            else
            {
                var p1   = new Point3D(ax, ay, 0);
                var p2   = new Point3D(bx, by, 0);
                var line = new CadEntityInfo { Type = "Line", Layer = layer, StartPoint = p1, EndPoint = p2, Length = Dist(p1, p2) };
                AddLineInfoToMesh(line, ctx);
                ctx.Entities.Add(line);
            }
        }
    }

    // ──────────────────────────────────────────────────────────────────
    // Single geometric entity
    // ──────────────────────────────────────────────────────────────────

    private CadEntityInfo? ProcessSingleEntity(
        Entity entity, ParseContext ctx,
        double offsetX, double offsetY, double scaleX, double scaleY, double rotation)
    {
        string layer = entity.Layer?.Name ?? "0";

        switch (entity)
        {
            case Line line:
            {
                var (ax, ay) = T(line.StartPoint.X, line.StartPoint.Y, offsetX, offsetY, scaleX, scaleY, rotation);
                var (bx, by) = T(line.EndPoint.X,   line.EndPoint.Y,   offsetX, offsetY, scaleX, scaleY, rotation);
                var p1   = new Point3D(ax, ay, line.StartPoint.Z);
                var p2   = new Point3D(bx, by, line.EndPoint.Z);
                var info = new CadEntityInfo { Type = "Line", Layer = layer, StartPoint = p1, EndPoint = p2, Length = Dist(p1, p2) };
                AddLineInfoToMesh(info, ctx);
                return info;
            }

            case Arc arc:
            {
                var (cx, cy) = T(arc.Center.X, arc.Center.Y, offsetX, offsetY, scaleX, scaleY, rotation);
                double r        = arc.Radius * Math.Max(Math.Abs(scaleX), Math.Abs(scaleY));
                double rotDeg   = rotation * Rad2Deg;
                double startDeg = arc.StartAngle * Rad2Deg + rotDeg;
                double endDeg   = arc.EndAngle   * Rad2Deg + rotDeg;
                var info = new CadEntityInfo
                {
                    Type = "Arc", Layer = layer,
                    Center = new Point3D(cx, cy, arc.Center.Z), Radius = r,
                    StartAngle = startDeg, EndAngle = endDeg,
                    Length = r * Math.Abs(arc.EndAngle - arc.StartAngle)
                };
                AddArcInfoToMesh(info, ctx);
                return info;
            }

            case Circle circle:
            {
                var (cx, cy) = T(circle.Center.X, circle.Center.Y, offsetX, offsetY, scaleX, scaleY, rotation);
                double r     = circle.Radius * Math.Max(Math.Abs(scaleX), Math.Abs(scaleY));
                var info = new CadEntityInfo
                {
                    Type = "Circle", Layer = layer,
                    Center = new Point3D(cx, cy, circle.Center.Z), Radius = r,
                    StartAngle = 0, EndAngle = 360, Length = 2 * Math.PI * r
                };
                AddArcInfoToMesh(info, ctx);
                return info;
            }

            case Spline spline:
            {
                var pts = spline.ControlPoints
                    .Select(p => { var (tx, ty) = T(p.X, p.Y, offsetX, offsetY, scaleX, scaleY, rotation); return new Point3D(tx, ty, p.Z); })
                    .ToList();
                return new CadEntityInfo { Type = "Spline", Layer = layer, Points = pts, Length = EstimateSplineLength(spline) };
            }

            default:
                return new CadEntityInfo { Type = entity.GetType().Name, Layer = layer };
        }
    }

    // ──────────────────────────────────────────────────────────────────
    // Bulge → Arc conversion
    // ──────────────────────────────────────────────────────────────────

    private static CadEntityInfo? BulgeToArc(double x1, double y1, double x2, double y2, double bulge, string layer)
    {
        double dx = x2 - x1, dy = y2 - y1;
        double d  = Math.Sqrt(dx * dx + dy * dy);
        if (d < 1e-10) return null;

        double theta   = 4.0 * Math.Atan(Math.Abs(bulge));
        double sinHalf = Math.Sin(theta / 2.0);
        if (Math.Abs(sinHalf) < 1e-10) return null;

        double r     = d / (2.0 * sinHalf);
        double sign  = Math.Sign(bulge);
        double perpX = -dy / d * sign;
        double perpY =  dx / d * sign;
        double dist  = Math.Sqrt(Math.Max(0, r * r - (d / 2.0) * (d / 2.0)));
        double cx    = (x1 + x2) / 2.0 + perpX * dist;
        double cy    = (y1 + y2) / 2.0 + perpY * dist;

        double startDeg = Math.Atan2(y1 - cy, x1 - cx) * Rad2Deg;
        double endDeg   = Math.Atan2(y2 - cy, x2 - cx) * Rad2Deg;

        if (bulge < 0) (startDeg, endDeg) = (endDeg, startDeg);

        return new CadEntityInfo
        {
            Type = "Arc", Layer = layer,
            Center = new Point3D(cx, cy, 0), Radius = r,
            StartAngle = startDeg, EndAngle = endDeg,
            Length = r * theta
        };
    }

    // ──────────────────────────────────────────────────────────────────
    // Mesh builders
    // ──────────────────────────────────────────────────────────────────

    private static void AddLineInfoToMesh(CadEntityInfo info, ParseContext ctx)
    {
        if (info.StartPoint == null || info.EndPoint == null) return;
        const double th = 50.0, hh = 15.0;

        double dx  = info.EndPoint.X - info.StartPoint.X;
        double dy  = info.EndPoint.Y - info.StartPoint.Y;
        double len = Math.Sqrt(dx * dx + dy * dy);
        if (len < 0.001) return;

        double px = -dy / len * th / 2, py = dx / len * th / 2;
        var s = info.StartPoint;
        var e = info.EndPoint;

        AddVtx(ctx, s.X - px, s.Y - py, s.Z - hh); AddVtx(ctx, s.X + px, s.Y + py, s.Z - hh);
        AddVtx(ctx, s.X - px, s.Y - py, s.Z + hh); AddVtx(ctx, s.X + px, s.Y + py, s.Z + hh);
        AddVtx(ctx, e.X - px, e.Y - py, e.Z - hh); AddVtx(ctx, e.X + px, e.Y + py, e.Z - hh);
        AddVtx(ctx, e.X - px, e.Y - py, e.Z + hh); AddVtx(ctx, e.X + px, e.Y + py, e.Z + hh);

        int v = ctx.VertexOffset;
        ctx.Indices.AddRange([v,v+1,v+3, v,v+3,v+2, v+4,v+6,v+7, v+4,v+7,v+5,
                               v+2,v+3,v+7, v+2,v+7,v+6, v,v+4,v+5, v,v+5,v+1,
                               v,v+2,v+6,   v,v+6,v+4,   v+1,v+5,v+7, v+1,v+7,v+3]);
        ctx.VertexOffset += 8;
    }

    private static void AddArcInfoToMesh(CadEntityInfo info, ParseContext ctx)
    {
        if (info.Center == null || info.Radius == null ||
            info.StartAngle == null || info.EndAngle == null) return;

        const int SEG = 16;
        const double th = 50.0, hh = 15.0;
        double halfTh = th / 2;

        double startRad = info.StartAngle.Value * Math.PI / 180.0;
        double endRad   = info.EndAngle.Value   * Math.PI / 180.0;
        if (endRad < startRad) endRad += 2 * Math.PI;
        double step = (endRad - startRad) / SEG;

        double px = 0, py = 0, qx = 0, qy = 0;
        bool first = true;
        double cz = info.Center.Z;

        for (int i = 0; i <= SEG; i++)
        {
            double a   = startRad + i * step;
            double cos = Math.Cos(a), sin = Math.Sin(a);
            double ox  = info.Center.X + (info.Radius.Value + halfTh) * cos;
            double oy  = info.Center.Y + (info.Radius.Value + halfTh) * sin;
            double ix  = info.Center.X + (info.Radius.Value - halfTh) * cos;
            double iy  = info.Center.Y + (info.Radius.Value - halfTh) * sin;

            if (!first)
            {
                int v = ctx.VertexOffset;
                AddVtx(ctx, px, py, cz-hh); AddVtx(ctx, px, py, cz+hh);
                AddVtx(ctx, ox, oy, cz-hh); AddVtx(ctx, ox, oy, cz+hh);
                AddVtx(ctx, qx, qy, cz-hh); AddVtx(ctx, qx, qy, cz+hh);
                AddVtx(ctx, ix, iy, cz-hh); AddVtx(ctx, ix, iy, cz+hh);
                ctx.Indices.AddRange([v,v+2,v+3, v,v+3,v+1, v+4,v+5,v+7, v+4,v+7,v+6,
                                       v+1,v+3,v+7, v+1,v+7,v+5, v,v+4,v+6, v,v+6,v+2]);
                ctx.VertexOffset += 8;
            }

            px = ox; py = oy; qx = ix; qy = iy; first = false;
        }
    }

    private static void AddVtx(ParseContext ctx, double x, double y, double z)
    {
        ctx.Vertices.Add((float)x);
        ctx.Vertices.Add((float)y);
        ctx.Vertices.Add((float)z);
    }

    // ──────────────────────────────────────────────────────────────────
    // Track section identification
    // ──────────────────────────────────────────────────────────────────

    private static TrackSection? IdentifyTrackSection(CadEntityInfo e)
    {
        if (e.Type == "Line" && e.Length > 100)
            return new TrackSection { Type = "Straight", Length = e.Length, StartPoint = e.StartPoint, EndPoint = e.EndPoint };

        if (e.Type == "Arc" && e.Radius > 200)
            return new TrackSection
            {
                Type   = "Curve",
                Length = e.Length,
                Radius = e.Radius,
                Angle  = Math.Abs((e.EndAngle ?? 0) - (e.StartAngle ?? 0)),
                Center = e.Center
            };

        if (e.Type == "Polyline" && e.Length > 100)
            return new TrackSection { Type = "Straight", Length = e.Length, Points = e.Points };

        return null;
    }

    // ──────────────────────────────────────────────────────────────────
    // Bounding box
    // ──────────────────────────────────────────────────────────────────

    private static BoundingBox3D CalculateBoundingBox(List<CadEntityInfo> entities)
    {
        double minX = double.MaxValue, minY = double.MaxValue, minZ = double.MaxValue;
        double maxX = double.MinValue, maxY = double.MinValue, maxZ = double.MinValue;

        void Expand(double x, double y, double z)
        {
            minX = Math.Min(minX, x); minY = Math.Min(minY, y); minZ = Math.Min(minZ, z);
            maxX = Math.Max(maxX, x); maxY = Math.Max(maxY, y); maxZ = Math.Max(maxZ, z);
        }

        foreach (var e in entities)
        {
            if (e.StartPoint != null) Expand(e.StartPoint.X, e.StartPoint.Y, e.StartPoint.Z);
            if (e.EndPoint   != null) Expand(e.EndPoint.X,   e.EndPoint.Y,   e.EndPoint.Z);
            if (e.Points     != null) foreach (var p in e.Points) Expand(p.X, p.Y, p.Z);
            if (e.Center != null && e.Radius.HasValue)
            {
                Expand(e.Center.X - e.Radius.Value, e.Center.Y - e.Radius.Value, e.Center.Z);
                Expand(e.Center.X + e.Radius.Value, e.Center.Y + e.Radius.Value, e.Center.Z);
            }
        }

        if (minX == double.MaxValue)
            return new BoundingBox3D();

        return new BoundingBox3D
        {
            Min    = new Point3D(minX, minY, minZ),
            Max    = new Point3D(maxX, maxY, maxZ),
            Width  = maxX - minX,
            Height = maxY - minY,
            Depth  = maxZ - minZ
        };
    }

    // ──────────────────────────────────────────────────────────────────
    // Suggested config
    // ──────────────────────────────────────────────────────────────────

    private static SuggestedConveyorConfig GenerateSuggestedConfig(CadImportResult r)
    {
        var cfg = new SuggestedConveyorConfig
        {
            TrackLength     = r.TotalTrackLength + r.TotalCurveLength,
            HeightFromFloor = r.BoundingBox?.Max.Z > 0 ? r.BoundingBox.Max.Z : 3000,
            IncludeCurves   = r.CurveCount > 0,
            SuggestedProfile = "24.000"
        };

        if (cfg.IncludeCurves)
        {
            var radii = r.TrackSections.Where(t => t.Type == "Curve" && t.Radius.HasValue).Select(t => t.Radius!.Value).ToList();
            cfg.CurveRadius = radii.Any() ? radii.Average() : 500;
            cfg.CurveCount  = r.CurveCount;
        }

        cfg.NumCarriers  = Math.Max(1, (int)(cfg.TrackLength / 1000.0));
        cfg.CarrierSpacing = 1000;
        return cfg;
    }

    // ──────────────────────────────────────────────────────────────────
    // Transform helpers
    // ──────────────────────────────────────────────────────────────────

    private const double Rad2Deg = 180.0 / Math.PI;

    private static (double x, double y) T(double x, double y,
        double offX, double offY, double sx, double sy, double rot)
    {
        double cos = Math.Cos(rot), sin = Math.Sin(rot);
        return (offX + (x * sx * cos - y * sy * sin),
                offY + (x * sx * sin + y * sy * cos));
    }

    private static (double offX, double offY, double sx, double sy, double rot)
        ComposeTransform(double pox, double poy, double psx, double psy, double pr,
                         double cx,  double cy,  double csx, double csy, double cr)
    {
        double cos = Math.Cos(pr), sin = Math.Sin(pr);
        double wx  = pox + (cx * psx * cos - cy * psy * sin);
        double wy  = poy + (cx * psx * sin + cy * psy * cos);
        return (wx, wy, psx * csx, psy * csy, pr + cr);
    }

    private static double NormDeg(double d)
    {
        d %= 360;
        return d < 0 ? d + 360 : d;
    }

    // ──────────────────────────────────────────────────────────────────
    // Utility
    // ──────────────────────────────────────────────────────────────────

    private static double Dist(Point3D a, Point3D b)
    {
        double dx = b.X - a.X, dy = b.Y - a.Y, dz = b.Z - a.Z;
        return Math.Sqrt(dx * dx + dy * dy + dz * dz);
    }

    private static double EstimateSplineLength(Spline spline)
    {
        double len = 0;
        var pts = spline.ControlPoints.ToList();
        for (int i = 0; i < pts.Count - 1; i++)
        {
            double dx = pts[i+1].X - pts[i].X, dy = pts[i+1].Y - pts[i].Y, dz = pts[i+1].Z - pts[i].Z;
            len += Math.Sqrt(dx * dx + dy * dy + dz * dz);
        }
        return len;
    }

    /// <summary>Strip MText formatting codes, leaving clean readable text.</summary>
    private static string CleanMText(string raw)
    {
        if (string.IsNullOrWhiteSpace(raw)) return "";
        // Paragraph break → newline
        var s = raw.Replace("\\P", "\n").Replace("\\p", "\n");
        // Escape sequences like \fFontName|b0|i0; or \C1; or \H2.5x; etc.
        s = Regex.Replace(s, @"\\[A-Za-z][^;\\]*;", "");
        // Remaining single-char escapes e.g. \~ \U+xxxx
        s = Regex.Replace(s, @"\\.", " ");
        // Strip grouping braces (keep content)
        s = s.Replace("{", "").Replace("}", "");
        return s.Trim();
    }
}

// ──────────────────────────────────────────────────────────────────────────
// Result models
// ──────────────────────────────────────────────────────────────────────────

public class CadImportResult
{
    public bool    Success  { get; set; }
    public string? Error    { get; set; }
    public string? FileName { get; set; }

    public List<CadEntityInfo>   Entities     { get; set; } = [];
    public List<TextItem>        Texts        { get; set; } = [];
    public List<TrackSection>    TrackSections{ get; set; } = [];

    public double TotalTrackLength { get; set; }
    public double TotalCurveLength { get; set; }
    public int    CurveCount       { get; set; }

    public BoundingBox3D?        BoundingBox   { get; set; }
    public CadMeshData?          MeshData      { get; set; }
    public SuggestedConveyorConfig? SuggestedConfig { get; set; }

    public Dictionary<string, int> EntityTypeCounts { get; set; } = [];
}

public class CadEntityInfo
{
    public string    Type   { get; set; } = "";
    public string    Layer  { get; set; } = "";
    public Point3D?  StartPoint { get; set; }
    public Point3D?  EndPoint   { get; set; }
    public Point3D?  Center     { get; set; }
    public double?   Radius     { get; set; }
    public double?   StartAngle { get; set; }
    public double?   EndAngle   { get; set; }
    public double    Length     { get; set; }
    public List<Point3D>? Points { get; set; }
    public bool      IsClosed   { get; set; }
}

public class TextItem
{
    public string   Content  { get; set; } = "";
    public Point3D  Position { get; set; } = new();
    public double   Height   { get; set; }   // DXF units (mm)
    public double   Rotation { get; set; }   // degrees
    public string   Layer    { get; set; } = "";
    public string   ItemType { get; set; } = "Text"; // "Text" | "MText" | "Dimension"
}

public class TrackSection
{
    public string   Type    { get; set; } = "";
    public double   Length  { get; set; }
    public double?  Radius  { get; set; }
    public double?  Angle   { get; set; }
    public Point3D? StartPoint { get; set; }
    public Point3D? EndPoint   { get; set; }
    public Point3D? Center     { get; set; }
    public List<Point3D>? Points { get; set; }
}

public class Point3D
{
    public double X { get; set; }
    public double Y { get; set; }
    public double Z { get; set; }
    public Point3D() { }
    public Point3D(double x, double y, double z) { X = x; Y = y; Z = z; }
}

public class BoundingBox3D
{
    public Point3D Min    { get; set; } = new();
    public Point3D Max    { get; set; } = new();
    public double  Width  { get; set; }
    public double  Height { get; set; }
    public double  Depth  { get; set; }
}

public class CadMeshData
{
    public float[] Vertices { get; set; } = [];
    public float[] Normals  { get; set; } = [];
    public int[]   Indices  { get; set; } = [];
}

public class SuggestedConveyorConfig
{
    public double TrackLength      { get; set; }
    public double HeightFromFloor  { get; set; } = 3000;
    public string SuggestedProfile { get; set; } = "24.000";
    public bool   IncludeCurves    { get; set; }
    public double CurveRadius      { get; set; }
    public int    CurveCount       { get; set; }
    public int    NumCarriers      { get; set; }
    public double CarrierSpacing   { get; set; } = 1000;
}
