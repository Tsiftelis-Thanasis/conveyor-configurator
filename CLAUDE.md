# Conveyor Configurator

## Project Overview

Industrial design proof-of-concept web application for real-time 3D visualization and configuration of conveyor systems. Supports two conveyor types:

- **Roller Conveyor**: Traditional belt/roller systems with adjustable dimensions, roller properties, and drive types
- **Overhead Conveyor**: Enclosed track overhead systems with carriers, trolleys, and track layouts including curves

Primary use cases: manufacturing equipment configuration, quote generation, and CAD export.

## Tech Stack

### Frontend
- **Blazor WebAssembly** (.NET 9) - Component-based UI framework
- **Razor Components** - Reusable UI components
- CSS3 (CSS variables for dark theme)
- **Three.js v0.160.0** - 3D rendering via JavaScript interop
- ES6+ JavaScript modules for Three.js integration

### Backend
- **.NET 10** - Minimal APIs with ASP.NET Core
- C# with record types and nullable reference types
- **ACadSharp** - DWG/DXF CAD file parsing
- **Entity Framework Core** - SQLite database for product catalog
- RESTful API endpoints

### Development
- Windows batch scripts for local development
- Separate backend (port 5000) and frontend (port 5050) servers

### File Formats
- STEP (ISO 10303-21) for CAD import/export
- IGES for CAD import
- CSV for configuration import/export
- JSON for fallback/localStorage

## Architecture

```
Backend (.NET 10) - Port 5000
├── API Endpoints
│   ├── GET  /api/health
│   ├── GET  /api/clients/* (product catalog)
│   ├── POST /api/export/overhead-step
│   ├── POST /api/import/cad (DWG/DXF parsing)
│   └── POST /api/quotes
├── Services
│   └── CadImportService (ACadSharp)
├── Data
│   ├── ProductDbContext (EF Core)
│   └── SQLite database (niko-products.db)
└── Models (DTOs, entities)

Frontend (Blazor WASM) - Port 5050
├── Pages
│   └── Home.razor (main configurator)
├── Shared Components
│   ├── OverheadControls.razor
│   ├── BomPanel.razor
│   ├── QuoteModal.razor
│   ├── CadImportModal.razor
│   └── CadPreview2D.razor
├── Services
│   ├── ConveyorApiService (HTTP client)
│   └── ThreeJsInterop (JS interop)
├── Models (DTOs)
└── wwwroot
    ├── js/ (Three.js integration)
    └── css/ (dark theme styles)
```

### Data Flow
1. User adjusts controls → OverheadConfig object updates
2. Config change triggers OnConfigChanged → ThreeJsInterop.BuildOverheadConveyorAsync()
3. Three.js rebuilds 3D model in viewport
4. Generate BOM → Fetches parts from product catalog API → Calculates quantities
5. Export STEP → config sent to backend → STEP file generated → downloaded
6. Import CAD → DWG/DXF uploaded → ACadSharp parses → 2D preview + 3D mesh → config auto-populated
7. Request Quote → form + config + BOM → saved to backend/quotes/

### Component Communication
- Blazor components use EventCallback for parent-child communication
- Services injected via DI (@inject ConveyorApiService, @inject ThreeJsInterop)
- JavaScript interop via IJSRuntime for Three.js integration
- HTTP calls to backend API via HttpClient

## File Organization

```
conveyor-configurator/
├── backend/
│   ├── Data/               # Database context and entities
│   ├── Models/             # DTOs and data models
│   ├── Services/           # Business logic (CadImportService)
│   ├── Program.cs          # .NET Minimal API endpoints
│   ├── ConveyorApi.csproj  # .NET 10 project file
│   ├── niko-products.db    # SQLite database
│   └── quotes/             # Saved quote requests (runtime)
├── frontend/
│   ├── Pages/              # Razor pages (Home.razor)
│   ├── Shared/             # Reusable components
│   ├── Services/           # API clients and JS interop
│   ├── Models/             # Frontend DTOs
│   ├── wwwroot/
│   │   ├── js/             # Three.js integration
│   │   └── css/            # Dark theme styles
│   ├── Program.cs          # Blazor WASM entry point
│   └── frontend.csproj     # .NET 9 Blazor project
├── test-track.dxf          # Sample DXF for CAD import testing
├── sample-configs.csv      # Configuration examples
├── start.bat               # Launches backend server
├── start-dev.bat           # Launches both backend and frontend
└── README.md               # Project documentation
```

## Coding Standards

### Blazor/Razor
- Component-based architecture with `.razor` files
- Parameter passing via `[Parameter]` attributes
- EventCallback for parent-child communication
- Code-behind in `@code` blocks
- Dependency injection via `@inject` directive
- Lifecycle methods: `OnAfterRenderAsync`, `OnParametersSetAsync`

