var Months = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];

$(document).ready(function () {
    $("#fromdate, #todate").datepicker({
        changeMonth: true,
        changeYear: true
    });

    // <!-- js tree js -->
    $('#html').jstree({
        "checkbox": {
            "keep_selected_style": false
        },
        "plugins": ["checkbox"]
    });
    // <!-- js tree js End-->	

    $(".date-picker").keydown(function (e) {
        return false;
    });

    $(".date-picker").datepicker({
        changeMonth: true,
        changeYear: true,
        dateFormat: 'dd M yy'
    });

    $(".date-picker").attr("readonly", "readonly");

    $(".date-picker").css("background-color", "#FFF");

    // <!-- datepicker js end-->

    // <!-- hamburger js -->
    if (window.location.href.match(/([^\/]*)\/*$/)[1] == "WTODashboard") {
        $("#toggleOpenClose").addClass("is-open");
        $("#toggleOpenClose").removeClass("is-closed");
        $('#wrapper').toggleClass('toggled');
        var trigger = $('.hamburger'),
            isClosed = false;

        function buttonSwitch() {
            if (isClosed === true) {
                trigger.removeClass('is-closed');
                trigger.addClass('is-open');
                isClosed = false;
            } else {
                trigger.removeClass('is-open');
                trigger.addClass('is-closed');
                isClosed = true;
            }
        }

        trigger.click(function () {
            buttonSwitch();
        });
        $('[data-toggle="offcanvas"]').click(function () {
            $('#wrapper').toggleClass('toggled');
            $('#wrapper').addClass('transitioncss');
            $('#sidebar-wrapper').addClass('transitioncss');
        });
    }
    else {
        $("#toggleOpenClose").removeClass("is-open");
        $("#toggleOpenClose").addClass("is-closed");

        var trigger = $('.hamburger'),
            isClosed = true;

        function buttonSwitch() {
            if (isClosed === true) {
                trigger.removeClass('is-closed');
                trigger.addClass('is-open');
                isClosed = false;
            } else {
                trigger.removeClass('is-open');
                trigger.addClass('is-closed');
                isClosed = true;
            }
        }

        trigger.click(function () {
            buttonSwitch();
        });

        $('[data-toggle="offcanvas"]').click(function () {
            $('#wrapper').toggleClass('toggled');
            $('#wrapper').addClass('transitioncss');
            $('#sidebar-wrapper').addClass('transitioncss');
        });
    }
    // <!-- hamburger js -->

    if ($(window).width() < 700) {
        if (window.location.href.match(/([^\/]*)\/*$/)[1] == "WTODashboard") {
            $("#toggleOpenClose").addClass("is-closed");
            $("#toggleOpenClose").removeClass("is-open");
            $('#wrapper').removeClass('toggled');
            var trigger = $('.hamburger'),
                isOpen = true;

            function buttonSwitch() {
                if (isOpen === true) {
                    trigger.removeClass('is-closed');
                    trigger.addClass('is-open');
                    isOpen = false;
                } else {
                    trigger.removeClass('is-open');
                    trigger.addClass('is-closed');
                    isOpen = true;
                }
            }
            trigger.click(function () {
                buttonSwitch();
            });
        }
    }

    debugger;
    // <!-- Active menu binding start-->
    var _PageName = $.trim($('.topbg').text());
    //_PageName = _PageName.toLocaleLowerCase().replace(/ /g, '')
    var location = document.location.href;

    if (location.indexOf("WTODashboard") >= 1)
        $('.dashboard').addClass("active");
    else
        $('.dashboard').removeClass("active");

    if (location.indexOf("NotificationList") > -1) {
        $('.notification').addClass("active");
    }
    else {
        $('.notification').removeClass("active");
    }
    if (location.indexOf("AddNotification") > -1) {
        $('.Addnotification').addClass("active");
    }
    else {
        $('.Addnotification').removeClass("active");
    }
    if (location.indexOf("MOMList") > -1) {
        $('.AddMOMList').addClass("active");
    }
    else {
        $('.AddMOMList').removeClass("active");
    }
    if (location.indexOf("AddMOM") > -1) {
        $('.AddMOM').addClass("active");
    }
    else {
        $('.AddMOM').removeClass("active");
    }
    if (location.indexOf("ManageAccess") > -1) {
        $('.ManageAccess').addClass("active");
    }
    else {
        $('.ManageAccess').removeClass("active");
    }
    // <!-- Active menu binding end-->

    $('.panel-group').on('hidden.bs.collapse', toggleIcon);
    $('.panel-group').on('shown.bs.collapse', toggleIcon);
});

function toggleIcon(e) {
    $(e.target)
        .prev('.panel-heading')
        .find(".more-less")
        .toggleClass('glyphicon-chevron-right glyphicon-chevron-down');
}

$(function () {
    $('[data-toggle="tooltip"]').tooltip()
})