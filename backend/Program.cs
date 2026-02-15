using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ConveyorApi.Data;
using ConveyorApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add Entity Framework with SQLite
builder.Services.AddDbContext<ProductDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("NikoProducts")));

// Add CORS for frontend (still useful for development scenarios)
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Ensure database is created and seeded
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ProductDbContext>();
    await db.Database.EnsureCreatedAsync();
    await NikoProductSeeder.SeedAsync(db);
}

app.UseCors();

// Serve static files from wwwroot
app.UseDefaultFiles();
app.UseStaticFiles();

// Store quotes in memory (use database in production)
var quotes = new List<QuoteRequest>();

// Health check
app.MapGet("/api/health", () => Results.Ok(new { status = "healthy", timestamp = DateTime.UtcNow }));

// Export STEP file (Roller conveyor)
app.MapPost("/api/export/step", async ([FromBody] ConveyorConfig config) =>
{
    try
    {
        // Generate STEP file content
        var stepContent = GenerateStepFile(config);

        var bytes = System.Text.Encoding.UTF8.GetBytes(stepContent);
        return Results.File(bytes, "application/step", "conveyor.step");
    }
    catch (Exception ex)
    {
        return Results.Problem($"Error generating STEP file: {ex.Message}");
    }
});

// Export STEP file (Overhead conveyor)
app.MapPost("/api/export/overhead-step", async ([FromBody] OverheadConveyorConfig config) =>
{
    try
    {
        // Generate STEP file content for overhead conveyor
        var stepContent = GenerateOverheadStepFile(config);

        var bytes = System.Text.Encoding.UTF8.GetBytes(stepContent);
        return Results.File(bytes, "application/step", "overhead-conveyor.step");
    }
    catch (Exception ex)
    {
        return Results.Problem($"Error generating overhead STEP file: {ex.Message}");
    }
});

// Submit quote request
app.MapPost("/api/quotes", async ([FromBody] QuoteRequest quote) =>
{
    quote.Id = Guid.NewGuid().ToString();
    quote.SubmittedAt = DateTime.UtcNow;
    quotes.Add(quote);

    // In production: save to database, send email notification, etc.
    Console.WriteLine($"New quote request from {quote.Company} - {quote.Email}");

    // Save to file for persistence
    var quotesDir = Path.Combine(Directory.GetCurrentDirectory(), "quotes");
    Directory.CreateDirectory(quotesDir);
    var filePath = Path.Combine(quotesDir, $"quote_{quote.Id}.json");
    await File.WriteAllTextAsync(filePath, JsonSerializer.Serialize(quote, new JsonSerializerOptions { WriteIndented = true }));

    return Results.Ok(new { success = true, quoteId = quote.Id, message = "Quote request received" });
});

// Get all quotes (admin endpoint)
app.MapGet("/api/quotes", () => Results.Ok(quotes));

// Parse uploaded CAD file (DWG/DXF) and return mesh data
app.MapPost("/api/import/cad", async (HttpRequest request) =>
{
    if (!request.HasFormContentType)
        return Results.BadRequest("Expected form data");

    var form = await request.ReadFormAsync();
    var file = form.Files.FirstOrDefault();

    if (file == null)
        return Results.BadRequest("No file uploaded");

    var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
    if (extension != ".dwg" && extension != ".dxf")
        return Results.BadRequest("Unsupported file format. Please upload DWG (.dwg) or DXF (.dxf) files.");

    try
    {
        var service = new ConveyorApi.Services.CadImportService();
        var result = service.ImportFile(file.OpenReadStream(), file.FileName);

        return result.Success
            ? Results.Ok(result)
            : Results.BadRequest(new { error = result.Error });
    }
    catch (Exception ex)
    {
        return Results.Problem($"Error importing CAD file: {ex.Message}");
    }
});

// =============================================
// Product Catalog API Endpoints
// =============================================

// Get all clients
app.MapGet("/api/clients", async (ProductDbContext db) =>
    await db.Clients.Where(c => c.IsActive).ToListAsync());

// Get client by code
app.MapGet("/api/clients/{code}", async (string code, ProductDbContext db) =>
    await db.Clients.FirstOrDefaultAsync(c => c.Code == code) is Client client
        ? Results.Ok(client)
        : Results.NotFound());

// Get categories for a client
app.MapGet("/api/clients/{clientCode}/categories", async (string clientCode, ProductDbContext db) =>
{
    var client = await db.Clients.FirstOrDefaultAsync(c => c.Code == clientCode);
    if (client == null) return Results.NotFound("Client not found");

    var categories = await db.Categories.Where(c => c.ClientId == client.Id).ToListAsync();
    return Results.Ok(categories);
});

