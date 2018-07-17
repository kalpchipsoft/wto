﻿var Action, Action;
var IsScrollingOnce = true;
$(document).ready(function () {
    $('#ddlCountry').combobox();
    // from and to date cross button
    $("#txtfromdate").change(function () {
        if ($("#txtfromdate").val == "") {
            $("#fromdateclender").removeClass("hidden");
            $("#fromdatecross").addClass("hidden");
        }
        else {
            $("#fromdateclender").addClass("hidden");
            $("#fromdatecross").removeClass("hidden");
        }
    })

    $("#txttodate").change(function () {
        if ($("#txttodate").val == "") {
            $("#todateclender").removeClass("hidden");
            $("#todatecross").addClass("hidden");
        }
        else {
            $("#todateclender").addClass("hidden");
            $("#todatecross").removeClass("hidden");
        }
    })

    $("#txtMeetingDate").change(function () {
        if ($("#txtMeetingDate").val == "") {
            $("#Meetingdateclender").removeClass("hidden");
            $("#Meetingdatecross").addClass("hidden");
        }
        else {
            $("#Meetingdateclender").addClass("hidden");
            $("#Meetingdatecross").removeClass("hidden");
        }
    })

    // from and to date notification cross button
    $("#txtNotificationfromdate").change(function () {
        if ($("#txtfromdate").val == "") {
            $("#fromdatenoticlender").removeClass("hidden");
            $("#fromdatenoticross").addClass("hidden");
        }
        else {
            $("#fromdatenoticlender").addClass("hidden");
            $("#fromdatenoticross").removeClass("hidden");
        }
    })

    $("#txtNotificationtodate").change(function () {
        if ($("#txtNotificationtodate").val == "") {
            $("#todatenoticlender").removeClass("hidden");
            $("#todatenoticross").addClass("hidden");
        }
        else {
            $("#todatenoticlender").addClass("hidden");
            $("#todatenoticross").removeClass("hidden");
        }
    });

    //If Redirect from dashboard with search parameters then open advance search panel
    var ActionId = GetParameterValues1('ActionId');
    var ActionStatus = GetParameterValues1('ActionStatus');
    if (ActionId == "" && ActionStatus == "") {
        if ($("#GlySearch").hasClass("glyphicon glyphicon-plus")) {
            $(".SearchAdvnc").addClass('hidden');
            $("#DivBtn").addClass("adv");
        }
    }
    else if (ActionId != "" || ActionStatus != "") {
        $("#GlySearch").removeClass("glyphicon glyphicon-plus");
        $("#GlySearch").addClass("glyphicon glyphicon-minus");
        $(".SearchAdvnc").removeClass('hidden');
        $("#DivBtn").addClass("col-xs-12 col-sm-12 col-md-12");
        $("#DivBtn").removeClass("adv");
    }
});

$(window).on("scroll", function (e) {
    var $o = $(e.currentTarget);
    if ($(window).scrollTop() + $(window).innerHeight() >= $(document).height() - 25 && IsScrollingOnce) {
        IsScrollingOnce = false;
        $('#divPaginationLoadingPanel').removeClass('hidden');
        var NewVal = 0;
        var NewVal = Number($('#hdnPageIndex').val()) + Number(1);
        var MaxVal = parseInt($('#hdnMaxPageIndex').val());
        if (NewVal <= MaxVal) {
            SearchNotification(NewVal);
        }
        else
            $('#divPaginationLoadingPanel').addClass('hidden');
    }
});

