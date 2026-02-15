import * as THREE from 'three';
import { OrbitControls } from 'three/addons/controls/OrbitControls.js';

// Backend API URL (change for production)
const API_URL = 'http://localhost:5000/api';

// Active conveyor type
let activeType = 'roller';

// Roller configuration state
const config = {
    length: 2000,      // mm
    width: 600,        // mm
    height: 750,       // mm
    rollerDiameter: 50,// mm
    rollerSpacing: 100,// mm
    loadCapacity: 300, // kg
    driveType: 'powered'
};

// Overhead configuration state
const overheadConfig = {
    trackLength: 10000,      // mm (total track length)
    heightFromFloor: 3000,   // mm
    trackProfile: 'i-beam',  // i-beam, box, tube
    carrierSpacing: 1000,    // mm
    loadPerCarrier: 50,      // kg
    numCarriers: 10,
    includeCurves: false,
    curveRadius: 500,        // mm
    inclineAngle: 0,         // degrees
    declineAngle: 0,         // degrees
    driveUnits: 1
};

// Loaded configurations from CSV
let loadedConfigs = [];

// Imported CAD model
let importedModel = null;

// Scale factor (mm to Three.js units)
const SCALE = 0.001;

// Scene setup
const canvas = document.getElementById('canvas');
const scene = new THREE.Scene();
scene.background = new THREE.Color(0x0d1b2a);

// Camera
const camera = new THREE.PerspectiveCamera(
    50,
    canvas.clientWidth / canvas.clientHeight,
    0.1,
    100
);
camera.position.set(3, 2, 3);

// Renderer
const renderer = new THREE.WebGLRenderer({ canvas, antialias: true });
renderer.setSize(canvas.clientWidth, canvas.clientHeight);
renderer.setPixelRatio(Math.min(window.devicePixelRatio, 2));
renderer.shadowMap.enabled = true;
renderer.shadowMap.type = THREE.PCFSoftShadowMap;

// Controls
const controls = new OrbitControls(camera, canvas);
controls.enableDamping = true;
controls.dampingFactor = 0.05;
controls.minDistance = 1;
controls.maxDistance = 10;

// Lighting
const ambientLight = new THREE.AmbientLight(0xffffff, 0.4);
scene.add(ambientLight);

const directionalLight = new THREE.DirectionalLight(0xffffff, 0.8);
directionalLight.position.set(5, 10, 5);
directionalLight.castShadow = true;
directionalLight.shadow.mapSize.width = 2048;
directionalLight.shadow.mapSize.height = 2048;
scene.add(directionalLight);

const fillLight = new THREE.DirectionalLight(0x4a90d9, 0.3);
fillLight.position.set(-5, 3, -5);
scene.add(fillLight);

// Ground plane
const groundGeometry = new THREE.PlaneGeometry(20, 20);
const groundMaterial = new THREE.MeshStandardMaterial({
    color: 0x1a2a3a,
    roughness: 0.9
});
const ground = new THREE.Mesh(groundGeometry, groundMaterial);
ground.rotation.x = -Math.PI / 2;
ground.receiveShadow = true;
scene.add(ground);

// Grid helper
const gridHelper = new THREE.GridHelper(10, 20, 0x2a3a4a, 0x1a2a3a);
scene.add(gridHelper);

// Materials
const frameMaterial = new THREE.MeshStandardMaterial({
    color: 0x4a5568,
    roughness: 0.6,
    metalness: 0.8
});

const rollerMaterial = new THREE.MeshStandardMaterial({
    color: 0x718096,
    roughness: 0.3,
    metalness: 0.9
});

const legMaterial = new THREE.MeshStandardMaterial({
    color: 0x2d3748,
    roughness: 0.7,
    metalness: 0.6
});

const motorMaterial = new THREE.MeshStandardMaterial({
    color: 0xe94560,
    roughness: 0.5,
    metalness: 0.7
});

// Overhead-specific materials
const trackMaterial = new THREE.MeshStandardMaterial({
    color: 0x2d3748,
    roughness: 0.6,
    metalness: 0.9
});

const trolleyMaterial = new THREE.MeshStandardMaterial({
    color: 0x4a5568,
    roughness: 0.4,
    metalness: 0.8
});

const dropRodMaterial = new THREE.MeshStandardMaterial({
    color: 0x718096,
    roughness: 0.3,
    metalness: 0.9
});

const carrierMaterial = new THREE.MeshStandardMaterial({
    color: 0xe94560,
    roughness: 0.5,
    metalness: 0.6
});

// Conveyor group
let conveyorGroup = new THREE.Group();
scene.add(conveyorGroup);

// Build conveyor function - dispatches to appropriate builder
function buildConveyor() {
    // If showing imported model, don't rebuild conveyor
    if (importedModel) return;

    if (activeType === 'overhead') {
        buildOverheadConveyor();
    } else {
        buildRollerConveyor();
    }
}

