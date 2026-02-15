using ConveyorApi.Models;
using Microsoft.EntityFrameworkCore;

namespace ConveyorApi.Data;

public static class NikoProductSeeder
{
    public static async Task SeedAsync(ProductDbContext context)
    {
        // Check if NIKO client already exists
        if (await context.Clients.AnyAsync(c => c.Code == "NIKO"))
        {
            return; // Already seeded
        }

        // Create NIKO client
        var client = new Client
        {
            Code = "NIKO",
            Name = "NIKO Conveyor Systems",
            Description = "Enclosed track overhead conveyor systems for loads up to 2,000 kg",
            Country = "Greece",
            Website = "https://www.helmhellas.gr",
            CatalogueReference = "NIKO Product Catalogue October 2022",
            IsActive = true
        };
        context.Clients.Add(client);
        await context.SaveChangesAsync();

        // Seed Categories
        var categories = new Dictionary<string, Category>
        {
            ["PROFILES"] = new() { ClientId = client.Id, Code = "PROFILES", Name = "Track Profiles", Description = "Enclosed track tapered design profiles for loads up to 2,000 kg", PageReference = "12" },
            ["BENDS"] = new() { ClientId = client.Id, Code = "BENDS", Name = "Track Bends", Description = "Horizontal and vertical track bends at various angles", PageReference = "13" },
            ["BRACKETS"] = new() { ClientId = client.Id, Code = "BRACKETS", Name = "Brackets & Support Joints", Description = "Mounting brackets, splice joints, and support systems", PageReference = "14-23" },
            ["TROLLEYS"] = new() { ClientId = client.Id, Code = "TROLLEYS", Name = "Trolleys", Description = "2-wheel and 4-wheel trolleys with various configurations", PageReference = "24-39" },
            ["FLIGHTBARS"] = new() { ClientId = client.Id, Code = "FLIGHTBARS", Name = "Flight Bars", Description = "Load carrying bars with eye nuts and hooks", PageReference = "33-39" },
            ["SWITCHES"] = new() { ClientId = client.Id, Code = "SWITCHES", Name = "Switches", Description = "Tongue switches, swivel switches, and turntable switches", PageReference = "40-48" },
            ["STOPPERS"] = new() { ClientId = client.Id, Code = "STOPPERS", Name = "Stoppers", Description = "Directional stoppers and spring loaded stoppers", PageReference = "49-50" },
            ["SWIVEL"] = new() { ClientId = client.Id, Code = "SWIVEL", Name = "Swivel Units", Description = "Manual and pneumatic swivel units", PageReference = "50-51" },
            ["INTERLOCK"] = new() { ClientId = client.Id, Code = "INTERLOCK", Name = "Bridge Interlocks", Description = "Bridge interlock and track transfer assemblies", PageReference = "52-53" },
            ["DROPLIFT"] = new() { ClientId = client.Id, Code = "DROPLIFT", Name = "Drop-Lift Units", Description = "Standard and low headroom drop-lift stations", PageReference = "54-55" },
            ["ACCESSORIES"] = new() { ClientId = client.Id, Code = "ACCESSORIES", Name = "Accessories", Description = "End stops, track covers, dilation joints", PageReference = "50" },
            ["BEARINGS"] = new() { ClientId = client.Id, Code = "BEARINGS", Name = "Bearing Options", Description = "Special bearing options for various temperature ranges", PageReference = "23" },
            ["PNEUMATIC"] = new() { ClientId = client.Id, Code = "PNEUMATIC", Name = "Pneumatic Controls", Description = "Pneumatic controls and air distributors", PageReference = "42" },
            ["STAINLESS"] = new() { ClientId = client.Id, Code = "STAINLESS", Name = "Stainless Steel Series", Description = "Components in stainless steel 304 and 316", PageReference = "56-62" },
        };
        context.Categories.AddRange(categories.Values);
        await context.SaveChangesAsync();

        // Seed Materials
        var materials = new Dictionary<string, Material>
        {
            ["STL-GOLD"] = new() { ClientId = client.Id, Code = "STL-GOLD", Name = "Steel Gold Finish", Description = "Standard steel with gold zinc-plated finish" },
            ["STL-SILVER"] = new() { ClientId = client.Id, Code = "STL-SILVER", Name = "Steel Silver Finish", Description = "Standard steel with silver zinc-plated finish" },
            ["STL-BLACK"] = new() { ClientId = client.Id, Code = "STL-BLACK", Name = "Steel Black Finish", Description = "Steel with black finish" },
            ["STL-PLAIN"] = new() { ClientId = client.Id, Code = "STL-PLAIN", Name = "Steel Plain/Unplated", Description = "Plain unplated steel" },
            ["SS304"] = new() { ClientId = client.Id, Code = "SS304", Name = "Stainless Steel 304 (A2)", Description = "AISI 304 stainless steel, also known as A2" },
            ["SS316"] = new() { ClientId = client.Id, Code = "SS316", Name = "Stainless Steel 316 (A4)", Description = "AISI 316 stainless steel, also known as A4" },
        };
        context.Materials.AddRange(materials.Values);
        await context.SaveChangesAsync();

        // Seed Profile Series
        var profileSeries = new Dictionary<string, ProfileSeries>
        {
            // Standard Steel Series
            ["21.000"] = new() { ClientId = client.Id, SeriesCode = "21.000", HeightMm = 28.0, WidthMm = 30.0, SlotWidthMm = 8.0, WallThicknessMm = 1.75, MaxLoadKg = 20, MaterialId = materials["STL-GOLD"].Id },
            ["23.000"] = new() { ClientId = client.Id, SeriesCode = "23.000", HeightMm = 35.0, WidthMm = 40.0, SlotWidthMm = 11.0, WallThicknessMm = 2.75, MaxLoadKg = 40, MaterialId = materials["STL-GOLD"].Id },
            ["24.000"] = new() { ClientId = client.Id, SeriesCode = "24.000", HeightMm = 43.5, WidthMm = 48.5, SlotWidthMm = 15.0, WallThicknessMm = 3.20, MaxLoadKg = 80, MaterialId = materials["STL-GOLD"].Id },
            ["25.000"] = new() { ClientId = client.Id, SeriesCode = "25.000", HeightMm = 60.0, WidthMm = 65.0, SlotWidthMm = 18.0, WallThicknessMm = 3.60, MaxLoadKg = 200, MaterialId = materials["STL-GOLD"].Id },
            ["26.000"] = new() { ClientId = client.Id, SeriesCode = "26.000", HeightMm = 75.0, WidthMm = 80.0, SlotWidthMm = 22.0, WallThicknessMm = 4.50, MaxLoadKg = 400, MaterialId = materials["STL-GOLD"].Id },
            ["27.000"] = new() { ClientId = client.Id, SeriesCode = "27.000", HeightMm = 110.0, WidthMm = 90.0, SlotWidthMm = 25.0, WallThicknessMm = 6.50, MaxLoadKg = 800, MaterialId = materials["STL-GOLD"].Id },
            // Stainless Steel 304 Series
            ["21.050"] = new() { ClientId = client.Id, SeriesCode = "21.050", HeightMm = 28.0, WidthMm = 30.0, SlotWidthMm = 8.0, WallThicknessMm = 1.75, MaxLoadKg = 20, MaterialId = materials["SS304"].Id },
            ["23.050"] = new() { ClientId = client.Id, SeriesCode = "23.050", HeightMm = 35.0, WidthMm = 40.0, SlotWidthMm = 11.0, WallThicknessMm = 3.00, MaxLoadKg = 40, MaterialId = materials["SS304"].Id },
            ["24.050"] = new() { ClientId = client.Id, SeriesCode = "24.050", HeightMm = 43.5, WidthMm = 48.5, SlotWidthMm = 15.0, WallThicknessMm = 3.20, MaxLoadKg = 80, MaterialId = materials["SS304"].Id },
            ["25.050"] = new() { ClientId = client.Id, SeriesCode = "25.050", HeightMm = 60.0, WidthMm = 65.0, SlotWidthMm = 18.0, WallThicknessMm = 4.00, MaxLoadKg = 200, MaterialId = materials["SS304"].Id },
            // Stainless Steel 316 Series
            ["21.070"] = new() { ClientId = client.Id, SeriesCode = "21.070", HeightMm = 28.0, WidthMm = 30.0, SlotWidthMm = 8.0, WallThicknessMm = 1.75, MaxLoadKg = 20, MaterialId = materials["SS316"].Id },
            ["23.070"] = new() { ClientId = client.Id, SeriesCode = "23.070", HeightMm = 35.0, WidthMm = 40.0, SlotWidthMm = 11.0, WallThicknessMm = 3.00, MaxLoadKg = 40, MaterialId = materials["SS316"].Id },
            ["24.070"] = new() { ClientId = client.Id, SeriesCode = "24.070", HeightMm = 43.5, WidthMm = 48.5, SlotWidthMm = 15.0, WallThicknessMm = 3.20, MaxLoadKg = 80, MaterialId = materials["SS316"].Id },
            ["25.070"] = new() { ClientId = client.Id, SeriesCode = "25.070", HeightMm = 60.0, WidthMm = 65.0, SlotWidthMm = 18.0, WallThicknessMm = 4.00, MaxLoadKg = 200, MaterialId = materials["SS316"].Id },
        };
        context.ProfileSeries.AddRange(profileSeries.Values);
        await context.SaveChangesAsync();

        // Seed Track Profiles
        await SeedTrackProfilesAsync(context, client, profileSeries, materials);

        // Seed Track Bends
        await SeedTrackBendsAsync(context, client, profileSeries, materials, categories);

        // Seed Brackets
        await SeedBracketsAsync(context, client, profileSeries, materials, categories);

        // Seed Trolleys
        await SeedTrolleysAsync(context, client, profileSeries, materials, categories);

        // Seed Flight Bars
        await SeedFlightBarsAsync(context, client, profileSeries, materials, categories);

        // Seed Switches
        await SeedSwitchesAsync(context, client, profileSeries, materials, categories);

        // Seed Turn Table Switches
        await SeedTurnTableSwitchesAsync(context, client, profileSeries, materials, categories);

        // Seed Stoppers
        await SeedStoppersAsync(context, client, profileSeries, materials, categories);

        // Seed Swivel Units
        await SeedSwivelUnitsAsync(context, client, profileSeries, materials, categories);

        // Seed Bridge Interlocks
        await SeedBridgeInterlocksAsync(context, client, profileSeries, materials, categories);

        // Seed Drop Lift Units
        await SeedDropLiftUnitsAsync(context, client, profileSeries, materials, categories);

        // Seed Accessories
        await SeedAccessoriesAsync(context, client, profileSeries, materials, categories);

        // Seed Bearing Options
        await SeedBearingOptionsAsync(context, client, profileSeries, categories);

        // Seed Pneumatic Controls
        await SeedPneumaticControlsAsync(context, client, categories);
    }