function SearchNotification(PageIndx) {
    $('#hdnPageIndex').val(PageIndx);
    var FinalDateFrom = $('[id$=txtfromdate]').val();
    var FinalDateTo = $('[id$=txttodate]').val();

    if (FinalDateFrom != null && FinalDateFrom != '' && FinalDateTo != null && FinalDateTo != '') {
        if (new Date(FinalDateFrom) > new Date(FinalDateTo)) {
            Alert('Alert', 'From date cannnot be greater than To date.<br/>', 'Ok');
            return false;
        }
    }

    ShowGlobalLodingPanel();
    var Callfr = localStorage.getItem("CallFrom");
    var CallfrStatus = localStorage.getItem("Status");
    if (Callfr != null && Callfr != '' && Callfr == 'Dashboard') {
        if (CallfrStatus != null && CallfrStatus != '' && CallfrStatus > 0 && CallfrStatus < 8) {
            $('[id$=ddlStage]').val(CallfrStatus);
            localStorage.setItem("CallFrom", "");
            localStorage.setItem("Status", "");
        }
    }

    var NotificationDateFrom = $('[id$=txtNotificationfromdate]').val();
    var NotificationDateTo = $('[id$=txtNotificationtodate]').val();
    if (NotificationDateFrom != null && NotificationDateFrom != '' && NotificationDateTo != null && NotificationDateTo != '') {
        if (new Date(NotificationDateFrom) > new Date(NotificationDateTo)) {
            Alert('Alert', 'From date cannnot be greater than To date.<br/>', 'Ok');
            return false;
        }
    }

    if (PageIndx == null || PageIndx == 0)
        PageIndx = 1;
    var obj = {
        PageIndex: PageIndx,
        PageSize: 10,
        NotificationNumber: $('[id$=txtNotificationNumber]').val(),
        CountryId: $('[id$=ddlCountry]').val(),
        FinalDateOfComments_From: $('[id$=txtfromdate]').val(),
        FinalDateOfComments_To: $('[id$=txttodate]').val(),
        NotificationType: $('[id$=ddlType]').val(),
        StatusId: $('[id$=ddlStage]').val(),
        ActionId: $('[id$=ddlAction]').val(),
        ActionStatus: $('[id$=ddlActionStatus]').val(),
        MeetingDate: $('[id$=txtMeetingDate]').val(),
        DateofNotification_From: $('[id$=txtNotificationfromdate]').val(),
        DateofNotification_To: $('[id$=txtNotificationtodate]').val(),
        Hscode: $('[id$=hdnHsCode]').val()
    }
    $.ajax({
        url: "/api/NotificationList/GetNotificationsList",
        async: false,
        type: "POST",
        data: JSON.stringify(obj),
        contentType: "application/json; charset=utf-8",
        success: function (result) {
            $('#divPaginationLoadingPanel').addClass('hidden');
            if (result != null) {
                if (PageIndx == 1) {
                    var Total = parseInt(result.TotalCount);
                    var tmpPages = Total / 10;
                    var tmpArr = tmpPages.toString().split('.');
                    var TotalPages = 0;
                    if (parseInt(tmpArr[1]) > 0)
                        TotalPages = parseInt(tmpArr[0]) + 1;
                    else
                        TotalPages = parseInt(tmpArr[0]);

                    $('[id$=hdnMaxPageIndex]').val(TotalPages);
                    $('#lblCount').text(parseInt(result.TotalCount));
                    $('#NotificationData').empty();
                }
                var NewRow = '';
                debugger;
                if (result.ItemsList != null && result.ItemsList.length > 0) {
                    $.each(result.ItemsList, function (i, v) {
                        NewRow += '<tr class="hoverborder">';
                        NewRow += '<td class="width-30"><a href="/WTO/NotificationView/' + v.NotificationId + '" target="_blank" class="red-color "><p class="red-color" style="width:450px;">' + v.NotificationNumber + '</p></a><p style="word-wrap: break-word; width:450px;"><b>Title :</b>' + v.Title + '</p></td>';
                        NewRow += '<td class="text-center width-10">' + v.NotificationDate + '</td>';
                        NewRow += '<td class="text-center width-10">' + v.FinalDateOfComments + '</td>';
                        NewRow += '<td class="text-center width-10">' + v.Country + '</td>';

                        //Discussion
                        if (result.NotificationProcessDots != null) {
                            var NotificationProcessDot = $.map(result.NotificationProcessDots, function (item, i) {
                                if (item.NotificationId == v.NotificationId)
                                    return item;
                            });

                            NotificationProcessDot.sort(function (a, b) {
                                return a.Sequence - b.Sequence;
                            });

                            NewRow += '<td class="tooltiprelative width-10">';
                            $.each(NotificationProcessDot, function (indx, val) {
                                NewRow += '<div class="small-circle" style="background:' + val.ColorCode + '" data-toggle="tooltip" data-placement="bottom" title="' + val.TooltipText + '"></div>';
                            });
                            NewRow += '</td>';
                        }

                        if (v.MailCount > 0)
                            NewRow += '<td class="text-center width-10">' + v.ResponseCount + '/' + v.MailCount + '</td>';
                        else
                            NewRow += '<td class="text-center width-10">--</td>';

                        //Actions
                        if (result.NotificationActionDots != null) {
                            var NotificationActionDots = $.map(result.NotificationActionDots, function (item, i) {
                                if (item.NotificationId == v.NotificationId)
                                    return item;
                            });

                            NotificationActionDots.sort(function (a, b) {
                                return a.Sequence - b.Sequence;
                            });

                            NewRow += '<td class="tooltiprelative">';
                            $.each(NotificationActionDots, function (indx, val) {
                                NewRow += '<div class="small-circle" style="background:' + val.ColorCode + '" data-toggle="tooltip" data-placement="bottom" title="' + val.TooltipText + '"></div>';
                            });
                            NewRow += '</td>';
                        }
                        else {
                            NewRow += '<td class="tooltiprelative width-10">--</td>';
                        }
                        NewRow += '<td class="width-10 text-right" >';
                        debugger;
                        if (!v.IsInMeeting)
                            NewRow += '<a data-callfor="' + v.MeetingDate + '" title="Add Meeting" onclick="return CheckNotificationExistMOM(this, ' + v.NotificationId + ');"><img src="/contents/img/MeetingIcon.png" style="width:25px;" /></a>';
                        NewRow += '<a href="../AddNotification/Edit_Notification/' + v.NotificationId + '" onclick="StoreHTML();"><img src="../contents/img/bedit.png" /></a>';
                        NewRow += '<a href="/API/AddUpdateNotification/Download/' + v.NotificationId + '" title="Export Details"><img src="/contents/img/export2.png"></a></td>';
                        NewRow += '</td></tr>';
                    });
                }
                else
                    NewRow += '<tr><td colspan="10">No Record Found ...</td></tr>';

                $('#NotificationCount').text('(' + result.TotalCount + ')');
                $('#hdnNotificationCount').val(result.TotalCount);

                $('#NotificationData').append(NewRow);
            }
            HideGlobalLodingPanel();
            IsScrollingOnce = true;
        },
        failure: function (result) {
            Alert("Alert", "Something went wrong.<br/>", "Ok");
        },
        error: function (result) {
            Alert("Alert", "Something went wrong.<br/>", "Ok");
        },
        complete: function () {

        }
    });
    return false;
}