// Build roller conveyor
function buildRollerConveyor() {

    // Clear existing
    while (conveyorGroup.children.length > 0) {
        const child = conveyorGroup.children[0];
        conveyorGroup.remove(child);
        if (child.geometry) child.geometry.dispose();
    }

    const length = config.length * SCALE;
    const width = config.width * SCALE;
    const height = config.height * SCALE;
    const rollerDiameter = config.rollerDiameter * SCALE;
    const rollerRadius = rollerDiameter / 2;
    const rollerSpacing = config.rollerSpacing * SCALE;

    // Frame thickness
    const frameThickness = 0.04;
    const frameHeight = 0.06;

    // Side frames
    const sideFrameGeometry = new THREE.BoxGeometry(length, frameHeight, frameThickness);

    const leftFrame = new THREE.Mesh(sideFrameGeometry, frameMaterial);
    leftFrame.position.set(0, height, width / 2 + frameThickness / 2);
    leftFrame.castShadow = true;
    conveyorGroup.add(leftFrame);

    const rightFrame = new THREE.Mesh(sideFrameGeometry, frameMaterial);
    rightFrame.position.set(0, height, -width / 2 - frameThickness / 2);
    rightFrame.castShadow = true;
    conveyorGroup.add(rightFrame);

    // End frames
    const endFrameGeometry = new THREE.BoxGeometry(frameThickness, frameHeight, width + frameThickness * 2);

    const frontFrame = new THREE.Mesh(endFrameGeometry, frameMaterial);
    frontFrame.position.set(length / 2 + frameThickness / 2, height, 0);
    frontFrame.castShadow = true;
    conveyorGroup.add(frontFrame);

    const backFrame = new THREE.Mesh(endFrameGeometry, frameMaterial);
    backFrame.position.set(-length / 2 - frameThickness / 2, height, 0);
    backFrame.castShadow = true;
    conveyorGroup.add(backFrame);

    // Rollers
    const rollerGeometry = new THREE.CylinderGeometry(rollerRadius, rollerRadius, width, 24);
    const numRollers = Math.floor(length / rollerSpacing);
    const startX = -length / 2 + rollerSpacing / 2;

    for (let i = 0; i < numRollers; i++) {
        const roller = new THREE.Mesh(rollerGeometry, rollerMaterial);
        roller.rotation.x = Math.PI / 2;
        roller.position.set(startX + i * rollerSpacing, height - rollerRadius - 0.005, 0);
        roller.castShadow = true;
        conveyorGroup.add(roller);
    }

    // Legs
    const legGeometry = new THREE.BoxGeometry(0.04, height - 0.02, 0.04);

    const legPositions = [
        [length / 2 - 0.1, width / 2],
        [length / 2 - 0.1, -width / 2],
        [-length / 2 + 0.1, width / 2],
        [-length / 2 + 0.1, -width / 2]
    ];

    // Add middle legs for longer conveyors
    if (length > 1.5) {
        legPositions.push([0, width / 2]);
        legPositions.push([0, -width / 2]);
    }

    legPositions.forEach(([x, z]) => {
        const leg = new THREE.Mesh(legGeometry, legMaterial);
        leg.position.set(x, height / 2 - 0.01, z);
        leg.castShadow = true;
        conveyorGroup.add(leg);
    });

    // Cross braces
    const braceGeometry = new THREE.BoxGeometry(0.02, 0.02, width);

    legPositions.filter((_, i) => i % 2 === 0).forEach(([x, _], index) => {
        if (index < legPositions.length / 2) {
            const brace = new THREE.Mesh(braceGeometry, legMaterial);
            brace.position.set(x, 0.15, 0);
            conveyorGroup.add(brace);
        }
    });

    // Motor (if powered)
    if (config.driveType === 'powered') {
        const motorBodyGeometry = new THREE.CylinderGeometry(0.08, 0.08, 0.15, 16);
        const motorBody = new THREE.Mesh(motorBodyGeometry, motorMaterial);
        motorBody.rotation.x = Math.PI / 2;
        motorBody.position.set(length / 2 - 0.1, height - 0.15, -width / 2 - 0.15);
        motorBody.castShadow = true;
        conveyorGroup.add(motorBody);

        // Motor mount
        const mountGeometry = new THREE.BoxGeometry(0.1, 0.08, 0.04);
        const mount = new THREE.Mesh(mountGeometry, frameMaterial);
        mount.position.set(length / 2 - 0.1, height - 0.1, -width / 2 - 0.05);
        conveyorGroup.add(mount);
    }

    // Center conveyor in view
    conveyorGroup.position.set(0, 0, 0);

    // Update specifications
    updateRollerSpecs(numRollers);
}

