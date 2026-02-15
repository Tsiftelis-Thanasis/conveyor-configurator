using ACadSharp;
using ACadSharp.Entities;
using ACadSharp.IO;

namespace ConveyorApi.Services;

/// <summary>
/// Service for importing and parsing CAD files (DWG/DXF)
/// </summary>
public class CadImportService
{
    /// <summary>
    /// Import a CAD file and extract conveyor layout information
    /// </summary>
    public CadImportResult ImportFile(Stream fileStream, string fileName)
    {
        var extension = Path.GetExtension(fileName).ToLowerInvariant();

        CadDocument? document = null;

        try
        {
            // Save stream to temp file (ACadSharp needs file path for some operations)
            var tempPath = Path.Combine(Path.GetTempPath(), $"cad_import_{Guid.NewGuid()}{extension}");
            using (var fileStreamOut = File.Create(tempPath))
            {
                fileStream.CopyTo(fileStreamOut);
            }

            try
            {
                if (extension == ".dwg")
                {
                    using var reader = new DwgReader(tempPath);
                    document = reader.Read();
                }
                else if (extension == ".dxf")
                {
                    using var reader = new DxfReader(tempPath);
                    document = reader.Read();
                }
                else
                {
                    return new CadImportResult
                    {
                        Success = false,
                        Error = $"Unsupported file format: {extension}. Supported formats: .dwg, .dxf"
                    };
                }

                // Extract geometry and analyze
                var result = AnalyzeDocument(document, fileName);
                return result;
            }
            finally
            {
                // Clean up temp file
                if (File.Exists(tempPath))
                    File.Delete(tempPath);
            }
        }
        catch (Exception ex)
        {
            return new CadImportResult
            {
                Success = false,
                Error = $"Failed to parse CAD file: {ex.Message}"
            };
        }
    }

    private CadImportResult AnalyzeDocument(CadDocument document, string fileName)
    {
        var result = new CadImportResult
        {
            Success = true,
            FileName = fileName,
            Entities = new List<CadEntityInfo>(),
            TrackSections = new List<TrackSection>(),
            MeshData = new CadMeshData()
        };

        var allVertices = new List<float>();
        var allIndices = new List<int>();
        var vertexOffset = 0;

        // Process all entities in model space
        var modelSpace = document.ModelSpace;

        foreach (var entity in modelSpace.Entities)
        {
            var entityInfo = ProcessEntity(entity, allVertices, allIndices, ref vertexOffset);
            if (entityInfo != null)
            {
                result.Entities.Add(entityInfo);

                // Identify track sections
                var trackSection = IdentifyTrackSection(entityInfo);
                if (trackSection != null)
                {
                    result.TrackSections.Add(trackSection);
                }
            }
        }

        // Calculate totals
        result.TotalTrackLength = result.TrackSections
            .Where(t => t.Type == "Straight")
            .Sum(t => t.Length);

        result.TotalCurveLength = result.TrackSections
            .Where(t => t.Type == "Curve")
            .Sum(t => t.Length);

        result.CurveCount = result.TrackSections.Count(t => t.Type == "Curve");

        // Get bounding box
        if (result.Entities.Any())
        {
            result.BoundingBox = CalculateBoundingBox(result.Entities);
        }

        // Set mesh data
        result.MeshData.Vertices = allVertices.ToArray();
        result.MeshData.Indices = allIndices.ToArray();

        // Generate suggested configuration
        result.SuggestedConfig = GenerateSuggestedConfig(result);

        return result;
    }

