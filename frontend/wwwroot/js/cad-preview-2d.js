// 2D CAD Preview Renderer
export function resizeCadCanvas(canvas) {
    const parent = canvas.parentElement;
    if (!parent) return;
    canvas.width  = parent.clientWidth  || 1100;
    canvas.height = parent.clientHeight || 750;
}

export function renderCadPreview(canvas, result) {
    const ctx = canvas.getContext('2d');
    if (!ctx) return;

    ctx.clearRect(0, 0, canvas.width, canvas.height);
    ctx.fillStyle = '#ffffff';
    ctx.fillRect(0, 0, canvas.width, canvas.height);

    if (!result || !result.entities || result.entities.length === 0) {
        ctx.fillStyle = '#999';
        ctx.font = '16px Arial';
        ctx.textAlign = 'center';
        ctx.fillText('No entities to display', canvas.width / 2, canvas.height / 2);
        return;
    }

    // Build bounding box from geometry (lines + arcs) only — ignoring outlying text
    const geomBounds = calcGeomBounds(result.entities);
    if (!geomBounds) return;

    // Expand 5% for breathing room
    const margin = Math.max(geomBounds.width, geomBounds.height) * 0.05;
    const bounds = {
        min: { x: geomBounds.minX - margin, y: geomBounds.minY - margin },
        max: { x: geomBounds.maxX + margin, y: geomBounds.maxY + margin },
        width:  geomBounds.width  + 2 * margin,
        height: geomBounds.height + 2 * margin,
    };
    if (bounds.width < 1 || bounds.height < 1) return;

    const padding = 50;
    const drawWidth  = canvas.width  - 2 * padding;
    const drawHeight = canvas.height - 2 * padding;
    const scaleX = drawWidth  / bounds.width;
    const scaleY = drawHeight / bounds.height;
    const scale  = Math.min(scaleX, scaleY) * 0.92;

    const offsetX = padding + (drawWidth  - bounds.width  * scale) / 2;
    const offsetY = padding + (drawHeight - bounds.height * scale) / 2;

    // CAD coords → canvas coords (flip Y axis)
    function tx(x, y) {
        return {
            x: offsetX + (x - bounds.min.x) * scale,
            y: canvas.height - (offsetY + (y - bounds.min.y) * scale)
        };
    }

    // ── Grid ──────────────────────────────────────────────────────────
    ctx.strokeStyle = '#e8e8e8';
    ctx.lineWidth = 0.5;
    const gridSize = pickGridSize(bounds.width);
    for (let gx = Math.floor(bounds.min.x / gridSize) * gridSize; gx <= bounds.max.x; gx += gridSize) {
        const a = tx(gx, bounds.min.y), b = tx(gx, bounds.max.y);
        ctx.beginPath(); ctx.moveTo(a.x, a.y); ctx.lineTo(b.x, b.y); ctx.stroke();
    }
    for (let gy = Math.floor(bounds.min.y / gridSize) * gridSize; gy <= bounds.max.y; gy += gridSize) {
        const a = tx(bounds.min.x, gy), b = tx(bounds.max.x, gy);
        ctx.beginPath(); ctx.moveTo(a.x, a.y); ctx.lineTo(b.x, b.y); ctx.stroke();
    }

    // ── Geometry entities ─────────────────────────────────────────────
    result.entities.forEach(entity => {
        const layer = (entity.layer || '').toLowerCase();

        // Colour by entity type / layer
        if (entity.type === 'Arc' || entity.type === 'Circle') {
            ctx.strokeStyle = isTrackLayer(layer) ? '#ff6644' : '#5599ff';
        } else if (entity.type === 'Line') {
            ctx.strokeStyle = isTrackLayer(layer) ? '#4499ff' : '#557799';
        } else {
            ctx.strokeStyle = '#446688';
        }
        ctx.lineWidth = isTrackLayer(layer) ? 2 : 1;

        switch (entity.type) {
            case 'Line':
                if (entity.startPoint && entity.endPoint) {
                    const s = tx(entity.startPoint.x, entity.startPoint.y);
                    const e = tx(entity.endPoint.x,   entity.endPoint.y);
                    ctx.beginPath(); ctx.moveTo(s.x, s.y); ctx.lineTo(e.x, e.y); ctx.stroke();
                }
                break;

            case 'Arc':
                if (entity.center && entity.radius != null &&
                    entity.startAngle != null && entity.endAngle != null) {
                    const c = tx(entity.center.x, entity.center.y);
                    const r = entity.radius * scale;
                    // Negate angles for Y-flip; CCW in DXF = CW on canvas → anticlockwise=true
                    const sa = -entity.startAngle * Math.PI / 180;
                    const ea = -entity.endAngle   * Math.PI / 180;
                    ctx.beginPath();
                    ctx.arc(c.x, c.y, r, sa, ea, true);
                    ctx.stroke();
                }
                break;

            case 'Circle':
                if (entity.center && entity.radius != null) {
                    const c = tx(entity.center.x, entity.center.y);
                    const r = entity.radius * scale;
                    ctx.beginPath(); ctx.arc(c.x, c.y, r, 0, 2 * Math.PI); ctx.stroke();
                }
                break;

            case 'Polyline':
                if (entity.points && entity.points.length > 1) {
                    ctx.beginPath();
                    const first = tx(entity.points[0].x, entity.points[0].y);
                    ctx.moveTo(first.x, first.y);
                    for (let i = 1; i < entity.points.length; i++) {
                        const p = tx(entity.points[i].x, entity.points[i].y);
                        ctx.lineTo(p.x, p.y);
                    }
                    if (entity.isClosed) ctx.closePath();
                    ctx.stroke();
                }
                break;
        }
    });

    // ── Text labels ───────────────────────────────────────────────────
    if (result.texts && result.texts.length > 0) {
        result.texts.forEach(item => {
            if (!item.content || !item.position) return;

            const pos      = tx(item.position.x, item.position.y);
            const fontSize = Math.max(7, Math.min(item.height * scale, 18));

            // Skip tiny text that won't be readable
            if (item.height * scale < 4) return;

            ctx.save();
            ctx.translate(pos.x, pos.y);
            // Negate rotation because Y is flipped
            if (item.rotation) ctx.rotate(-item.rotation * Math.PI / 180);

            ctx.font = `${fontSize}px monospace`;
            ctx.textAlign    = 'left';
            ctx.textBaseline = 'bottom';

            // Colour by type
            if (item.itemType === 'Dimension') {
                ctx.fillStyle = '#b45309';
            } else if ((item.layer || '').toLowerCase().includes('note') ||
                       (item.layer || '').toLowerCase().includes('text')) {
                ctx.fillStyle = '#15803d';
            } else {
                ctx.fillStyle = '#4b5563';
            }

            // Handle multi-line MText
            const lines = item.content.split('\n');
            lines.forEach((line, idx) => {
                ctx.fillText(line, 0, -idx * (fontSize + 1));
            });

            ctx.restore();
        });
    }

    // ── Axes ──────────────────────────────────────────────────────────
    const origin = tx(bounds.min.x, bounds.min.y);
    const xTip   = tx(bounds.min.x + bounds.width * 0.05, bounds.min.y);
    const yTip   = tx(bounds.min.x, bounds.min.y + bounds.height * 0.05);

    ctx.lineWidth = 2;
    ctx.strokeStyle = '#00cc44';
    ctx.beginPath(); ctx.moveTo(origin.x, origin.y); ctx.lineTo(xTip.x, xTip.y); ctx.stroke();
    ctx.strokeStyle = '#cc2222';
    ctx.beginPath(); ctx.moveTo(origin.x, origin.y); ctx.lineTo(yTip.x, yTip.y); ctx.stroke();

    ctx.font = '12px Arial';
    ctx.fillStyle = '#00cc44'; ctx.fillText('X', xTip.x + 6, xTip.y);
    ctx.fillStyle = '#cc2222'; ctx.fillText('Y', yTip.x,     yTip.y - 6);
}