function StoreHTML() {
    var html = $('#NotificationData').html();
    localStorage.setItem("NotificationList", html);
    localStorage.setItem("PageIndex", $('#hdnPageIndex').val());
    localStorage.setItem("MaxPageIndex", $('#hdnMaxPageIndex').val());
    localStorage.setItem("TotalCount", $('#lblCount').text());

    localStorage.setItem("Noti_Num", $('[id$=txtNotificationNumber]').val());
    localStorage.setItem("Noti_Country", $('[id$=ddlCountry]').val());
    localStorage.setItem("Noti_fromdate", $('[id$=txtfromdate]').val());
    localStorage.setItem("Noti_todate", $('[id$=txttodate]').val());
    localStorage.setItem("Noti_Type", $('[id$=ddlType]').val());
    localStorage.setItem("Noti_Stage", $('[id$=ddlStage]').val());
}

function Clear() {
    ShowGlobalLodingPanel();
    $('[id$=txtNotificationNumber]').val('');
    $('[id$=ddlCountry]').val('');
    //$('.combobox-container').find('.glyphicon-remove').click();
    $('[id$=txtfromdate]').val('');
    $("#fromdateclender").removeClass("hidden");
    $("#fromdatecross").addClass("hidden");
    $('[id$=txttodate]').val('');
    $("#todateclender").removeClass("hidden");
    $("#todatecross").addClass("hidden");
    $('[id$=ddlType]').val('0');
    $('[id$=ddlStage]').val('');
    $('[id$=ddlAction]').val('');
    $('[id$=ddlActionStatus]').val('');
    $('[id$=txtMeetingDate]').val('');
    $("#Meetingdateclender").removeClass("hidden");
    $("#Meetingdatecross").addClass("hidden");
    $('[id$=txtNotificationfromdate]').val('');
    $('[id$=txtNotificationtodate]').val('');
    $('[id$=txtHscode]').val('');
    $('[id$=hdnHsCode]').val('');

    //localStorage.setItem("NotificationList", '');
    //localStorage.setItem("PageIndex", 1);
    //localStorage.setItem("MaxPageIndex", 1);
    //localStorage.setItem("TotalCount", 0);

    //localStorage.setItem("Noti_Num", '');
    //localStorage.setItem("Noti_Country", '');
    //localStorage.setItem("Noti_fromdate", '');
    //localStorage.setItem("Noti_todate", '');
    //localStorage.setItem("Noti_Type", 0);
    //localStorage.setItem("Noti_Stage", '');

    SearchNotification(1);
    HideGlobalLodingPanel();
}

