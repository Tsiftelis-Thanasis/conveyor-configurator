# Plan: Navigation Fixes + Remove Configurator + Sketch Recognition

## Context
After the light-theme refactor, three issues remain:
1. **No way back to 2D drawing** — after "Build 3D" the user is stuck in 3D view until "Clear Drawing"
2. **Unneeded initial configurator** — the `Configure` state shows OverheadControls sliders that the user doesn't want; "Adjust Design" is sufficient
3. **Sketch recognition (new feature)** — let engineers upload a photo of a hand-drawn sketch and have AI (Claude Vision) convert it to the same `CadImportResult` the app already understands

## Changes

### 1. Simplify State Machine — `Home.razor`

**Rename** `Configure` → `Upload`. Remove `OverheadControls` from the Upload state. The sidebar shows ONLY the file upload area.

```
Upload       → File upload (DWG/DXF/images). Viewport: empty placeholder
CadLoaded    → Action panel + 2D preview (or 3D if toggled)
AdjustDesign → Track section editor + 2D preview
```

Key code changes in `Home.razor`:
- Rename `AppState.Configure` → `AppState.Upload`
- Remove `<OverheadControls>` and the Generate Parts / Export STEP action buttons from Upload state
- Remove `BuildConveyor()` call from `OnAfterRenderAsync` first render
- Change `ShowingViewport3D` from `CurrentState == AppState.Configure || Show3DView` → just `Show3DView`
- `ClearDrawing` transitions to `AppState.Upload`, no longer calls `BuildConveyor()` or `ResizeAsync()`
- Keep Three.js init in `OnAfterRenderAsync` (lazy — only used when Build 3D is clicked)
- Update `InputFile` accept to include image types: `.dwg,.dxf,.jpg,.jpeg,.png,.bmp`
- Add `UploadingMessage` field — "Analyzing CAD file..." for DWG/DXF, "Analyzing sketch with AI..." for images
- Branch in `HandleFileSelected`: check extension, call `Api.ImportCadFileAsync()` or `Api.ImportSketchAsync()`
- Add upload hint text: "Upload a DWG/DXF file or a photo of a hand-drawn sketch"

### 2. "Back to Drawing" Navigation — `CadActionPanel.razor` + `Home.razor`

**In `CadActionPanel.razor`**, add two new parameters:
- `[Parameter] public bool Is3DActive { get; set; }`
- `[Parameter] public EventCallback OnBackToDrawing { get; set; }`

The "Build 3D Model" button toggles:
- When `Is3DActive == false`: shows "Build 3D Model" → calls `OnBuild3D`
- When `Is3DActive == true`: shows "View 2D Drawing" → calls `OnBackToDrawing`

**In `Home.razor`**:
- Add `BackToDrawing()` method: sets `Show3DView = false`
- Pass `Is3DActive="Show3DView"` and `OnBackToDrawing="BackToDrawing"` to `<CadActionPanel>`
- Add a "2D VIEW" button in the `view-controls` bar (alongside ISO/TOP/FRONT/SIDE/RESET) that appears when `ShowingViewport3D && ImportResult != null`

### 3. New Backend Service — `backend/Services/SketchRecognitionService.cs` (NEW)

Calls the Anthropic Claude Messages API with vision to analyze a sketch image. Uses raw `HttpClient` — no SDK dependency needed.

**Flow:**
1. Read image stream → convert to base64
2. Determine MIME type from extension
3. POST to `https://api.anthropic.com/v1/messages` with image + prompt
4. Parse structured JSON from response
5. Convert to `CadEntityInfo` list + build mesh + track sections + suggested config
6. Return `CadImportResult`

**Claude Vision prompt** (embedded in service):
```
You are analyzing an engineering sketch of an overhead conveyor track layout.
Extract all track geometry. Return ONLY valid JSON:
{
  "segments": [
    {"type":"line","startX":<mm>,"startY":<mm>,"endX":<mm>,"endY":<mm>},
    {"type":"arc","centerX":<mm>,"centerY":<mm>,"radius":<mm>,"startAngleDeg":<deg>,"endAngleDeg":<deg>}
  ],
  "dimensions": [{"label":"<text>","valueMm":<number>}],
  "notes": ["<annotations>"]
}
Guidelines:
- Use annotated dimensions if visible; otherwise estimate (track 5000-50000mm, curves 300-1500mm)
- All measurements in millimeters, angles in degrees CCW from +X
- Origin at bottom-left of layout
- Return ONLY the JSON, no markdown
```

**Internal DTOs** for deserializing Claude's response:
```csharp
private record SketchResponse(List<SketchSegment> Segments, List<SketchDimension> Dimensions, List<string> Notes);
private record SketchSegment(string Type, double? StartX, double? StartY, double? EndX, double? EndY, double? CenterX, double? CenterY, double? Radius, double? StartAngleDeg, double? EndAngleDeg);
private record SketchDimension(string Label, double ValueMm);
```

**Conversion** from `SketchResponse` → `CadImportResult`:
- Each `line` segment → `CadEntityInfo { Type = "Line", StartPoint, EndPoint, Length }`
- Each `arc` segment → `CadEntityInfo { Type = "Arc", Center, Radius, StartAngle, EndAngle, Length }`
- Dimensions → `TextItem { ItemType = "Dimension", Content = label + value }`
- Notes → `TextItem { ItemType = "Text" }`
- Call `CadImportService.BuildResult()` (extracted — see below)
- Set `FileName = "originalname.jpg (AI interpreted)"`

