$(document).ready(function () {
    $('.quick-content-contentbox').hover(function () {
        $(this).addClass('quick-content-hover');
    },
    function () {
        $(this).removeClass('quick-content-hover');
    }
    );
});