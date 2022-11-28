
function replaceUrlParam(url, paramName, paramValue) {
    var pattern = new RegExp('(\\?|\\&)(' + paramName + '=).*?(&|$)')
    var newUrl = url;

    if (url.search(pattern) >= 0) {
        newUrl = url.replace(pattern, '$1$2' + paramValue + '$3');
    }
    else {
        newUrl = newUrl + (newUrl.indexOf('?') > 0 ? '&' : '?') + paramName + '=' + paramValue
    }
    return newUrl
}

function menuActive(val) {
    $('.w_menu a[data-val="' + val + '"]').addClass('active');
}