    private static async Task SeedTrackProfilesAsync(ProductDbContext context, Client client,
        Dictionary<string, ProfileSeries> series, Dictionary<string, Material> materials)
    {
        var trackProfiles = new List<TrackProfile>
        {
            // Standard Steel
            new() { ClientId = client.Id, PartNumber = "21.001", SeriesId = series["21.000"].Id, MaterialId = materials["STL-GOLD"].Id },
            new() { ClientId = client.Id, PartNumber = "23.001", SeriesId = series["23.000"].Id, MaterialId = materials["STL-GOLD"].Id },
            new() { ClientId = client.Id, PartNumber = "24.001", SeriesId = series["24.000"].Id, MaterialId = materials["STL-GOLD"].Id },
            new() { ClientId = client.Id, PartNumber = "25.001", SeriesId = series["25.000"].Id, MaterialId = materials["STL-GOLD"].Id },
            new() { ClientId = client.Id, PartNumber = "26.001", SeriesId = series["26.000"].Id, MaterialId = materials["STL-GOLD"].Id },
            new() { ClientId = client.Id, PartNumber = "27.001", SeriesId = series["27.000"].Id, MaterialId = materials["STL-GOLD"].Id },
            // Stainless Steel 304
            new() { ClientId = client.Id, PartNumber = "21.051", SeriesId = series["21.050"].Id, MaterialId = materials["SS304"].Id },
            new() { ClientId = client.Id, PartNumber = "23.051", SeriesId = series["23.050"].Id, MaterialId = materials["SS304"].Id },
            new() { ClientId = client.Id, PartNumber = "24.051", SeriesId = series["24.050"].Id, MaterialId = materials["SS304"].Id },
            new() { ClientId = client.Id, PartNumber = "25.051", SeriesId = series["25.050"].Id, MaterialId = materials["SS304"].Id },
            // Stainless Steel 316
            new() { ClientId = client.Id, PartNumber = "21.071", SeriesId = series["21.070"].Id, MaterialId = materials["SS316"].Id },
            new() { ClientId = client.Id, PartNumber = "23.071", SeriesId = series["23.070"].Id, MaterialId = materials["SS316"].Id },
            new() { ClientId = client.Id, PartNumber = "24.071", SeriesId = series["24.070"].Id, MaterialId = materials["SS316"].Id },
            new() { ClientId = client.Id, PartNumber = "25.071", SeriesId = series["25.070"].Id, MaterialId = materials["SS316"].Id },
        };
        context.TrackProfiles.AddRange(trackProfiles);
        await context.SaveChangesAsync();
    }

