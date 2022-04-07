var notificationDown = false;
var notificationBaseUrl = "/Notification";

$(document).ready(function () {
    $('.dropdownlist').select2();
    $('.dropdownlist').bind('change', function () {
        $(this).trigger('blur');
    });
    $(".datepicker").datepicker({
        format: "dd-mm-yyyy",
        autoHide: true,
    });
    $(".datepicker, .datepickerDate").each(function () {
        if ($(this).val().length > 0) {
            $(this).val($(this).val().substring(0, 10));
        }
    });
    $(".datepicker, .datepickerDate").keypress(function (e) { e.preventDefault(); });

    if ($('#notication-details').length) {
        getNotifactions();
    }

    $('.link-active').each(function () {
        $(this).attr("href", "javascript: void(0)")

    });

    $('#sidebarCollapse').on('click', function () {
        $('#sidebar').toggleClass('active');
    });

    $(".rotate").click(function () {
        $(this).toggleClass("down");
    });
});

$(window).on('load', function () {
    hideLoading();
})

function showLoading() {
    $('.loading').show();
}

function hideLoading() {
    $('.loading').hide();
}

function displayModel(modelId) {
    $(`#${modelId}`).modal("show");
}

function hideModel(modelId) {
    $(`#${modelId}`).modal("hide");
}

function getNotifactions() {
    $.ajax({
        url: `${notificationBaseUrl}/GetNotifications`,
        type: 'POST',
        success: function (result) {
            $("#notication-details").html(result);
        },
        error: function (error) {
        },
    });
}

function showFilter() {
    $('.btn-filter').hide(200);
    $('.filter-detail').show(200);
}

function hideFilter() {
    $('.btn-filter').show(200);
    $('.filter-detail').hide(200);
}

function createCookie(name, value, hours) {
    if (hours) {
        var date = new Date();
        date.setTime(date.getTime() + (hours * 60 * 60 * 1000));
        var expires = "; expires=" + date.toGMTString();
    }
    else var expires = "";

    document.cookie = name + "=" + value + expires + "; path=/";
}

function readCookie(name) {
    var nameEQ = name + "=";
    var ca = document.cookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') c = c.substring(1, c.length);
        if (c.indexOf(nameEQ) == 0) return c.substring(nameEQ.length, c.length);
    }
    return null;
}

function eraseCookie(name) {
    createCookie(name, "", -1);
}

function openNav() {
    $("#sidebar").css({
        "width": "299px",
        "marginLeft" : "-27px",
    });
    
    resizeDataTable();
}

function closeNav() {
    $("#sidebar").css({
        "width": "0",
        "marginLeft": "-40px",
    });
    $("#main").css({

        "marginLeft": "4px",
    });
    
    resizeDataTable();
}