    private CadEntityInfo? ProcessEntity(Entity entity, List<float> vertices, List<int> indices, ref int vertexOffset)
    {
        var info = new CadEntityInfo
        {
            Type = entity.GetType().Name,
            Layer = entity.Layer?.Name ?? "0"
        };

        switch (entity)
        {
            case Line line:
                info.Type = "Line";
                info.StartPoint = new Point3D(line.StartPoint.X, line.StartPoint.Y, line.StartPoint.Z);
                info.EndPoint = new Point3D(line.EndPoint.X, line.EndPoint.Y, line.EndPoint.Z);
                info.Length = CalculateDistance(info.StartPoint, info.EndPoint);

                // Add to mesh (as a thin box for visualization)
                AddLineToMesh(line, vertices, indices, ref vertexOffset);
                break;

            case Arc arc:
                info.Type = "Arc";
                info.Center = new Point3D(arc.Center.X, arc.Center.Y, arc.Center.Z);
                info.Radius = arc.Radius;
                info.StartAngle = arc.StartAngle * (180 / Math.PI);
                info.EndAngle = arc.EndAngle * (180 / Math.PI);
                info.Length = arc.Radius * Math.Abs(arc.EndAngle - arc.StartAngle);

                // Add to mesh
                AddArcToMesh(arc, vertices, indices, ref vertexOffset);
                break;

            case Circle circle:
                info.Type = "Circle";
                info.Center = new Point3D(circle.Center.X, circle.Center.Y, circle.Center.Z);
                info.Radius = circle.Radius;
                info.Length = 2 * Math.PI * circle.Radius;

                AddCircleToMesh(circle, vertices, indices, ref vertexOffset);
                break;

            case LwPolyline polyline:
                info.Type = "Polyline";
                info.Points = polyline.Vertices.Select(v => new Point3D(v.Location.X, v.Location.Y, 0)).ToList();
                info.Length = CalculatePolylineLength(polyline);
                info.IsClosed = polyline.IsClosed;

                AddPolylineToMesh(polyline, vertices, indices, ref vertexOffset);
                break;

            case Polyline2D polyline2d:
                info.Type = "Polyline";
                info.Points = polyline2d.Vertices.Select(v => new Point3D(v.Location.X, v.Location.Y, 0)).ToList();
                info.IsClosed = polyline2d.IsClosed;

                // Calculate length
                double len = 0;
                for (int i = 0; i < info.Points.Count - 1; i++)
                {
                    len += CalculateDistance(info.Points[i], info.Points[i + 1]);
                }
                if (info.IsClosed && info.Points.Count > 1)
                {
                    len += CalculateDistance(info.Points[^1], info.Points[0]);
                }
                info.Length = len;
                break;

            case Spline spline:
                info.Type = "Spline";
                info.Points = spline.ControlPoints.Select(p => new Point3D(p.X, p.Y, p.Z)).ToList();
                // Approximate spline length
                info.Length = EstimateSplineLength(spline);
                break;

            default:
                // Skip unsupported entities but log them
                info.Type = entity.GetType().Name;
                info.Length = 0;
                break;
        }

        return info;
    }

    private void AddLineToMesh(Line line, List<float> vertices, List<int> indices, ref int vertexOffset)
    {
        // Create a thin 3D box along the line for visualization
        var thickness = 50.0; // 50mm track width for visualization
        var height = 30.0;    // 30mm track height

        var start = new CSMath.XYZ(line.StartPoint.X, line.StartPoint.Y, line.StartPoint.Z);
        var end = new CSMath.XYZ(line.EndPoint.X, line.EndPoint.Y, line.EndPoint.Z);

        // Direction vector
        var dir = new CSMath.XYZ(end.X - start.X, end.Y - start.Y, end.Z - start.Z);
        var length = Math.Sqrt(dir.X * dir.X + dir.Y * dir.Y + dir.Z * dir.Z);
        if (length < 0.001) return;

        dir = new CSMath.XYZ(dir.X / length, dir.Y / length, dir.Z / length);

        // Perpendicular vector (for width)
        var perp = new CSMath.XYZ(-dir.Y, dir.X, 0);
        var perpLen = Math.Sqrt(perp.X * perp.X + perp.Y * perp.Y);
        if (perpLen > 0.001)
        {
            perp = new CSMath.XYZ(perp.X / perpLen * thickness / 2, perp.Y / perpLen * thickness / 2, 0);
        }

        // Create 8 vertices for a box
        var halfHeight = height / 2;

        // Bottom vertices (start)
        AddVertex(vertices, start.X - perp.X, start.Y - perp.Y, start.Z - halfHeight);
        AddVertex(vertices, start.X + perp.X, start.Y + perp.Y, start.Z - halfHeight);
        // Top vertices (start)
        AddVertex(vertices, start.X - perp.X, start.Y - perp.Y, start.Z + halfHeight);
        AddVertex(vertices, start.X + perp.X, start.Y + perp.Y, start.Z + halfHeight);
        // Bottom vertices (end)
        AddVertex(vertices, end.X - perp.X, end.Y - perp.Y, end.Z - halfHeight);
        AddVertex(vertices, end.X + perp.X, end.Y + perp.Y, end.Z - halfHeight);
        // Top vertices (end)
        AddVertex(vertices, end.X - perp.X, end.Y - perp.Y, end.Z + halfHeight);
        AddVertex(vertices, end.X + perp.X, end.Y + perp.Y, end.Z + halfHeight);

        // Add indices for box faces
        var v = vertexOffset;
        // Front face
        indices.AddRange(new[] { v, v + 1, v + 3, v, v + 3, v + 2 });
        // Back face
        indices.AddRange(new[] { v + 4, v + 6, v + 7, v + 4, v + 7, v + 5 });
        // Top face
        indices.AddRange(new[] { v + 2, v + 3, v + 7, v + 2, v + 7, v + 6 });
        // Bottom face
        indices.AddRange(new[] { v, v + 4, v + 5, v, v + 5, v + 1 });
        // Left face
        indices.AddRange(new[] { v, v + 2, v + 6, v, v + 6, v + 4 });
        // Right face
        indices.AddRange(new[] { v + 1, v + 5, v + 7, v + 1, v + 7, v + 3 });

        vertexOffset += 8;
    }