    private static async Task SeedTrackBendsAsync(ProductDbContext context, Client client,
        Dictionary<string, ProfileSeries> series, Dictionary<string, Material> materials, Dictionary<string, Category> categories)
    {
        var bends = new List<TrackBend>
        {
            // 90 Degree Standard Radius
            new() { ClientId = client.Id, PartNumber = "21.C06", SeriesId = series["21.000"].Id, MaterialId = materials["STL-GOLD"].Id, AngleDegrees = 90, RadiusMm = 650, DimensionA_Mm = 490, TotalLengthMm = 2000, CategoryId = categories["BENDS"].Id },
            new() { ClientId = client.Id, PartNumber = "23.C07", SeriesId = series["23.000"].Id, MaterialId = materials["STL-GOLD"].Id, AngleDegrees = 90, RadiusMm = 690, DimensionA_Mm = 460, TotalLengthMm = 2000, CategoryId = categories["BENDS"].Id },
            new() { ClientId = client.Id, PartNumber = "24.C06", SeriesId = series["24.000"].Id, MaterialId = materials["STL-GOLD"].Id, AngleDegrees = 90, RadiusMm = 580, DimensionA_Mm = 550, TotalLengthMm = 2000, CategoryId = categories["BENDS"].Id },
            new() { ClientId = client.Id, PartNumber = "25.C06", SeriesId = series["25.000"].Id, MaterialId = materials["STL-GOLD"].Id, AngleDegrees = 90, RadiusMm = 580, DimensionA_Mm = 550, TotalLengthMm = 2000, CategoryId = categories["BENDS"].Id },
            new() { ClientId = client.Id, PartNumber = "26.C08", SeriesId = series["26.000"].Id, MaterialId = materials["STL-GOLD"].Id, AngleDegrees = 90, RadiusMm = 770, DimensionA_Mm = 900, TotalLengthMm = 3000, CategoryId = categories["BENDS"].Id },
            new() { ClientId = client.Id, PartNumber = "27.C10", SeriesId = series["27.000"].Id, MaterialId = materials["STL-GOLD"].Id, AngleDegrees = 90, RadiusMm = 1035, DimensionA_Mm = 690, TotalLengthMm = 3000, CategoryId = categories["BENDS"].Id },
            // 90 Degree Small Radius
            new() { ClientId = client.Id, PartNumber = "21.C02", SeriesId = series["21.000"].Id, MaterialId = materials["STL-GOLD"].Id, AngleDegrees = 90, RadiusMm = 205, DimensionA_Mm = 590, TotalLengthMm = 1500, CategoryId = categories["BENDS"].Id },
            new() { ClientId = client.Id, PartNumber = "21.C03", SeriesId = series["21.000"].Id, MaterialId = materials["STL-GOLD"].Id, AngleDegrees = 90, RadiusMm = 300, DimensionA_Mm = 510, TotalLengthMm = 1500, CategoryId = categories["BENDS"].Id },
            new() { ClientId = client.Id, PartNumber = "21.C04", SeriesId = series["21.000"].Id, MaterialId = materials["STL-GOLD"].Id, AngleDegrees = 90, RadiusMm = 400, DimensionA_Mm = 690, TotalLengthMm = 2000, CategoryId = categories["BENDS"].Id },
            new() { ClientId = client.Id, PartNumber = "23.C04", SeriesId = series["23.000"].Id, MaterialId = materials["STL-GOLD"].Id, AngleDegrees = 90, RadiusMm = 400, DimensionA_Mm = 690, TotalLengthMm = 2000, CategoryId = categories["BENDS"].Id },
            new() { ClientId = client.Id, PartNumber = "24.C04", SeriesId = series["24.000"].Id, MaterialId = materials["STL-GOLD"].Id, AngleDegrees = 90, RadiusMm = 440, DimensionA_Mm = 650, TotalLengthMm = 2000, CategoryId = categories["BENDS"].Id },
            // 60 Degree
            new() { ClientId = client.Id, PartNumber = "21.C12", SeriesId = series["21.000"].Id, MaterialId = materials["STL-GOLD"].Id, AngleDegrees = 60, RadiusMm = 205, DimensionA_Mm = 640, TotalLengthMm = 1500, CategoryId = categories["BENDS"].Id },
            new() { ClientId = client.Id, PartNumber = "21.C16", SeriesId = series["21.000"].Id, MaterialId = materials["STL-GOLD"].Id, AngleDegrees = 60, RadiusMm = 650, DimensionA_Mm = 660, TotalLengthMm = 2000, CategoryId = categories["BENDS"].Id },
            new() { ClientId = client.Id, PartNumber = "23.C17", SeriesId = series["23.000"].Id, MaterialId = materials["STL-GOLD"].Id, AngleDegrees = 60, RadiusMm = 690, DimensionA_Mm = 640, TotalLengthMm = 2000, CategoryId = categories["BENDS"].Id },
            new() { ClientId = client.Id, PartNumber = "24.C16", SeriesId = series["24.000"].Id, MaterialId = materials["STL-GOLD"].Id, AngleDegrees = 60, RadiusMm = 580, DimensionA_Mm = 700, TotalLengthMm = 2000, CategoryId = categories["BENDS"].Id },
            new() { ClientId = client.Id, PartNumber = "25.C16", SeriesId = series["25.000"].Id, MaterialId = materials["STL-GOLD"].Id, AngleDegrees = 60, RadiusMm = 580, DimensionA_Mm = 700, TotalLengthMm = 2000, CategoryId = categories["BENDS"].Id },
            // 45 Degree
            new() { ClientId = client.Id, PartNumber = "21.C22", SeriesId = series["21.000"].Id, MaterialId = materials["STL-GOLD"].Id, AngleDegrees = 45, RadiusMm = 205, DimensionA_Mm = 670, TotalLengthMm = 1500, CategoryId = categories["BENDS"].Id },
            new() { ClientId = client.Id, PartNumber = "21.C26", SeriesId = series["21.000"].Id, MaterialId = materials["STL-GOLD"].Id, AngleDegrees = 45, RadiusMm = 650, DimensionA_Mm = 730, TotalLengthMm = 2000, CategoryId = categories["BENDS"].Id },
            new() { ClientId = client.Id, PartNumber = "23.C27", SeriesId = series["23.000"].Id, MaterialId = materials["STL-GOLD"].Id, AngleDegrees = 45, RadiusMm = 690, DimensionA_Mm = 700, TotalLengthMm = 2000, CategoryId = categories["BENDS"].Id },
            new() { ClientId = client.Id, PartNumber = "24.C26", SeriesId = series["24.000"].Id, MaterialId = materials["STL-GOLD"].Id, AngleDegrees = 45, RadiusMm = 580, DimensionA_Mm = 760, TotalLengthMm = 2000, CategoryId = categories["BENDS"].Id },
            // 30 Degree
            new() { ClientId = client.Id, PartNumber = "21.C36", SeriesId = series["21.000"].Id, MaterialId = materials["STL-GOLD"].Id, AngleDegrees = 30, RadiusMm = 650, DimensionA_Mm = 790, TotalLengthMm = 2000, CategoryId = categories["BENDS"].Id },
            new() { ClientId = client.Id, PartNumber = "23.C37", SeriesId = series["23.000"].Id, MaterialId = materials["STL-GOLD"].Id, AngleDegrees = 30, RadiusMm = 690, DimensionA_Mm = 760, TotalLengthMm = 2000, CategoryId = categories["BENDS"].Id },
            // Vertical Bends
            new() { ClientId = client.Id, PartNumber = "21.V12", SeriesId = series["21.000"].Id, MaterialId = materials["STL-GOLD"].Id, AngleDegrees = 12, RadiusMm = 1200, TotalLengthMm = 1500, CategoryId = categories["BENDS"].Id },
            new() { ClientId = client.Id, PartNumber = "23.V12", SeriesId = series["23.000"].Id, MaterialId = materials["STL-GOLD"].Id, AngleDegrees = 12, RadiusMm = 1500, TotalLengthMm = 1500, CategoryId = categories["BENDS"].Id },
            new() { ClientId = client.Id, PartNumber = "24.V12", SeriesId = series["24.000"].Id, MaterialId = materials["STL-GOLD"].Id, AngleDegrees = 12, RadiusMm = 2000, TotalLengthMm = 2000, CategoryId = categories["BENDS"].Id },
        };
        context.TrackBends.AddRange(bends);
        await context.SaveChangesAsync();
    }

    private static async Task SeedBracketsAsync(ProductDbContext context, Client client,
        Dictionary<string, ProfileSeries> series, Dictionary<string, Material> materials, Dictionary<string, Category> categories)
    {
        var brackets = new List<Bracket>
        {
            // Wall Brackets Type A
            new() { ClientId = client.Id, PartNumber = "21.101", SeriesId = series["21.000"].Id, MaterialId = materials["STL-GOLD"].Id, BracketType = "Wall Bracket Type A", LengthMm = 100, HeightMm = 30, WidthMm = 45, WallThicknessMm = 3.0, CategoryId = categories["BRACKETS"].Id },
            new() { ClientId = client.Id, PartNumber = "23.101", SeriesId = series["23.000"].Id, MaterialId = materials["STL-GOLD"].Id, BracketType = "Wall Bracket Type A", LengthMm = 100, HeightMm = 40, WidthMm = 55, WallThicknessMm = 4.0, CategoryId = categories["BRACKETS"].Id },
            new() { ClientId = client.Id, PartNumber = "24.101", SeriesId = series["24.000"].Id, MaterialId = materials["STL-GOLD"].Id, BracketType = "Wall Bracket Type A", LengthMm = 120, HeightMm = 50, WidthMm = 65, WallThicknessMm = 5.0, CategoryId = categories["BRACKETS"].Id },
            new() { ClientId = client.Id, PartNumber = "25.101", SeriesId = series["25.000"].Id, MaterialId = materials["STL-GOLD"].Id, BracketType = "Wall Bracket Type A", LengthMm = 150, HeightMm = 65, WidthMm = 80, WallThicknessMm = 6.0, CategoryId = categories["BRACKETS"].Id },
            new() { ClientId = client.Id, PartNumber = "26.101", SeriesId = series["26.000"].Id, MaterialId = materials["STL-GOLD"].Id, BracketType = "Wall Bracket Type A", LengthMm = 180, HeightMm = 80, WidthMm = 95, WallThicknessMm = 6.0, CategoryId = categories["BRACKETS"].Id },
            new() { ClientId = client.Id, PartNumber = "27.101", SeriesId = series["27.000"].Id, MaterialId = materials["STL-GOLD"].Id, BracketType = "Wall Bracket Type A", LengthMm = 220, HeightMm = 110, WidthMm = 110, WallThicknessMm = 8.0, CategoryId = categories["BRACKETS"].Id },
            // Splice Joints
            new() { ClientId = client.Id, PartNumber = "21.110", SeriesId = series["21.000"].Id, MaterialId = materials["STL-GOLD"].Id, BracketType = "Splice Joint", LengthMm = 120, CategoryId = categories["BRACKETS"].Id },
            new() { ClientId = client.Id, PartNumber = "23.110", SeriesId = series["23.000"].Id, MaterialId = materials["STL-GOLD"].Id, BracketType = "Splice Joint", LengthMm = 150, CategoryId = categories["BRACKETS"].Id },
            new() { ClientId = client.Id, PartNumber = "24.110", SeriesId = series["24.000"].Id, MaterialId = materials["STL-GOLD"].Id, BracketType = "Splice Joint", LengthMm = 180, CategoryId = categories["BRACKETS"].Id },
            new() { ClientId = client.Id, PartNumber = "25.110", SeriesId = series["25.000"].Id, MaterialId = materials["STL-GOLD"].Id, BracketType = "Splice Joint", LengthMm = 200, CategoryId = categories["BRACKETS"].Id },
            new() { ClientId = client.Id, PartNumber = "26.110", SeriesId = series["26.000"].Id, MaterialId = materials["STL-GOLD"].Id, BracketType = "Splice Joint", LengthMm = 250, CategoryId = categories["BRACKETS"].Id },
            new() { ClientId = client.Id, PartNumber = "27.110", SeriesId = series["27.000"].Id, MaterialId = materials["STL-GOLD"].Id, BracketType = "Splice Joint", LengthMm = 300, CategoryId = categories["BRACKETS"].Id },
            // Suspension Brackets
            new() { ClientId = client.Id, PartNumber = "21.120", SeriesId = series["21.000"].Id, MaterialId = materials["STL-GOLD"].Id, BracketType = "Suspension Bracket", HoleDiameterMm = 11, CategoryId = categories["BRACKETS"].Id },
            new() { ClientId = client.Id, PartNumber = "23.120", SeriesId = series["23.000"].Id, MaterialId = materials["STL-GOLD"].Id, BracketType = "Suspension Bracket", HoleDiameterMm = 14, CategoryId = categories["BRACKETS"].Id },
            new() { ClientId = client.Id, PartNumber = "24.120", SeriesId = series["24.000"].Id, MaterialId = materials["STL-GOLD"].Id, BracketType = "Suspension Bracket", HoleDiameterMm = 18, CategoryId = categories["BRACKETS"].Id },
            new() { ClientId = client.Id, PartNumber = "25.120", SeriesId = series["25.000"].Id, MaterialId = materials["STL-GOLD"].Id, BracketType = "Suspension Bracket", HoleDiameterMm = 22, CategoryId = categories["BRACKETS"].Id },
            // Adjustable Suspension
            new() { ClientId = client.Id, PartNumber = "21.130", SeriesId = series["21.000"].Id, MaterialId = materials["STL-GOLD"].Id, BracketType = "Adjustable Suspension", MaxAdjustmentMm = 100, ThreadSize = "M10", CategoryId = categories["BRACKETS"].Id },
            new() { ClientId = client.Id, PartNumber = "23.130", SeriesId = series["23.000"].Id, MaterialId = materials["STL-GOLD"].Id, BracketType = "Adjustable Suspension", MaxAdjustmentMm = 150, ThreadSize = "M12", CategoryId = categories["BRACKETS"].Id },
            new() { ClientId = client.Id, PartNumber = "24.130", SeriesId = series["24.000"].Id, MaterialId = materials["STL-GOLD"].Id, BracketType = "Adjustable Suspension", MaxAdjustmentMm = 200, ThreadSize = "M16", CategoryId = categories["BRACKETS"].Id },
            new() { ClientId = client.Id, PartNumber = "25.130", SeriesId = series["25.000"].Id, MaterialId = materials["STL-GOLD"].Id, BracketType = "Adjustable Suspension", MaxAdjustmentMm = 250, ThreadSize = "M20", CategoryId = categories["BRACKETS"].Id },
        };
        context.Brackets.AddRange(brackets);
        await context.SaveChangesAsync();
    }

