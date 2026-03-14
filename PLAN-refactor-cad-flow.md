# Refactor: CAD Import Flow + Light Theme

## Context
Currently the CAD import lives inside a modal dialog with a 3-step wizard (Upload → 2D Preview → 3D + Apply). The user wants a more integrated experience: load a DWG directly, preview it in the main viewport (not a modal), and then choose from 3 actions. The dark navy/purple theme needs to become light/white.

## Overview of Changes

### 1. Light/White Theme — `frontend/wwwroot/css/app.css`
Replace the CSS custom properties:
```css
--bg-dark:      #ffffff       (was #1a1a2e)
--bg-panel:     #f8f9fa       (was #16213e)
--bg-section:   #f0f2f5       (was #0f3460)
--text-primary: #1a1a2e       (was #e8e8e8)
--text-secondary: #6c757d     (was #a0a0a0)
--accent:       #2563eb       (was #e94560 — blue instead of red)
--highlight:    #1e40af       (was #f6ad55 — dark blue instead of amber)
--success:      #16a34a       (was #48bb78)
--info:         #0284c7       (was #4299e1)
--border-color: #dee2e6       (was #2a2a4a)
```
Update hardcoded dark colors throughout the CSS (hover states, input backgrounds, scrollbar, etc.). Add `box-shadow` for depth instead of dark borders. Update `.btn-primary:hover` to `#1d4ed8`.

### 2. Light Theme for Canvas — `frontend/wwwroot/js/cad-preview-2d.js`
- Background: `#ffffff` (was `#1a1a1a`)
- Grid: `#e8e8e8` (was `#2a2a2a`)
- Text colors: Dimension `#b45309` (was `#ffcc44`), Notes `#15803d` (was `#88ddaa`), Default `#4b5563` (was `#cccccc`)
- Empty state text: `#999` (was `#666`)
- Add `resizeCadCanvas(canvas)` export — reads parent container size, sets `canvas.width`/`canvas.height`

### 3. Light Theme for Three.js — `frontend/wwwroot/js/three-scene.js`
- Scene background: `0xf0f2f5` (was `0x1a1a2e`)
- Grid colors: `0xcccccc, 0xe0e0e0` (was dark)

### 4. Page State Machine — `frontend/Pages/Home.razor`
Add an `AppState` enum to drive the entire page:
```
Configure    → No DWG loaded; sidebar shows upload + overhead controls + viewport shows Three.js
CadLoaded    → DWG parsed; sidebar shows file info + 3 action buttons, viewport shows 2D preview
AdjustDesign → User editing dimensions; sidebar shows editable track sections, viewport shows 2D preview
```

**Sidebar content by state:**

| State | Sidebar Content |
|-------|----------------|
| `Configure` | Panel header + inline file upload area + `OverheadControls` + action buttons (Generate Parts, Export STEP) |
| `CadLoaded` | File name + drawing stats + **3 action buttons** (Generate Parts, Build 3D, Adjust Design) + detected dimensions + "Clear Drawing" link |
| `AdjustDesign` | "Adjust Design" header + Back button + editable track section list + Apply button |

**Viewport by state:**
- `Configure` or after "Build 3D": Three.js container + view controls
- `CadLoaded` / `AdjustDesign` (no 3D): Full-viewport `CadPreview2D`

**New fields in Home.razor `@code`:**
- `AppState CurrentState`
- `CadImportResult? ImportResult`
- `bool IsUploading`, `string? ImportError`
- `bool Show3DView` (set true after "Build 3D")

**File upload handler** moves from CadImportModal into Home.razor — inline `InputFile` in the sidebar.

### 5. New Component: `CadActionPanel.razor` — `frontend/Shared/`
Sidebar content when `CadLoaded`. Shows:
- File name and drawing statistics (entity count, track length, curves)
- Three stacked action buttons: **Generate Parts** | **Build 3D Model** | **Adjust Design**
- Detected dimensions list (read-only)
- "Clear Drawing" link

Parameters: `CadImportResult Result`, `EventCallback OnGenerateParts`, `EventCallback OnBuild3D`, `EventCallback OnAdjustDesign`, `EventCallback OnClearDrawing`

### 6. New Component: `TrackSectionEditor.razor` — `frontend/Shared/`
Sidebar content when `AdjustDesign`. Shows:
- Back button
- For each `TrackSection` in `ImportResult.TrackSections`:
  - Straight: editable length input (mm)
  - Curve: editable radius + angle inputs
- "Apply Changes" button
- Empty state message if no track sections detected

Parameters: `List<TrackSection> Sections`, `EventCallback OnBack`, `EventCallback<List<TrackSection>> OnApplyChanges`

When applied: recalculates `TotalTrackLength`, `TotalCurveLength`, updates `SuggestedConfig`, returns to `CadLoaded`.

### 7. Update `CadPreview2D.razor` — `frontend/Shared/`
- Add `[Parameter] public bool FullViewport { get; set; }`
- When `FullViewport = true`: canvas fills parent container (100% width/height), stats/legend/notes move to sidebar (hidden from this component)
- When `FullViewport = false`: keep current fixed-size behavior (for potential future modal use)
- Call `resizeCadCanvas` from JS before rendering when in full viewport mode

### 8. Delete `CadImportModal.razor` — `frontend/Shared/`
All its logic is absorbed by Home.razor and the new components. Remove the file and all references in Home.razor.

### 9. Three.js Re-initialization
When toggling between 2D preview and 3D view, the `#three-container` DOM element gets removed/added. Handle by:
- Keep both containers in DOM, toggle visibility with CSS classes (`display:none` vs `display:block`)
- This avoids needing to re-initialize Three.js when switching back to 3D

## Files Modified
| File | Action |
|------|--------|
| `frontend/wwwroot/css/app.css` | Update theme variables + hardcoded colors + add new action button styles |
| `frontend/wwwroot/js/cad-preview-2d.js` | Light background/grid/text colors + `resizeCadCanvas` export |
| `frontend/wwwroot/js/three-scene.js` | Light scene background + grid colors |
| `frontend/Pages/Home.razor` | State machine, inline upload, conditional viewport, remove modal reference |
| `frontend/Shared/CadPreview2D.razor` | Add `FullViewport` parameter, dynamic sizing |
| `frontend/Shared/CadActionPanel.razor` | **NEW** — post-import sidebar with 3 actions |
| `frontend/Shared/TrackSectionEditor.razor` | **NEW** — editable track dimensions |
| `frontend/Shared/CadImportModal.razor` | **DELETE** |

## Implementation Order
1. CSS theme swap (app.css) — immediate visual verification
2. JS theme updates (cad-preview-2d.js, three-scene.js)
3. Create `CadActionPanel.razor`
4. Create `TrackSectionEditor.razor`
5. Refactor `Home.razor` — state machine, inline upload, conditional viewport
6. Update `CadPreview2D.razor` — full viewport mode
7. Delete `CadImportModal.razor`

## Verification
1. Start both projects in VS (backend + frontend)
2. Verify light theme renders correctly — white backgrounds, blue accents, readable text
3. Upload a DWG file from the sidebar → 2D preview fills the main viewport
4. Click "Generate Parts" → BOM panel opens with parts from the DWG data
5. Click "Build 3D" → Three.js 3D model appears in viewport
6. Click "Adjust Design" → sidebar shows editable track sections → modify a length → Apply → stats update
7. Click "Clear Drawing" → returns to initial Configure state with Three.js viewport
