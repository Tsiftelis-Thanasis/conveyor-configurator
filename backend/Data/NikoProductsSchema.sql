-- Conveyor Products Database Schema (Multi-Client)
-- Supports multiple product catalog clients (e.g., NIKO, other suppliers)

-- Clients table (product catalog suppliers)
CREATE TABLE IF NOT EXISTS Clients (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Code TEXT NOT NULL UNIQUE,
    Name TEXT NOT NULL,
    Description TEXT,
    Country TEXT,
    Website TEXT,
    CatalogueReference TEXT,
    IsActive INTEGER DEFAULT 1,
    CreatedAt TEXT DEFAULT CURRENT_TIMESTAMP
);

-- Categories table
CREATE TABLE IF NOT EXISTS Categories (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    ClientId INTEGER NOT NULL,
    Code TEXT NOT NULL,
    Name TEXT NOT NULL,
    Description TEXT,
    PageReference TEXT,
    FOREIGN KEY (ClientId) REFERENCES Clients(Id),
    UNIQUE (ClientId, Code)
);

-- Materials table
CREATE TABLE IF NOT EXISTS Materials (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    ClientId INTEGER NOT NULL,
    Code TEXT NOT NULL,
    Name TEXT NOT NULL,
    Description TEXT,
    FOREIGN KEY (ClientId) REFERENCES Clients(Id),
    UNIQUE (ClientId, Code)
);

-- Profile Series table (21.000, 23.000, 24.000, 25.000, 26.000, 27.000)
CREATE TABLE IF NOT EXISTS ProfileSeries (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    ClientId INTEGER NOT NULL,
    SeriesCode TEXT NOT NULL,
    HeightMm REAL NOT NULL,
    WidthMm REAL NOT NULL,
    SlotWidthMm REAL NOT NULL,
    WallThicknessMm REAL NOT NULL,
    MaxLoadKg INTEGER,
    MaterialId INTEGER,
    FOREIGN KEY (ClientId) REFERENCES Clients(Id),
    FOREIGN KEY (MaterialId) REFERENCES Materials(Id),
    UNIQUE (ClientId, SeriesCode)
);

-- Track Profiles
CREATE TABLE IF NOT EXISTS TrackProfiles (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    ClientId INTEGER NOT NULL,
    PartNumber TEXT NOT NULL,
    SeriesId INTEGER NOT NULL,
    MaterialId INTEGER NOT NULL,
    LengthMm REAL,
    PricePerMeter REAL,
    FOREIGN KEY (ClientId) REFERENCES Clients(Id),
    FOREIGN KEY (SeriesId) REFERENCES ProfileSeries(Id),
    FOREIGN KEY (MaterialId) REFERENCES Materials(Id),
    UNIQUE (ClientId, PartNumber)
);

-- Track Bends
CREATE TABLE IF NOT EXISTS TrackBends (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    ClientId INTEGER NOT NULL,
    PartNumber TEXT NOT NULL,
    SeriesId INTEGER NOT NULL,
    MaterialId INTEGER NOT NULL,
    AngleDegrees INTEGER NOT NULL,
    RadiusMm REAL NOT NULL,
    DimensionA_Mm REAL,
    TotalLengthMm REAL,
    CategoryId INTEGER,
    Price REAL,
    FOREIGN KEY (ClientId) REFERENCES Clients(Id),
    FOREIGN KEY (SeriesId) REFERENCES ProfileSeries(Id),
    FOREIGN KEY (MaterialId) REFERENCES Materials(Id),
    FOREIGN KEY (CategoryId) REFERENCES Categories(Id),
    UNIQUE (ClientId, PartNumber)
);