    private static async Task SeedTrolleysAsync(ProductDbContext context, Client client,
        Dictionary<string, ProfileSeries> series, Dictionary<string, Material> materials, Dictionary<string, Category> categories)
    {
        var trolleys = new List<Trolley>
        {
            // 2-Wheel Trolleys
            new() { ClientId = client.Id, PartNumber = "21.201", SeriesId = series["21.000"].Id, MaterialId = materials["STL-GOLD"].Id, TrolleyType = "2-Wheel Standard", WheelCount = 2, SafeWorkingLoadKg = 20, LengthMm = 60, HeightMm = 45, WidthMm = 30, CategoryId = categories["TROLLEYS"].Id },
            new() { ClientId = client.Id, PartNumber = "23.201", SeriesId = series["23.000"].Id, MaterialId = materials["STL-GOLD"].Id, TrolleyType = "2-Wheel Standard", WheelCount = 2, SafeWorkingLoadKg = 40, LengthMm = 75, HeightMm = 55, WidthMm = 40, CategoryId = categories["TROLLEYS"].Id },
            new() { ClientId = client.Id, PartNumber = "24.201", SeriesId = series["24.000"].Id, MaterialId = materials["STL-GOLD"].Id, TrolleyType = "2-Wheel Standard", WheelCount = 2, SafeWorkingLoadKg = 80, LengthMm = 90, HeightMm = 68, WidthMm = 48, CategoryId = categories["TROLLEYS"].Id },
            new() { ClientId = client.Id, PartNumber = "25.201", SeriesId = series["25.000"].Id, MaterialId = materials["STL-GOLD"].Id, TrolleyType = "2-Wheel Standard", WheelCount = 2, SafeWorkingLoadKg = 200, LengthMm = 115, HeightMm = 90, WidthMm = 65, CategoryId = categories["TROLLEYS"].Id },
            new() { ClientId = client.Id, PartNumber = "26.201", SeriesId = series["26.000"].Id, MaterialId = materials["STL-GOLD"].Id, TrolleyType = "2-Wheel Standard", WheelCount = 2, SafeWorkingLoadKg = 400, LengthMm = 140, HeightMm = 110, WidthMm = 80, CategoryId = categories["TROLLEYS"].Id },
            new() { ClientId = client.Id, PartNumber = "27.201", SeriesId = series["27.000"].Id, MaterialId = materials["STL-GOLD"].Id, TrolleyType = "2-Wheel Standard", WheelCount = 2, SafeWorkingLoadKg = 800, LengthMm = 180, HeightMm = 145, WidthMm = 90, CategoryId = categories["TROLLEYS"].Id },
            // 4-Wheel Trolleys
            new() { ClientId = client.Id, PartNumber = "21.401", SeriesId = series["21.000"].Id, MaterialId = materials["STL-GOLD"].Id, TrolleyType = "4-Wheel Standard", WheelCount = 4, SafeWorkingLoadKg = 20, LengthMm = 120, HeightMm = 45, WidthMm = 30, CategoryId = categories["TROLLEYS"].Id },
            new() { ClientId = client.Id, PartNumber = "23.401", SeriesId = series["23.000"].Id, MaterialId = materials["STL-GOLD"].Id, TrolleyType = "4-Wheel Standard", WheelCount = 4, SafeWorkingLoadKg = 40, LengthMm = 150, HeightMm = 55, WidthMm = 40, CategoryId = categories["TROLLEYS"].Id },
            new() { ClientId = client.Id, PartNumber = "24.401", SeriesId = series["24.000"].Id, MaterialId = materials["STL-GOLD"].Id, TrolleyType = "4-Wheel Standard", WheelCount = 4, SafeWorkingLoadKg = 80, LengthMm = 180, HeightMm = 68, WidthMm = 48, CategoryId = categories["TROLLEYS"].Id },
            new() { ClientId = client.Id, PartNumber = "25.401", SeriesId = series["25.000"].Id, MaterialId = materials["STL-GOLD"].Id, TrolleyType = "4-Wheel Standard", WheelCount = 4, SafeWorkingLoadKg = 200, LengthMm = 230, HeightMm = 90, WidthMm = 65, CategoryId = categories["TROLLEYS"].Id },
            new() { ClientId = client.Id, PartNumber = "26.401", SeriesId = series["26.000"].Id, MaterialId = materials["STL-GOLD"].Id, TrolleyType = "4-Wheel Standard", WheelCount = 4, SafeWorkingLoadKg = 400, LengthMm = 280, HeightMm = 110, WidthMm = 80, CategoryId = categories["TROLLEYS"].Id },
            new() { ClientId = client.Id, PartNumber = "27.401", SeriesId = series["27.000"].Id, MaterialId = materials["STL-GOLD"].Id, TrolleyType = "4-Wheel Standard", WheelCount = 4, SafeWorkingLoadKg = 800, LengthMm = 360, HeightMm = 145, WidthMm = 90, CategoryId = categories["TROLLEYS"].Id },
            // Trolleys with Rotating Screw
            new() { ClientId = client.Id, PartNumber = "23.211", SeriesId = series["23.000"].Id, MaterialId = materials["STL-GOLD"].Id, TrolleyType = "2-Wheel Rotating Screw", WheelCount = 2, SafeWorkingLoadKg = 40, ThreadSize = "M10", HasRotatingScrew = true, CategoryId = categories["TROLLEYS"].Id },
            new() { ClientId = client.Id, PartNumber = "24.211", SeriesId = series["24.000"].Id, MaterialId = materials["STL-GOLD"].Id, TrolleyType = "2-Wheel Rotating Screw", WheelCount = 2, SafeWorkingLoadKg = 80, ThreadSize = "M12", HasRotatingScrew = true, CategoryId = categories["TROLLEYS"].Id },
            new() { ClientId = client.Id, PartNumber = "25.211", SeriesId = series["25.000"].Id, MaterialId = materials["STL-GOLD"].Id, TrolleyType = "2-Wheel Rotating Screw", WheelCount = 2, SafeWorkingLoadKg = 200, ThreadSize = "M16", HasRotatingScrew = true, CategoryId = categories["TROLLEYS"].Id },
            // Trolleys with Guide Rollers
            new() { ClientId = client.Id, PartNumber = "24.215", SeriesId = series["24.000"].Id, MaterialId = materials["STL-GOLD"].Id, TrolleyType = "2-Wheel Guide Rollers", WheelCount = 2, SafeWorkingLoadKg = 80, HasGuideRollers = true, CategoryId = categories["TROLLEYS"].Id },
            new() { ClientId = client.Id, PartNumber = "25.215", SeriesId = series["25.000"].Id, MaterialId = materials["STL-GOLD"].Id, TrolleyType = "2-Wheel Guide Rollers", WheelCount = 2, SafeWorkingLoadKg = 200, HasGuideRollers = true, CategoryId = categories["TROLLEYS"].Id },
            new() { ClientId = client.Id, PartNumber = "26.215", SeriesId = series["26.000"].Id, MaterialId = materials["STL-GOLD"].Id, TrolleyType = "2-Wheel Guide Rollers", WheelCount = 2, SafeWorkingLoadKg = 400, HasGuideRollers = true, CategoryId = categories["TROLLEYS"].Id },
        };
        context.Trolleys.AddRange(trolleys);
        await context.SaveChangesAsync();
    }

