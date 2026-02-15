# Conveyor Configurator

Industrial design proof-of-concept for configuring roller and overhead conveyor systems with real-time 3D visualization.

## Features

- **Two Conveyor Types**: Roller conveyor and enclosed track overhead conveyor
- **Real-time 3D Preview**: Three.js-powered visualization with orbit controls
- **Parameter Controls**: Sliders and inputs for all configuration options
- **CSV Import/Export**: Load and save configurations
- **CAD Import**: Load STEP/IGES files for viewing
- **STEP Export**: Generate STEP files via .NET backend
- **Quote Request**: Submit quote requests with configuration details

## Project Structure

```
conveyor-configurator/
├── index.html                    # Main HTML structure
├── app.js                        # Three.js 3D rendering & UI logic
├── styles.css                    # Styling with CSS variables
├── start.bat                     # Windows batch file to run both servers
├── sample-configs.csv            # Sample roller conveyor configurations
├── sample-overhead-configs.csv   # Sample overhead conveyor configurations
├── README.md                     # This file
└── backend/
    └── Program.cs                # .NET minimal API backend
```

## How to Run

### Option 1: Batch File (Windows)
```batch
cd C:\Users\tsift\conveyor-configurator
start.bat
```

### Option 2: Manual Start

**Terminal 1 - Backend (requires .NET SDK 6+):**
```bash
cd conveyor-configurator/backend
dotnet run --urls=http://localhost:5000
```

**Terminal 2 - Frontend (requires Python 3):**
```bash
cd conveyor-configurator
python -m http.server 8080
```

**Open browser:** http://localhost:8080

### Frontend Only (No Backend)
The app works without the backend - STEP export falls back to JSON, quotes save to localStorage.

## Conveyor Types

### 1. Roller Conveyor
Traditional belt/roller conveyor with adjustable:
- **Dimensions**: Length (500-5000mm), Width (200-1500mm), Height (400-1200mm)
- **Rollers**: Diameter (30-100mm), Spacing (50-200mm)
- **Load Capacity**: 100kg, 300kg, 500kg, 1000kg
- **Drive Type**: Gravity or Powered

### 2. Overhead Conveyor (NEW)
Enclosed track overhead conveyor system with:
- **Track Configuration**:
  - Track Length: 5000-30000mm
  - Height from Floor: 2000-6000mm
  - Track Profile: I-Beam, Box, or Tubular
- **Carriers**:
  - Carrier Spacing: 500-3000mm
  - Number of Carriers: 2-50
  - Load per Carrier: 10-200kg
- **Track Layout**:
  - Include Curves (toggle)
  - Curve Radius: 300-1500mm
  - Incline Angle: 0-30 degrees
  - Decline Angle: 0-30 degrees
  - Drive Units: 1-5

## 3D Model Components

### Roller Conveyor
- Side frames (steel)
- End frames
- Rollers (galvanized/steel/aluminum based on load)
- Support legs with cross braces
- Motor assembly (if powered)

### Overhead Conveyor
- I-beam enclosed track (top/bottom flanges + web)
- Curved track sections (when enabled)
- Trolley assemblies with 4 wheels each
- Drop rods (vertical pendants)
- J-hook carriers
- Drive unit housings with motors
- Support columns with cross braces

## API Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/health` | Health check |
| POST | `/api/export/step` | Export roller conveyor STEP file |
| POST | `/api/export/overhead-step` | Export overhead conveyor STEP file |
| POST | `/api/quotes` | Submit quote request |
| GET | `/api/quotes` | Get all quotes (admin) |
| POST | `/api/import/cad` | Parse CAD file (placeholder) |

## Configuration Objects

### Roller Config (JavaScript)
```javascript
const config = {
    length: 2000,        // mm
    width: 600,          // mm
    height: 750,         // mm
    rollerDiameter: 50,  // mm
    rollerSpacing: 100,  // mm
    loadCapacity: 300,   // kg
    driveType: 'powered' // 'gravity' or 'powered'
};
```

