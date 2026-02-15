// Three.js Scene Manager for Blazor Interop
import * as THREE from 'https://cdn.jsdelivr.net/npm/three@0.160.0/build/three.module.js';
import { OrbitControls } from 'https://cdn.jsdelivr.net/npm/three@0.160.0/examples/jsm/controls/OrbitControls.js';

const SCALE = 0.001; // mm to Three.js units

let scene, camera, renderer, controls;
let conveyorGroup;
let container;

export function initScene(containerId) {
    container = document.getElementById(containerId);
    if (!container) {
        console.error(`Container ${containerId} not found`);
        return;
    }

    // Scene
    scene = new THREE.Scene();
    scene.background = new THREE.Color(0x1a1a2e);

    // Camera
    const aspect = container.clientWidth / container.clientHeight;
    camera = new THREE.PerspectiveCamera(60, aspect, 0.1, 1000);
    camera.position.set(3, 2, 3);

    // Renderer
    renderer = new THREE.WebGLRenderer({ antialias: true });
    renderer.setSize(container.clientWidth, container.clientHeight);
    renderer.setPixelRatio(window.devicePixelRatio);
    renderer.shadowMap.enabled = true;
    renderer.shadowMap.type = THREE.PCFSoftShadowMap;
    container.appendChild(renderer.domElement);

    // Controls
    controls = new OrbitControls(camera, renderer.domElement);
    controls.enableDamping = true;
    controls.dampingFactor = 0.05;

    // Lights
    const ambientLight = new THREE.AmbientLight(0xffffff, 0.4);
    scene.add(ambientLight);

    const directionalLight = new THREE.DirectionalLight(0xffffff, 0.8);
    directionalLight.position.set(5, 10, 5);
    directionalLight.castShadow = true;
    directionalLight.shadow.mapSize.width = 2048;
    directionalLight.shadow.mapSize.height = 2048;
    scene.add(directionalLight);

    // Grid
    const gridHelper = new THREE.GridHelper(10, 20, 0x444444, 0x222222);
    scene.add(gridHelper);

    // Conveyor group
    conveyorGroup = new THREE.Group();
    scene.add(conveyorGroup);

    // Animation loop
    function animate() {
        requestAnimationFrame(animate);
        controls.update();
        renderer.render(scene, camera);
    }
    animate();

    // Handle resize
    window.addEventListener('resize', resize);
}

export function resize() {
    if (!container || !camera || !renderer) return;
    const width = container.clientWidth;
    const height = container.clientHeight;
    camera.aspect = width / height;
    camera.updateProjectionMatrix();
    renderer.setSize(width, height);
}

export function clearScene() {
    if (!conveyorGroup) return;
    while (conveyorGroup.children.length > 0) {
        const child = conveyorGroup.children[0];
        if (child.geometry) child.geometry.dispose();
        if (child.material) {
            if (Array.isArray(child.material)) {
                child.material.forEach(m => m.dispose());
            } else {
                child.material.dispose();
            }
        }
        conveyorGroup.remove(child);
    }
}