    private static async Task SeedFlightBarsAsync(ProductDbContext context, Client client,
        Dictionary<string, ProfileSeries> series, Dictionary<string, Material> materials, Dictionary<string, Category> categories)
    {
        var flightBars = new List<FlightBar>
        {
            // Standard Flight Bars
            new() { ClientId = client.Id, PartNumber = "23.FB1000", SeriesId = series["23.000"].Id, MaterialId = materials["STL-GOLD"].Id, FlightBarType = "Standard", SafeWorkingLoadKg = 40, LengthMm = 1000, SpanMm = 900, HeightMm = 40, EyeNutDiameterMm = 10, HasRotatingEyeNut = true, CategoryId = categories["FLIGHTBARS"].Id },
            new() { ClientId = client.Id, PartNumber = "24.FB1000", SeriesId = series["24.000"].Id, MaterialId = materials["STL-GOLD"].Id, FlightBarType = "Standard", SafeWorkingLoadKg = 80, LengthMm = 1000, SpanMm = 900, HeightMm = 50, EyeNutDiameterMm = 12, HasRotatingEyeNut = true, CategoryId = categories["FLIGHTBARS"].Id },
            new() { ClientId = client.Id, PartNumber = "25.FB1000", SeriesId = series["25.000"].Id, MaterialId = materials["STL-GOLD"].Id, FlightBarType = "Standard", SafeWorkingLoadKg = 200, LengthMm = 1000, SpanMm = 900, HeightMm = 65, EyeNutDiameterMm = 16, HasRotatingEyeNut = true, CategoryId = categories["FLIGHTBARS"].Id },
            new() { ClientId = client.Id, PartNumber = "26.FB1200", SeriesId = series["26.000"].Id, MaterialId = materials["STL-GOLD"].Id, FlightBarType = "Standard", SafeWorkingLoadKg = 400, LengthMm = 1200, SpanMm = 1100, HeightMm = 80, EyeNutDiameterMm = 20, HasRotatingEyeNut = true, CategoryId = categories["FLIGHTBARS"].Id },
            new() { ClientId = client.Id, PartNumber = "27.FB1500", SeriesId = series["27.000"].Id, MaterialId = materials["STL-GOLD"].Id, FlightBarType = "Standard", SafeWorkingLoadKg = 800, LengthMm = 1500, SpanMm = 1400, HeightMm = 110, EyeNutDiameterMm = 24, HasRotatingEyeNut = true, CategoryId = categories["FLIGHTBARS"].Id },
            // Heavy Duty Flight Bars
            new() { ClientId = client.Id, PartNumber = "25.FBHD1200", SeriesId = series["25.000"].Id, MaterialId = materials["STL-GOLD"].Id, FlightBarType = "Heavy Duty", SafeWorkingLoadKg = 250, LengthMm = 1200, SpanMm = 1100, HeightMm = 80, CategoryId = categories["FLIGHTBARS"].Id },
            new() { ClientId = client.Id, PartNumber = "26.FBHD1500", SeriesId = series["26.000"].Id, MaterialId = materials["STL-GOLD"].Id, FlightBarType = "Heavy Duty", SafeWorkingLoadKg = 500, LengthMm = 1500, SpanMm = 1400, HeightMm = 100, CategoryId = categories["FLIGHTBARS"].Id },
            new() { ClientId = client.Id, PartNumber = "27.FBHD2000", SeriesId = series["27.000"].Id, MaterialId = materials["STL-GOLD"].Id, FlightBarType = "Heavy Duty", SafeWorkingLoadKg = 1000, LengthMm = 2000, SpanMm = 1900, HeightMm = 130, CategoryId = categories["FLIGHTBARS"].Id },
        };
        context.FlightBars.AddRange(flightBars);
        await context.SaveChangesAsync();
    }

    private static async Task SeedSwitchesAsync(ProductDbContext context, Client client,
        Dictionary<string, ProfileSeries> series, Dictionary<string, Material> materials, Dictionary<string, Category> categories)
    {
        var switches = new List<Switch>
        {
            // Manual Tongue Switches
            new() { ClientId = client.Id, PartNumber = "23.SW01", SeriesId = series["23.000"].Id, MaterialId = materials["STL-GOLD"].Id, SwitchType = "Manual Tongue", RadiusMm = 400, WidthMm = 800, LengthMm = 1200, AngleDegrees = 30, IsAutomatic = false, CategoryId = categories["SWITCHES"].Id },
            new() { ClientId = client.Id, PartNumber = "24.SW01", SeriesId = series["24.000"].Id, MaterialId = materials["STL-GOLD"].Id, SwitchType = "Manual Tongue", RadiusMm = 440, WidthMm = 900, LengthMm = 1400, AngleDegrees = 30, IsAutomatic = false, CategoryId = categories["SWITCHES"].Id },
            new() { ClientId = client.Id, PartNumber = "25.SW01", SeriesId = series["25.000"].Id, MaterialId = materials["STL-GOLD"].Id, SwitchType = "Manual Tongue", RadiusMm = 580, WidthMm = 1000, LengthMm = 1600, AngleDegrees = 30, IsAutomatic = false, CategoryId = categories["SWITCHES"].Id },
            new() { ClientId = client.Id, PartNumber = "26.SW01", SeriesId = series["26.000"].Id, MaterialId = materials["STL-GOLD"].Id, SwitchType = "Manual Tongue", RadiusMm = 770, WidthMm = 1200, LengthMm = 1800, AngleDegrees = 30, IsAutomatic = false, CategoryId = categories["SWITCHES"].Id },
            // Automatic Tongue Switches
            new() { ClientId = client.Id, PartNumber = "23.SW02", SeriesId = series["23.000"].Id, MaterialId = materials["STL-GOLD"].Id, SwitchType = "Automatic Tongue", RadiusMm = 400, WidthMm = 800, LengthMm = 1200, AngleDegrees = 30, IsAutomatic = true, CategoryId = categories["SWITCHES"].Id },
            new() { ClientId = client.Id, PartNumber = "24.SW02", SeriesId = series["24.000"].Id, MaterialId = materials["STL-GOLD"].Id, SwitchType = "Automatic Tongue", RadiusMm = 440, WidthMm = 900, LengthMm = 1400, AngleDegrees = 30, IsAutomatic = true, CategoryId = categories["SWITCHES"].Id },
            new() { ClientId = client.Id, PartNumber = "25.SW02", SeriesId = series["25.000"].Id, MaterialId = materials["STL-GOLD"].Id, SwitchType = "Automatic Tongue", RadiusMm = 580, WidthMm = 1000, LengthMm = 1600, AngleDegrees = 30, IsAutomatic = true, CategoryId = categories["SWITCHES"].Id },
            // Pneumatic Tongue Switches
            new() { ClientId = client.Id, PartNumber = "23.SW03", SeriesId = series["23.000"].Id, MaterialId = materials["STL-GOLD"].Id, SwitchType = "Pneumatic Tongue", RadiusMm = 400, WidthMm = 800, LengthMm = 1200, IsPneumatic = true, IncludesCylinder = true, CategoryId = categories["SWITCHES"].Id },
            new() { ClientId = client.Id, PartNumber = "24.SW03", SeriesId = series["24.000"].Id, MaterialId = materials["STL-GOLD"].Id, SwitchType = "Pneumatic Tongue", RadiusMm = 440, WidthMm = 900, LengthMm = 1400, IsPneumatic = true, IncludesCylinder = true, CategoryId = categories["SWITCHES"].Id },
            new() { ClientId = client.Id, PartNumber = "25.SW03", SeriesId = series["25.000"].Id, MaterialId = materials["STL-GOLD"].Id, SwitchType = "Pneumatic Tongue", RadiusMm = 580, WidthMm = 1000, LengthMm = 1600, IsPneumatic = true, IncludesCylinder = true, CategoryId = categories["SWITCHES"].Id },
            new() { ClientId = client.Id, PartNumber = "26.SW03", SeriesId = series["26.000"].Id, MaterialId = materials["STL-GOLD"].Id, SwitchType = "Pneumatic Tongue", RadiusMm = 770, WidthMm = 1200, LengthMm = 1800, IsPneumatic = true, IncludesCylinder = true, CategoryId = categories["SWITCHES"].Id },
        };
        context.Switches.AddRange(switches);
        await context.SaveChangesAsync();
    }