### C#
- Record types for immutable DTOs
- Nullable reference types enabled
- Minimal API style with `MapGet()` / `MapPost()`
- Entity Framework Core for database access
- ACadSharp for DWG/DXF parsing
- Async/await throughout

### JavaScript (Three.js Integration)
- ES6 modules for Three.js imports
- Exported functions for Blazor JSInterop
- `SCALE` constant (0.001) converts millimeters to Three.js units
- Functions: `initScene`, `buildOverheadConveyor`, `loadCadMesh`, `setView`

### CSS
- CSS custom properties at `:root` for theming
- Variables: `--bg-dark`, `--bg-panel`, `--accent`, `--highlight`, `--success`
- Component-scoped styles where applicable
- Responsive design with media queries
- Dark theme throughout

## Key Patterns

### 3D Model Building (Three.js via JSInterop)
- Three.js scene managed in `three-scene.js`
- `conveyorGroup` (THREE.Group) cleared and rebuilt on config changes
- `THREE.Box3` used for centering calculations
- Camera auto-fitted to model bounds
- Shadow casting enabled on all geometry

### Blazor Component Patterns
```csharp
// Configuration binding
[Parameter] public OverheadConveyorConfig Config { get; set; } = new();
[Parameter] public EventCallback OnConfigChanged { get; set; }

// Service injection
@inject ConveyorApiService Api
@inject ThreeJsInterop ThreeJs
@inject IJSRuntime JS

// Async lifecycle
protected override async Task OnAfterRenderAsync(bool firstRender)
{
    if (firstRender)
    {
        await ThreeJs.InitializeSceneAsync("three-container");
        await BuildConveyor();
    }
}
```

### CAD Import Workflow
1. User uploads DWG/DXF file → `CadImportModal`
2. Backend parses with ACadSharp → extracts entities
3. Returns 2D entity data + 3D mesh data
4. `CadPreview2D` renders 2D canvas
5. `ThreeJsInterop.LoadCadMeshAsync()` creates 3D mesh
6. Suggested config auto-populates controls

### BOM Generation
- Queries product catalog API by series
- Calculates quantities based on config
- Selects parts by load capacity and dimensions
- Supports part swapping with alternatives

### Error Handling
- Status messages with auto-clear (3 seconds)
- Try-catch around API calls
- Null-safe operations with `?.` and `??`
- Validation on file uploads (type, size)

## Running the Project

```batch
# Option 1: Start backend only (serves API)
cd backend
dotnet run --urls=http://localhost:5000

# Option 2: Start frontend only (separate Blazor WASM dev server)
cd frontend
dotnet run --urls=http://localhost:5050

# Option 3: Start both (recommended for development)
start-dev.bat
# Then open http://localhost:5050 in your browser
```

**Ports:**
- Backend API: http://localhost:5000
- Frontend: http://localhost:5050

## API Endpoints

### Health & Configuration
| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/health` | Health check |

### CAD Operations
| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | `/api/export/overhead-step` | Generate overhead STEP file |
| POST | `/api/import/cad` | Parse DWG/DXF and return mesh data + config |

### Quote Management
| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | `/api/quotes` | Submit quote request |
| GET | `/api/quotes` | List all quotes |

### Product Catalog
| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/clients` | Get all clients |
| GET | `/api/clients/{code}` | Get client by code |
| GET | `/api/clients/{code}/series` | Get profile series for client |
| GET | `/api/clients/{code}/trolleys` | Get trolleys (filterable by series) |
| GET | `/api/clients/{code}/bends` | Get track bends (filterable by series/angle) |
| GET | `/api/clients/{code}/brackets` | Get mounting brackets |
| GET | `/api/clients/{code}/switches` | Get switches |
| GET | `/api/clients/{code}/flightbars` | Get flight bars |
| GET | `/api/clients/{code}/products/search` | Search products (multi-category) |

## Dependencies

### Backend (.NET 10)
- ACadSharp - DWG/DXF parsing
- Microsoft.EntityFrameworkCore.Sqlite - Database
- Microsoft.EntityFrameworkCore.Design - Migrations

### Frontend (.NET 9 Blazor WASM)
- Microsoft.AspNetCore.Components.WebAssembly - Blazor framework
- Three.js v0.160.0 (loaded via CDN)

## Known Limitations

- STEP export contains parameters in comments only (not full CAD geometry)
- Track profiles render as I-beam regardless of selected series
- Curves approximated with line segments in 3D visualization
- DWG/DXF import supports basic 2D entities only (no 3D solids)
- No authentication on API endpoints (development mode)
- Quote storage is file-based in `backend/quotes/` (production needs database)
- Single conveyor type supported (overhead only)
- Product catalog limited to NIKO client data