// Build overhead conveyor
function buildOverheadConveyor() {
    // Clear existing
    while (conveyorGroup.children.length > 0) {
        const child = conveyorGroup.children[0];
        conveyorGroup.remove(child);
        if (child.geometry) child.geometry.dispose();
    }

    const trackLength = overheadConfig.trackLength * SCALE;
    const heightFromFloor = overheadConfig.heightFromFloor * SCALE;
    const carrierSpacing = overheadConfig.carrierSpacing * SCALE;
    const numCarriers = overheadConfig.numCarriers;
    const curveRadius = overheadConfig.curveRadius * SCALE;
    const includeCurves = overheadConfig.includeCurves;
    const inclineAngle = overheadConfig.inclineAngle * Math.PI / 180;
    const declineAngle = overheadConfig.declineAngle * Math.PI / 180;

    // Track I-beam profile dimensions
    const trackWidth = 0.1;   // 100mm
    const trackHeight = 0.08; // 80mm
    const flangeThickness = 0.008; // 8mm
    const webThickness = 0.006;    // 6mm

    // Calculate track sections
    let straightLength = trackLength;
    if (includeCurves) {
        // Leave room for two 90-degree curves
        straightLength = (trackLength - Math.PI * curveRadius) / 2;
    }

    // Build I-beam track
    function createIBeamSection(length) {
        const group = new THREE.Group();

        // Top flange
        const topFlangeGeo = new THREE.BoxGeometry(length, flangeThickness, trackWidth);
        const topFlange = new THREE.Mesh(topFlangeGeo, trackMaterial);
        topFlange.position.y = trackHeight / 2 - flangeThickness / 2;
        topFlange.castShadow = true;
        group.add(topFlange);

        // Bottom flange
        const bottomFlangeGeo = new THREE.BoxGeometry(length, flangeThickness, trackWidth);
        const bottomFlange = new THREE.Mesh(bottomFlangeGeo, trackMaterial);
        bottomFlange.position.y = -trackHeight / 2 + flangeThickness / 2;
        bottomFlange.castShadow = true;
        group.add(bottomFlange);

        // Web
        const webGeo = new THREE.BoxGeometry(length, trackHeight - flangeThickness * 2, webThickness);
        const web = new THREE.Mesh(webGeo, trackMaterial);
        web.castShadow = true;
        group.add(web);

        return group;
    }

    // Create curved track section
    function createCurvedTrack(radius, angle) {
        const group = new THREE.Group();
        const segments = 16;

        for (let i = 0; i < segments; i++) {
            const segmentAngle = angle / segments;
            const currentAngle = i * segmentAngle;

            // Small straight segment approximation
            const segLength = radius * segmentAngle;
            const segment = createIBeamSection(segLength);

            // Position along arc
            const midAngle = currentAngle + segmentAngle / 2;
            segment.position.x = radius * Math.sin(midAngle);
            segment.position.z = radius * (1 - Math.cos(midAngle));
            segment.rotation.y = -midAngle;

            group.add(segment);
        }

        return group;
    }

    // Main straight section 1
    const section1 = createIBeamSection(straightLength);
    section1.position.set(-straightLength / 2, heightFromFloor, 0);

    // Apply incline if set
    if (inclineAngle > 0) {
        section1.rotation.z = inclineAngle;
        section1.position.y = heightFromFloor - straightLength / 2 * Math.sin(inclineAngle);
    }
    conveyorGroup.add(section1);

    if (includeCurves) {
        // First curve (90 degrees)
        const curve1 = createCurvedTrack(curveRadius, Math.PI / 2);
        curve1.position.set(-straightLength, heightFromFloor, 0);
        curve1.rotation.y = Math.PI;
        conveyorGroup.add(curve1);

        // Second straight section (perpendicular)
        const section2 = createIBeamSection(straightLength);
        section2.position.set(-straightLength - curveRadius, heightFromFloor, curveRadius + straightLength / 2);
        section2.rotation.y = Math.PI / 2;

        // Apply decline if set
        if (declineAngle > 0) {
            section2.rotation.x = -declineAngle;
        }
        conveyorGroup.add(section2);

        // Second curve
        const curve2 = createCurvedTrack(curveRadius, Math.PI / 2);
        curve2.position.set(-straightLength - curveRadius, heightFromFloor, curveRadius + straightLength);
        curve2.rotation.y = Math.PI / 2;
        conveyorGroup.add(curve2);
    } else if (declineAngle > 0) {
        // Add decline section if no curves but decline angle set
        const declineLength = straightLength * 0.3;
        const declineSection = createIBeamSection(declineLength);
        declineSection.position.set(-straightLength - declineLength / 2, heightFromFloor, 0);
        declineSection.rotation.z = -declineAngle;
        conveyorGroup.add(declineSection);
    }

    // Add trolleys and carriers along the track
    const dropRodLength = 0.35; // 350mm drop rod
    const trolleyGeo = new THREE.BoxGeometry(0.06, 0.03, 0.05);
    const wheelGeo = new THREE.CylinderGeometry(0.012, 0.012, 0.015, 12);
    const dropRodGeo = new THREE.CylinderGeometry(0.01, 0.01, dropRodLength, 8);

    // J-hook carrier shape
    function createCarrierHook() {
        const group = new THREE.Group();

        // Vertical part
        const vertGeo = new THREE.BoxGeometry(0.02, 0.15, 0.02);
        const vert = new THREE.Mesh(vertGeo, carrierMaterial);
        vert.position.y = -0.075;
        group.add(vert);

        // Curved hook part (approximated with box)
        const hookGeo = new THREE.BoxGeometry(0.06, 0.02, 0.02);
        const hook = new THREE.Mesh(hookGeo, carrierMaterial);
        hook.position.set(0.03, -0.16, 0);
        group.add(hook);

        // Hook tip
        const tipGeo = new THREE.BoxGeometry(0.02, 0.04, 0.02);
        const tip = new THREE.Mesh(tipGeo, carrierMaterial);
        tip.position.set(0.06, -0.14, 0);
        group.add(tip);

        return group;
    }

    // Place carriers along straight section
    const carriersOnSection1 = Math.min(numCarriers, Math.floor(straightLength / carrierSpacing));
    for (let i = 0; i < carriersOnSection1; i++) {
        const xPos = -carrierSpacing / 2 - i * carrierSpacing;
        let yPos = heightFromFloor;

        // Adjust for incline
        if (inclineAngle > 0) {
            const distFromStart = straightLength / 2 + xPos;
            yPos = heightFromFloor - straightLength / 2 * Math.sin(inclineAngle) + distFromStart * Math.sin(inclineAngle);
        }

        // Trolley
        const trolley = new THREE.Mesh(trolleyGeo, trolleyMaterial);
        trolley.position.set(xPos, yPos - trackHeight / 2 - 0.015, 0);
        trolley.castShadow = true;
        conveyorGroup.add(trolley);

        // Wheels (4 per trolley)
        const wheelPositions = [
            [-0.02, 0.01, 0.02], [0.02, 0.01, 0.02],
            [-0.02, 0.01, -0.02], [0.02, 0.01, -0.02]
        ];
        wheelPositions.forEach(([wx, wy, wz]) => {
            const wheel = new THREE.Mesh(wheelGeo, frameMaterial);
            wheel.rotation.x = Math.PI / 2;
            wheel.position.set(xPos + wx, yPos - trackHeight / 2 - 0.015 + wy, wz);
            conveyorGroup.add(wheel);
        });

        // Drop rod
        const dropRod = new THREE.Mesh(dropRodGeo, dropRodMaterial);
        dropRod.position.set(xPos, yPos - trackHeight / 2 - 0.03 - dropRodLength / 2, 0);
        dropRod.castShadow = true;
        conveyorGroup.add(dropRod);

        // Carrier hook
        const carrier = createCarrierHook();
        carrier.position.set(xPos, yPos - trackHeight / 2 - 0.03 - dropRodLength, 0);
        conveyorGroup.add(carrier);
    }

    // Add drive unit(s)
    const driveUnitGeo = new THREE.BoxGeometry(0.3, 0.2, 0.2);
    const motorGeo = new THREE.CylinderGeometry(0.06, 0.06, 0.15, 16);

    for (let i = 0; i < overheadConfig.driveUnits; i++) {
        const drivePos = i === 0 ? 0 : -straightLength * (i / overheadConfig.driveUnits);

        // Drive housing
        const driveUnit = new THREE.Mesh(driveUnitGeo, frameMaterial);
        driveUnit.position.set(drivePos, heightFromFloor + 0.15, -0.15);
        driveUnit.castShadow = true;
        conveyorGroup.add(driveUnit);

        // Motor
        const motor = new THREE.Mesh(motorGeo, motorMaterial);
        motor.rotation.z = Math.PI / 2;
        motor.position.set(drivePos, heightFromFloor + 0.15, -0.32);
        motor.castShadow = true;
        conveyorGroup.add(motor);
    }

    // Add support columns
    const columnGeo = new THREE.BoxGeometry(0.06, heightFromFloor - 0.1, 0.06);
    const columnPositions = [0, -straightLength / 2, -straightLength];

    columnPositions.forEach(xPos => {
        const column = new THREE.Mesh(columnGeo, legMaterial);
        column.position.set(xPos, (heightFromFloor - 0.1) / 2, 0.15);
        column.castShadow = true;
        conveyorGroup.add(column);

        // Second column on other side
        const column2 = new THREE.Mesh(columnGeo, legMaterial);
        column2.position.set(xPos, (heightFromFloor - 0.1) / 2, -0.15);
        column2.castShadow = true;
        conveyorGroup.add(column2);

        // Cross brace
        const braceGeo = new THREE.BoxGeometry(0.04, 0.04, 0.34);
        const brace = new THREE.Mesh(braceGeo, legMaterial);
        brace.position.set(xPos, 0.3, 0);
        conveyorGroup.add(brace);
    });

    // Center the conveyor group
    const box = new THREE.Box3().setFromObject(conveyorGroup);
    const center = box.getCenter(new THREE.Vector3());
    conveyorGroup.position.x = -center.x;
    conveyorGroup.position.z = -center.z;

    // Update specifications
    updateOverheadSpecs();
}