// Get profile series for a client
app.MapGet("/api/clients/{clientCode}/series", async (string clientCode, ProductDbContext db) =>
{
    var client = await db.Clients.FirstOrDefaultAsync(c => c.Code == clientCode);
    if (client == null) return Results.NotFound("Client not found");

    var series = await db.ProfileSeries
        .Where(s => s.ClientId == client.Id)
        .Include(s => s.Material)
        .ToListAsync();
    return Results.Ok(series);
});

// Get trolleys for a client with optional series filter
app.MapGet("/api/clients/{clientCode}/trolleys", async (string clientCode, string? series, ProductDbContext db) =>
{
    var client = await db.Clients.FirstOrDefaultAsync(c => c.Code == clientCode);
    if (client == null) return Results.NotFound("Client not found");

    var query = db.Trolleys
        .Where(t => t.ClientId == client.Id)
        .Include(t => t.Series)
        .Include(t => t.Material)
        .AsQueryable();

    if (!string.IsNullOrEmpty(series))
    {
        query = query.Where(t => t.Series != null && t.Series.SeriesCode == series);
    }

    return Results.Ok(await query.ToListAsync());
});

// Get track bends for a client with optional series filter
app.MapGet("/api/clients/{clientCode}/bends", async (string clientCode, string? series, int? angle, ProductDbContext db) =>
{
    var client = await db.Clients.FirstOrDefaultAsync(c => c.Code == clientCode);
    if (client == null) return Results.NotFound("Client not found");

    var query = db.TrackBends
        .Where(b => b.ClientId == client.Id)
        .Include(b => b.Series)
        .Include(b => b.Material)
        .AsQueryable();

    if (!string.IsNullOrEmpty(series))
    {
        query = query.Where(b => b.Series != null && b.Series.SeriesCode == series);
    }
    if (angle.HasValue)
    {
        query = query.Where(b => b.AngleDegrees == angle.Value);
    }

    return Results.Ok(await query.ToListAsync());
});

// Get brackets for a client
app.MapGet("/api/clients/{clientCode}/brackets", async (string clientCode, string? series, ProductDbContext db) =>
{
    var client = await db.Clients.FirstOrDefaultAsync(c => c.Code == clientCode);
    if (client == null) return Results.NotFound("Client not found");

    var query = db.Brackets
        .Where(b => b.ClientId == client.Id)
        .Include(b => b.Series)
        .Include(b => b.Material)
        .AsQueryable();

    if (!string.IsNullOrEmpty(series))
    {
        query = query.Where(b => b.Series != null && b.Series.SeriesCode == series);
    }

    return Results.Ok(await query.ToListAsync());
});

// Get switches for a client
app.MapGet("/api/clients/{clientCode}/switches", async (string clientCode, string? series, ProductDbContext db) =>
{
    var client = await db.Clients.FirstOrDefaultAsync(c => c.Code == clientCode);
    if (client == null) return Results.NotFound("Client not found");

    var query = db.Switches
        .Where(s => s.ClientId == client.Id)
        .Include(s => s.Series)
        .Include(s => s.Material)
        .AsQueryable();

    if (!string.IsNullOrEmpty(series))
    {
        query = query.Where(s => s.Series != null && s.Series.SeriesCode == series);
    }

    return Results.Ok(await query.ToListAsync());
});

// Get flight bars for a client
app.MapGet("/api/clients/{clientCode}/flightbars", async (string clientCode, string? series, ProductDbContext db) =>
{
    var client = await db.Clients.FirstOrDefaultAsync(c => c.Code == clientCode);
    if (client == null) return Results.NotFound("Client not found");

    var query = db.FlightBars
        .Where(f => f.ClientId == client.Id)
        .Include(f => f.Series)
        .Include(f => f.Material)
        .AsQueryable();

    if (!string.IsNullOrEmpty(series))
    {
        query = query.Where(f => f.Series != null && f.Series.SeriesCode == series);
    }

    return Results.Ok(await query.ToListAsync());
});