    private static async Task SeedTurnTableSwitchesAsync(ProductDbContext context, Client client,
        Dictionary<string, ProfileSeries> series, Dictionary<string, Material> materials, Dictionary<string, Category> categories)
    {
        var turnTables = new List<TurnTableSwitch>
        {
            // Manual Turn Tables 90 Degree
            new() { ClientId = client.Id, PartNumber = "23.TT90M", SeriesId = series["23.000"].Id, MaterialId = materials["STL-GOLD"].Id, SwitchType = "Turn Table 90", DimensionA_Mm = 600, DimensionB_Mm = 600, OperationType = "Manual", SupportPoints = 8, CategoryId = categories["SWITCHES"].Id },
            new() { ClientId = client.Id, PartNumber = "24.TT90M", SeriesId = series["24.000"].Id, MaterialId = materials["STL-GOLD"].Id, SwitchType = "Turn Table 90", DimensionA_Mm = 700, DimensionB_Mm = 700, OperationType = "Manual", SupportPoints = 8, CategoryId = categories["SWITCHES"].Id },
            new() { ClientId = client.Id, PartNumber = "25.TT90M", SeriesId = series["25.000"].Id, MaterialId = materials["STL-GOLD"].Id, SwitchType = "Turn Table 90", DimensionA_Mm = 800, DimensionB_Mm = 800, OperationType = "Manual", SupportPoints = 8, CategoryId = categories["SWITCHES"].Id },
            // Pneumatic Turn Tables 90 Degree
            new() { ClientId = client.Id, PartNumber = "23.TT90P", SeriesId = series["23.000"].Id, MaterialId = materials["STL-GOLD"].Id, SwitchType = "Turn Table 90", DimensionA_Mm = 600, DimensionB_Mm = 600, OperationType = "Pneumatic", SupportPoints = 8, CategoryId = categories["SWITCHES"].Id },
            new() { ClientId = client.Id, PartNumber = "24.TT90P", SeriesId = series["24.000"].Id, MaterialId = materials["STL-GOLD"].Id, SwitchType = "Turn Table 90", DimensionA_Mm = 700, DimensionB_Mm = 700, OperationType = "Pneumatic", SupportPoints = 8, CategoryId = categories["SWITCHES"].Id },
            new() { ClientId = client.Id, PartNumber = "25.TT90P", SeriesId = series["25.000"].Id, MaterialId = materials["STL-GOLD"].Id, SwitchType = "Turn Table 90", DimensionA_Mm = 800, DimensionB_Mm = 800, OperationType = "Pneumatic", SupportPoints = 8, CategoryId = categories["SWITCHES"].Id },
            // 180 Degree Turn Tables
            new() { ClientId = client.Id, PartNumber = "23.TT180", SeriesId = series["23.000"].Id, MaterialId = materials["STL-GOLD"].Id, SwitchType = "Turn Table 180", DimensionA_Mm = 800, DimensionB_Mm = 400, OperationType = "Manual", SupportPoints = 4, CategoryId = categories["SWITCHES"].Id },
            new() { ClientId = client.Id, PartNumber = "24.TT180", SeriesId = series["24.000"].Id, MaterialId = materials["STL-GOLD"].Id, SwitchType = "Turn Table 180", DimensionA_Mm = 900, DimensionB_Mm = 450, OperationType = "Manual", SupportPoints = 4, CategoryId = categories["SWITCHES"].Id },
        };
        context.TurnTableSwitches.AddRange(turnTables);
        await context.SaveChangesAsync();
    }

    private static async Task SeedStoppersAsync(ProductDbContext context, Client client,
        Dictionary<string, ProfileSeries> series, Dictionary<string, Material> materials, Dictionary<string, Category> categories)
    {
        var stoppers = new List<Stopper>
        {
            // Spring Loaded Stoppers
            new() { ClientId = client.Id, PartNumber = "21.ST01", SeriesId = series["21.000"].Id, MaterialId = materials["STL-GOLD"].Id, StopperType = "Spring Loaded", LengthMm = 80, HeightMm = 60, IsSpringLoaded = true, CategoryId = categories["STOPPERS"].Id },
            new() { ClientId = client.Id, PartNumber = "23.ST01", SeriesId = series["23.000"].Id, MaterialId = materials["STL-GOLD"].Id, StopperType = "Spring Loaded", LengthMm = 100, HeightMm = 75, IsSpringLoaded = true, CategoryId = categories["STOPPERS"].Id },
            new() { ClientId = client.Id, PartNumber = "24.ST01", SeriesId = series["24.000"].Id, MaterialId = materials["STL-GOLD"].Id, StopperType = "Spring Loaded", LengthMm = 120, HeightMm = 90, IsSpringLoaded = true, CategoryId = categories["STOPPERS"].Id },
            new() { ClientId = client.Id, PartNumber = "25.ST01", SeriesId = series["25.000"].Id, MaterialId = materials["STL-GOLD"].Id, StopperType = "Spring Loaded", LengthMm = 150, HeightMm = 110, IsSpringLoaded = true, CategoryId = categories["STOPPERS"].Id },
            // Directional Stoppers
            new() { ClientId = client.Id, PartNumber = "23.ST02", SeriesId = series["23.000"].Id, MaterialId = materials["STL-GOLD"].Id, StopperType = "Directional", LengthMm = 100, HeightMm = 75, CategoryId = categories["STOPPERS"].Id },
            new() { ClientId = client.Id, PartNumber = "24.ST02", SeriesId = series["24.000"].Id, MaterialId = materials["STL-GOLD"].Id, StopperType = "Directional", LengthMm = 120, HeightMm = 90, CategoryId = categories["STOPPERS"].Id },
            new() { ClientId = client.Id, PartNumber = "25.ST02", SeriesId = series["25.000"].Id, MaterialId = materials["STL-GOLD"].Id, StopperType = "Directional", LengthMm = 150, HeightMm = 110, CategoryId = categories["STOPPERS"].Id },
            // Pneumatic Stoppers
            new() { ClientId = client.Id, PartNumber = "24.ST03", SeriesId = series["24.000"].Id, MaterialId = materials["STL-GOLD"].Id, StopperType = "Pneumatic", IsPneumatic = true, CategoryId = categories["STOPPERS"].Id },
            new() { ClientId = client.Id, PartNumber = "25.ST03", SeriesId = series["25.000"].Id, MaterialId = materials["STL-GOLD"].Id, StopperType = "Pneumatic", IsPneumatic = true, CategoryId = categories["STOPPERS"].Id },
            new() { ClientId = client.Id, PartNumber = "26.ST03", SeriesId = series["26.000"].Id, MaterialId = materials["STL-GOLD"].Id, StopperType = "Pneumatic", IsPneumatic = true, CategoryId = categories["STOPPERS"].Id },
        };
        context.Stoppers.AddRange(stoppers);
        await context.SaveChangesAsync();
    }

    private static async Task SeedSwivelUnitsAsync(ProductDbContext context, Client client,
        Dictionary<string, ProfileSeries> series, Dictionary<string, Material> materials, Dictionary<string, Category> categories)
    {
        var swivelUnits = new List<SwivelUnit>
        {
            // Manual Swivel Units Left
            new() { ClientId = client.Id, PartNumber = "23.SV01L", SeriesId = series["23.000"].Id, MaterialId = materials["STL-GOLD"].Id, SwivelType = "Manual", Direction = "Left", L1_Mm = 200, L2_Mm = 300, L3_Mm = 200, CategoryId = categories["SWIVEL"].Id },
            new() { ClientId = client.Id, PartNumber = "24.SV01L", SeriesId = series["24.000"].Id, MaterialId = materials["STL-GOLD"].Id, SwivelType = "Manual", Direction = "Left", L1_Mm = 250, L2_Mm = 350, L3_Mm = 250, CategoryId = categories["SWIVEL"].Id },
            new() { ClientId = client.Id, PartNumber = "25.SV01L", SeriesId = series["25.000"].Id, MaterialId = materials["STL-GOLD"].Id, SwivelType = "Manual", Direction = "Left", L1_Mm = 300, L2_Mm = 400, L3_Mm = 300, CategoryId = categories["SWIVEL"].Id },
            // Manual Swivel Units Right
            new() { ClientId = client.Id, PartNumber = "23.SV01R", SeriesId = series["23.000"].Id, MaterialId = materials["STL-GOLD"].Id, SwivelType = "Manual", Direction = "Right", L1_Mm = 200, L2_Mm = 300, L3_Mm = 200, CategoryId = categories["SWIVEL"].Id },
            new() { ClientId = client.Id, PartNumber = "24.SV01R", SeriesId = series["24.000"].Id, MaterialId = materials["STL-GOLD"].Id, SwivelType = "Manual", Direction = "Right", L1_Mm = 250, L2_Mm = 350, L3_Mm = 250, CategoryId = categories["SWIVEL"].Id },
            new() { ClientId = client.Id, PartNumber = "25.SV01R", SeriesId = series["25.000"].Id, MaterialId = materials["STL-GOLD"].Id, SwivelType = "Manual", Direction = "Right", L1_Mm = 300, L2_Mm = 400, L3_Mm = 300, CategoryId = categories["SWIVEL"].Id },
            // Pneumatic Swivel Units
            new() { ClientId = client.Id, PartNumber = "24.SV02", SeriesId = series["24.000"].Id, MaterialId = materials["STL-GOLD"].Id, SwivelType = "Pneumatic", IsPneumatic = true, CategoryId = categories["SWIVEL"].Id },
            new() { ClientId = client.Id, PartNumber = "25.SV02", SeriesId = series["25.000"].Id, MaterialId = materials["STL-GOLD"].Id, SwivelType = "Pneumatic", IsPneumatic = true, CategoryId = categories["SWIVEL"].Id },
            new() { ClientId = client.Id, PartNumber = "26.SV02", SeriesId = series["26.000"].Id, MaterialId = materials["STL-GOLD"].Id, SwivelType = "Pneumatic", IsPneumatic = true, CategoryId = categories["SWIVEL"].Id },
        };
        context.SwivelUnits.AddRange(swivelUnits);
        await context.SaveChangesAsync();
    }

