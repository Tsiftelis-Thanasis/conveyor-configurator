# Conveyor Configurator

## Project Overview

Industrial design proof-of-concept web application for real-time 3D visualization and configuration of conveyor systems. Supports two conveyor types:

- **Roller Conveyor**: Traditional belt/roller systems with adjustable dimensions, roller properties, and drive types
- **Overhead Conveyor**: Enclosed track overhead systems with carriers, trolleys, and track layouts including curves

Primary use cases: manufacturing equipment configuration, quote generation, and CAD export.

## Tech Stack

### Frontend
- HTML5 with ES Modules
- CSS3 (CSS variables for dark theme)
- Vanilla JavaScript (ES6+)
- **Three.js v0.160.0** - 3D rendering

### Backend
- **.NET 10** - Minimal APIs with ASP.NET Core
- C# with record types and nullable reference types
- **Ab4d.OpenCascade** - Server-side STEP/IGES CAD parsing
- Static file serving (frontend hosted by .NET)

### Development
- Windows batch scripts for local development

### File Formats
- STEP (ISO 10303-21) for CAD import/export
- IGES for CAD import
- CSV for configuration import/export
- JSON for fallback/localStorage

## Architecture

```
.NET 10 Server (Port 5000)
├── Static Files (wwwroot/)
│   ├── index.html
│   ├── app.js
│   └── styles.css
├── API Endpoints
│   ├── GET  /api/health
│   ├── POST /api/export/step          (roller)
│   ├── POST /api/export/overhead-step (overhead)
│   ├── POST /api/import/cad           (STEP/IGES parsing)
│   └── POST /api/quotes
└── Ab4d.OpenCascade (CAD processing)

Browser
├── Three.js 3D Scene
├── Controls Panel (sliders, inputs)
├── Real-time model rendering
└── REST API calls to same origin
```

### Data Flow
1. User adjusts controls → config objects update
2. Config change triggers `buildConveyor()` → 3D model rebuilds
3. Export STEP → config sent to backend → file generated
4. Import CAD → file sent to backend → Ab4d.OpenCascade parses → mesh data returned → Three.js renders
5. Request Quote → form + config → saved as JSON

### Type Switching
- `activeType` variable tracks current mode ('roller' | 'overhead')
- CSS classes `mode-roller` / `mode-overhead` on body
- Elements use `.roller-only` / `.overhead-only` for visibility

## File Organization

```
conveyor-configurator/
├── backend/
│   ├── wwwroot/            # Static frontend files
│   │   ├── index.html      # Main HTML with controls panel and canvas
│   │   ├── app.js          # Core application logic (~1150 lines)
│   │   └── styles.css      # Dark theme styling, controls, modals
│   ├── Program.cs          # .NET Minimal API endpoints + static file serving
│   ├── ConveyorApi.csproj  # .NET 10 project file with Ab4d.OpenCascade
│   ├── bin/                # Compiled output
│   └── obj/                # Build artifacts
├── sample-configs.csv          # Roller conveyor examples
├── sample-overhead-configs.csv # Overhead conveyor examples
├── start.bat               # Launches .NET server
└── README.md               # Project documentation
```

## Coding Standards

### JavaScript
- ES6+ syntax: `const`/`let`, arrow functions, template literals, async/await
- Event-driven architecture
- No external dependencies except Three.js
- `SCALE` constant (0.001) converts millimeters to Three.js units
- Functions prefixed by purpose: `build*`, `create*`, `generate*`, `parse*`
- API_URL uses relative path `/api` (same origin)

### C#
- Record types for immutable DTOs (`ConveyorConfig`, `OverheadConveyorConfig`, `QuoteRequest`, `MeshDto`)
- Nullable reference types enabled
- String interpolation for STEP generation
- Minimal API style with `MapGet()` / `MapPost()`
- Ab4d.OpenCascade for CAD file parsing

### CSS
- CSS custom properties at `:root` for theming
- Variables: `--bg-dark`, `--bg-panel`, `--accent`, `--highlight`, `--success`
- BEM-like class naming
- Visibility classes: `.roller-only`, `.overhead-only`

### HTML
- Semantic elements: `<aside>`, `<section>`, `<output>`
- Labels wrap form controls
- Modal structure with overlay pattern

## Key Patterns

### 3D Model Building
- Models built into `conveyorGroup` (THREE.Group)
- Cleared and rebuilt on any config change
- `THREE.Box3` used for centering calculations
- Shadow casting enabled on all geometry

### Configuration Objects
```javascript
// Roller
config = { length, width, height, rollerDiameter, rollerSpacing, loadCapacity, driveType }

// Overhead
overheadConfig = { trackLength, trackHeight, carrierSpacing, carrierCount, loadPerCarrier,
                   trackProfile, includeCurves, curveRadius, inclineAngle, driveUnits }
```

### CAD Import (Server-Side)
- File uploaded via FormData to `/api/import/cad`
- Ab4d.OpenCascade parses STEP/IGES on server
- Returns mesh data (vertices, normals, indices, colors) as JSON
- Frontend creates Three.js BufferGeometry from mesh data

### Error Handling
- Backend unavailable: falls back to JSON export
- Try-catch around file operations
- Status messages auto-clear after 3 seconds

## Running the Project

```batch
# Start server (Windows)
start.bat

# Manual startup
cd backend && dotnet run --urls=http://localhost:5000
```

Open http://localhost:5000 in your browser.

## API Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/health` | Health check |
| POST | `/api/export/step` | Generate roller STEP file |
| POST | `/api/export/overhead-step` | Generate overhead STEP file |
| POST | `/api/import/cad` | Parse STEP/IGES and return mesh data |
| POST | `/api/quotes` | Submit quote request |
| GET | `/api/quotes` | List all quotes (admin) |

## Dependencies

- .NET 10 SDK
- Ab4d.OpenCascade NuGet package (wraps Open CASCADE Technology for .NET)

## Known Limitations

- STEP export contains parameters in comments only (not full CAD geometry)
- Track profiles (box, tube) render as I-beam visually
- Curves approximated with line segments
- No authentication on API endpoints
- Quote storage is file-based (production needs database)
