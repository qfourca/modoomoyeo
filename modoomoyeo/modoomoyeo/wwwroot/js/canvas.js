var canvas = document.getElementById('canvas');
var context = canvas.getContext("2d");
context.fillText('My text', 100, 100);
context.fillStyle = "rgba(0, 0, 200, 0.5)";

var isAbleDraw = false;
const options = {
    type: 'stroke',
    strokeStyle: 'blue',
    lineWidth: 5,
};
const rects = [];
let currentRect = null;
document.addEventListener('keydown', (event) => {
    if (event.ctrlKey && event.key === 'z') {
      alert('Undo!');
    }
  });
document.getElementById('canvas').addEventListener('mouseup', () => {
    if(isAbleDraw == true) {
    isAbleDraw = false;
    rects.push(currentRect);
    currentRect = null;
    console.log(rects);
    }
})
document.getElementById('canvas').addEventListener('mousedown', () => {
    isAbleDraw = true;
    currentRect = {
        type: options.type,
        strokeStyle: options.strokeStyle,
        lineWidth: options.lineWidth,
        coordinates: [],
    };
})
document.getElementById('canvas').addEventListener('mousemove', (event) => {
    if (isAbleDraw) {
        currentRect.coordinates.push([event.offsetX, event.offsetY]);
        drawTools.execute(rects);
        if (currentRect.type === 'stroke') drawTools.stroke(currentRect.coordinates, options.strokeStyle, currentRect.lineWidth);
        else if (currentRect.type === 'eraser') drawTools.eraser(currentRect.coordinates, currentRect.lineWidth);
    }
});

const drawTools = {
    clear() {
        const canvas = document.getElementById('canvas');
        const ctx = canvas.getContext('2d');
        ctx.clearRect(0, 0, canvas.width, canvas.height);
    },
    execute(rects) {
        for (let i = 0; i < rects.length; i += 1) {
            const rect = rects[i];
            const { type } = rect;
            if (type === 'stroke') this.stroke(rect.coordinates, rect.strokeStyle, rect.lineWidth);
            if (type === 'eraser') this.eraser(rect.coordinates, rect.lineWidth);
            if (type === 'square') this.square(rect.coordinates, rect.strokeStyle);
        }
    },
    stroke(coordinates, color, lineWidth) {
        if (coordinates.length > 0) {
            const ctx = document.getElementById('canvas').getContext('2d');
            const firstCoordinate = coordinates[0];
            ctx.beginPath();
            ctx.moveTo(firstCoordinate[0], firstCoordinate[1]);
            for (let i = 1; i < coordinates.length; i += 1) {
                ctx.lineTo(coordinates[i][0], coordinates[i][1]);
            }
            ctx.strokeStyle = color;
            ctx.lineWidth = lineWidth;
            ctx.stroke();
            ctx.closePath();
        }
    },
    eraser(coordinates, lineWidth) {
        const canvas = document.getElementById('canvas');
        const ctx = canvas.getContext('2d');
        for (let i = 0; i < coordinates.length; i += 1) {
            ctx.beginPath();
            const coordinate = coordinates[i];
            const [x, y] = coordinate;
            ctx.fillStyle = 'white';
            ctx.arc(x, y, lineWidth / 2, 0, Math.PI * 2);
            ctx.fill();
            ctx.closePath();
        }
    },
    square(coordinates, color) {
        const canvas = document.getElementById('canvas');
        const ctx = canvas.getContext('2d');
        const start = coordinates[0];
        const end = coordinates[coordinates.length - 1];
        const [startX, startY] = start;
        const [endX, endY] = [end[0] - startX, end[1] - startY];
        ctx.beginPath();
        ctx.fillStyle = color;
        ctx.fillRect(startX, startY, endX, endY);
        ctx.closePath();
    },
};
document.getElementById('type').addEventListener('change', (e) => {
    options.type = e.target.value;
});
document.getElementById('strokeStyle').addEventListener('change', (e) => {
    options.strokeStyle = e.target.value;
});
document.getElementById('lineWidth').addEventListener('change', (e) => {
    options.lineWidth = e.target.value;
});