// Update roller specifications display
function updateRollerSpecs(numRollers) {
    document.getElementById('spec-rollers').textContent = numRollers;

    // Estimate weight (simplified calculation)
    const frameWeight = (config.length / 1000) * 8 * 2;
    const rollerWeight = numRollers * 0.8;
    const legWeight = 6 * 2;
    const motorWeight = config.driveType === 'powered' ? 15 : 0;
    const totalWeight = Math.round(frameWeight + rollerWeight + legWeight + motorWeight);

    document.getElementById('spec-weight').textContent = `${totalWeight} kg`;

    // Material based on load capacity
    const load = parseInt(config.loadCapacity);
    if (load >= 500) {
        document.getElementById('spec-material').textContent = 'Heavy Steel';
        document.getElementById('spec-roller-material').textContent = 'Steel';
    } else if (load >= 300) {
        document.getElementById('spec-material').textContent = 'Steel';
        document.getElementById('spec-roller-material').textContent = 'Galvanized';
    } else {
        document.getElementById('spec-material').textContent = 'Light Steel';
        document.getElementById('spec-roller-material').textContent = 'Aluminum';
    }
}

// Update overhead specifications display
function updateOverheadSpecs() {
    const numCarriers = overheadConfig.numCarriers;
    const trackLength = overheadConfig.trackLength;
    const loadPerCarrier = overheadConfig.loadPerCarrier;

    document.getElementById('spec-carriers').textContent = numCarriers;
    document.getElementById('spec-track-length').textContent = `${(trackLength / 1000).toFixed(1)} m`;

    // Estimate weight
    const trackWeight = (trackLength / 1000) * 15; // ~15 kg per meter of track
    const carrierWeight = numCarriers * 8; // ~8 kg per carrier assembly
    const driveWeight = overheadConfig.driveUnits * 25; // ~25 kg per drive unit
    const supportWeight = Math.ceil(trackLength / 3000) * 20; // ~20 kg per support
    const totalWeight = Math.round(trackWeight + carrierWeight + driveWeight + supportWeight);

    document.getElementById('spec-overhead-weight').textContent = `${totalWeight} kg`;
    document.getElementById('spec-total-load').textContent = `${numCarriers * loadPerCarrier} kg`;
}