    private static async Task SeedBridgeInterlocksAsync(ProductDbContext context, Client client,
        Dictionary<string, ProfileSeries> series, Dictionary<string, Material> materials, Dictionary<string, Category> categories)
    {
        var bridgeInterlocks = new List<BridgeInterlock>
        {
            // Bridge Plates
            new() { ClientId = client.Id, PartNumber = "23.BI01", SeriesId = series["23.000"].Id, MaterialId = materials["STL-GOLD"].Id, PartType = "Bridge Plate", WidthMm = 200, HeightMm = 100, LengthMm = 400, CategoryId = categories["INTERLOCK"].Id },
            new() { ClientId = client.Id, PartNumber = "24.BI01", SeriesId = series["24.000"].Id, MaterialId = materials["STL-GOLD"].Id, PartType = "Bridge Plate", WidthMm = 250, HeightMm = 120, LengthMm = 500, CategoryId = categories["INTERLOCK"].Id },
            new() { ClientId = client.Id, PartNumber = "25.BI01", SeriesId = series["25.000"].Id, MaterialId = materials["STL-GOLD"].Id, PartType = "Bridge Plate", WidthMm = 300, HeightMm = 150, LengthMm = 600, CategoryId = categories["INTERLOCK"].Id },
            // Height Adjustable Interlocks
            new() { ClientId = client.Id, PartNumber = "23.BI02", SeriesId = series["23.000"].Id, MaterialId = materials["STL-GOLD"].Id, PartType = "Height Adjustable", WidthMm = 200, MinHeightMm = 100, MaxHeightMm = 200, LengthMm = 400, CategoryId = categories["INTERLOCK"].Id },
            new() { ClientId = client.Id, PartNumber = "24.BI02", SeriesId = series["24.000"].Id, MaterialId = materials["STL-GOLD"].Id, PartType = "Height Adjustable", WidthMm = 250, MinHeightMm = 120, MaxHeightMm = 250, LengthMm = 500, CategoryId = categories["INTERLOCK"].Id },
            new() { ClientId = client.Id, PartNumber = "25.BI02", SeriesId = series["25.000"].Id, MaterialId = materials["STL-GOLD"].Id, PartType = "Height Adjustable", WidthMm = 300, MinHeightMm = 150, MaxHeightMm = 300, LengthMm = 600, CategoryId = categories["INTERLOCK"].Id },
            // Track Transfer Sections
            new() { ClientId = client.Id, PartNumber = "24.TT01", SeriesId = series["24.000"].Id, MaterialId = materials["STL-GOLD"].Id, PartType = "Track Transfer", WidthMm = 500, HeightMm = 150, LengthMm = 1000, CategoryId = categories["INTERLOCK"].Id },
            new() { ClientId = client.Id, PartNumber = "25.TT01", SeriesId = series["25.000"].Id, MaterialId = materials["STL-GOLD"].Id, PartType = "Track Transfer", WidthMm = 600, HeightMm = 180, LengthMm = 1200, CategoryId = categories["INTERLOCK"].Id },
            new() { ClientId = client.Id, PartNumber = "26.TT01", SeriesId = series["26.000"].Id, MaterialId = materials["STL-GOLD"].Id, PartType = "Track Transfer", WidthMm = 700, HeightMm = 220, LengthMm = 1500, CategoryId = categories["INTERLOCK"].Id },
        };
        context.BridgeInterlocks.AddRange(bridgeInterlocks);
        await context.SaveChangesAsync();
    }

    private static async Task SeedDropLiftUnitsAsync(ProductDbContext context, Client client,
        Dictionary<string, ProfileSeries> series, Dictionary<string, Material> materials, Dictionary<string, Category> categories)
    {
        var dropLifts = new List<DropLiftUnit>
        {
            // Standard Drop Lifts
            new() { ClientId = client.Id, PartNumber = "24.DL01", SeriesId = series["24.000"].Id, MaterialId = materials["STL-GOLD"].Id, LiftType = "Standard", MinTrackLengthMm = 1500, ApproachLengthMm = 500, HeightMm = 2000, SafeWorkingLoadKg = 80, CategoryId = categories["DROPLIFT"].Id },
            new() { ClientId = client.Id, PartNumber = "25.DL01", SeriesId = series["25.000"].Id, MaterialId = materials["STL-GOLD"].Id, LiftType = "Standard", MinTrackLengthMm = 2000, ApproachLengthMm = 600, HeightMm = 2500, SafeWorkingLoadKg = 200, CategoryId = categories["DROPLIFT"].Id },
            new() { ClientId = client.Id, PartNumber = "26.DL01", SeriesId = series["26.000"].Id, MaterialId = materials["STL-GOLD"].Id, LiftType = "Standard", MinTrackLengthMm = 2500, ApproachLengthMm = 700, HeightMm = 3000, SafeWorkingLoadKg = 400, CategoryId = categories["DROPLIFT"].Id },
            // Low Headroom Drop Lifts
            new() { ClientId = client.Id, PartNumber = "24.DL02", SeriesId = series["24.000"].Id, MaterialId = materials["STL-GOLD"].Id, LiftType = "Low Headroom", MinTrackLengthMm = 1200, ApproachLengthMm = 400, HeightMm = 1500, WidthMm = 800, SafeWorkingLoadKg = 80, CategoryId = categories["DROPLIFT"].Id },
            new() { ClientId = client.Id, PartNumber = "25.DL02", SeriesId = series["25.000"].Id, MaterialId = materials["STL-GOLD"].Id, LiftType = "Low Headroom", MinTrackLengthMm = 1500, ApproachLengthMm = 500, HeightMm = 1800, WidthMm = 1000, SafeWorkingLoadKg = 200, CategoryId = categories["DROPLIFT"].Id },
            new() { ClientId = client.Id, PartNumber = "26.DL02", SeriesId = series["26.000"].Id, MaterialId = materials["STL-GOLD"].Id, LiftType = "Low Headroom", MinTrackLengthMm = 2000, ApproachLengthMm = 600, HeightMm = 2200, WidthMm = 1200, SafeWorkingLoadKg = 400, CategoryId = categories["DROPLIFT"].Id },
        };
        context.DropLiftUnits.AddRange(dropLifts);
        await context.SaveChangesAsync();
    }

