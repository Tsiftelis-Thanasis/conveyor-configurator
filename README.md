# Conveyor Configurator

Industrial design web application for configuring overhead conveyor systems with real-time 3D visualization, BOM generation, and CAD import.

## Features

- **3D Visualization**: Real-time Three.js rendering of overhead conveyor systems
- **Product Catalog**: NIKO enclosed track components (trolleys, bends, brackets, switches)
- **BOM Generation**: Automatic parts list with pricing and part swapping
- **CAD Import**: Upload DWG/DXF files → 2D preview → 3D mesh → auto-configure
- **STEP Export**: Generate CAD files for manufacturing
- **Quote Requests**: Submit configurations with contact details

## Quick Start

### Prerequisites
- .NET 10 SDK
- Modern web browser

### Run the Application

```bash
# Start both backend and frontend
start-both.bat

# Or manually:
# Terminal 1 - Backend API
cd backend
dotnet run --urls=http://localhost:5000

# Terminal 2 - Frontend
cd frontend
dotnet run --urls=http://localhost:5050
```

**Open in browser:** http://localhost:5050

## Architecture

```
Backend (.NET 10) - Port 5000
├── API Endpoints (RESTful)
├── ACadSharp (DWG/DXF parsing)
├── Entity Framework Core (SQLite)
└── Product Catalog (NIKO)

Frontend (Blazor WASM) - Port 5050
├── Razor Components
├── Three.js Integration (JS Interop)
├── API Client Services
└── Dark Theme UI
```

## Project Structure

```
conveyor-configurator/
├── backend/              # .NET 10 API
│   ├── Data/            # EF Core DbContext
│   ├── Models/          # DTOs and entities
│   ├── Services/        # CadImportService
│   └── Program.cs       # API endpoints
├── frontend/            # Blazor WebAssembly
│   ├── Pages/          # Home.razor
│   ├── Shared/         # Reusable components
│   ├── Services/       # API clients
│   └── wwwroot/        # Static assets
├── test-track.dxf      # CAD import sample
├── sample-*.csv        # Config examples
└── start-both.bat      # Launch script
```

## Configuration

Adjust overhead conveyor parameters:
- **Track Length**: 5000-30000mm
- **Height from Floor**: 2000-6000mm
- **Carrier Spacing**: 500-3000mm
- **Load per Carrier**: 10-200kg
- **Track Profile**: Select from NIKO series (15.000-32.000)
- **Curves**: Optional with configurable radius
- **Drive Units**: 1-5 motorized units

## CAD Import Workflow

1. Click "Import CAD Drawing"
2. Upload DWG or DXF file
3. Review 2D canvas preview with track statistics
4. Generate 3D mesh visualization
5. Apply auto-detected configuration
6. Adjust parameters as needed
7. Generate BOM for parts list

**Test File:** Use `test-track.dxf` for testing the import feature

## API Endpoints

### Health
- `GET /api/health` - Service status

### CAD Operations
- `POST /api/import/cad` - Parse DWG/DXF files
- `POST /api/export/overhead-step` - Generate STEP files

### Product Catalog (NIKO)
- `GET /api/clients/{code}/series` - Profile series
- `GET /api/clients/{code}/trolleys` - Available trolleys
- `GET /api/clients/{code}/bends` - Track bends
- `GET /api/clients/{code}/brackets` - Mounting brackets

### Quotes
- `POST /api/quotes` - Submit quote request
- `GET /api/quotes` - List all quotes

## BOM Generation

1. Configure conveyor parameters
2. Click "Generate Parts List"
3. Review calculated quantities:
   - Track profile sections (6m lengths)
   - Trolleys (matched to load capacity)
   - Track bends (if curves enabled)
   - Mounting brackets (2m spacing)
   - End stops (2 per system)
   - Drive units (as configured)
4. Swap parts for alternatives
5. Request quote or export

## Technologies

**Backend:**
- .NET 10 Minimal APIs
- ACadSharp (DWG/DXF parsing)
- Entity Framework Core + SQLite
- ASP.NET Core

**Frontend:**
- Blazor WebAssembly (.NET 9)
- Three.js v0.160.0 (3D rendering)
- Razor Components
- JavaScript Interop

## Development

```bash
# Build backend
cd backend
dotnet build

# Build frontend
cd frontend
dotnet build

# Run tests (if available)
dotnet test
```

## File Formats

- **DWG/DXF**: CAD import (ACadSharp)
- **STEP**: CAD export (ISO 10303-21)
- **CSV**: Configuration import/export
- **JSON**: Quote storage

## Database

SQLite database: `backend/niko-products.db`

Contains:
- Clients (NIKO)
- Profile series (15.000 - 32.000)
- Trolleys (various SWL ratings)
- Track bends (90°, 180°, custom radii)
- Brackets, switches, flight bars

## 3D Viewport Controls

- **Left-drag**: Rotate camera
- **Scroll**: Zoom in/out
- **Right-drag**: Pan view
- **Buttons**: ISO, TOP, FRONT, SIDE, RESET

## Limitations

- Overhead conveyor only (roller removed)
- STEP export contains metadata, not full geometry
- No authentication (development mode)
- Quotes saved to `backend/quotes/` folder
- DWG/DXF import: 2D entities only

## License

Proof of Concept - Internal Use

## Documentation

See `CLAUDE.md` for detailed project documentation and coding standards.