// Update UI from config (roller)
function updateUIFromConfig() {
    const inputs = ['length', 'width', 'height', 'rollerDiameter', 'rollerSpacing'];
    inputs.forEach(id => {
        const input = document.getElementById(id);
        const output = document.getElementById(`${id}-value`);
        if (input && output) {
            input.value = config[id];
            output.textContent = config[id];
        }
    });

    document.getElementById('loadCapacity').value = config.loadCapacity;
    document.getElementById('driveType').value = config.driveType;
}

// Update UI from overhead config
function updateOverheadUIFromConfig() {
    const inputs = ['trackLength', 'heightFromFloor', 'carrierSpacing', 'numCarriers', 'loadPerCarrier', 'curveRadius', 'inclineAngle', 'declineAngle', 'driveUnits'];
    inputs.forEach(id => {
        const input = document.getElementById(id);
        const output = document.getElementById(`${id}-value`);
        if (input && output) {
            input.value = overheadConfig[id];
            output.textContent = overheadConfig[id];
        }
    });

    document.getElementById('trackProfile').value = overheadConfig.trackProfile;
    document.getElementById('includeCurves').checked = overheadConfig.includeCurves;
    document.getElementById('curveRadius-group').style.display = overheadConfig.includeCurves ? 'block' : 'none';
}

// Get config summary string
function getConfigSummary() {
    if (activeType === 'overhead') {
        return `Overhead Conveyor Configuration:
- Track Length: ${overheadConfig.trackLength} mm
- Height from Floor: ${overheadConfig.heightFromFloor} mm
- Track Profile: ${overheadConfig.trackProfile}
- Carrier Spacing: ${overheadConfig.carrierSpacing} mm
- Number of Carriers: ${overheadConfig.numCarriers}
- Load per Carrier: ${overheadConfig.loadPerCarrier} kg
- Include Curves: ${overheadConfig.includeCurves}
- Curve Radius: ${overheadConfig.curveRadius} mm
- Incline Angle: ${overheadConfig.inclineAngle}°
- Decline Angle: ${overheadConfig.declineAngle}°
- Drive Units: ${overheadConfig.driveUnits}
- Total Load Capacity: ${overheadConfig.numCarriers * overheadConfig.loadPerCarrier} kg`;
    }

    return `Roller Conveyor Configuration:
- Length: ${config.length} mm
- Width: ${config.width} mm
- Height: ${config.height} mm
- Roller Diameter: ${config.rollerDiameter} mm
- Roller Spacing: ${config.rollerSpacing} mm
- Load Capacity: ${config.loadCapacity} kg
- Drive Type: ${config.driveType}
- Rollers: ${Math.floor(config.length / config.rollerSpacing)}`;
}

// Parse CSV - detects roller vs overhead based on headers
function parseCSV(text) {
    const lines = text.trim().split('\n');
    if (lines.length < 2) return [];

    const headers = lines[0].split(',').map(h => h.trim().toLowerCase());
    const configs = [];

    // Detect if this is an overhead config based on headers
    const isOverhead = headers.some(h => h.includes('track') || h.includes('carrier') || h.includes('heightfromfloor'));

    for (let i = 1; i < lines.length; i++) {
        const values = lines[i].split(',').map(v => v.trim());
        const cfg = { type: isOverhead ? 'overhead' : 'roller' };

        headers.forEach((header, idx) => {
            const value = values[idx];

            if (isOverhead) {
                // Overhead config mapping
                if (header.includes('tracklength') || header === 'length') cfg.trackLength = parseInt(value) || 10000;
                else if (header.includes('heightfromfloor') || header.includes('height')) cfg.heightFromFloor = parseInt(value) || 3000;
                else if (header.includes('carrierspacing') || header.includes('spacing')) cfg.carrierSpacing = parseInt(value) || 1000;
                else if (header.includes('loadpercarrier') || header.includes('load')) cfg.loadPerCarrier = parseInt(value) || 50;
                else if (header.includes('numcarriers') || header.includes('carriers')) cfg.numCarriers = parseInt(value) || 10;
                else if (header.includes('includecurves') || header.includes('curves')) cfg.includeCurves = value === 'true' || value === '1';
                else if (header.includes('curveradius')) cfg.curveRadius = parseInt(value) || 500;
                else if (header.includes('incline')) cfg.inclineAngle = parseInt(value) || 0;
                else if (header.includes('decline')) cfg.declineAngle = parseInt(value) || 0;
                else if (header.includes('driveunits') || header.includes('drive')) cfg.driveUnits = parseInt(value) || 1;
                else if (header.includes('name') || header.includes('id')) cfg.name = value;
            } else {
                // Roller config mapping
                if (header.includes('length')) cfg.length = parseInt(value) || 2000;
                else if (header.includes('width')) cfg.width = parseInt(value) || 600;
                else if (header.includes('height')) cfg.height = parseInt(value) || 750;
                else if (header.includes('diameter')) cfg.rollerDiameter = parseInt(value) || 50;
                else if (header.includes('spacing')) cfg.rollerSpacing = parseInt(value) || 100;
                else if (header.includes('load') || header.includes('capacity')) cfg.loadCapacity = parseInt(value) || 300;
                else if (header.includes('drive') || header.includes('type')) cfg.driveType = value || 'powered';
                else if (header.includes('name') || header.includes('id')) cfg.name = value;
            }
        });

        cfg.name = cfg.name || `Config ${i}`;
        configs.push(cfg);
    }

    return configs;
}

