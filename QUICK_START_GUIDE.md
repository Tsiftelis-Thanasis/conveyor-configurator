# CAD Import Feature - Quick Start Guide

## 🚀 Getting Started

### Prerequisites
- .NET 10 SDK installed
- Both backend and frontend running

### Starting the Application

**Option 1: Using separate terminals**

Terminal 1 (Backend):
```bash
cd backend
dotnet run --urls=http://localhost:5000
```

Terminal 2 (Frontend):
```bash
cd frontend
dotnet run --urls=http://localhost:5050
```

**Option 2: Using start scripts (if available)**
```bash
# Start backend
cd backend && start.bat

# Start frontend
cd frontend && start.bat
```

### Accessing the Application

Open your browser and navigate to:
```
http://localhost:5050
```

## 📥 Using CAD Import

### Step-by-Step Walkthrough

#### 1. Open Import Modal
- Look for the control panel on the left side
- Click the **"Import CAD Drawing"** button
- The CAD Import modal will appear

#### 2. Upload File
- Click the file input area or drag-and-drop
- Select a `.dwg` or `.dxf` file
- For testing, use the included `test-track.dxf` file
- Wait for upload and analysis (usually < 2 seconds)

#### 3. Review 2D Preview
The modal will show:
- **Canvas**: Top-down view of your track layout
  - Blue lines = Straight track sections
  - Red curves = Curved track sections
  - Gray = Other entities
- **Statistics Panel**:
  - Total Straight Track: e.g., 10000 mm
  - Total Curve Length: e.g., 1571 mm
  - Number of Curves: e.g., 2
  - Total Entities: e.g., 15

#### 4. Generate 3D Model
- Review the 2D preview to ensure it looks correct
- Click **"Generate 3D Model"** button
- The 3D mesh will appear in the main viewport
- Use view controls to inspect:
  - **ISO**: Isometric view (default)
  - **TOP**: Top-down view
  - **FRONT**: Front elevation
  - **SIDE**: Side elevation
  - **RESET**: Reset camera position

#### 5. Review Suggested Configuration
The modal will display calculated parameters:
- **Track Length**: Total length in mm
- **Height from Floor**: Default 3000mm or from CAD Z-coordinates
- **Suggested Profile**: Default 24.000 (80kg capacity)
- **Includes Curves**: Yes/No based on arc detection
- **Curve Radius**: Average radius if curves present
- **Number of Curves**: Count of detected arcs
- **Carriers**: Calculated based on track length ÷ 1000mm spacing
- **Carrier Spacing**: Default 1000mm

#### 6. Apply Configuration
- Review the suggested values
- Click **"Apply Configuration"**
- The modal will close
- Control panel sliders will update with imported values
- 3D viewport will rebuild with the configuration

#### 7. Fine-Tune Settings
After import, you can adjust any parameter:
- Track Length
- Height from Floor
- Track Profile (select from NIKO series)
- Carrier Spacing
- Load per Carrier
- Number of Carriers
- Include Curves
- Curve Radius
- Incline/Decline Angles
- Drive Units

#### 8. Generate BOM
- Click **"Generate Parts List"**
- Review the Bill of Materials based on your configuration
- Swap parts if alternatives are available
- Click **"Request Quote"** to submit

## 🧪 Testing with Sample File

### Using test-track.dxf

The included `test-track.dxf` contains:
- **Straight section**: 5000mm horizontal
- **Incline section**: Angled transition
- **90° curve**: 500mm radius arc
- **Return section**: 5000mm horizontal

**Expected Results:**
- Total Track Length: ~11,571 mm
- Curves Detected: Yes
- Curve Count: 1
- Curve Radius: 500 mm
- Number of Carriers: 12
- Height: 3000 mm

### Verification Steps

1. Import `test-track.dxf`
2. Check 2D preview shows complete layout
3. Verify statistics match expected values
4. Generate 3D model
5. Confirm mesh appears in viewport
6. Apply configuration
7. Check that sliders show:
   - Track Length: ~11570 mm
   - Carriers: 12
   - Include Curves: ✓ checked
   - Curve Radius: 500 mm

## 🎨 Understanding the 2D Preview

### Canvas Elements

**Grid Lines** (dark gray)
- Spaced at 1000mm intervals
- Helps visualize scale

**Axes** (corner)
- Green line: X-axis
- Red line: Y-axis

**Track Sections**
- Blue (thick): Straight track (identified for BOM)
- Red (thick): Curved track (identified for BOM)
- Gray (thin): Other entities (reference only)