// Search products across all types
app.MapGet("/api/clients/{clientCode}/products/search", async (string clientCode, string? q, string? category, ProductDbContext db) =>
{
    var client = await db.Clients.FirstOrDefaultAsync(c => c.Code == clientCode);
    if (client == null) return Results.NotFound("Client not found");

    var results = new Dictionary<string, object>();

    // Search trolleys
    var trolleys = await db.Trolleys
        .Where(t => t.ClientId == client.Id)
        .Where(t => string.IsNullOrEmpty(q) || t.PartNumber.Contains(q) || t.TrolleyType.Contains(q))
        .Include(t => t.Series)
        .Take(20)
        .ToListAsync();
    if (trolleys.Any()) results["trolleys"] = trolleys;

    // Search track bends
    var bends = await db.TrackBends
        .Where(b => b.ClientId == client.Id)
        .Where(b => string.IsNullOrEmpty(q) || b.PartNumber.Contains(q))
        .Include(b => b.Series)
        .Take(20)
        .ToListAsync();
    if (bends.Any()) results["trackBends"] = bends;

    // Search brackets
    var brackets = await db.Brackets
        .Where(b => b.ClientId == client.Id)
        .Where(b => string.IsNullOrEmpty(q) || b.PartNumber.Contains(q) || b.BracketType.Contains(q))
        .Include(b => b.Series)
        .Take(20)
        .ToListAsync();
    if (brackets.Any()) results["brackets"] = brackets;

    // Search switches
    var switches = await db.Switches
        .Where(s => s.ClientId == client.Id)
        .Where(s => string.IsNullOrEmpty(q) || s.PartNumber.Contains(q) || s.SwitchType.Contains(q))
        .Include(s => s.Series)
        .Take(20)
        .ToListAsync();
    if (switches.Any()) results["switches"] = switches;

    return Results.Ok(results);
});

app.Run();

// Generate STEP file (simplified - real implementation would use OpenCascade)
static string GenerateStepFile(ConveyorConfig config)
{
    var timestamp = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss");
    var numRollers = (int)(config.Length / config.RollerSpacing);

    // This is a simplified STEP file structure
    // In production, you would use OpenCascade (OCC) to generate proper geometry
    var step = $@"ISO-10303-21;
HEADER;
FILE_DESCRIPTION(('Roller Conveyor Configuration'),'2;1');
FILE_NAME('conveyor.step','{timestamp}',('Conveyor Configurator'),('Industrial Design PoC'),'','','');
FILE_SCHEMA(('AUTOMOTIVE_DESIGN'));
ENDSEC;

DATA;
/* Conveyor Configuration Parameters */
/* Length: {config.Length} mm */
/* Width: {config.Width} mm */
/* Height: {config.Height} mm */
/* Roller Diameter: {config.RollerDiameter} mm */
/* Roller Spacing: {config.RollerSpacing} mm */
/* Number of Rollers: {numRollers} */
/* Load Capacity: {config.LoadCapacity} kg */
/* Drive Type: {config.DriveType} */

#1=APPLICATION_CONTEXT('automotive design');
#2=APPLICATION_PROTOCOL_DEFINITION('international standard','automotive_design',2000,#1);
#3=PRODUCT_CONTEXT('',#1,'mechanical');
#4=PRODUCT('RollerConveyor','Roller Conveyor Assembly','Configured conveyor system',(#3));
#5=PRODUCT_DEFINITION_FORMATION('','',#4);
#6=PRODUCT_DEFINITION_CONTEXT('part definition',#1,'design');
#7=PRODUCT_DEFINITION('design','',#5,#6);

/* Frame - Left Side */
#10=CARTESIAN_POINT('Origin',(0.0,0.0,0.0));
#11=DIRECTION('Z',(0.0,0.0,1.0));
#12=DIRECTION('X',(1.0,0.0,0.0));
#13=AXIS2_PLACEMENT_3D('',#10,#11,#12);

/* Frame dimensions */
#20=CARTESIAN_POINT('FrameStart',(0.0,{config.Height},{config.Width / 2.0}));
#21=CARTESIAN_POINT('FrameEnd',({config.Length},{config.Height},{config.Width / 2.0}));

/* Roller positions */
";

    // Add roller definitions
    for (int i = 0; i < numRollers; i++)
    {
        var xPos = config.RollerSpacing / 2.0 + i * config.RollerSpacing;
        var yPos = config.Height - config.RollerDiameter / 2.0;
        step += $@"#{ 100 + i}=CARTESIAN_POINT('Roller{i + 1}',({xPos},{yPos},0.0));
";
    }

    step += @"
ENDSEC;
END-ISO-10303-21;";

    return step;
}