    private void AddArcToMesh(Arc arc, List<float> vertices, List<int> indices, ref int vertexOffset)
    {
        var segments = 16;
        var thickness = 50.0;
        var height = 30.0;
        var halfHeight = height / 2;
        var halfThick = thickness / 2;

        var startAngle = arc.StartAngle;
        var endAngle = arc.EndAngle;
        if (endAngle < startAngle) endAngle += 2 * Math.PI;
        var angleStep = (endAngle - startAngle) / segments;

        var prevOuter = new CSMath.XYZ();
        var prevInner = new CSMath.XYZ();
        var first = true;

        for (int i = 0; i <= segments; i++)
        {
            var angle = startAngle + i * angleStep;
            var cos = Math.Cos(angle);
            var sin = Math.Sin(angle);

            var centerX = arc.Center.X;
            var centerY = arc.Center.Y;
            var centerZ = arc.Center.Z;

            var outerX = centerX + (arc.Radius + halfThick) * cos;
            var outerY = centerY + (arc.Radius + halfThick) * sin;
            var innerX = centerX + (arc.Radius - halfThick) * cos;
            var innerY = centerY + (arc.Radius - halfThick) * sin;

            if (!first)
            {
                // Add quad between previous and current positions
                var v = vertexOffset;

                // Previous outer bottom, top
                AddVertex(vertices, prevOuter.X, prevOuter.Y, centerZ - halfHeight);
                AddVertex(vertices, prevOuter.X, prevOuter.Y, centerZ + halfHeight);
                // Current outer bottom, top
                AddVertex(vertices, outerX, outerY, centerZ - halfHeight);
                AddVertex(vertices, outerX, outerY, centerZ + halfHeight);
                // Previous inner bottom, top
                AddVertex(vertices, prevInner.X, prevInner.Y, centerZ - halfHeight);
                AddVertex(vertices, prevInner.X, prevInner.Y, centerZ + halfHeight);
                // Current inner bottom, top
                AddVertex(vertices, innerX, innerY, centerZ - halfHeight);
                AddVertex(vertices, innerX, innerY, centerZ + halfHeight);

                // Outer face
                indices.AddRange(new[] { v, v + 2, v + 3, v, v + 3, v + 1 });
                // Inner face
                indices.AddRange(new[] { v + 4, v + 5, v + 7, v + 4, v + 7, v + 6 });
                // Top face
                indices.AddRange(new[] { v + 1, v + 3, v + 7, v + 1, v + 7, v + 5 });
                // Bottom face
                indices.AddRange(new[] { v, v + 4, v + 6, v, v + 6, v + 2 });

                vertexOffset += 8;
            }

            prevOuter = new CSMath.XYZ(outerX, outerY, 0);
            prevInner = new CSMath.XYZ(innerX, innerY, 0);
            first = false;
        }
    }

    private void AddCircleToMesh(Circle circle, List<float> vertices, List<int> indices, ref int vertexOffset)
    {
        // Treat circle as a closed arc
        var arc = new Arc
        {
            Center = circle.Center,
            Radius = circle.Radius,
            StartAngle = 0,
            EndAngle = 2 * Math.PI
        };
        AddArcToMesh(arc, vertices, indices, ref vertexOffset);
    }

