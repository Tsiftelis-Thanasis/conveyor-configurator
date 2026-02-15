namespace frontend.Models;

public record Client(
    int Id,
    string Code,
    string Name,
    string? Description,
    string? Country,
    string? Website,
    string? CatalogueReference,
    bool IsActive
);

public record Category(
    int Id,
    int ClientId,
    string Code,
    string Name,
    string? Description,
    string? PageReference
);

public record Material(
    int Id,
    int ClientId,
    string Code,
    string Name,
    string? Description
);

public record ProfileSeries(
    int Id,
    int ClientId,
    string SeriesCode,
    double HeightMm,
    double WidthMm,
    double SlotWidthMm,
    double WallThicknessMm,
    int? MaxLoadKg,
    Material? Material
);

public record Trolley(
    int Id,
    int ClientId,
    string PartNumber,
    int SeriesId,
    ProfileSeries? Series,
    Material? Material,
    string TrolleyType,
    int WheelCount,
    int SafeWorkingLoadKg,
    double? LengthMm,
    double? HeightMm,
    double? WidthMm,
    string? ThreadSize,
    bool HasRotatingScrew,
    bool HasGuideRollers,
    double? Price
);

public record TrackBend(
    int Id,
    int ClientId,
    string PartNumber,
    int SeriesId,
    ProfileSeries? Series,
    Material? Material,
    int AngleDegrees,
    double RadiusMm,
    double? DimensionA_Mm,
    double? TotalLengthMm,
    double? Price
);

public record Bracket(
    int Id,
    int ClientId,
    string PartNumber,
    int SeriesId,
    ProfileSeries? Series,
    Material? Material,
    string BracketType,
    double? LengthMm,
    double? HeightMm,
    double? WidthMm,
    string? ThreadSize,
    double? MaxAdjustmentMm,
    double? Price
);

public record Switch(
    int Id,
    int ClientId,
    string PartNumber,
    int SeriesId,
    ProfileSeries? Series,
    Material? Material,
    string SwitchType,
    double? RadiusMm,
    double? WidthMm,
    double? LengthMm,
    int? AngleDegrees,
    bool IsAutomatic,
    bool IsPneumatic,
    bool IncludesCylinder,
    double? Price
);

public record FlightBar(
    int Id,
    int ClientId,
    string PartNumber,
    int SeriesId,
    ProfileSeries? Series,
    Material? Material,
    string FlightBarType,
    int SafeWorkingLoadKg,
    double LengthMm,
    double SpanMm,
    double? HeightMm,
    double? EyeNutDiameterMm,
    bool HasRotatingEyeNut,
    double? Price
);
