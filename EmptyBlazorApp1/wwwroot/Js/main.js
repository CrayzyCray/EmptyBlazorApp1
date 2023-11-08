function reloadPage() {
    window.location.reload();
}

function deleteCookie(name) {
    document.cookie = name + '=; expires=Thu, 01 Jan 1970 00:00:00 UTC;';
}

function setCookieMy(name, value) {
    document.cookie = name + "=" + value + "; secure;";
}

function getCookieValue(key) {
    let cookieString = document.cookie;
    let cookieArray = cookieString.split(";");
    for (let i = 0; i < cookieArray.length; i++) {
        let pair = cookieArray[i].trim();
        let keyValue = pair.split("=");
        if (keyValue[0] === key) {
            return keyValue[1];
        }
    }
    return null;
}

function initHistoryApi(objRef, data) {
    console.log("initHistoryApi");
    window.history.replaceState(data, null, "");
    setPageState(data);
    window.onpopstate = function (event) {
        objRef.invokeMethodAsync('JsOnpopstateCallback', event.state);
    }
}

function setPageState(data) {
    window.history.pushState(data, null, "");
    console.log("set new PageState to: " + data);
}

function goBack() {
    window.history.back();
}

function test2(data) {
    console.log("New page state: " + data);
    window.history.pushState(data, null, "");
    //DotNet.invokeMethodAsync('EmptyBlazorApp1', 'test');
}