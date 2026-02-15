// 2D CAD Preview Renderer
export function renderCadPreview(canvas, result) {
    const ctx = canvas.getContext('2d');
    if (!ctx) return;

    // Clear canvas
    ctx.clearRect(0, 0, canvas.width, canvas.height);
    ctx.fillStyle = '#1a1a1a';
    ctx.fillRect(0, 0, canvas.width, canvas.height);

    if (!result || !result.entities || result.entities.length === 0) {
        ctx.fillStyle = '#666';
        ctx.font = '16px Arial';
        ctx.textAlign = 'center';
        ctx.fillText('No entities to display', canvas.width / 2, canvas.height / 2);
        return;
    }

    // Calculate bounds and scale
    const bounds = result.boundingBox;
    if (!bounds) return;

    const padding = 50;
    const drawWidth = canvas.width - 2 * padding;
    const drawHeight = canvas.height - 2 * padding;

    const scaleX = drawWidth / bounds.width;
    const scaleY = drawHeight / bounds.height;
    const scale = Math.min(scaleX, scaleY) * 0.9; // 90% to add some margin

    const offsetX = padding + (drawWidth - bounds.width * scale) / 2;
    const offsetY = padding + (drawHeight - bounds.height * scale) / 2;

    // Transform function: CAD coords -> Canvas coords
    function transform(x, y) {
        return {
            x: offsetX + (x - bounds.min.x) * scale,
            y: canvas.height - (offsetY + (y - bounds.min.y) * scale) // Flip Y axis
        };
    }

    // Draw grid
    ctx.strokeStyle = '#333';
    ctx.lineWidth = 0.5;
    const gridSize = 1000; // 1000mm grid
    for (let x = Math.floor(bounds.min.x / gridSize) * gridSize; x <= bounds.max.x; x += gridSize) {
        const pt = transform(x, bounds.min.y);
        const pt2 = transform(x, bounds.max.y);
        ctx.beginPath();
        ctx.moveTo(pt.x, pt.y);
        ctx.lineTo(pt2.x, pt2.y);
        ctx.stroke();
    }
    for (let y = Math.floor(bounds.min.y / gridSize) * gridSize; y <= bounds.max.y; y += gridSize) {
        const pt = transform(bounds.min.x, y);
        const pt2 = transform(bounds.max.x, y);
        ctx.beginPath();
        ctx.moveTo(pt.x, pt.y);
        ctx.lineTo(pt2.x, pt2.y);
        ctx.stroke();
    }

    // Draw entities
    result.entities.forEach(entity => {
        const isTrack = result.trackSections.some(t =>
            (t.type === 'Straight' && entity.type === 'Line') ||
            (t.type === 'Curve' && entity.type === 'Arc')
        );

        ctx.strokeStyle = isTrack
            ? (entity.type === 'Arc' || entity.type === 'Circle' ? '#ff4444' : '#4488ff')
            : '#888888';
        ctx.lineWidth = isTrack ? 3 : 1;

        switch (entity.type) {
            case 'Line':
                if (entity.startPoint && entity.endPoint) {
                    const start = transform(entity.startPoint.x, entity.startPoint.y);
                    const end = transform(entity.endPoint.x, entity.endPoint.y);
                    ctx.beginPath();
                    ctx.moveTo(start.x, start.y);
                    ctx.lineTo(end.x, end.y);
                    ctx.stroke();
                }
                break;

            case 'Arc':
                if (entity.center && entity.radius != null && entity.startAngle != null && entity.endAngle != null) {
                    const center = transform(entity.center.x, entity.center.y);
                    const radius = entity.radius * scale;

                    // Convert angles to radians and flip for canvas coordinate system
                    const startAngle = -entity.endAngle * Math.PI / 180;
                    const endAngle = -entity.startAngle * Math.PI / 180;

                    ctx.beginPath();
                    ctx.arc(center.x, center.y, radius, startAngle, endAngle, false);
                    ctx.stroke();
                }
                break;

            case 'Circle':
                if (entity.center && entity.radius != null) {
                    const center = transform(entity.center.x, entity.center.y);
                    const radius = entity.radius * scale;
                    ctx.beginPath();
                    ctx.arc(center.x, center.y, radius, 0, 2 * Math.PI);
                    ctx.stroke();
                }
                break;

            case 'Polyline':
                if (entity.points && entity.points.length > 1) {
                    ctx.beginPath();
                    const first = transform(entity.points[0].x, entity.points[0].y);
                    ctx.moveTo(first.x, first.y);
                    for (let i = 1; i < entity.points.length; i++) {
                        const pt = transform(entity.points[i].x, entity.points[i].y);
                        ctx.lineTo(pt.x, pt.y);
                    }
                    if (entity.isClosed) {
                        ctx.closePath();
                    }
                    ctx.stroke();
                }
                break;
        }
    });

    // Draw axes
    ctx.strokeStyle = '#00ff00';
    ctx.lineWidth = 2;
    const origin = transform(bounds.min.x, bounds.min.y);
    const xAxis = transform(bounds.min.x + bounds.width * 0.1, bounds.min.y);
    const yAxis = transform(bounds.min.x, bounds.min.y + bounds.height * 0.1);

    // X axis
    ctx.beginPath();
    ctx.moveTo(origin.x, origin.y);
    ctx.lineTo(xAxis.x, xAxis.y);
    ctx.stroke();

    // Y axis
    ctx.strokeStyle = '#ff0000';
    ctx.beginPath();
    ctx.moveTo(origin.x, origin.y);
    ctx.lineTo(yAxis.x, yAxis.y);
    ctx.stroke();

    // Labels
    ctx.fillStyle = '#00ff00';
    ctx.font = '12px Arial';
    ctx.fillText('X', xAxis.x + 10, xAxis.y);
    ctx.fillStyle = '#ff0000';
    ctx.fillText('Y', yAxis.x, yAxis.y - 10);
}