function isTrackLayer(layer) {
    return layer.includes('track') || layer.includes('convey') || layer.includes('rail');
}

function pickGridSize(width) {
    if (width > 50000) return 5000;
    if (width > 10000) return 1000;
    if (width > 2000)  return 500;
    return 100;
}

function calcGeomBounds(entities) {
    let minX = Infinity, minY = Infinity, maxX = -Infinity, maxY = -Infinity;
    let found = false;

    function expand(x, y) {
        if (x == null || y == null || !isFinite(x) || !isFinite(y)) return;
        minX = Math.min(minX, x); minY = Math.min(minY, y);
        maxX = Math.max(maxX, x); maxY = Math.max(maxY, y);
        found = true;
    }

    (entities || []).forEach(e => {
        if (e.type === 'Line') {
            if (e.startPoint) expand(e.startPoint.x, e.startPoint.y);
            if (e.endPoint)   expand(e.endPoint.x,   e.endPoint.y);
        } else if (e.type === 'Arc' && e.center && e.radius) {
            expand(e.center.x - e.radius, e.center.y - e.radius);
            expand(e.center.x + e.radius, e.center.y + e.radius);
        } else if (e.type === 'Circle' && e.center && e.radius) {
            expand(e.center.x - e.radius, e.center.y - e.radius);
            expand(e.center.x + e.radius, e.center.y + e.radius);
        } else if (e.type === 'Polyline' && e.points) {
            e.points.forEach(p => expand(p.x, p.y));
        }
    });

    if (!found) return null;
    return { minX, minY, maxX, maxY, width: maxX - minX, height: maxY - minY };
}
