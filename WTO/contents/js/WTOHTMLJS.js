﻿var Months = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];

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

    //Calculate some cutoff dates based on Final date for comments 
    //$("#FinalDateforCommentsId").change(function () {
    //    var FinalDateforComments = $(this).val();
    //    if (!isNaN(new Date($.trim(FinalDateforComments)).valueOf())) {
    //        var _FinalDate = new Date(FinalDateforComments);
    //        var DateofNotification = $('#DateofNotificationId').val();
    //        var _DateofNotification = new Date(DateofNotification);

    //        //Set Send Response due date i.e. Final date for comments – 20 days 
    //        var ResponseOn = new Date(FinalDateforComments);
    //        ResponseOn.setDate(ResponseOn.getDate() - 20);
    //        var _ResponseOn = ResponseOn.getDate() + ' ' + Months[ResponseOn.getMonth()] + ' ' + ResponseOn.getFullYear();
    //        $("#SendResponseById").val(_ResponseOn);
    //        if (_DateofNotification > ResponseOn) {
    //            $("#SendResponseError").removeClass("hidden");
    //            return false;
    //        }
    //        else {
    //            $("#SendResponseError").addClass("hidden");
    //            return true;
    //        }
    //    }
    //    else {
    //        $("#SendResponseById").val('');
    //    }

    //});

    //$("#SendResponseById").change(function () {
    //    var FinalDateforComments = $('#FinalDateforCommentsId').val();
    //    var _FinalDate = new Date(FinalDateforComments);
    //    var DateofNotification = $('#DateofNotificationId').val();
    //    var _DateofNotification = new Date(DateofNotification);

    //    //Set Send Response due date i.e. Final date for comments – 20 days 
    //    var ResponseOn = new Date($(this).val());
    //    if (_DateofNotification > ResponseOn) {
    //        $("#SendResponseError").removeClass("hidden");
    //        return false;
    //    }
    //    else if (_FinalDate < ResponseOn) {
    //        $("#SendResponseError").removeClass("hidden");
    //        return false;
    //    }
    //    else {
    //        $("#SendResponseError").addClass("hidden");
    //        return true;
    //    }
    //});

    //$("#DateofNotificationId").change(function () {
    //    var DateofNotification = $('#DateofNotificationId').val();
    //    DateofNotification = new Date(DateofNotification);

    //    var FinalDateforComments = new Date($('#FinalDateforCommentsId').val());
    //    FinalDateforComments = new Date(FinalDateforComments);
    //    FinalDateforComments.setDate(ResponseOn.getDate() - 60);

    //    if (_DateofNotification > FinalDateforComments) {
    //        $("#DateofNotificationId").removeClass("hidden");
    //        return false;
    //    }
    //    else {
    //        $("#SendResponseError").addClass("hidden");
    //        return true;
    //    }
    //});

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

    // <!-- image swaping -->

    var dashboardblack = "/contents/img/dashboard.png";
    var dashboardwhite = "/contents/img/dashboard1.png";
    var notificationblack = "/contents/img/notification1.png";
    var notificationwhite = "/contents/img/notification.png";
    var Addnotificationblack = "/contents/img/Addnotification.png";
    var Addnotificationwhite = "/contents/img/Addnotification1.png";
    var AddMOMListWhite = "/contents/img/MOMlistW.png";
    var AddMOMListBlack = "/contents/img/MOMlistB.png";
    var AddMOMWhite = "/contents/img/AddMOM.png";
    var AddMOMBlack = "/contents/img/AddMOMB.png";
    var ManageAccessB = "/contents/img/manageB.png";
    var ManageAccessW = "/contents/img/manageW.png";

    var location = document.location.href;

    if (location.indexOf("WTODashboard") >= 1) {
        $('.dashboard').addClass("active");
        //if ($('.dashboard img').attr('src') == dashboardwhite) {
        //    $('.dashboard img').attr('src', dashboardblack);
        //} else {
        //    $('.dashboard img').attr('src', dashboardwhite);
        //}
    }
    else {
        $('.dashboard').removeClass("active");
        //$('.dashboard').on('mouseenter mouseleave', function () {
        //    if ($('.dashboard img').attr('src') == dashboardwhite) {
        //        $('.dashboard img').attr('src', dashboardblack);
        //    } else {
        //        $('.dashboard img').attr('src', dashboardwhite);
        //    }
        //});
    }
    if (location.indexOf("NotificationList") > -1) {
        $('.notification').addClass("active");
        //if ($('.notification img').attr('src') == notificationwhite) {
        //    $('.notification img').attr('src', notificationblack);
        //} else {
        //    $('.notification img').attr('src', notificationwhite);
        //}
    }
    else {
        $('.notification').removeClass("active");
        //$('.notification').on('mouseenter mouseleave', function () {
        //    if ($('.notification img').attr('src') == notificationwhite) {
        //        $('.notification img').attr('src', notificationblack);
        //    } else {
        //        $('.notification img').attr('src', notificationwhite);
        //    }
        //});
    }
    if (location.indexOf("AddNotification") > -1) {
        $('.Addnotification').addClass("active");
        //if ($('.Addnotification img').attr('src') == Addnotificationwhite) {
        //    $('.Addnotification img').attr('src', Addnotificationblack);
        //} else {
        //    $('.Addnotification img').attr('src', Addnotificationwhite);
        //}
    }
    else {
        $('.Addnotification').removeClass("active");
        //$('.Addnotification').on('mouseenter mouseleave', function () {
        //    if ($('.Addnotification img').attr('src') == Addnotificationwhite) {
        //        $('.Addnotification img').attr('src', Addnotificationblack);
        //    } else {
        //        $('.Addnotification img').attr('src', Addnotificationwhite);
        //    }
        //});
    }
    if (location.indexOf("MOMList") > -1) {
        $('.AddMOMList').addClass("active");
        //if ($('.AddMOMList img').attr('src') == AddMOMListWhite) {
        //    $('.AddMOMList img').attr('src', AddMOMListBlack);
        //} else {
        //    $('.AddMOMList img').attr('src', AddMOMListWhite);
        //}
    }
    else {
        $('.AddMOMList').removeClass("active");
        //$('.AddMOMList').on('mouseenter mouseleave', function () {
        //    if ($('.AddMOMList img').attr('src') == AddMOMListWhite) {
        //        $('.AddMOMList img').attr('src', AddMOMListBlack);
        //    } else {
        //        $('.AddMOMList img').attr('src', AddMOMListWhite);
        //    }
        //});
    }
    if (location.indexOf("AddMOM") > -1) {
        $('.AddMOM').addClass("active");
        //if ($('.AddMOM img').attr('src') == AddMOMWhite) {
        //    $('.AddMOM img').attr('src', AddMOMBlack);
        //} else {
        //    $('.AddMOM img').attr('src', AddMOMWhite);
        //}
    }
    else {
        $('.AddMOM').removeClass("active");
        //$('.AddMOM').on('mouseenter mouseleave', function () {
        //    if ($('.AddMOM img').attr('src') == AddMOMWhite) {
        //        $('.AddMOM img').attr('src', AddMOMBlack);
        //    } else {
        //        $('.AddMOM img').attr('src', AddMOMWhite);
        //    }
        //});
    }
    if (location.indexOf("ManageAccess") > -1) {
        $('.ManageAccess').addClass("active");
        //if ($('.ManageAccess img').attr('src') == ManageAccessW) {
        //    $('.ManageAccess img').attr('src', ManageAccessB);
        //} else {
        //    $('.ManageAccess img').attr('src', ManageAccessW);
        //}
    }
    else {
        $('.ManageAccess').removeClass("active");
        //$('.ManageAccess').on('mouseenter mouseleave', function () {
        //    if ($('.ManageAccess img').attr('src') == ManageAccessW) {
        //        $('.ManageAccess img').attr('src', ManageAccessB);
        //    } else {
        //        $('.ManageAccess img').attr('src', ManageAccessW);
        //    }
        //});
    }
    // <!-- image swaping End-->

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