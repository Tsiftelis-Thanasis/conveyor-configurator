# CAD Import Feature - DWG/DXF Support

## Overview

The CAD Import feature allows users to upload DWG or DXF files containing conveyor track layouts, automatically extract dimensions, and generate a 3D model with suggested configuration parameters.

## Features

### 1. File Upload
- Supports DWG and DXF file formats
- Maximum file size: 10MB
- Drag-and-drop interface

### 2. 2D Preview
- Interactive canvas visualization
- Top-down view of imported geometry
- Color-coded track sections:
  - **Blue**: Straight track sections
  - **Red**: Curved track sections
  - **Gray**: Other entities
- Grid overlay for scale reference
- Axis indicators (X = green, Y = red)

### 3. Statistics Display
- Total straight track length (mm)
- Total curve length (mm)
- Number of curves detected
- Total entities imported

### 4. 3D Mesh Generation
- Automatic conversion of 2D CAD entities to 3D mesh
- Track visualization with realistic dimensions:
  - Width: 50mm
  - Height: 30mm
- Integration with Three.js scene

### 5. Configuration Auto-Population
- Automatically calculates:
  - Total track length
  - Height from floor (from Z coordinates or default 3000mm)
  - Number of carriers (based on track length and 1000mm spacing)
  - Curve inclusion (if arcs detected)
  - Average curve radius
  - Suggested profile series (default: 24.000 for 80kg capacity)

## Workflow

### Step 1: Import
1. Click "Import CAD Drawing" button in control panel
2. Select a DWG or DXF file from your computer
3. Wait for upload and analysis

### Step 2: 2D Preview
1. Review the 2D preview showing extracted geometry
2. Check statistics to verify track dimensions
3. Click "Generate 3D Model" to proceed

### Step 3: 3D Visualization
1. View the 3D mesh in the main viewport
2. Use view controls (ISO, TOP, FRONT, SIDE) to inspect
3. Review suggested configuration parameters

### Step 4: Apply Configuration
1. Review suggested parameters:
   - Track length
   - Height from floor
   - Number of carriers
   - Carrier spacing
   - Curve settings
   - Suggested profile series
2. Click "Apply Configuration" to populate controls
3. Fine-tune parameters using sliders
4. Generate BOM for parts list

## Supported CAD Entities

The import system recognizes the following CAD entities:

| Entity Type | Description | Track Detection |
|-------------|-------------|-----------------|
| **Line** | Straight line segments | ✓ Identified as straight track (if length > 100mm) |
| **Arc** | Curved segments | ✓ Identified as curved track (if radius > 200mm) |
| **Circle** | Full circles | Rendered but not identified as track |
| **Polyline** | Connected line segments | ✓ Identified as straight track (if length > 100mm) |
| **LwPolyline** | Lightweight polylines | ✓ Identified as straight track |
| **Spline** | Smooth curves | Rendered with approximation |

### Track Section Criteria

**Straight Track:**
- Type: Line or Polyline
- Minimum length: 100mm

**Curved Track:**
- Type: Arc
- Minimum radius: 200mm

## Backend Implementation

### Technology Stack
- **ACadSharp Library**: Parses DWG/DXF files
- **Version**: 3.0.0 (auto-resolved from 1.5.1)
- **.NET 10**: Backend API

### API Endpoint

```
POST /api/import/cad
Content-Type: multipart/form-data
```

**Request:**
- File upload with `.dwg` or `.dxf` extension

**Response:**
```json
{
  "success": true,
  "fileName": "track-layout.dxf",
  "entities": [...],
  "trackSections": [...],
  "totalTrackLength": 10000.0,
  "totalCurveLength": 1570.8,
  "curveCount": 2,
  "boundingBox": {
    "min": { "x": 0, "y": 0, "z": 0 },
    "max": { "x": 8500, "y": 4000, "z": 3000 },
    "width": 8500,
    "height": 4000,
    "depth": 3000
  },
  "meshData": {
    "vertices": [...],
    "indices": [...],
    "normals": []
  },
  "suggestedConfig": {
    "trackLength": 11570.8,
    "heightFromFloor": 3000,
    "suggestedProfile": "24.000",
    "includeCurves": true,
    "curveRadius": 500,
    "curveCount": 2,
    "numCarriers": 12,
    "carrierSpacing": 1000
  }
}
```

### Service Architecture

**CadImportService.cs** (backend/Services/)
- `ImportFile(Stream, string)`: Main entry point
- `AnalyzeDocument()`: Extracts entities and track sections
- `ProcessEntity()`: Converts CAD entities to CadEntityInfo
- `AddLineToMesh()`, `AddArcToMesh()`, etc.: Generate 3D mesh data
- `IdentifyTrackSection()`: Classifies entities as track sections
- `GenerateSuggestedConfig()`: Calculates configuration parameters

## Frontend Implementation

### Components

**CadImportModal.razor** (frontend/Shared/)
- Multi-step workflow UI
- File upload handling
- 2D preview integration
- Configuration application

