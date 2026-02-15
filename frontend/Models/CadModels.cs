namespace frontend.Models;

/// <summary>
/// Result from CAD import operation
/// </summary>
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

/// <summary>
/// Information about a CAD entity (line, arc, circle, etc.)
/// </summary>
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

/// <summary>
/// Represents a track section (straight or curve)
/// </summary>
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

/// <summary>
/// 3D point
/// </summary>
public class Point3D
{
    public double X { get; set; }
    public double Y { get; set; }
    public double Z { get; set; }
}

/// <summary>
/// 3D bounding box
/// </summary>
public class BoundingBox3D
{
    public Point3D Min { get; set; } = new();
    public Point3D Max { get; set; } = new();
    public double Width { get; set; }
    public double Height { get; set; }
    public double Depth { get; set; }
}

/// <summary>
/// Mesh data for 3D visualization
/// </summary>
public class CadMeshData
{
    public float[] Vertices { get; set; } = Array.Empty<float>();
    public float[] Normals { get; set; } = Array.Empty<float>();
    public int[] Indices { get; set; } = Array.Empty<int>();
}

/// <summary>
/// Suggested conveyor configuration from CAD analysis
/// </summary>
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