### Reading Coordinates
- Origin (0,0) is typically at bottom-left
- X increases to the right
- Y increases upward
- All units in millimeters

## 🔧 Troubleshooting

### Upload Issues

**Problem**: "No file uploaded"
- **Solution**: Ensure you selected a file before submitting

**Problem**: "Unsupported file format"
- **Solution**: File must have `.dwg` or `.dxf` extension

**Problem**: "Failed to parse CAD file"
- **Solution**: File may be corrupted or unsupported DWG version
- Try resaving in AutoCAD 2018 or earlier format

### Preview Issues

**Problem**: 2D canvas is blank
- **Solution**: File may have no entities in model space
- Check that entities aren't in paper space or blocks

**Problem**: Track sections not colored blue/red
- **Solution**: Entities may not meet minimum size criteria:
  - Lines must be > 100mm for straight track
  - Arcs must have radius > 200mm for curves

### 3D Issues

**Problem**: 3D model doesn't appear
- **Solution**: Click "RESET" button to fit camera to model
- Check browser console for JavaScript errors

**Problem**: Mesh looks incorrect
- **Solution**: CAD file may have unexpected entity types
- Try simplifying the drawing to basic lines and arcs

### Configuration Issues

**Problem**: Values don't apply to controls
- **Solution**: Wait for profile series to load (check network tab)
- Refresh page and try again

**Problem**: BOM doesn't match imported config
- **Solution**: Ensure you clicked "Apply Configuration" first
- Re-import and verify values

## 📊 Understanding the Statistics

### Total Straight Track
Sum of all LINE and POLYLINE lengths > 100mm
- Used for: Track profile quantity calculation
- Affects: Number of 6m track sections needed

### Total Curve Length
Sum of all ARC lengths with radius > 200mm
- Used for: Curve bend quantity calculation
- Affects: Number of bend pieces needed

### Number of Curves
Count of detected ARC entities
- Used for: "Include Curves" checkbox
- Affects: Whether curve radius slider appears

### Total Entities
All CAD entities in the file
- Informational only
- Includes unidentified entities (text, dimensions, etc.)

## 🎯 Best Practices

### Creating CAD Files for Import

1. **Use Simple Entities**
   - Lines for straight sections
   - Arcs for curves
   - Avoid complex blocks and hatches

2. **Work in Model Space**
   - Don't put track layout in paper space
   - Layouts/viewports won't be imported

3. **Use Correct Units**
   - Draw in millimeters
   - 1 unit = 1 mm in the drawing

4. **Minimum Sizes**
   - Straight sections: minimum 100mm
   - Curve radii: minimum 200mm

5. **Clean Drawings**
   - Remove unnecessary layers
   - Delete unused blocks
   - Purge before saving

6. **Test Your File**
   - Open in AutoCAD/viewer first
   - Verify entities are visible
   - Check units and scale

## 🔄 Workflow Integration

### Typical Use Case

1. **Client provides DXF** of existing layout
2. **Import to configurator** via CAD Import
3. **Review 2D preview** for accuracy
4. **Generate 3D model** to visualize
5. **Apply configuration** to populate controls
6. **Fine-tune parameters** (carriers, profile, etc.)
7. **Generate BOM** for pricing
8. **Request quote** or export STEP

### Collaboration Workflow

1. Engineer creates layout in AutoCAD
2. Exports to DXF
3. Sales team imports to configurator
4. Adjusts configuration based on requirements
5. Generates BOM with pricing
6. Sends quote to customer
7. Exports STEP for manufacturing

## 📚 Additional Resources

- **Full Documentation**: See `CAD_IMPORT_README.md`
- **Implementation Details**: See `IMPLEMENTATION_SUMMARY.md`
- **Project README**: See `README.md`

## 🆘 Getting Help

If you encounter issues not covered here:

1. Check browser console (F12) for errors
2. Verify both backend and frontend are running
3. Test with the provided `test-track.dxf` first
4. Review error messages in the modal
5. Check network tab for failed API calls

## 🎉 Success Indicators

You'll know the feature is working correctly when:

✅ Import button appears in control panel
✅ Modal opens on button click
✅ File upload shows progress
✅ 2D canvas displays track layout
✅ Statistics show reasonable values
✅ 3D mesh appears in viewport
✅ Configuration values populate controls
✅ BOM generates with imported config
✅ No console errors in browser

---

**Ready to try it?** Upload `test-track.dxf` and follow the steps above!
