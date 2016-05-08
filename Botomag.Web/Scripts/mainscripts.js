$(function () {
    $(".owl-carousel").owlCarousel({
        items: 1,
        autoplay: true,
        autoplaySpeed: 500,
        loop: true
    });
});

$(".btn-ios").click(function (e) {
    e.preventDefault();
    var btniosoffset = $(".btn-ios").offset();
    $(".ios-body").css({
        left: btniosoffset.left,
        top: btniosoffset.top - 428
    });
    $(".ios-popup").show();
});

$(".ios-popup").click(function () {
    $('form').find("input[name='email']").val("");
    $(this).hide();
});

$(".ios-popup input").click(function (e) {
    e.stopPropagation();
});

$(".ios-popup button").click(function (e) {
    e.stopPropagation();
});

$(".ios-body").click(function (e) {
    e.stopPropagation();
});

function sendMessageSuccess()
{
    $(".ios-popup").hide();
    $(".ios-msg").css({
        left: window.pageXOffset + window.innerWidth - 420,
        top: window.pageYOffset + window.innerHeight - 120
    });
    $(".ios-msg").show(200).delay(2000);
    setTimeout(function () {
        $(".ios-msg").hide(200);
    }, 2000);
    $('form').find("input[name='email']").val("");
}