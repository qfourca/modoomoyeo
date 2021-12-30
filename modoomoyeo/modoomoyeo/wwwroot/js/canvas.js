var canvas = document.getElementById('canvas');
var context = canvas.getContext("2d");
var img = new Image();
img.onload = function () {
    context.drawImage(img, 10, 10);
}
img.src = 'https://upload.wikimedia.org/wikipedia/commons/6/6d/Source_code_in_C.png';
context.drawImage(img, 10, 10)
context.fillStyle = "rgba(0, 0, 200, 0.5)";
var isAbleDraw = false;
const options = {
    type: 'stroke',
    strokeStyle: 'blue',
    lineWidth: 2,
};
const rects = [];
var currentRect = null;
var recvRect = {
    type: 'stroke',
    strokeStyle: 'blue',
    lineWidth: 2,
    coordinates: [],
};



var connection = new signalR.HubConnectionBuilder().withUrl("/canvasHub").build();
connection.on("ReceiveMessage", function (type, width, x, y) { 
    recvRect.coordinates.push([x, y]);
    if (type === 'stroke') drawTools.stroke(recvRect.coordinates, 'blue', width);
    else if (type === 'eraser') drawTools.eraser(recvRect.coordinates, width);

});
connection.start().then(function () {
    console.log("SignalR Start")
}).catch(function (err) {
    return console.error(err.toString());
});
document.addEventListener('keydown', (event) => {
    if (event.ctrlKey && event.key === 'z') {
      alert('Undo!');
    }
  });

document.getElementById('canvas').addEventListener('mouseup', () => {
    connection.invoke("SendMessage", options.type, options.lineWidth, -1, -1).catch(function (err) {
        return console.error(err.toString());
    });
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
        connection.invoke("SendMessage", options.type, options.lineWidth, event.offsetX, event.offsetY).catch(function (err) {
            return console.error(err.toString());
        });

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
    stroke(coordinates, color, lineWidth) {
        if (coordinates.length > 0) {
            console.log('debug')
            const ctx = document.getElementById('canvas').getContext('2d');
            const firstCoordinate = coordinates[0];
            ctx.beginPath();
            ctx.moveTo(firstCoordinate[0], firstCoordinate[1]);
            for (let i = 1; i < coordinates.length; i++) {
                if(coordinates[i][0] == -1) {
                    i++
                    ctx.moveTo(coordinates[i][0], coordinates[i][1])
                }
                else
                    ctx.lineTo(coordinates[i][0], coordinates[i][1])
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
            ctx.arc(x, y, lineWidth, 0, Math.PI * 2);
            ctx.fill();
            ctx.closePath();
        }
    },
};