    private static async Task SeedAccessoriesAsync(ProductDbContext context, Client client,
        Dictionary<string, ProfileSeries> series, Dictionary<string, Material> materials, Dictionary<string, Category> categories)
    {
        var accessories = new List<Accessory>
        {
            // End Stops
            new() { ClientId = client.Id, PartNumber = "21.ES01", SeriesId = series["21.000"].Id, MaterialId = materials["STL-GOLD"].Id, AccessoryType = "End Stop", Description = "Track end stop with buffer", LengthMm = 50, HeightMm = 30, WidthMm = 30, CategoryId = categories["ACCESSORIES"].Id },
            new() { ClientId = client.Id, PartNumber = "23.ES01", SeriesId = series["23.000"].Id, MaterialId = materials["STL-GOLD"].Id, AccessoryType = "End Stop", Description = "Track end stop with buffer", LengthMm = 60, HeightMm = 40, WidthMm = 40, CategoryId = categories["ACCESSORIES"].Id },
            new() { ClientId = client.Id, PartNumber = "24.ES01", SeriesId = series["24.000"].Id, MaterialId = materials["STL-GOLD"].Id, AccessoryType = "End Stop", Description = "Track end stop with buffer", LengthMm = 70, HeightMm = 50, WidthMm = 48, CategoryId = categories["ACCESSORIES"].Id },
            new() { ClientId = client.Id, PartNumber = "25.ES01", SeriesId = series["25.000"].Id, MaterialId = materials["STL-GOLD"].Id, AccessoryType = "End Stop", Description = "Track end stop with buffer", LengthMm = 90, HeightMm = 65, WidthMm = 65, CategoryId = categories["ACCESSORIES"].Id },
            // Track Covers (per meter)
            new() { ClientId = client.Id, PartNumber = "21.TC01", SeriesId = series["21.000"].Id, MaterialId = materials["STL-GOLD"].Id, AccessoryType = "Track Cover", Description = "Protective track cover, sold per meter", LengthMm = 1000, CategoryId = categories["ACCESSORIES"].Id },
            new() { ClientId = client.Id, PartNumber = "23.TC01", SeriesId = series["23.000"].Id, MaterialId = materials["STL-GOLD"].Id, AccessoryType = "Track Cover", Description = "Protective track cover, sold per meter", LengthMm = 1000, CategoryId = categories["ACCESSORIES"].Id },
            new() { ClientId = client.Id, PartNumber = "24.TC01", SeriesId = series["24.000"].Id, MaterialId = materials["STL-GOLD"].Id, AccessoryType = "Track Cover", Description = "Protective track cover, sold per meter", LengthMm = 1000, CategoryId = categories["ACCESSORIES"].Id },
            // Dilation Joints
            new() { ClientId = client.Id, PartNumber = "23.DJ01", SeriesId = series["23.000"].Id, MaterialId = materials["STL-GOLD"].Id, AccessoryType = "Dilation Joint", Description = "Thermal expansion joint", LengthMm = 150, CategoryId = categories["ACCESSORIES"].Id },
            new() { ClientId = client.Id, PartNumber = "24.DJ01", SeriesId = series["24.000"].Id, MaterialId = materials["STL-GOLD"].Id, AccessoryType = "Dilation Joint", Description = "Thermal expansion joint", LengthMm = 180, CategoryId = categories["ACCESSORIES"].Id },
            new() { ClientId = client.Id, PartNumber = "25.DJ01", SeriesId = series["25.000"].Id, MaterialId = materials["STL-GOLD"].Id, AccessoryType = "Dilation Joint", Description = "Thermal expansion joint", LengthMm = 200, CategoryId = categories["ACCESSORIES"].Id },
        };
        context.Accessories.AddRange(accessories);
        await context.SaveChangesAsync();
    }

    private static async Task SeedBearingOptionsAsync(ProductDbContext context, Client client,
        Dictionary<string, ProfileSeries> series, Dictionary<string, Category> categories)
    {
        var bearings = new List<BearingOption>
        {
            // Standard Temperature Range
            new() { ClientId = client.Id, PartNumber = "STD.23", SeriesId = series["23.000"].Id, BearingType = "Standard", MinTemperatureC = -20, MaxTemperatureC = 80, Description = "Standard sealed bearings" },
            new() { ClientId = client.Id, PartNumber = "STD.24", SeriesId = series["24.000"].Id, BearingType = "Standard", MinTemperatureC = -20, MaxTemperatureC = 80, Description = "Standard sealed bearings" },
            new() { ClientId = client.Id, PartNumber = "STD.25", SeriesId = series["25.000"].Id, BearingType = "Standard", MinTemperatureC = -20, MaxTemperatureC = 80, Description = "Standard sealed bearings" },
            new() { ClientId = client.Id, PartNumber = "STD.26", SeriesId = series["26.000"].Id, BearingType = "Standard", MinTemperatureC = -20, MaxTemperatureC = 80, Description = "Standard sealed bearings" },
            // High Temperature
            new() { ClientId = client.Id, PartNumber = "HT.23", SeriesId = series["23.000"].Id, BearingType = "High Temperature", MinTemperatureC = 0, MaxTemperatureC = 200, Description = "High temperature bearings with special grease" },
            new() { ClientId = client.Id, PartNumber = "HT.24", SeriesId = series["24.000"].Id, BearingType = "High Temperature", MinTemperatureC = 0, MaxTemperatureC = 200, Description = "High temperature bearings with special grease" },
            new() { ClientId = client.Id, PartNumber = "HT.25", SeriesId = series["25.000"].Id, BearingType = "High Temperature", MinTemperatureC = 0, MaxTemperatureC = 200, Description = "High temperature bearings with special grease" },
            // Low Temperature / Freezer
            new() { ClientId = client.Id, PartNumber = "LT.23", SeriesId = series["23.000"].Id, BearingType = "Low Temperature", MinTemperatureC = -40, MaxTemperatureC = 40, Description = "Low temperature bearings for freezer applications" },
            new() { ClientId = client.Id, PartNumber = "LT.24", SeriesId = series["24.000"].Id, BearingType = "Low Temperature", MinTemperatureC = -40, MaxTemperatureC = 40, Description = "Low temperature bearings for freezer applications" },
            new() { ClientId = client.Id, PartNumber = "LT.25", SeriesId = series["25.000"].Id, BearingType = "Low Temperature", MinTemperatureC = -40, MaxTemperatureC = 40, Description = "Low temperature bearings for freezer applications" },
            // Stainless Steel
            new() { ClientId = client.Id, PartNumber = "SS.23", SeriesId = series["23.050"].Id, BearingType = "Stainless Steel", MinTemperatureC = -20, MaxTemperatureC = 80, Description = "Stainless steel bearings for corrosive environments" },
            new() { ClientId = client.Id, PartNumber = "SS.24", SeriesId = series["24.050"].Id, BearingType = "Stainless Steel", MinTemperatureC = -20, MaxTemperatureC = 80, Description = "Stainless steel bearings for corrosive environments" },
            new() { ClientId = client.Id, PartNumber = "SS.25", SeriesId = series["25.050"].Id, BearingType = "Stainless Steel", MinTemperatureC = -20, MaxTemperatureC = 80, Description = "Stainless steel bearings for corrosive environments" },
        };
        context.BearingOptions.AddRange(bearings);
        await context.SaveChangesAsync();
    }

    private static async Task SeedPneumaticControlsAsync(ProductDbContext context, Client client,
        Dictionary<string, Category> categories)
    {
        var pneumatics = new List<PneumaticControl>
        {
            // Manual Valve Controls
            new() { ClientId = client.Id, PartNumber = "X7.003", ControlType = "Manual Valve", Description = "Manual valve with 10m tube for tongue switch control", TubeLengthM = 10, CategoryId = categories["PNEUMATIC"].Id },
            new() { ClientId = client.Id, PartNumber = "X7.005", ControlType = "Manual Valve", Description = "Manual valve with 5m tube for tongue switch control", TubeLengthM = 5, CategoryId = categories["PNEUMATIC"].Id },
            // Air Distributors
            new() { ClientId = client.Id, PartNumber = "X7.023", ControlType = "Air Distributor", Description = "3+1 ways air distributor/manifold", WayCount = 3, CategoryId = categories["PNEUMATIC"].Id },
            new() { ClientId = client.Id, PartNumber = "X7.024", ControlType = "Air Distributor", Description = "4+1 ways air distributor/manifold", WayCount = 4, CategoryId = categories["PNEUMATIC"].Id },
            new() { ClientId = client.Id, PartNumber = "X7.025", ControlType = "Air Distributor", Description = "5+1 ways air distributor/manifold", WayCount = 5, CategoryId = categories["PNEUMATIC"].Id },
            new() { ClientId = client.Id, PartNumber = "X7.026", ControlType = "Air Distributor", Description = "6+1 ways air distributor/manifold", WayCount = 6, CategoryId = categories["PNEUMATIC"].Id },
            // Solenoid Valves
            new() { ClientId = client.Id, PartNumber = "X7.101", ControlType = "Solenoid Valve", Description = "5/2 way solenoid valve 24V DC", CategoryId = categories["PNEUMATIC"].Id },
            new() { ClientId = client.Id, PartNumber = "X7.102", ControlType = "Solenoid Valve", Description = "5/2 way solenoid valve 230V AC", CategoryId = categories["PNEUMATIC"].Id },
            // Pneumatic Cylinders
            new() { ClientId = client.Id, PartNumber = "X7.201", ControlType = "Pneumatic Cylinder", Description = "Double acting cylinder for switches - 50mm stroke", CategoryId = categories["PNEUMATIC"].Id },
            new() { ClientId = client.Id, PartNumber = "X7.202", ControlType = "Pneumatic Cylinder", Description = "Double acting cylinder for switches - 100mm stroke", CategoryId = categories["PNEUMATIC"].Id },
        };
        context.PneumaticControls.AddRange(pneumatics);
        await context.SaveChangesAsync();
    }
}