-- Brackets and Supports
CREATE TABLE IF NOT EXISTS Brackets (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    ClientId INTEGER NOT NULL,
    PartNumber TEXT NOT NULL,
    SeriesId INTEGER NOT NULL,
    MaterialId INTEGER NOT NULL,
    BracketType TEXT NOT NULL,
    LengthMm REAL,
    HeightMm REAL,
    WidthMm REAL,
    WallThicknessMm REAL,
    HoleDiameterMm REAL,
    ThreadSize TEXT,
    MaxAdjustmentMm REAL,
    CategoryId INTEGER,
    Price REAL,
    FOREIGN KEY (ClientId) REFERENCES Clients(Id),
    FOREIGN KEY (SeriesId) REFERENCES ProfileSeries(Id),
    FOREIGN KEY (MaterialId) REFERENCES Materials(Id),
    FOREIGN KEY (CategoryId) REFERENCES Categories(Id),
    UNIQUE (ClientId, PartNumber)
);

-- Trolleys
CREATE TABLE IF NOT EXISTS Trolleys (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    ClientId INTEGER NOT NULL,
    PartNumber TEXT NOT NULL,
    SeriesId INTEGER NOT NULL,
    MaterialId INTEGER NOT NULL,
    TrolleyType TEXT NOT NULL,
    WheelCount INTEGER NOT NULL,
    SafeWorkingLoadKg INTEGER NOT NULL,
    LengthMm REAL,
    HeightMm REAL,
    WidthMm REAL,
    HoleDiameterMm REAL,
    ThreadSize TEXT,
    HasRotatingScrew INTEGER DEFAULT 0,
    HasGuideRollers INTEGER DEFAULT 0,
    CategoryId INTEGER,
    Price REAL,
    FOREIGN KEY (ClientId) REFERENCES Clients(Id),
    FOREIGN KEY (SeriesId) REFERENCES ProfileSeries(Id),
    FOREIGN KEY (MaterialId) REFERENCES Materials(Id),
    FOREIGN KEY (CategoryId) REFERENCES Categories(Id),
    UNIQUE (ClientId, PartNumber)
);

-- Flight Bars
CREATE TABLE IF NOT EXISTS FlightBars (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    ClientId INTEGER NOT NULL,
    PartNumber TEXT NOT NULL,
    SeriesId INTEGER NOT NULL,
    MaterialId INTEGER NOT NULL,
    FlightBarType TEXT NOT NULL,
    SafeWorkingLoadKg INTEGER NOT NULL,
    LengthMm REAL NOT NULL,
    SpanMm REAL NOT NULL,
    HeightMm REAL,
    EyeNutDiameterMm REAL,
    HasRotatingEyeNut INTEGER DEFAULT 0,
    CategoryId INTEGER,
    Price REAL,
    FOREIGN KEY (ClientId) REFERENCES Clients(Id),
    FOREIGN KEY (SeriesId) REFERENCES ProfileSeries(Id),
    FOREIGN KEY (MaterialId) REFERENCES Materials(Id),
    FOREIGN KEY (CategoryId) REFERENCES Categories(Id),
    UNIQUE (ClientId, PartNumber)
);

-- Switches
CREATE TABLE IF NOT EXISTS Switches (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    ClientId INTEGER NOT NULL,
    PartNumber TEXT NOT NULL,
    SeriesId INTEGER NOT NULL,
    MaterialId INTEGER NOT NULL,
    SwitchType TEXT NOT NULL,
    OperationType TEXT,
    RadiusMm REAL,
    WidthMm REAL,
    LengthMm REAL,
    AngleDegrees INTEGER,
    IsAutomatic INTEGER DEFAULT 0,
    IsPneumatic INTEGER DEFAULT 0,
    IncludesCylinder INTEGER DEFAULT 0,
    CategoryId INTEGER,
    Price REAL,
    FOREIGN KEY (ClientId) REFERENCES Clients(Id),
    FOREIGN KEY (SeriesId) REFERENCES ProfileSeries(Id),
    FOREIGN KEY (MaterialId) REFERENCES Materials(Id),
    FOREIGN KEY (CategoryId) REFERENCES Categories(Id),
    UNIQUE (ClientId, PartNumber)
);