### Overhead Config (JavaScript)
```javascript
const overheadConfig = {
    trackLength: 10000,      // mm
    heightFromFloor: 3000,   // mm
    trackProfile: 'i-beam',  // 'i-beam', 'box', 'tube'
    carrierSpacing: 1000,    // mm
    loadPerCarrier: 50,      // kg
    numCarriers: 10,
    includeCurves: false,
    curveRadius: 500,        // mm
    inclineAngle: 0,         // degrees
    declineAngle: 0,         // degrees
    driveUnits: 1
};
```

## CSV Format

### Roller Conveyor CSV
```csv
name,length,width,height,rollerDiameter,rollerSpacing,loadCapacity,driveType
Config1,2000,600,750,50,100,300,powered
```

### Overhead Conveyor CSV
```csv
name,trackLength,heightFromFloor,carrierSpacing,loadPerCarrier,numCarriers,includeCurves,driveUnits
Paint Line,15000,3500,1200,30,12,true,2
```

## Key Files Overview

### `index.html`
- Type selector toggle (Roller/Overhead buttons)
- Conditional sections with `roller-only` and `overhead-only` classes
- Slider controls, selects, and specifications panels
- Quote modal form

### `app.js`
- `activeType`: Tracks current conveyor type ('roller' or 'overhead')
- `buildConveyor()`: Dispatches to appropriate builder
- `buildRollerConveyor()`: Creates roller conveyor 3D model
- `buildOverheadConveyor()`: Creates overhead conveyor 3D model
- `switchType(type)`: Handles type toggle
- `parseCSV()`: Auto-detects roller vs overhead format
- `generateCSV()`: Exports current type's config

### `styles.css`
- CSS variables for theming (--bg-dark, --highlight, etc.)
- `.mode-roller` and `.mode-overhead` body classes control visibility
- Type toggle button styling

### `backend/Program.cs`
- `ConveyorConfig`: Roller conveyor model
- `OverheadConveyorConfig`: Overhead conveyor model
- `QuoteRequest`: Supports both conveyor types
- `GenerateStepFile()`: Roller STEP generation
- `GenerateOverheadStepFile()`: Overhead STEP generation

## Technologies Used

- **Frontend**: HTML5, CSS3, JavaScript (ES Modules)
- **3D Rendering**: Three.js v0.160.0
- **CAD Import**: occt-import-js (client-side STEP parser)
- **Backend**: .NET 6+ Minimal API
- **File Format**: STEP (ISO 10303-21)

## Browser Controls

- **Drag**: Rotate camera around model
- **Scroll**: Zoom in/out
- **Right-drag**: Pan view

## Future Enhancements (Ideas)

- [ ] Add more track profiles (box, tubular) with different geometries
- [ ] Implement chain/belt visualization on track
- [ ] Add collision detection for curve radii
- [ ] Support multiple track circuits (loops, spurs)
- [ ] Add load visualization on carriers
- [ ] Implement OpenCascade for proper STEP geometry export
- [ ] Add cost estimation based on configuration
- [ ] Support for custom carrier types (hooks, platforms, baskets)

## Session Context

This project was enhanced to add overhead conveyor support to the existing roller conveyor configurator. The implementation includes:

1. Type selector UI at top of controls panel
2. Separate control sections for each conveyor type
3. Full 3D model builder for overhead conveyor with I-beam track, trolleys, drop rods, carriers, and drive units
4. CSV import/export for both types (auto-detects format)
5. Backend STEP export endpoint for overhead conveyors
6. Updated quote system to handle both conveyor types

## Requirements

- Node.js (optional, for alternative server)
- Python 3 (for simple HTTP server)
- .NET SDK 6+ (for backend API)
- Modern browser with ES Module support

## License

Proof of Concept - Internal Use
