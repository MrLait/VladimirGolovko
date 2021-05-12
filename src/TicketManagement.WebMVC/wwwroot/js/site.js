var language = window.navigator ? (window.navigator.language ||
    window.navigator.systemLanguage ||
    window.navigator.userLanguage) : "ru";
language = language.substr(0, 2).toLowerCase();


$('#stringName').replaceWith(localStorage.getItem(language));

function setMyValue(value) {
    $('#myVar').val(value);
}