### 4. Extract Shared Helpers from CadImportService — `backend/Services/CadImportService.cs`

Make these methods `public static` so `SketchRecognitionService` can reuse them:

- **`IdentifyTrackSection(CadEntityInfo)`** — already static, just make public
- **`CalculateBoundingBox(List<CadEntityInfo>)`** — already static, just make public
- **`GenerateSuggestedConfig(CadImportResult)`** — already static, just make public

Add a new **`public static`** method that encapsulates mesh + analysis:
```csharp
public static CadImportResult BuildResult(List<CadEntityInfo> entities, List<TextItem> texts, string fileName)
{
    // Build mesh from entities (AddLineInfoToMesh / AddArcInfoToMesh)
    // Identify track sections
    // Calculate totals, bounding box, suggested config
    // Return complete CadImportResult
}
```

This extracts lines 61–97 of `AnalyzeDocument` into a reusable method. The `AddLineInfoToMesh` and `AddArcInfoToMesh` methods stay private but are called internally by `BuildResult`. The `ParseContext` type stays internal.

The existing `AnalyzeDocument` simplifies to: walk entities → collect CadEntityInfo + TextItem lists → call `BuildResult()`.

### 5. New API Endpoint — `backend/Program.cs`

```csharp
// Read API key
var anthropicKey = builder.Configuration["Anthropic:ApiKey"]
    ?? Environment.GetEnvironmentVariable("ANTHROPIC_API_KEY")
    ?? "";

// Endpoint
app.MapPost("/api/import/sketch", async (HttpRequest request) =>
{
    if (string.IsNullOrEmpty(anthropicKey))
        return Results.Problem("Sketch recognition not configured", statusCode: 503);
    // Validate form, file, extension (.jpg/.jpeg/.png/.bmp)
    // Call new SketchRecognitionService(anthropicKey).AnalyzeSketchAsync(stream, fileName)
    // Return CadImportResult
});
```

### 6. API Key Config — `backend/appsettings.json`

Add:
```json
"Anthropic": {
  "ApiKey": ""
}
```

The actual key goes in `appsettings.Development.json` (gitignored) or env var `ANTHROPIC_API_KEY`.

### 7. Frontend API Client — `frontend/Services/ConveyorApiService.cs`

Add `ImportSketchAsync` — identical pattern to `ImportCadFileAsync` but POSTs to `/api/import/sketch`:
```csharp
public async Task<CadImportResult?> ImportSketchAsync(Stream fileStream, string fileName)
{
    // Same multipart form upload as ImportCadFileAsync
    // POST to "/api/import/sketch"
    // Returns CadImportResult?
}
```

### 8. CSS Tweaks — `frontend/wwwroot/css/app.css`

- `.upload-hint` style (text below the upload heading)
- `.btn-back-drawing` style for the floating "2D VIEW" button in the viewport

## Files Modified

| File | Action |
|------|--------|
| `frontend/Pages/Home.razor` | Rename Configure→Upload, remove OverheadControls, add image upload branching, add BackToDrawing, add 2D VIEW button |
| `frontend/Shared/CadActionPanel.razor` | Add Is3DActive param, toggle Build 3D / View 2D Drawing button |
| `frontend/Services/ConveyorApiService.cs` | Add `ImportSketchAsync` method |
| `frontend/wwwroot/css/app.css` | Add `.upload-hint`, `.btn-back-drawing` styles |
| `backend/Services/CadImportService.cs` | Extract `BuildResult()` public static method, make helpers public static |
| `backend/Services/SketchRecognitionService.cs` | **NEW** — Claude Vision API integration |
| `backend/Program.cs` | Add `/api/import/sketch` endpoint, read Anthropic API key |
| `backend/appsettings.json` | Add `Anthropic.ApiKey` config |

## Implementation Order

1. **State machine simplification** — Home.razor (rename Configure→Upload, remove OverheadControls)
2. **Back to Drawing navigation** — CadActionPanel.razor + Home.razor
3. **Extract shared helpers** — CadImportService.cs (BuildResult, public static methods)
4. **Create SketchRecognitionService** — new backend service
5. **Add API endpoint + config** — Program.cs + appsettings.json
6. **Frontend sketch upload** — ConveyorApiService.cs + Home.razor (image file branching)
7. **CSS tweaks** — app.css

## Verification

1. Launch both projects
2. Confirm Upload state shows ONLY the file upload area (no sliders, no 3D)
3. Upload a DWG → 2D preview fills viewport, sidebar shows action panel
4. Click "Build 3D" → 3D appears, button changes to "View 2D Drawing", floating "2D VIEW" button appears
5. Click "View 2D Drawing" or "2D VIEW" → returns to 2D preview
6. Click "Generate Parts" → BOM overlay opens, closing it shows 2D preview (not stuck in 3D)
7. Click "Clear Drawing" → returns to Upload state
8. Upload a .jpg sketch photo → "Analyzing sketch with AI..." spinner → 2D preview of interpreted geometry
9. Verify sketch result has track sections, dimensions, suggested config
10. Click "Adjust Design" from sketch result → edit track sections → Apply → stats update
