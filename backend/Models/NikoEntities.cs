namespace ConveyorApi.Models;

/// <summary>
/// Represents a product catalog client/manufacturer (e.g., NIKO, other suppliers)
/// </summary>
public class Client
{
    public int Id { get; set; }
    public required string Code { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public string? Country { get; set; }
    public string? Website { get; set; }
    public string? CatalogueReference { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

public class Category
{
    public int Id { get; set; }
    public int ClientId { get; set; }
    public Client? Client { get; set; }
    public required string Code { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public string? PageReference { get; set; }
}

public class Material
{
    public int Id { get; set; }
    public int ClientId { get; set; }
    public Client? Client { get; set; }
    public required string Code { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
}

public class ProfileSeries
{
    public int Id { get; set; }
    public int ClientId { get; set; }
    public Client? Client { get; set; }
    public required string SeriesCode { get; set; }
    public double HeightMm { get; set; }
    public double WidthMm { get; set; }
    public double SlotWidthMm { get; set; }
    public double WallThicknessMm { get; set; }
    public int? MaxLoadKg { get; set; }
    public int? MaterialId { get; set; }
    public Material? Material { get; set; }
}

public class TrackProfile
{
    public int Id { get; set; }
    public int ClientId { get; set; }
    public Client? Client { get; set; }
    public required string PartNumber { get; set; }
    public int SeriesId { get; set; }
    public ProfileSeries? Series { get; set; }
    public int MaterialId { get; set; }
    public Material? Material { get; set; }
    public double? LengthMm { get; set; }
    public double? PricePerMeter { get; set; }
}

public class TrackBend
{
    public int Id { get; set; }
    public int ClientId { get; set; }
    public Client? Client { get; set; }
    public required string PartNumber { get; set; }
    public int SeriesId { get; set; }
    public ProfileSeries? Series { get; set; }
    public int MaterialId { get; set; }
    public Material? Material { get; set; }
    public int AngleDegrees { get; set; }
    public double RadiusMm { get; set; }
    public double? DimensionA_Mm { get; set; }
    public double? TotalLengthMm { get; set; }
    public int? CategoryId { get; set; }
    public Category? Category { get; set; }
    public double? Price { get; set; }
}

public class Bracket
{
    public int Id { get; set; }
    public int ClientId { get; set; }
    public Client? Client { get; set; }
    public required string PartNumber { get; set; }
    public int SeriesId { get; set; }
    public ProfileSeries? Series { get; set; }
    public int MaterialId { get; set; }
    public Material? Material { get; set; }
    public required string BracketType { get; set; }
    public double? LengthMm { get; set; }
    public double? HeightMm { get; set; }
    public double? WidthMm { get; set; }
    public double? WallThicknessMm { get; set; }
    public double? HoleDiameterMm { get; set; }
    public string? ThreadSize { get; set; }
    public double? MaxAdjustmentMm { get; set; }
    public int? CategoryId { get; set; }
    public Category? Category { get; set; }
    public double? Price { get; set; }
}

public class Trolley
{
    public int Id { get; set; }
    public int ClientId { get; set; }
    public Client? Client { get; set; }
    public required string PartNumber { get; set; }
    public int SeriesId { get; set; }
    public ProfileSeries? Series { get; set; }
    public int MaterialId { get; set; }
    public Material? Material { get; set; }
    public required string TrolleyType { get; set; }
    public int WheelCount { get; set; }
    public int SafeWorkingLoadKg { get; set; }
    public double? LengthMm { get; set; }
    public double? HeightMm { get; set; }
    public double? WidthMm { get; set; }
    public double? HoleDiameterMm { get; set; }
    public string? ThreadSize { get; set; }
    public bool HasRotatingScrew { get; set; }
    public bool HasGuideRollers { get; set; }
    public int? CategoryId { get; set; }
    public Category? Category { get; set; }
    public double? Price { get; set; }
}

public class FlightBar
{
    public int Id { get; set; }
    public int ClientId { get; set; }
    public Client? Client { get; set; }
    public required string PartNumber { get; set; }
    public int SeriesId { get; set; }
    public ProfileSeries? Series { get; set; }
    public int MaterialId { get; set; }
    public Material? Material { get; set; }
    public required string FlightBarType { get; set; }
    public int SafeWorkingLoadKg { get; set; }
    public double LengthMm { get; set; }
    public double SpanMm { get; set; }
    public double? HeightMm { get; set; }
    public double? EyeNutDiameterMm { get; set; }
    public bool HasRotatingEyeNut { get; set; }
    public int? CategoryId { get; set; }
    public Category? Category { get; set; }
    public double? Price { get; set; }
}

public class Switch
{
    public int Id { get; set; }
    public int ClientId { get; set; }
    public Client? Client { get; set; }
    public required string PartNumber { get; set; }
    public int SeriesId { get; set; }
    public ProfileSeries? Series { get; set; }
    public int MaterialId { get; set; }
    public Material? Material { get; set; }
    public required string SwitchType { get; set; }
    public string? OperationType { get; set; }
    public double? RadiusMm { get; set; }
    public double? WidthMm { get; set; }
    public double? LengthMm { get; set; }
    public int? AngleDegrees { get; set; }
    public bool IsAutomatic { get; set; }
    public bool IsPneumatic { get; set; }
    public bool IncludesCylinder { get; set; }
    public int? CategoryId { get; set; }
    public Category? Category { get; set; }
    public double? Price { get; set; }
}

public class Stopper
{
    public int Id { get; set; }
    public int ClientId { get; set; }
    public Client? Client { get; set; }
    public required string PartNumber { get; set; }
    public int SeriesId { get; set; }
    public ProfileSeries? Series { get; set; }
    public int MaterialId { get; set; }
    public Material? Material { get; set; }
    public required string StopperType { get; set; }
    public string? TongueType { get; set; }
    public double? LengthMm { get; set; }
    public double? HeightMm { get; set; }
    public bool IsPneumatic { get; set; }
    public bool IsSpringLoaded { get; set; }
    public int? CategoryId { get; set; }
    public Category? Category { get; set; }
    public double? Price { get; set; }
}

public class SwivelUnit
{
    public int Id { get; set; }
    public int ClientId { get; set; }
    public Client? Client { get; set; }
    public required string PartNumber { get; set; }
    public int SeriesId { get; set; }
    public ProfileSeries? Series { get; set; }
    public int MaterialId { get; set; }
    public Material? Material { get; set; }
    public required string SwivelType { get; set; }
    public string? Direction { get; set; }
    public double? L1_Mm { get; set; }
    public double? L2_Mm { get; set; }
    public double? L3_Mm { get; set; }
    public bool IsPneumatic { get; set; }
    public bool IsSpringLoaded { get; set; }
    public int? CategoryId { get; set; }
    public Category? Category { get; set; }
    public double? Price { get; set; }
}

public class BridgeInterlock
{
    public int Id { get; set; }
    public int ClientId { get; set; }
    public Client? Client { get; set; }
    public required string PartNumber { get; set; }
    public int SeriesId { get; set; }
    public ProfileSeries? Series { get; set; }
    public int MaterialId { get; set; }
    public Material? Material { get; set; }
    public required string PartType { get; set; }
    public double? WidthMm { get; set; }
    public double? HeightMm { get; set; }
    public double? LengthMm { get; set; }
    public double? MinHeightMm { get; set; }
    public double? MaxHeightMm { get; set; }
    public int? CategoryId { get; set; }
    public Category? Category { get; set; }
    public double? Price { get; set; }
}

public class DropLiftUnit
{
    public int Id { get; set; }
    public int ClientId { get; set; }
    public Client? Client { get; set; }
    public required string PartNumber { get; set; }
    public int SeriesId { get; set; }
    public ProfileSeries? Series { get; set; }
    public int MaterialId { get; set; }
    public Material? Material { get; set; }
    public required string LiftType { get; set; }
    public double? MinTrackLengthMm { get; set; }
    public double? ApproachLengthMm { get; set; }
    public double? HeightMm { get; set; }
    public double? WidthMm { get; set; }
    public int? SafeWorkingLoadKg { get; set; }
    public int? CategoryId { get; set; }
    public Category? Category { get; set; }
    public double? Price { get; set; }
}

public class Accessory
{
    public int Id { get; set; }
    public int ClientId { get; set; }
    public Client? Client { get; set; }
    public required string PartNumber { get; set; }
    public int SeriesId { get; set; }
    public ProfileSeries? Series { get; set; }
    public int MaterialId { get; set; }
    public Material? Material { get; set; }
    public required string AccessoryType { get; set; }
    public string? Description { get; set; }
    public double? LengthMm { get; set; }
    public double? HeightMm { get; set; }
    public double? WidthMm { get; set; }
    public int? CategoryId { get; set; }
    public Category? Category { get; set; }
    public double? Price { get; set; }
}

public class BearingOption
{
    public int Id { get; set; }
    public int ClientId { get; set; }
    public Client? Client { get; set; }
    public required string PartNumber { get; set; }
    public int SeriesId { get; set; }
    public ProfileSeries? Series { get; set; }
    public required string BearingType { get; set; }
    public double? MinTemperatureC { get; set; }
    public double? MaxTemperatureC { get; set; }
    public string? Description { get; set; }
    public double? Price { get; set; }
}

public class TurnTableSwitch
{
    public int Id { get; set; }
    public int ClientId { get; set; }
    public Client? Client { get; set; }
    public required string PartNumber { get; set; }
    public int SeriesId { get; set; }
    public ProfileSeries? Series { get; set; }
    public int MaterialId { get; set; }
    public Material? Material { get; set; }
    public required string SwitchType { get; set; }
    public double? DimensionA_Mm { get; set; }
    public double? DimensionB_Mm { get; set; }
    public double? ProfileHeightMm { get; set; }
    public string? OperationType { get; set; }
    public int SupportPoints { get; set; } = 8;
    public int? CategoryId { get; set; }
    public Category? Category { get; set; }
    public double? Price { get; set; }
}

public class PneumaticControl
{
    public int Id { get; set; }
    public int ClientId { get; set; }
    public Client? Client { get; set; }
    public required string PartNumber { get; set; }
    public required string ControlType { get; set; }
    public string? Description { get; set; }
    public int? WayCount { get; set; }
    public double? TubeLengthM { get; set; }
    public int? CategoryId { get; set; }
    public Category? Category { get; set; }
    public double? Price { get; set; }
}