function txttodateClear() {
    $('[id$=txttodate]').val('');
    $("#todateclender").removeClass("hidden");
    $("#todatecross").addClass("hidden");

    return false;
}

function txtfromdateClear() {
    $('[id$=txtfromdate]').val('');
    $("#fromdateclender").removeClass("hidden");
    $("#fromdatecross").addClass("hidden");

    return false;
}

function txtMeetingdateClear() {
    $('[id$=txtMeetingDate]').val('');
    $("#Meetingdateclender").removeClass("hidden");
    $("#Meetingdatecross").addClass("hidden");

    return false;
}

function AdvanceSearch() {
    if ($("#GlySearch").hasClass("glyphicon glyphicon-plus")) {
        $("#GlySearch").removeClass("glyphicon glyphicon-plus");
        $("#GlySearch").addClass("glyphicon glyphicon-minus");
        $(".SearchAdvnc").removeClass('hidden');
        $("#DivBtn").addClass("col-xs-12 col-sm-12 col-md-12");
        $("#DivBtn").removeClass("adv");
    }
    else if ($("#GlySearch").hasClass("glyphicon glyphicon-minus")) {
        $("#GlySearch").removeClass("glyphicon glyphicon-minus");
        $("#GlySearch").addClass("glyphicon glyphicon-plus");
        $(".SearchAdvnc").addClass('hidden');
        $("#DivBtn").removeClass("col-xs-12 col-sm-12 col-md-12");
        $("#DivBtn").addClass("adv");

    }
    return false;
}

function GetParameterValues1(param) {

    var value = '';
    var url = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
    // alert(url.length);
    for (var i = 0; i < url.length; i++) {
        var urlparam = url[i].split('=');
        if (urlparam[0] == param) {
            value = urlparam[1];
        }
        //else
        //    return null;
    }
    return value;
}

function ExportToExcelNotification() {
    if ($('#hdnNotificationCount').val() != "") {
        if (parseInt($("#hdnNotificationCount").val()) > 0) {
            var NotificationNumber = $('[id$=txtNotificationNumber]').val();
            var CountryId = $('[id$=ddlCountry]').val();
            if (CountryId == "")
                CountryId = 0;
            var FinalDateOfComments_From = $('[id$=txtfromdate]').val();
            var FinalDateOfComments_To = $('[id$=txttodate]').val();
            var NotificationType = $('[id$=ddlType]').val();
            var StatusId = $('[id$=ddlStage]').val();
            if (StatusId == "")
                StatusId = 0;
            var ActionId = $('[id$=ddlAction]').val();
            if (ActionId == "")
                ActionId = 0;
            var ActionStatus = $('[id$=ddlActionStatus]').val();
            var MeetingDate = $('[id$=txtMeetingDate]').val();
            window.location.href = "NotificationList/NotificationExport?NotificationNumber=" + NotificationNumber + "&CountryId=" + CountryId + "&FinalDateOfComments_From=" + FinalDateOfComments_From + "&FinalDateOfComments_To=" + FinalDateOfComments_To + "&NotificationType=" + NotificationType + "&StatusId=" + StatusId + "&ActionId=" + ActionId + "&ActionStatus=" + ActionStatus + "&MeetingDate=" + MeetingDate;
        }
        else {
            Alert('Alert', 'No Record for Export', 'Ok');
        }
    }
    else {
        Alert('Alert', 'No Record for Export', 'Ok');
    }
}

