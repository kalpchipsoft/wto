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
    else
        $('#toggleOpenClose').click();

    debugger;
    // <!-- Active menu binding start-->
    var _PageName = $.trim($('.topbg').text());
    //_PageName = _PageName.toLocaleLowerCase().replace(/ /g, '')
    var location = document.location.href.toLocaleLowerCase();

    if (location.indexOf("wtodashboard") >= 1)
        $('.dashboard').addClass("active");
    else
        $('.dashboard').removeClass("active");

    if (_PageName.toLocaleLowerCase().indexOf('notification') >= 0) {
        $('.notification').addClass('open');
        $('.notification').find('.dropdown-toggle').attr('aria-expanded', true);
        $('.notification').find('.dropdown-toggle').addClass('active');

        if (location.indexOf("addnotification") >= 0 && _PageName.toLocaleLowerCase().indexOf('add notification') >= 0)
            $('.addnotification').addClass("active");
        else
            $('.addnotification').removeClass("active");

        if (location.indexOf("notificationlist") >= 0 && _PageName.toLocaleLowerCase().indexOf('notification list') >= 0)
            $('.notificationlist').addClass("active");
        else
            $('.notificationlist').removeClass("active");
    }
    else {
        $('.notification').removeClass('open');
        $('.notification').find('.dropdown-toggle').attr('aria-expanded', false);
    }

    if (_PageName.toLocaleLowerCase().indexOf('meeting') >= 0) {
        $('.meeting').addClass('open');
        $('.meeting').find('.dropdown-toggle').attr('aria-expanded', true);
        $('.meeting').find('.dropdown-toggle').addClass('active');

        if (location.indexOf("mom/add") >= 0 && _PageName.toLocaleLowerCase().indexOf('schedule meeting') >= 0)
            $('.addmeeting').addClass("active");
        else
            $('.addmeeting').removeClass("active");

        if (location.indexOf("mom/edit/") >= 0 && _PageName.toLocaleLowerCase().indexOf('current meeting') >= 0)
            $('.currentmeeting').addClass("active");
        else
            $('.currentmeeting').removeClass("active");

        if (location.indexOf("/mom") >= 0 && (_PageName.toLocaleLowerCase().indexOf('meeting schedules') >= 0 || _PageName.toLocaleLowerCase().indexOf('plan action') >= 0))
            $('.meetinglist').addClass("active");
        else
            $('.meetinglist').removeClass("active");
    }
    else {
        $('.meeting').removeClass('open');
        $('.meeting').find('.dropdown-toggle').attr('aria-expanded', false);
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