namespace frontend.Models;

/// <summary>
/// Configuration for overhead conveyor
/// </summary>
public class OverheadConveyorConfig
{
    public double TrackLength { get; set; } = 10000;
    public double HeightFromFloor { get; set; } = 3000;
    public string TrackProfile { get; set; } = "24.000";
    public double CarrierSpacing { get; set; } = 1000;
    public double LoadPerCarrier { get; set; } = 50;
    public int NumCarriers { get; set; } = 10;
    public bool IncludeCurves { get; set; } = false;
    public double CurveRadius { get; set; } = 500;
    public double InclineAngle { get; set; } = 0;
    public double DeclineAngle { get; set; } = 0;
    public int DriveUnits { get; set; } = 1;
}

/// <summary>
/// Bill of Materials item
/// </summary>
public class BomItem
{
    public string Category { get; set; } = "";
    public string PartNumber { get; set; } = "";
    public string Description { get; set; } = "";
    public int Quantity { get; set; } = 1;
    public double UnitPrice { get; set; }
    public bool CanSwap { get; set; }
    public List<string> AlternativePartNumbers { get; set; } = new();

    public double TotalPrice => Quantity * UnitPrice;
}

/// <summary>
/// Quote request model
/// </summary>
public class QuoteRequest
{
    public string? Id { get; set; }
    public string Company { get; set; } = "";
    public string Contact { get; set; } = "";
    public string Email { get; set; } = "";
    public string? Phone { get; set; }
    public int Quantity { get; set; } = 1;
    public string? Notes { get; set; }
    public string ConveyorType { get; set; } = "overhead";
    public OverheadConveyorConfig? OverheadConfiguration { get; set; }
    public List<BomItem>? BomItems { get; set; }
}

public record QuoteResponse(bool Success, string? QuoteId, string? Message);