export function buildRollerConveyor(config) {
    clearScene();

    const { length, width, height, rollerDiameter, rollerSpacing, driveType } = config;
    const l = length * SCALE;
    const w = width * SCALE;
    const h = height * SCALE;
    const rd = rollerDiameter * SCALE;
    const rs = rollerSpacing * SCALE;

    // Frame material
    const frameMaterial = new THREE.MeshStandardMaterial({
        color: 0x4a5568,
        metalness: 0.7,
        roughness: 0.3
    });

    // Side frames
    const frameGeometry = new THREE.BoxGeometry(l, 0.05, 0.05);
    const leftFrame = new THREE.Mesh(frameGeometry, frameMaterial);
    leftFrame.position.set(l / 2, h, w / 2);
    leftFrame.castShadow = true;
    conveyorGroup.add(leftFrame);

    const rightFrame = new THREE.Mesh(frameGeometry, frameMaterial);
    rightFrame.position.set(l / 2, h, -w / 2);
    rightFrame.castShadow = true;
    conveyorGroup.add(rightFrame);

    // Rollers
    const rollerMaterial = new THREE.MeshStandardMaterial({
        color: driveType === 'powered' ? 0x3182ce : 0x718096,
        metalness: 0.6,
        roughness: 0.4
    });

    const rollerGeometry = new THREE.CylinderGeometry(rd / 2, rd / 2, w - 0.02, 16);
    rollerGeometry.rotateX(Math.PI / 2);

    const numRollers = Math.floor(l / rs);
    for (let i = 0; i < numRollers; i++) {
        const roller = new THREE.Mesh(rollerGeometry, rollerMaterial);
        roller.position.set(rs / 2 + i * rs, h - rd / 2, 0);
        roller.castShadow = true;
        conveyorGroup.add(roller);
    }

    // Legs
    const legGeometry = new THREE.BoxGeometry(0.04, h - 0.05, 0.04);
    const legMaterial = new THREE.MeshStandardMaterial({
        color: 0x2d3748,
        metalness: 0.5,
        roughness: 0.5
    });

    const legPositions = [
        [0.1, h / 2 - 0.025, w / 2 - 0.05],
        [0.1, h / 2 - 0.025, -w / 2 + 0.05],
        [l - 0.1, h / 2 - 0.025, w / 2 - 0.05],
        [l - 0.1, h / 2 - 0.025, -w / 2 + 0.05]
    ];

    legPositions.forEach(pos => {
        const leg = new THREE.Mesh(legGeometry, legMaterial);
        leg.position.set(...pos);
        leg.castShadow = true;
        conveyorGroup.add(leg);
    });

    centerAndFitCamera();
}

export function buildOverheadConveyor(config) {
    clearScene();

    const { trackLength, heightFromFloor, carrierSpacing, numCarriers, includeCurves, curveRadius } = config;
    const tl = trackLength * SCALE;
    const hf = heightFromFloor * SCALE;
    const cs = carrierSpacing * SCALE;
    const cr = curveRadius * SCALE;

    // Track material (I-beam profile)
    const trackMaterial = new THREE.MeshStandardMaterial({
        color: 0xf6ad55,
        metalness: 0.6,
        roughness: 0.3
    });

    // Create I-beam profile shape
    const trackWidth = 0.08;
    const trackHeight = 0.06;
    const flangeThickness = 0.008;
    const webThickness = 0.006;

    // Main track (straight section)
    const trackGeometry = new THREE.BoxGeometry(tl, trackHeight, trackWidth);
    const track = new THREE.Mesh(trackGeometry, trackMaterial);
    track.position.set(tl / 2, hf, 0);
    track.castShadow = true;
    conveyorGroup.add(track);

    // Add curves if enabled
    if (includeCurves && cr > 0) {
        const curveSegments = 16;
        const curveAngle = Math.PI / 2;

        // Create curve at the end
        for (let i = 0; i < curveSegments; i++) {
            const angle = (i / curveSegments) * curveAngle;
            const nextAngle = ((i + 1) / curveSegments) * curveAngle;
            const segLength = (curveAngle / curveSegments) * cr;

            const segGeometry = new THREE.BoxGeometry(segLength, trackHeight, trackWidth);
            const segment = new THREE.Mesh(segGeometry, trackMaterial);

            const midAngle = (angle + nextAngle) / 2;
            const x = tl + Math.sin(midAngle) * cr;
            const z = cr - Math.cos(midAngle) * cr;

            segment.position.set(x, hf, z);
            segment.rotation.y = -midAngle;
            segment.castShadow = true;
            conveyorGroup.add(segment);
        }
    }

    // Trolley/Carrier material
    const trolleyMaterial = new THREE.MeshStandardMaterial({
        color: 0x48bb78,
        metalness: 0.5,
        roughness: 0.4
    });

    const hookMaterial = new THREE.MeshStandardMaterial({
        color: 0x718096,
        metalness: 0.7,
        roughness: 0.3
    });

    // Add carriers
    const actualCarriers = Math.min(numCarriers, Math.floor(tl / cs));
    for (let i = 0; i < actualCarriers; i++) {
        const xPos = cs / 2 + i * cs;

        // Trolley body
        const trolleyGeometry = new THREE.BoxGeometry(0.06, 0.04, 0.05);
        const trolley = new THREE.Mesh(trolleyGeometry, trolleyMaterial);
        trolley.position.set(xPos, hf - trackHeight / 2 - 0.02, 0);
        trolley.castShadow = true;
        conveyorGroup.add(trolley);

        // Hook/hanger
        const hookGeometry = new THREE.CylinderGeometry(0.005, 0.005, 0.15, 8);
        const hook = new THREE.Mesh(hookGeometry, hookMaterial);
        hook.position.set(xPos, hf - trackHeight / 2 - 0.04 - 0.075, 0);
        hook.castShadow = true;
        conveyorGroup.add(hook);

        // Hook curve
        const hookCurveGeometry = new THREE.TorusGeometry(0.02, 0.005, 8, 12, Math.PI);
        const hookCurve = new THREE.Mesh(hookCurveGeometry, hookMaterial);
        hookCurve.position.set(xPos, hf - trackHeight / 2 - 0.04 - 0.15, 0);
        hookCurve.rotation.x = Math.PI / 2;
        hookCurve.castShadow = true;
        conveyorGroup.add(hookCurve);
    }

    // Support brackets
    const bracketMaterial = new THREE.MeshStandardMaterial({
        color: 0x4a5568,
        metalness: 0.6,
        roughness: 0.4
    });

    const numBrackets = Math.ceil(tl / 2) + 1;
    for (let i = 0; i < numBrackets; i++) {
        const xPos = i * 2;
        if (xPos > tl) break;

        // Vertical support
        const supportGeometry = new THREE.BoxGeometry(0.04, 0.3, 0.04);
        const support = new THREE.Mesh(supportGeometry, bracketMaterial);
        support.position.set(xPos, hf + 0.15, 0);
        support.castShadow = true;
        conveyorGroup.add(support);

        // Ceiling plate
        const plateGeometry = new THREE.BoxGeometry(0.1, 0.02, 0.1);
        const plate = new THREE.Mesh(plateGeometry, bracketMaterial);
        plate.position.set(xPos, hf + 0.31, 0);
        plate.castShadow = true;
        conveyorGroup.add(plate);
    }

    centerAndFitCamera();
}