-- Stoppers
CREATE TABLE IF NOT EXISTS Stoppers (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    ClientId INTEGER NOT NULL,
    PartNumber TEXT NOT NULL,
    SeriesId INTEGER NOT NULL,
    MaterialId INTEGER NOT NULL,
    StopperType TEXT NOT NULL,
    TongueType TEXT,
    LengthMm REAL,
    HeightMm REAL,
    IsPneumatic INTEGER DEFAULT 0,
    IsSpringLoaded INTEGER DEFAULT 0,
    CategoryId INTEGER,
    Price REAL,
    FOREIGN KEY (ClientId) REFERENCES Clients(Id),
    FOREIGN KEY (SeriesId) REFERENCES ProfileSeries(Id),
    FOREIGN KEY (MaterialId) REFERENCES Materials(Id),
    FOREIGN KEY (CategoryId) REFERENCES Categories(Id),
    UNIQUE (ClientId, PartNumber)
);

-- Swivel Units
CREATE TABLE IF NOT EXISTS SwivelUnits (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    ClientId INTEGER NOT NULL,
    PartNumber TEXT NOT NULL,
    SeriesId INTEGER NOT NULL,
    MaterialId INTEGER NOT NULL,
    SwivelType TEXT NOT NULL,
    Direction TEXT,
    L1_Mm REAL,
    L2_Mm REAL,
    L3_Mm REAL,
    IsPneumatic INTEGER DEFAULT 0,
    IsSpringLoaded INTEGER DEFAULT 0,
    CategoryId INTEGER,
    Price REAL,
    FOREIGN KEY (ClientId) REFERENCES Clients(Id),
    FOREIGN KEY (SeriesId) REFERENCES ProfileSeries(Id),
    FOREIGN KEY (MaterialId) REFERENCES Materials(Id),
    FOREIGN KEY (CategoryId) REFERENCES Categories(Id),
    UNIQUE (ClientId, PartNumber)
);

-- Bridge Interlocks
CREATE TABLE IF NOT EXISTS BridgeInterlocks (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    ClientId INTEGER NOT NULL,
    PartNumber TEXT NOT NULL,
    SeriesId INTEGER NOT NULL,
    MaterialId INTEGER NOT NULL,
    PartType TEXT NOT NULL,
    WidthMm REAL,
    HeightMm REAL,
    LengthMm REAL,
    MinHeightMm REAL,
    MaxHeightMm REAL,
    CategoryId INTEGER,
    Price REAL,
    FOREIGN KEY (ClientId) REFERENCES Clients(Id),
    FOREIGN KEY (SeriesId) REFERENCES ProfileSeries(Id),
    FOREIGN KEY (MaterialId) REFERENCES Materials(Id),
    FOREIGN KEY (CategoryId) REFERENCES Categories(Id),
    UNIQUE (ClientId, PartNumber)
);

-- Drop Lift Units
CREATE TABLE IF NOT EXISTS DropLiftUnits (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    ClientId INTEGER NOT NULL,
    PartNumber TEXT NOT NULL,
    SeriesId INTEGER NOT NULL,
    MaterialId INTEGER NOT NULL,
    LiftType TEXT NOT NULL,
    MinTrackLengthMm REAL,
    ApproachLengthMm REAL,
    HeightMm REAL,
    WidthMm REAL,
    SafeWorkingLoadKg INTEGER,
    CategoryId INTEGER,
    Price REAL,
    FOREIGN KEY (ClientId) REFERENCES Clients(Id),
    FOREIGN KEY (SeriesId) REFERENCES ProfileSeries(Id),
    FOREIGN KEY (MaterialId) REFERENCES Materials(Id),
    FOREIGN KEY (CategoryId) REFERENCES Categories(Id),
    UNIQUE (ClientId, PartNumber)
);

