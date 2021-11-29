// that is supposingly required...
var serializeEvent = function (e) {
    if (e) {
        let o = {
            code: e.code,
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

var SwipeStartX = null;
var SwipeStartY = null;
var SwipeWidth = null;
var SwipeArea = null;

function RegisterSwipe() {
    console.log("RegisterSwipe");
    const SwipeWidthMax = 100;
    SwipeArea = document; //.querySelector("main");
    console.log(SwipeArea);
    SwipeWidth = Math.min(window.innerWidth * 0.2, SwipeWidthMax);
    console.log("SwipeWidth: ", SwipeWidth);
    SwipeArea.addEventListener("touchstart", SwipeStart, false);
    SwipeArea.addEventListener("touchend", SwipeEnd, false);
}

function SwipeStart(evt) {
    console.log("SwipeStart");
    SwipeStartX = evt.changedTouches[0].clientX;
    SwipeStartY = evt.changedTouches[0].clientY;
};

function SwipeEnd(evt) {
    console.log("SwipeEnd");
    const SwipeEndX = evt.changedTouches[0].clientX;
    const SwipeEndY = evt.changedTouches[0].clientY;

    const SwipeLengthX = SwipeStartX - SwipeEndX;
    const SwipeLengthY = SwipeStartY - SwipeEndY;

    if (Math.abs(SwipeLengthX) > Math.abs(SwipeLengthY)) {
        if (SwipeLengthX > SwipeWidth) {
            /* left swipe */
            console.log("Swipped left");
            DotNet.invokeMethodAsync('Blazor2048', 'HandleKeyPress', { Code: "ArrowLeft" });
        }
        else if (SwipeLengthX < -SwipeWidth) {
            /* right swipe */
            console.log("Swipped right");
            DotNet.invokeMethodAsync('Blazor2048', 'HandleKeyPress', { Code: "ArrowRight" });
        }
    }
    else {
        if (SwipeLengthY > SwipeWidth) {
            /* down up */
            console.log("Swipped up");
            DotNet.invokeMethodAsync('Blazor2048', 'HandleKeyPress',  { Code: "ArrowUp" });
        }
        else if (SwipeLengthY < -SwipeWidth) {
            /* up down */
            console.log("Swipped down");
            DotNet.invokeMethodAsync('Blazor2048', 'HandleKeyPress',  { Code: "ArrowDown" });
        }
    }
}

// register a document event hanlder for key down
document.addEventListener("DOMContentLoaded", function (event) {
    RegisterKeyDownEventHandler();
    RegisterSwipe();
});