// Generate CSV from current config
function generateCSV() {
    if (activeType === 'overhead') {
        const headers = 'name,trackLength,heightFromFloor,carrierSpacing,loadPerCarrier,numCarriers,includeCurves,curveRadius,inclineAngle,declineAngle,driveUnits';
        const values = `Current,${overheadConfig.trackLength},${overheadConfig.heightFromFloor},${overheadConfig.carrierSpacing},${overheadConfig.loadPerCarrier},${overheadConfig.numCarriers},${overheadConfig.includeCurves},${overheadConfig.curveRadius},${overheadConfig.inclineAngle},${overheadConfig.declineAngle},${overheadConfig.driveUnits}`;
        return `${headers}\n${values}`;
    }

    const headers = 'name,length,width,height,rollerDiameter,rollerSpacing,loadCapacity,driveType';
    const values = `Current,${config.length},${config.width},${config.height},${config.rollerDiameter},${config.rollerSpacing},${config.loadCapacity},${config.driveType}`;
    return `${headers}\n${values}`;
}

// Show import status
function showImportStatus(message, isError = false) {
    const status = document.getElementById('import-status');
    status.textContent = message;
    status.className = isError ? 'import-status error' : 'import-status';
    setTimeout(() => { status.textContent = ''; }, 3000);
}

// Show/hide loading overlay
function setLoading(show) {
    document.getElementById('loading-overlay').style.display = show ? 'flex' : 'none';
}

// Clear imported model
function clearImportedModel() {
    if (importedModel) {
        scene.remove(importedModel);
        importedModel.traverse(child => {
            if (child.geometry) child.geometry.dispose();
            if (child.material) {
                if (Array.isArray(child.material)) {
                    child.material.forEach(m => m.dispose());
                } else {
                    child.material.dispose();
                }
            }
        });
        importedModel = null;
    }
}

// Switch conveyor type
function switchType(type) {
    activeType = type;

    // Update button states
    document.getElementById('type-roller').classList.toggle('active', type === 'roller');
    document.getElementById('type-overhead').classList.toggle('active', type === 'overhead');

    // Update body class for CSS visibility
    document.body.classList.remove('mode-roller', 'mode-overhead');
    document.body.classList.add(`mode-${type}`);

    // Clear imported model and rebuild
    clearImportedModel();
    buildConveyor();
}