    private void AddPolylineToMesh(LwPolyline polyline, List<float> vertices, List<int> indices, ref int vertexOffset)
    {
        var points = polyline.Vertices.ToList();
        for (int i = 0; i < points.Count - 1; i++)
        {
            var line = new Line
            {
                StartPoint = new CSMath.XYZ(points[i].Location.X, points[i].Location.Y, 0),
                EndPoint = new CSMath.XYZ(points[i + 1].Location.X, points[i + 1].Location.Y, 0)
            };
            AddLineToMesh(line, vertices, indices, ref vertexOffset);
        }

        if (polyline.IsClosed && points.Count > 1)
        {
            var line = new Line
            {
                StartPoint = new CSMath.XYZ(points[^1].Location.X, points[^1].Location.Y, 0),
                EndPoint = new CSMath.XYZ(points[0].Location.X, points[0].Location.Y, 0)
            };
            AddLineToMesh(line, vertices, indices, ref vertexOffset);
        }
    }

    private void AddVertex(List<float> vertices, double x, double y, double z)
    {
        vertices.Add((float)x);
        vertices.Add((float)y);
        vertices.Add((float)z);
    }

    private TrackSection? IdentifyTrackSection(CadEntityInfo entity)
    {
        // Identify if this entity represents a track section
        // Look for lines/polylines (straight track) and arcs (curves)

        if (entity.Type == "Line" && entity.Length > 100) // Min 100mm for track
        {
            return new TrackSection
            {
                Type = "Straight",
                Length = entity.Length,
                StartPoint = entity.StartPoint,
                EndPoint = entity.EndPoint
            };
        }
        else if (entity.Type == "Arc" && entity.Radius > 200) // Min 200mm radius for track curve
        {
            return new TrackSection
            {
                Type = "Curve",
                Length = entity.Length,
                Radius = entity.Radius,
                Angle = Math.Abs((entity.EndAngle ?? 0) - (entity.StartAngle ?? 0)),
                Center = entity.Center
            };
        }
        else if (entity.Type == "Polyline" && entity.Length > 100)
        {
            return new TrackSection
            {
                Type = "Straight",
                Length = entity.Length,
                Points = entity.Points
            };
        }

        return null;
    }

    private BoundingBox3D CalculateBoundingBox(List<CadEntityInfo> entities)
    {
        double minX = double.MaxValue, minY = double.MaxValue, minZ = double.MaxValue;
        double maxX = double.MinValue, maxY = double.MinValue, maxZ = double.MinValue;

        foreach (var entity in entities)
        {
            var points = new List<Point3D>();

            if (entity.StartPoint != null) points.Add(entity.StartPoint);
            if (entity.EndPoint != null) points.Add(entity.EndPoint);
            if (entity.Center != null) points.Add(entity.Center);
            if (entity.Points != null) points.AddRange(entity.Points);

            foreach (var p in points)
            {
                minX = Math.Min(minX, p.X);
                minY = Math.Min(minY, p.Y);
                minZ = Math.Min(minZ, p.Z);
                maxX = Math.Max(maxX, p.X);
                maxY = Math.Max(maxY, p.Y);
                maxZ = Math.Max(maxZ, p.Z);
            }

            // For arcs/circles, account for radius
            if (entity.Center != null && entity.Radius.HasValue)
            {
                minX = Math.Min(minX, entity.Center.X - entity.Radius.Value);
                minY = Math.Min(minY, entity.Center.Y - entity.Radius.Value);
                maxX = Math.Max(maxX, entity.Center.X + entity.Radius.Value);
                maxY = Math.Max(maxY, entity.Center.Y + entity.Radius.Value);
            }
        }

        return new BoundingBox3D
        {
            Min = new Point3D(minX, minY, minZ),
            Max = new Point3D(maxX, maxY, maxZ),
            Width = maxX - minX,
            Height = maxY - minY,
            Depth = maxZ - minZ
        };
    }

    private SuggestedConveyorConfig GenerateSuggestedConfig(CadImportResult result)
    {
        var config = new SuggestedConveyorConfig();

        // Calculate total track length
        config.TrackLength = result.TotalTrackLength + result.TotalCurveLength;

        // Estimate height from bounding box or default
        config.HeightFromFloor = result.BoundingBox?.Max.Z > 0
            ? result.BoundingBox.Max.Z
            : 3000; // Default 3m

        // Determine if curves are needed
        config.IncludeCurves = result.CurveCount > 0;

        // Get average curve radius if curves exist
        if (config.IncludeCurves)
        {
            var curveRadii = result.TrackSections
                .Where(t => t.Type == "Curve" && t.Radius.HasValue)
                .Select(t => t.Radius!.Value)
                .ToList();

            config.CurveRadius = curveRadii.Any() ? curveRadii.Average() : 500;
            config.CurveCount = result.CurveCount;
        }

        // Suggest profile series based on typical load (default to medium duty)
        config.SuggestedProfile = "24.000"; // 80kg capacity

        // Calculate suggested carrier count based on track length
        var carrierSpacing = 1000.0; // 1m default spacing
        config.NumCarriers = Math.Max(1, (int)(config.TrackLength / carrierSpacing));
        config.CarrierSpacing = carrierSpacing;

        return config;
    }