function centerAndFitCamera() {
    if (!conveyorGroup || conveyorGroup.children.length === 0) return;

    const box = new THREE.Box3().setFromObject(conveyorGroup);
    const center = box.getCenter(new THREE.Vector3());
    const size = box.getSize(new THREE.Vector3());

    const maxDim = Math.max(size.x, size.y, size.z);
    const fov = camera.fov * (Math.PI / 180);
    let cameraZ = Math.abs(maxDim / Math.sin(fov / 2)) * 0.8;

    camera.position.set(center.x + cameraZ * 0.5, center.y + cameraZ * 0.3, center.z + cameraZ * 0.5);
    controls.target.copy(center);
    controls.update();
}

export function resetCamera() {
    centerAndFitCamera();
}

export function setView(view) {
    if (!conveyorGroup || conveyorGroup.children.length === 0) return;

    const box = new THREE.Box3().setFromObject(conveyorGroup);
    const center = box.getCenter(new THREE.Vector3());
    const size = box.getSize(new THREE.Vector3());
    const maxDim = Math.max(size.x, size.y, size.z) * 1.5;

    switch (view) {
        case 'top':
            camera.position.set(center.x, center.y + maxDim, center.z);
            break;
        case 'front':
            camera.position.set(center.x, center.y, center.z + maxDim);
            break;
        case 'side':
            camera.position.set(center.x + maxDim, center.y, center.z);
            break;
        case 'iso':
        default:
            camera.position.set(center.x + maxDim * 0.5, center.y + maxDim * 0.3, center.z + maxDim * 0.5);
            break;
    }

    controls.target.copy(center);
    controls.update();
}

export function dispose() {
    window.removeEventListener('resize', resize);
    if (renderer) {
        renderer.dispose();
        container?.removeChild(renderer.domElement);
    }
    if (controls) controls.dispose();
    clearScene();
    scene = null;
    camera = null;
    renderer = null;
    controls = null;
    conveyorGroup = null;
}

// Expose functions to window for Blazor interop
window.threeScene = {
    initScene,
    clearScene,
    buildRollerConveyor,
    buildOverheadConveyor,
    resetCamera,
    setView,
    dispose,
    resize
};