// Input handlers
function setupInputs() {
    // Type toggle buttons
    document.getElementById('type-roller').addEventListener('click', () => switchType('roller'));
    document.getElementById('type-overhead').addEventListener('click', () => switchType('overhead'));

    // Initialize body class
    document.body.classList.add('mode-roller');

    // Roller conveyor inputs
    const rollerInputs = ['length', 'width', 'height', 'rollerDiameter', 'rollerSpacing'];

    rollerInputs.forEach(id => {
        const input = document.getElementById(id);
        const output = document.getElementById(`${id}-value`);

        input.addEventListener('input', () => {
            config[id] = parseInt(input.value);
            output.textContent = input.value;
            clearImportedModel();
            buildConveyor();
        });
    });

    // Roller selects
    document.getElementById('loadCapacity').addEventListener('change', (e) => {
        config.loadCapacity = parseInt(e.target.value);
        clearImportedModel();
        buildConveyor();
    });

    document.getElementById('driveType').addEventListener('change', (e) => {
        config.driveType = e.target.value;
        clearImportedModel();
        buildConveyor();
    });

    // Overhead conveyor inputs
    const overheadInputs = ['trackLength', 'heightFromFloor', 'carrierSpacing', 'numCarriers', 'loadPerCarrier', 'curveRadius', 'inclineAngle', 'declineAngle', 'driveUnits'];

    overheadInputs.forEach(id => {
        const input = document.getElementById(id);
        const output = document.getElementById(`${id}-value`);

        if (input && output) {
            input.addEventListener('input', () => {
                overheadConfig[id] = parseInt(input.value);
                output.textContent = input.value;
                clearImportedModel();
                buildConveyor();
            });
        }
    });

    // Overhead selects and checkboxes
    document.getElementById('trackProfile').addEventListener('change', (e) => {
        overheadConfig.trackProfile = e.target.value;
        clearImportedModel();
        buildConveyor();
    });

    document.getElementById('includeCurves').addEventListener('change', (e) => {
        overheadConfig.includeCurves = e.target.checked;
        document.getElementById('curveRadius-group').style.display = e.target.checked ? 'block' : 'none';
        clearImportedModel();
        buildConveyor();
    });

    // CSV Upload
    document.getElementById('csv-upload').addEventListener('change', async (e) => {
        const file = e.target.files[0];
        if (!file) return;

        try {
            const text = await file.text();
            loadedConfigs = parseCSV(text);

            if (loadedConfigs.length > 0) {
                const configType = loadedConfigs[0].type || 'roller';
                showImportStatus(`Loaded ${loadedConfigs.length} ${configType} configurations`);

                // Switch to the appropriate type
                if (configType === 'overhead' && activeType !== 'overhead') {
                    switchType('overhead');
                } else if (configType === 'roller' && activeType !== 'roller') {
                    switchType('roller');
                }

                // Show config list
                const section = document.getElementById('config-list-section');
                const select = document.getElementById('config-select');
                section.style.display = 'block';

                select.innerHTML = '';
                loadedConfigs.forEach((cfg, idx) => {
                    const option = document.createElement('option');
                    option.value = idx;
                    if (cfg.type === 'overhead') {
                        option.textContent = `${cfg.name} (${cfg.trackLength}mm track, ${cfg.numCarriers} carriers)`;
                    } else {
                        option.textContent = `${cfg.name} (${cfg.length}x${cfg.width}mm)`;
                    }
                    select.appendChild(option);
                });
            } else {
                showImportStatus('No valid configurations found', true);
            }
        } catch (err) {
            showImportStatus('Error reading CSV file', true);
            console.error(err);
        }

        e.target.value = '';
    });

    // Load selected config
    document.getElementById('load-config-btn').addEventListener('click', () => {
        const select = document.getElementById('config-select');
        const idx = parseInt(select.value);
        if (isNaN(idx) || !loadedConfigs[idx]) return;

        const cfg = loadedConfigs[idx];

        if (cfg.type === 'overhead') {
            Object.assign(overheadConfig, {
                trackLength: cfg.trackLength || overheadConfig.trackLength,
                heightFromFloor: cfg.heightFromFloor || overheadConfig.heightFromFloor,
                carrierSpacing: cfg.carrierSpacing || overheadConfig.carrierSpacing,
                loadPerCarrier: cfg.loadPerCarrier || overheadConfig.loadPerCarrier,
                numCarriers: cfg.numCarriers || overheadConfig.numCarriers,
                includeCurves: cfg.includeCurves !== undefined ? cfg.includeCurves : overheadConfig.includeCurves,
                curveRadius: cfg.curveRadius || overheadConfig.curveRadius,
                inclineAngle: cfg.inclineAngle || overheadConfig.inclineAngle,
                declineAngle: cfg.declineAngle || overheadConfig.declineAngle,
                driveUnits: cfg.driveUnits || overheadConfig.driveUnits
            });
            updateOverheadUIFromConfig();
        } else {
            Object.assign(config, {
                length: cfg.length || config.length,
                width: cfg.width || config.width,
                height: cfg.height || config.height,
                rollerDiameter: cfg.rollerDiameter || config.rollerDiameter,
                rollerSpacing: cfg.rollerSpacing || config.rollerSpacing,
                loadCapacity: cfg.loadCapacity || config.loadCapacity,
                driveType: cfg.driveType || config.driveType
            });
            updateUIFromConfig();
        }

        clearImportedModel();
        buildConveyor();
        showImportStatus(`Applied: ${cfg.name}`);
    });

    // CAD Upload
    document.getElementById('cad-upload').addEventListener('change', async (e) => {
        const file = e.target.files[0];
        if (!file) return;

        setLoading(true);

        try {
            // For PoC: Send to backend for parsing, or use client-side STEP parser
            // Using occt-import-js for client-side STEP parsing
            const arrayBuffer = await file.arrayBuffer();

            // Dynamic import of occt-import-js
            const occt = await import('https://cdn.jsdelivr.net/npm/occt-import-js@0.0.12/dist/occt-import-js.js');
            await occt.default();

            const result = occt.ReadStepFile(new Uint8Array(arrayBuffer), null);

            if (result.success && result.meshes.length > 0) {
                clearImportedModel();

                // Clear conveyor
                while (conveyorGroup.children.length > 0) {
                    const child = conveyorGroup.children[0];
                    conveyorGroup.remove(child);
                    if (child.geometry) child.geometry.dispose();
                }

                importedModel = new THREE.Group();

                result.meshes.forEach(mesh => {
                    const geometry = new THREE.BufferGeometry();

                    geometry.setAttribute('position',
                        new THREE.Float32BufferAttribute(mesh.attributes.position.array, 3)
                    );

                    if (mesh.attributes.normal) {
                        geometry.setAttribute('normal',
                            new THREE.Float32BufferAttribute(mesh.attributes.normal.array, 3)
                        );
                    } else {
                        geometry.computeVertexNormals();
                    }

                    if (mesh.index) {
                        geometry.setIndex(new THREE.BufferAttribute(mesh.index.array, 1));
                    }

                    const material = new THREE.MeshStandardMaterial({
                        color: mesh.color ? new THREE.Color(mesh.color[0], mesh.color[1], mesh.color[2]) : 0x718096,
                        roughness: 0.5,
                        metalness: 0.7
                    });

                    const threeMesh = new THREE.Mesh(geometry, material);
                    threeMesh.castShadow = true;
                    threeMesh.receiveShadow = true;
                    importedModel.add(threeMesh);
                });

                // Center and scale the model
                const box = new THREE.Box3().setFromObject(importedModel);
                const center = box.getCenter(new THREE.Vector3());
                const size = box.getSize(new THREE.Vector3());
                const maxDim = Math.max(size.x, size.y, size.z);
                const scale = 2 / maxDim;

                importedModel.scale.setScalar(scale);
                importedModel.position.sub(center.multiplyScalar(scale));

                scene.add(importedModel);
                showImportStatus(`Loaded: ${file.name}`);
            } else {
                showImportStatus('Could not parse CAD file', true);
            }
        } catch (err) {
            console.error('CAD import error:', err);
            showImportStatus('Error loading CAD file. Try STEP format.', true);
        }

        setLoading(false);
        e.target.value = '';
    });

    // Export STEP button
    document.getElementById('export-btn').addEventListener('click', async () => {
        const btn = document.getElementById('export-btn');
        btn.disabled = true;
        btn.textContent = 'Generating...';

        try {
            // Determine endpoint and config based on active type
            const endpoint = activeType === 'overhead' ? `${API_URL}/export/overhead-step` : `${API_URL}/export/step`;
            const exportConfig = activeType === 'overhead' ? overheadConfig : config;
            const filename = activeType === 'overhead' ? 'overhead-conveyor.step' : 'conveyor.step';

            // Try to call backend API
            const response = await fetch(endpoint, {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(exportConfig)
            });

            if (response.ok) {
                const blob = await response.blob();
                const url = URL.createObjectURL(blob);
                const a = document.createElement('a');
                a.href = url;
                a.download = filename;
                a.click();
                URL.revokeObjectURL(url);
            } else {
                throw new Error('Backend not available');
            }
        } catch (err) {
            // Fallback: download JSON config
            console.log('Backend not available, downloading JSON config');
            const exportConfig = activeType === 'overhead' ? overheadConfig : config;
            const filename = activeType === 'overhead' ? 'overhead-conveyor-config.json' : 'conveyor-config.json';
            const blob = new Blob([JSON.stringify(exportConfig, null, 2)], { type: 'application/json' });
            const url = URL.createObjectURL(blob);
            const a = document.createElement('a');
            a.href = url;
            a.download = filename;
            a.click();
            URL.revokeObjectURL(url);

            alert('Backend not running. Downloaded JSON config instead.\n\nTo enable STEP export, start the .NET backend.');
        }

        btn.disabled = false;
        btn.textContent = 'Export STEP File';
    });

    // Export CSV button
    document.getElementById('export-csv-btn').addEventListener('click', () => {
        const csv = generateCSV();
        const filename = activeType === 'overhead' ? 'overhead-conveyor-config.csv' : 'conveyor-config.csv';
        const blob = new Blob([csv], { type: 'text/csv' });
        const url = URL.createObjectURL(blob);
        const a = document.createElement('a');
        a.href = url;
        a.download = filename;
        a.click();
        URL.revokeObjectURL(url);
    });

    // Quote modal
    const modal = document.getElementById('quote-modal');
    const quoteBtn = document.getElementById('quote-btn');
    const closeBtn = document.getElementById('modal-close');
    const quoteForm = document.getElementById('quote-form');

    quoteBtn.addEventListener('click', () => {
        document.getElementById('config-summary').textContent = getConfigSummary();
        modal.style.display = 'flex';
    });

    closeBtn.addEventListener('click', () => {
        modal.style.display = 'none';
    });

    modal.addEventListener('click', (e) => {
        if (e.target === modal) modal.style.display = 'none';
    });

    quoteForm.addEventListener('submit', async (e) => {
        e.preventDefault();

        const quoteData = {
            company: document.getElementById('company-name').value,
            contact: document.getElementById('contact-name').value,
            email: document.getElementById('email').value,
            phone: document.getElementById('phone').value,
            quantity: parseInt(document.getElementById('quantity').value) || 1,
            notes: document.getElementById('notes').value,
            conveyorType: activeType,
            rollerConfiguration: activeType === 'roller' ? config : null,
            overheadConfiguration: activeType === 'overhead' ? overheadConfig : null,
            timestamp: new Date().toISOString()
        };

        const submitBtn = quoteForm.querySelector('button[type="submit"]');
        submitBtn.disabled = true;
        submitBtn.textContent = 'Submitting...';

        try {
            // Try to submit to backend
            const response = await fetch(`${API_URL}/quotes`, {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(quoteData)
            });

            if (response.ok) {
                alert('Quote request submitted successfully! We will contact you soon.');
                modal.style.display = 'none';
                quoteForm.reset();
            } else {
                throw new Error('Backend not available');
            }
        } catch (err) {
            // Fallback: save locally
            console.log('Quote data:', quoteData);

            // Store in localStorage as backup
            const quotes = JSON.parse(localStorage.getItem('pendingQuotes') || '[]');
            quotes.push(quoteData);
            localStorage.setItem('pendingQuotes', JSON.stringify(quotes));

            alert('Backend not running. Quote saved locally.\n\nTo enable quote submission, start the .NET backend.');
            modal.style.display = 'none';
            quoteForm.reset();
        }

        submitBtn.disabled = false;
        submitBtn.textContent = 'Submit Quote Request';
    });
}

// Animation loop
function animate() {
    requestAnimationFrame(animate);
    controls.update();

    // Slowly rotate imported models for showcase
    if (importedModel) {
        importedModel.rotation.y += 0.002;
    }

    renderer.render(scene, camera);
}

// Handle window resize
function onResize() {
    camera.aspect = canvas.clientWidth / canvas.clientHeight;
    camera.updateProjectionMatrix();
    renderer.setSize(canvas.clientWidth, canvas.clientHeight);
}

window.addEventListener('resize', onResize);

// Initialize
setupInputs();
buildConveyor();
animate();
