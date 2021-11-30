// that is supposingly required...
var serializeEvent = function (e) {
    if (e) {
        let o = {
            code: e.code,
        };
        return o;
    }
}

// to register a keyboard event controller for the document - which doesn't exists in the Blazor code - we need to register an
// JavaScrpt event handler that will call into the Blazor code
function RegisterKeyDownEventHandler() {
    console.log("Registering keydown event handler");
    document.addEventListener('keydown', function (e) {
        // call the assembly method HandleKeyPress
        // console.log("Calling HandleKeyPress");
        DotNet.invokeMethodAsync('Blazor2048', 'HandleKeyPress', serializeEvent(e));
    }, false);
}

// register a document event hanlder for key down
document.addEventListener("DOMContentLoaded", function (event) {
    RegisterKeyDownEventHandler();
    //window.addEventListener("scroll", (e) => {
    //    //e.preventDefault();
    //    // window.scrollTo(0, 0);
    //});
});
