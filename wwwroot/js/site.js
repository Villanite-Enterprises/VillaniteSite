function toggleMenu(triggeringElement) {
    triggeringElement.classList.toggle("menu-open");
    document.getElementById("SiteNav").classList.toggle("menu-open")
}
function getCookie(cookieName) {
    var name = cookieName + "=";
    var decodedCookies = decodeURIComponent(document.cookie);
    var cookieArray = decodedCookies.split(';');
    for (var i = 0; i < cookieArray.length; i++) {
        var currentCookie = cookieArray[i];
        while (currentCookie.charAt(0) == ' ') {
            currentCookie = currentCookie.substring(1);
        }
        if (currentCookie.indexOf(name) == 0) {
            return currentCookie.substring(name.length, currentCookie.length);
        }
    }
    return "";
}
function setCookie(cookieName, cookieValue, expirationDays) {
    var expirationDate = new Date();
    expirationDate.setTime(expirationDate.getTime() + (expirationDays * 24 * 60 * 60 * 1000));
    var expires = "expires=" + expirationDate.toUTCString();
    document.cookie = cookieName + "=" + cookieValue + ";" + expires + ";path=/";
}

function acceptCookieNotice() {
    setCookie("V_AcceptedCookieNotice", true, 365);
    document.getElementById("CookieNotice").classList.toggle("d-none")
}