-- Accessories (end stops, track covers, dilation joints, etc.)
CREATE TABLE IF NOT EXISTS Accessories (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    ClientId INTEGER NOT NULL,
    PartNumber TEXT NOT NULL,
    SeriesId INTEGER NOT NULL,
    MaterialId INTEGER NOT NULL,
    AccessoryType TEXT NOT NULL,
    Description TEXT,
    LengthMm REAL,
    HeightMm REAL,
    WidthMm REAL,
    CategoryId INTEGER,
    Price REAL,
    FOREIGN KEY (ClientId) REFERENCES Clients(Id),
    FOREIGN KEY (SeriesId) REFERENCES ProfileSeries(Id),
    FOREIGN KEY (MaterialId) REFERENCES Materials(Id),
    FOREIGN KEY (CategoryId) REFERENCES Categories(Id),
    UNIQUE (ClientId, PartNumber)
);

-- Bearing Options
CREATE TABLE IF NOT EXISTS BearingOptions (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    ClientId INTEGER NOT NULL,
    PartNumber TEXT NOT NULL,
    SeriesId INTEGER NOT NULL,
    BearingType TEXT NOT NULL,
    MinTemperatureC REAL,
    MaxTemperatureC REAL,
    Description TEXT,
    Price REAL,
    FOREIGN KEY (ClientId) REFERENCES Clients(Id),
    FOREIGN KEY (SeriesId) REFERENCES ProfileSeries(Id),
    UNIQUE (ClientId, PartNumber)
);

-- Turn Table Switches
CREATE TABLE IF NOT EXISTS TurnTableSwitches (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    ClientId INTEGER NOT NULL,
    PartNumber TEXT NOT NULL,
    SeriesId INTEGER NOT NULL,
    MaterialId INTEGER NOT NULL,
    SwitchType TEXT NOT NULL,
    DimensionA_Mm REAL,
    DimensionB_Mm REAL,
    ProfileHeightMm REAL,
    OperationType TEXT,
    SupportPoints INTEGER DEFAULT 8,
    CategoryId INTEGER,
    Price REAL,
    FOREIGN KEY (ClientId) REFERENCES Clients(Id),
    FOREIGN KEY (SeriesId) REFERENCES ProfileSeries(Id),
    FOREIGN KEY (MaterialId) REFERENCES Materials(Id),
    FOREIGN KEY (CategoryId) REFERENCES Categories(Id),
    UNIQUE (ClientId, PartNumber)
);

-- Pneumatic Controls and Air Distributors
CREATE TABLE IF NOT EXISTS PneumaticControls (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    ClientId INTEGER NOT NULL,
    PartNumber TEXT NOT NULL,
    ControlType TEXT NOT NULL,
    Description TEXT,
    WayCount INTEGER,
    TubeLengthM REAL,
    CategoryId INTEGER,
    Price REAL,
    FOREIGN KEY (ClientId) REFERENCES Clients(Id),
    FOREIGN KEY (CategoryId) REFERENCES Categories(Id),
    UNIQUE (ClientId, PartNumber)
);

-- Indexes for common queries
CREATE INDEX IF NOT EXISTS idx_categories_client ON Categories(ClientId);
CREATE INDEX IF NOT EXISTS idx_materials_client ON Materials(ClientId);
CREATE INDEX IF NOT EXISTS idx_profileseries_client ON ProfileSeries(ClientId);
CREATE INDEX IF NOT EXISTS idx_trolleys_client ON Trolleys(ClientId);
CREATE INDEX IF NOT EXISTS idx_trolleys_series ON Trolleys(SeriesId);
CREATE INDEX IF NOT EXISTS idx_trackbends_client ON TrackBends(ClientId);
CREATE INDEX IF NOT EXISTS idx_trackbends_angle ON TrackBends(AngleDegrees);
CREATE INDEX IF NOT EXISTS idx_brackets_client ON Brackets(ClientId);
CREATE INDEX IF NOT EXISTS idx_switches_client ON Switches(ClientId);
