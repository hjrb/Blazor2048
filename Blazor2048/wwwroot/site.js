// that is supposingly required...
var serializeEvent = function (e) {
    if (e) {
        let o = {
            altKey: e.altKey,
            button: e.button,
            buttons: e.buttons,
            code: e.code,
            clientX: e.clientX,
            clientY: e.clientY,
            ctrlKey: e.ctrlKey,
            metaKey: e.metaKey,
            movementX: e.movementX,
            movementY: e.movementY,
            offsetX: e.offsetX,
            offsetY: e.offsetY,
            pageX: e.pageX,
            pageY: e.pageY,
            screenX: e.screenX,
            screenY: e.screenY,
            shiftKey: e.shiftKey
        };
        return o;
    }
}

function RegisterKeyDownEventHandler() {
    console.log("Registering keydown event handler");
    document.addEventListener('keydown', function (e) {
        // call the assembly method HandleKeyPress
        console.log("Calling HandleKeyPress");
        DotNet.invokeMethodAsync('Blazor2048', 'HandleKeyPress', serializeEvent(e));
    }, false);
}

// register a document event hanlder for key down
document.addEventListener("DOMContentLoaded", function (event) {
    RegisterKeyDownEventHandler();
});