function CheckNotificationExistMOM(ctrl, NotificationId) {
    debugger;
    var MeetingDate = $(ctrl).attr('data-callfor');
    $("#hdnNotificationId").val(NotificationId);
    if (MeetingDate != "") {
        Confirm('Alert', 'Do you want to add notification in ' + MeetingDate + '', 'Yes', 'No', 'AddNotiMeeting("' + MeetingDate + '")');
    }
    else {
        $("#ModelNotificationMeeting").modal('show');
    }
    return false;
}

function CloseMeetingDatepopup() {
    $("#ModelNotificationMeeting").modal('hide');
    return false;
}

function AddNotiMeeting(MeetingDate) {
    debugger;
    var NotificationId = $("#hdnNotificationId").val();
    $.ajax({
        url: "/api/NotificationList/SaveMeetingNotification?Id=" + NotificationId + "&MeetingDate=" + MeetingDate + "&CreatedBy=" + myWTOAPP.UserId,
        async: false,
        type: "POST",
        contentType: "application/json; charset=utf-8",
        success: function (result) {
            debugger;
            if (result == true) {
                Alert("Alert", "Notification has been added successfully", "Ok");
                $("#ModelNotificationMeeting").modal('hide');
            }
            else {
                Alert("Alert", "Notification already exist", "Ok");
                $("#ModelNotificationMeeting").modal('hide');
            }
            $("#hdnNotificationId").val('');
            $('#txtmeetingdate').val('');
        },
        failure: function (result) {
            Alert("Alert", "Something went wrong.<br/>", "Ok");
        },
        error: function (result) {
            Alert("Alert", "Something went wrong.<br/>", "Ok");
        },
        complete: function () {

        }
    });
}

function IsMeetingExists(MoMId) {
    if ($('[id$=txtmeetingdate]').val() != null && $('[id$=txtmeetingdate]').val() != '') {
        $.ajax({
            url: "/api/MoM/ValidateMeetingdate?date=" + $.trim($('[id$=txtmeetingdate]').val()) + "&MoMId=" + MoMId,
            async: false,
            type: "POST",
            contentType: "application/json; charset=utf-8",
            success: function (result) {
                if (result != "") {
                    Alert("Alert", result + "<br/>", "Ok");
                    $('[id$=txtmeetingdate]').val('');
                }
            },
            failure: function (result) {
                Alert("Alert", "Something went wrong.<br/>", "Ok");
            },
            error: function (result) {
                Alert("Alert", "Something went wrong.<br/>", "Ok");
            }
        });
        return false;
    }
}

function SaveMeetingAndNotification() {
    var MeetingDate = $.trim($('#txtmeetingdate').val());
    if (MeetingDate == "") {
        Alert('Alert', 'Select Meeting Date<br/>', 'Ok');
    }
    else {
        AddNotiMeeting(MeetingDate);
    }
    return false;
}

function txttodatenotiClear() {
    $('[id$=txtNotificationtodate]').val('');
    $("#todatenoticlender").removeClass("hidden");
    $("#todatenoticross").addClass("hidden");
    return false;
}

function txtfromdatenotiClear() {
    $('[id$=txtNotificationfromdate]').val('');
    $("#fromdatenoticlender").removeClass("hidden");
    $("#fromdatenoticross").addClass("hidden");
    return false;
}