    private double CalculateDistance(Point3D p1, Point3D p2)
    {
        var dx = p2.X - p1.X;
        var dy = p2.Y - p1.Y;
        var dz = p2.Z - p1.Z;
        return Math.Sqrt(dx * dx + dy * dy + dz * dz);
    }

    private double CalculatePolylineLength(LwPolyline polyline)
    {
        double length = 0;
        var vertices = polyline.Vertices.ToList();

        for (int i = 0; i < vertices.Count - 1; i++)
        {
            var p1 = vertices[i].Location;
            var p2 = vertices[i + 1].Location;
            length += Math.Sqrt(Math.Pow(p2.X - p1.X, 2) + Math.Pow(p2.Y - p1.Y, 2));
        }

        if (polyline.IsClosed && vertices.Count > 1)
        {
            var p1 = vertices[^1].Location;
            var p2 = vertices[0].Location;
            length += Math.Sqrt(Math.Pow(p2.X - p1.X, 2) + Math.Pow(p2.Y - p1.Y, 2));
        }

        return length;
    }

    private double EstimateSplineLength(Spline spline)
    {
        // Approximate by summing distances between control points
        double length = 0;
        var points = spline.ControlPoints.ToList();

        for (int i = 0; i < points.Count - 1; i++)
        {
            var dx = points[i + 1].X - points[i].X;
            var dy = points[i + 1].Y - points[i].Y;
            var dz = points[i + 1].Z - points[i].Z;
            length += Math.Sqrt(dx * dx + dy * dy + dz * dz);
        }

        return length;
    }
}

// Result models
public class CadImportResult
{
    public bool Success { get; set; }
    public string? Error { get; set; }
    public string? FileName { get; set; }
    public List<CadEntityInfo> Entities { get; set; } = new();
    public List<TrackSection> TrackSections { get; set; } = new();
    public double TotalTrackLength { get; set; }
    public double TotalCurveLength { get; set; }
    public int CurveCount { get; set; }
    public BoundingBox3D? BoundingBox { get; set; }
    public CadMeshData? MeshData { get; set; }
    public SuggestedConveyorConfig? SuggestedConfig { get; set; }
}

public class CadEntityInfo
{
    public string Type { get; set; } = "";
    public string Layer { get; set; } = "";
    public Point3D? StartPoint { get; set; }
    public Point3D? EndPoint { get; set; }
    public Point3D? Center { get; set; }
    public double? Radius { get; set; }
    public double? StartAngle { get; set; }
    public double? EndAngle { get; set; }
    public double Length { get; set; }
    public List<Point3D>? Points { get; set; }
    public bool IsClosed { get; set; }
}

public class TrackSection
{
    public string Type { get; set; } = ""; // "Straight" or "Curve"
    public double Length { get; set; }
    public double? Radius { get; set; }
    public double? Angle { get; set; }
    public Point3D? StartPoint { get; set; }
    public Point3D? EndPoint { get; set; }
    public Point3D? Center { get; set; }
    public List<Point3D>? Points { get; set; }
}

public class Point3D
{
    public double X { get; set; }
    public double Y { get; set; }
    public double Z { get; set; }

    public Point3D() { }
    public Point3D(double x, double y, double z)
    {
        X = x;
        Y = y;
        Z = z;
    }
}

public class BoundingBox3D
{
    public Point3D Min { get; set; } = new();
    public Point3D Max { get; set; } = new();
    public double Width { get; set; }
    public double Height { get; set; }
    public double Depth { get; set; }
}

public class CadMeshData
{
    public float[] Vertices { get; set; } = Array.Empty<float>();
    public float[] Normals { get; set; } = Array.Empty<float>();
    public int[] Indices { get; set; } = Array.Empty<int>();
}

public class SuggestedConveyorConfig
{
    public double TrackLength { get; set; }
    public double HeightFromFloor { get; set; } = 3000;
    public string SuggestedProfile { get; set; } = "24.000";
    public bool IncludeCurves { get; set; }
    public double CurveRadius { get; set; }
    public int CurveCount { get; set; }
    public int NumCarriers { get; set; }
    public double CarrierSpacing { get; set; } = 1000;
}