**CadPreview2D.razor** (frontend/Shared/)
- Canvas-based 2D rendering
- Statistics display
- Legend for track types

### JavaScript Modules

**cad-preview-2d.js** (frontend/wwwroot/js/)
- Canvas rendering of CAD entities
- Coordinate transformation (CAD → Canvas)
- Grid and axis drawing
- Entity-specific rendering (lines, arcs, circles, polylines)

**three-scene.js** (frontend/wwwroot/js/)
- `loadCadMesh()`: Creates Three.js BufferGeometry from mesh data
- Automatic normal computation
- Material application
- Camera fitting

### API Integration

**ConveyorApiService.cs** (frontend/Services/)
- `ImportCadFileAsync()`: Uploads file and receives import result

**ThreeJsInterop.cs** (frontend/Services/)
- `LoadCadMeshAsync()`: Passes mesh data to JavaScript for rendering

## Configuration Mapping

| CAD Analysis | OverheadConfig Property | Calculation |
|--------------|------------------------|-------------|
| Total track length | `TrackLength` | Sum of straight + curve lengths |
| Bounding box max Z | `HeightFromFloor` | Max Z coordinate or default 3000mm |
| Curve detection | `IncludeCurves` | True if any arcs found |
| Average radius | `CurveRadius` | Mean of all arc radii |
| Track length / 1000 | `NumCarriers` | Carriers spaced at 1m intervals |
| Fixed | `CarrierSpacing` | 1000mm default |
| Load-based | `TrackProfile` | 24.000 (80kg SWL) default |

## Testing

### Test File Provided

**test-track.dxf** - Sample DXF file with:
- Straight section: 5000mm horizontal
- Incline section: 5000mm at angle
- 90° curve: 500mm radius
- Return section: 5000mm horizontal

### Test Procedure

1. Start both backend and frontend:
   ```bash
   # Terminal 1
   cd backend && dotnet run --urls=http://localhost:5000

   # Terminal 2
   cd frontend && dotnet run --urls=http://localhost:5050
   ```

2. Navigate to http://localhost:5050

3. Click "Import CAD Drawing"

4. Upload `test-track.dxf`

5. Verify 2D preview shows:
   - Blue straight lines
   - Red curved section
   - Correct statistics

6. Click "Generate 3D Model"

7. Verify 3D mesh appears in viewport

8. Review suggested configuration:
   - Track length: ~11.5 meters
   - Curves: Yes
   - Curve radius: 500mm
   - Carriers: 12

9. Click "Apply Configuration"

10. Verify controls are populated

11. Generate BOM to see parts list

## Troubleshooting

### File Upload Fails

**Error**: "Unsupported file format"
- **Solution**: Ensure file extension is `.dwg` or `.dxf`

**Error**: "Failed to parse CAD file"
- **Solution**: File may be corrupted or use unsupported DWG version
- Supported versions: AutoCAD 2000-2018 (AC1015-AC1032)

### 2D Preview Empty

**Issue**: Canvas shows "No entities to display"
- **Cause**: No valid entities in model space
- **Solution**: Check that entities are in model space, not paper space

### 3D Mesh Not Visible

**Issue**: "Generate 3D Model" succeeds but nothing appears
- **Cause**: Mesh may be outside viewport bounds
- **Solution**: Click "RESET" button to fit camera

### Configuration Not Applied

**Issue**: "Apply Configuration" doesn't update controls
- **Cause**: Profile series may not be loaded
- **Solution**: Wait for profile series to load, then retry import

## Limitations

1. **Entity Support**: Only basic 2D entities (line, arc, circle, polyline, spline)
2. **3D Entities**: 3D solids and surfaces not supported
3. **Blocks**: Block references are not expanded
4. **Attributes**: Text and attributes are ignored
5. **Layers**: All entities rendered regardless of layer visibility
6. **Units**: Assumes millimeters
7. **Coordinate System**: Assumes XY plane for 2D, Z for height

## Future Enhancements

- [ ] Support for 3D DWG/DXF entities
- [ ] Block expansion and nested entity processing
- [ ] Layer filtering and visibility control
- [ ] Unit detection and conversion
- [ ] Advanced track detection (switches, junctions)
- [ ] Multiple track loops detection
- [ ] Export modified layout back to DXF
- [ ] AI-powered track optimization suggestions

## Dependencies

### Backend
```xml
<PackageReference Include="ACadSharp" Version="1.5.1" />
```

### Frontend
- Three.js v0.160.0 (via CDN)
- HTML5 Canvas API

## API Reference

### Models

See `backend/Services/CadImportService.cs` for complete model definitions:

- `CadImportResult`
- `CadEntityInfo`
- `TrackSection`
- `Point3D`
- `BoundingBox3D`
- `CadMeshData`
- `SuggestedConveyorConfig`

## License

This feature uses the ACadSharp library, licensed under the MIT License.