// Generate STEP file for overhead conveyor (simplified - real implementation would use OpenCascade)
static string GenerateOverheadStepFile(OverheadConveyorConfig config)
{
    var timestamp = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss");
    var totalLoad = config.NumCarriers * config.LoadPerCarrier;

    // This is a simplified STEP file structure
    // In production, you would use OpenCascade (OCC) to generate proper geometry
    var step = $@"ISO-10303-21;
HEADER;
FILE_DESCRIPTION(('Overhead Conveyor Configuration'),'2;1');
FILE_NAME('overhead-conveyor.step','{timestamp}',('Conveyor Configurator'),('Industrial Design PoC'),'','','');
FILE_SCHEMA(('AUTOMOTIVE_DESIGN'));
ENDSEC;

DATA;
/* Overhead Conveyor Configuration Parameters */
/* Track Length: {config.TrackLength} mm */
/* Height from Floor: {config.HeightFromFloor} mm */
/* Carrier Spacing: {config.CarrierSpacing} mm */
/* Load per Carrier: {config.LoadPerCarrier} kg */
/* Number of Carriers: {config.NumCarriers} */
/* Include Curves: {config.IncludeCurves} */
/* Curve Radius: {config.CurveRadius} mm */
/* Incline Angle: {config.InclineAngle} degrees */
/* Decline Angle: {config.DeclineAngle} degrees */
/* Drive Units: {config.DriveUnits} */
/* Total Load Capacity: {totalLoad} kg */

#1=APPLICATION_CONTEXT('automotive design');
#2=APPLICATION_PROTOCOL_DEFINITION('international standard','automotive_design',2000,#1);
#3=PRODUCT_CONTEXT('',#1,'mechanical');
#4=PRODUCT('OverheadConveyor','Overhead Conveyor Assembly','Configured overhead conveyor system',(#3));
#5=PRODUCT_DEFINITION_FORMATION('','',#4);
#6=PRODUCT_DEFINITION_CONTEXT('part definition',#1,'design');
#7=PRODUCT_DEFINITION('design','',#5,#6);

/* I-Beam Track Profile */
/* Width: 100mm, Height: 80mm, Flange: 8mm, Web: 6mm */
#10=CARTESIAN_POINT('Origin',(0.0,0.0,0.0));
#11=DIRECTION('Z',(0.0,0.0,1.0));
#12=DIRECTION('X',(1.0,0.0,0.0));
#13=AXIS2_PLACEMENT_3D('',#10,#11,#12);

/* Track start and end points */
#20=CARTESIAN_POINT('TrackStart',(0.0,{config.HeightFromFloor},0.0));
#21=CARTESIAN_POINT('TrackEnd',({config.TrackLength},{config.HeightFromFloor},0.0));

/* Carrier positions */
";

    // Add carrier definitions
    for (int i = 0; i < config.NumCarriers; i++)
    {
        var xPos = config.CarrierSpacing / 2.0 + i * config.CarrierSpacing;
        step += $@"#{ 100 + i}=CARTESIAN_POINT('Carrier{i + 1}',({xPos},{config.HeightFromFloor},0.0));
";
    }

    // Add drive unit positions
    for (int i = 0; i < config.DriveUnits; i++)
    {
        var xPos = i == 0 ? 0 : config.TrackLength * i / config.DriveUnits;
        step += $@"#{ 200 + i}=CARTESIAN_POINT('DriveUnit{i + 1}',({xPos},{config.HeightFromFloor + 150},0.0));
";
    }

    step += @"
ENDSEC;
END-ISO-10303-21;";

    return step;
}

// Models
public record ConveyorConfig
{
    public double Length { get; init; } = 2000;
    public double Width { get; init; } = 600;
    public double Height { get; init; } = 750;
    public double RollerDiameter { get; init; } = 50;
    public double RollerSpacing { get; init; } = 100;
    public int LoadCapacity { get; init; } = 300;
    public string DriveType { get; init; } = "powered";
}

public record OverheadConveyorConfig
{
    public double TrackLength { get; init; } = 10000;
    public double HeightFromFloor { get; init; } = 3000;
    public string TrackProfile { get; init; } = "i-beam";
    public double CarrierSpacing { get; init; } = 1000;
    public double LoadPerCarrier { get; init; } = 50;
    public int NumCarriers { get; init; } = 10;
    public bool IncludeCurves { get; init; } = false;
    public double CurveRadius { get; init; } = 500;
    public double InclineAngle { get; init; } = 0;
    public double DeclineAngle { get; init; } = 0;
    public int DriveUnits { get; init; } = 1;
}

public record QuoteRequest
{
    public string? Id { get; set; }
    public string Company { get; init; } = "";
    public string Contact { get; init; } = "";
    public string Email { get; init; } = "";
    public string? Phone { get; init; }
    public int Quantity { get; init; } = 1;
    public string? Notes { get; init; }
    public string ConveyorType { get; init; } = "roller";
    public ConveyorConfig? RollerConfiguration { get; init; }
    public OverheadConveyorConfig? OverheadConfiguration { get; init; }
    public DateTime SubmittedAt { get; set; }
}

// DTO for mesh data sent to frontend
public record MeshDto
{
    public float[] Vertices { get; init; } = Array.Empty<float>();
    public float[] Normals { get; init; } = Array.Empty<float>();
    public int[] Indices { get; init; } = Array.Empty<int>();
    public float[]? Color { get; init; }
}
