$(function () {
    var _ExistingNotifications = GetExistingNotifications();
    $('#hdnExistingNotificationsId').val(_ExistingNotifications);

    $('#txtSearch_EditMoM').on("keypress", function (e) {
        if (e.keyCode == 13) {
            SearchMeetingNotifications($(this).next('span'));
            return false; // prevent the button click from happening
        }

        if ($.trim($(this).val()) == "") {
            $('[id$=clearSearch_EditMoM]').addClass('hidden');
        }
        else
            $('[id$=clearSearch_EditMoM]').removeClass('hidden');
    });

    $('#txtSearch').on("keypress", function (e) {
        if (e.keyCode == 13) {
            SearchNotifications($(this).next('span'));
            return false; // prevent the button click from happening
        }

        if ($.trim($(this).val()) == "") {
            $('[id$=clearSearch]').addClass('hidden');
        }
        else
            $('[id$=clearSearch]').removeClass('hidden');
    });

    $('[id$=txtmeetingdate]').change(function () {
        if ($('[id$=txtmeetingdate]').val() != null && $('[id$=txtmeetingdate]').val() != '') {
            $.ajax({
                url: "/api/MoM/CheckIfOpenMeetingExists?date=" + $.trim($('[id$=txtmeetingdate]').val()),
                async: false,
                type: "POST",
                contentType: "application/json; charset=utf-8",
                success: function (result) {
                    var date = $.trim($('[id$=txtmeetingdate]').val());
                    if (result) {
                        Alert("Alert", "An open meeting on " + date + " already exists. Close previous meeting to schedule a new meeting.<br/>", "Ok");
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
    });

});

//***********************Add Meeting Start**********************************
function ValidateMeeting() {
    var msg = "";
    if ($.trim($('[id$=txtmeetingdate]').val()) == "")
        msg += "Please enter meeting date. <br/>";

    var MoMDetails = GetMoMDetails();
    var IsRowSelected = false;
    $.each($("[id$=tblNotificationMoM] tr"), function (index, value) {
        if (index > 0) {
            if ($(this).find("[id*=chkNotification]") != undefined) {
                if ($(this).find("[id*=chkNotification]").is(':checked'))
                    IsRowSelected = true;
            }
        }
    });
    if (IsRowSelected == false)
        msg += "Please select notification(s) for meeting. <br/>";

    if (msg.length > 0) {
        Alert("Alert", msg, "Ok");
    }
    else
        AddMoM();
}

function AddMoM() {
    var obj = {
        MeetingDate: $.trim($('[id$=txtmeetingdate]').val()),
        MeetingDetails: GetMoMDetails()
    };

    $.ajax({
        url: "/api/MoM/Insert/" + myWTOAPP.UserId,
        async: false,
        type: "POST",
        data: JSON.stringify(obj),
        contentType: "application/json; charset=utf-8",
        success: function (result) {
            if (result != null) {
                AlertwithFunction("Alert", "Saved successfully<br/>", "Ok", "AfterSave()");
            }
        },
        failure: function (result) {
            Alert("Alert", "Something went wrong.<br/>", "Ok");
        },
        error: function (result) {
            Alert("Alert", "Something went wrong.<br/>", "Ok");
        }
    });
}

function AfterSave() {
    location.href = window.location.origin + "/MoM";
}

function GetMoMDetails() {
    var _MoMDetails = [];
    $.each($("[id$=tblNotificationMoM] tr"), function (i, v) {
        if ($(this).find("[id*=chkNotification]").is(':checked')) {
            var _NotificationId = parseInt($.trim($(this).find("[id^=hdnNotificationId_]").val()));
            var _MeetingNote = $.trim($(this).find("[id^=txtMeetingNote_]").val());
            var _NotificationGroup = $.trim($(this).find("[id^=txtNotificationGroup_]").val());
            var MeetingDetails = {
                NotificationId: _NotificationId,
                MeetingNote: _MeetingNote,
                NotificationGroup: _NotificationGroup
            };
            _MoMDetails.push(MeetingDetails);
        }
    });
    return _MoMDetails;
}

function SearchNotifications(ctrl) {
    ShowGlobalLodingPanel();
    var _SearchFor = $(ctrl).attr('data-SearchFor');
    if (_SearchFor == 'Clear') {
        $('#txtSearch').val('');
        $('#clearSearch').addClass('hidden')
    }
    else {
        var _ExistingNotifications = GetExistingNotifications();
        _ExistingNotifications = $('#hdnExistingNotificationsId').val() + _ExistingNotifications;
        $('#hdnExistingNotificationsId').val(_ExistingNotifications);

        $('#clearSearch').removeClass('hidden');
    }

    var _SearchText = $.trim($('#txtSearch').val());

    var obj = {
        callFor: 'All',
        ExistingNotifications: $('#hdnExistingNotificationsId').val(),
        SearchText: _SearchText
    }

    $.ajax({
        url: "/api/MoM/getNotifications",
        async: false,
        type: "POST",
        data: JSON.stringify(obj),
        contentType: "application/json; charset=utf-8",
        success: function (result) {
            var tblCount = 0;
            if ($('#tblNotificationMoM > tbody').find("[Type='checkbox']").attr('id') != null) {
                if ($('#tblNotificationMoM > tbody').length > 0)
                    tblCount = $('#tblNotificationMoM > tbody').length + 1;
            }
            else
                tblCount = 1;

            var html = '';
            if (result.Notification_MomList != null && result.Notification_MomList.length > 0) {
                $.each(result.Notification_MomList, function (i, v) {
                    html += '<tr>' +
                        '<td class="text-center">' +
                        '<div class="checkbox radio-margin" style="margin-top:2px;">' +
                        '<label>' +
                        '<input type="checkbox" name="Action" id="chkNotification_' + tblCount + '" checked="checked" onchange="CheckHeaderCheckbox()" />' +
                        '<span class="cr"><i class="cr-icon glyphicon glyphicon-ok cr-small"></i></span>' +
                        '</label>' +
                        '</div>' +
                        '<input type="hidden" id="hdnNotificationId_' + tblCount + '" value="' + v.NotificationId + '" />' +
                        '</td>' +
                        '<td style="width:400px;">' +
                        '<p class="red-color NotiNumber" data-toggle="tooltip" data-placement="bottom" title="' + v.Description + '"><a href="/WTO/NotificationView/' + v.NotificationId + '" target="_blank" class="red-color ">' + v.NotificationNumber + '</a></p>' +
                        '<p style="word-wrap:break-word; width:400px;">' + v.Title + '</p>' +
                        '</td>' +
                        '<td class="text-center">' + v.FinalDateofComments + '</td>' +
                        '<td class="text-center">' + v.Country + '</td>';

                    //Discussion
                    if (result.NotificationProcessDots != null) {
                        var NotificationProcessDot = $.map(result.NotificationProcessDots, function (item, i) {
                            if (item.NotificationId == v.NotificationId)
                                return item;
                        });

                        NotificationProcessDot.sort(function (a, b) {
                            return a.Sequence - b.Sequence;
                        });

                        html += '<td class="tooltiprelative">';
                        $.each(NotificationProcessDot, function (indx, val) {
                            html += '<div class="small-circle" style="background:' + val.ColorCode + '" data-toggle="tooltip" data-placement="bottom" title="' + val.TooltipText + '"></div>';
                        });
                        html += '</td>';
                    }
                    html += '<td>' +
                        '<textarea class="form-control textboxcontrol AutoHeight" cols="30" rows="1" id="txtMeetingNote_' + tblCount + '"></textarea>' +
                        '</td>' +
                        '<td>' +
                        '<input type="text" class="form-control textboxcontrol" maxlength="5" id="txtNotificationGroup_' + tblCount + '" value="' + v.NotificationGroup + '" />' +
                        '</td>' +
                        '</tr>';
                    tblCount++;
                });
            }
            else {
                html += '<tr><td colspan="6"> No record found</td></tr>';
            }
            $('[id$=tblNotificationMoM] tr> td').each(function (index, value) {

                if ($(value).attr('id') == 'tdNorecordFound') {
                    $('#tblNotificationMoM > tbody').html('');
                }
            })
            $('#tblNotificationMoM > tbody').empty();
            $('#tblNotificationMoM > tbody').append(html);
            $("#chkSelectAll").prop('checked', false);
            CheckHeaderCheckbox();
        },
        failure: function (result) {
            Alert("Alert", "Something went wrong.<br/>", "Ok");
        },
        error: function (result) {
            Alert("Alert", "Something went wrong.<br/>", "Ok");
        },
        complete: function () {
            HideGlobalLodingPanel();
            autosize($(".AutoHeight"));
        }
    });
    return false;
}

function GetAllNotifications(ctrl) {
    ShowGlobalLodingPanel();
    var NotificationId = "";
    var _SelectedNotifications = "";
    var len = $('#tblFilterResult tbody td').length;
    if (len > 0) {
        $('#tblFilterResult tbody tr').each(function () {
            var row = $(this);
            if (row.find('input[type="checkbox"]').is(':checked')) {
                _SelectedNotifications += (row.find('input[type="hidden"]')).val() + ",";
            }
        });
        if (_SelectedNotifications == "") {
            Alert('Alert', 'Please select atleast one notification.<br/>', 'Ok');
            HideGlobalLodingPanel();
        }
        else {
            var _ExistingNotifications = GetExistingNotifications();

            if ($.trim($('#txtSearch').val()) != "")
                _ExistingNotifications = $('#hdnExistingNotificationsId').val();
            else
                $('#hdnExistingNotificationsId').val(_ExistingNotifications);

            var _CallFor = $(ctrl).attr('data-CallFor');
            var _CountryId = 0;
            var _NotificationNumber = $.trim($("#txtNotificationNo").val());

            $('#txtSearch').val('');
            $('#txtSearch').next().find('i').attr('class', 'glyphicon glyphicon-search');

            var obj = {
                callFor: _CallFor,
                //NotificationNumber: _NotificationNumber,
                ExistingNotifications: _ExistingNotifications,
                SelectedNotifications: _SelectedNotifications
            }

            $.ajax({
                url: "/api/MoM/getNotifications",
                async: false,
                type: "POST",
                data: JSON.stringify(obj),
                contentType: "application/json; charset=utf-8",
                success: function (result) {
                    var tblCount = 0;
                    if ($('#tblNotificationMoM > tbody').find("[Type='checkbox']").attr('id') != null) {
                        if ($('#tblNotificationMoM > tbody').length > 0)
                            tblCount = $('#tblNotificationMoM > tbody').length + 1;
                    }
                    else
                        tblCount = 1;

                    var html = '';
                    if (result.Notification_MomList.length > 0) {
                        $.each(result.Notification_MomList, function (i, v) {
                            html += '<tr>' +
                                '<td class="text-center">' +
                                '<div class="checkbox radio-margin" style="margin-top:2px;">' +
                                '<label>' +
                                '<input type="checkbox" name="Action" id="chkNotification_' + tblCount + '" checked="checked" onchange="CheckHeaderCheckbox()" />' +
                                '<span class="cr"><i class="cr-icon glyphicon glyphicon-ok cr-small"></i></span>' +
                                '</label>' +
                                '</div>' +
                                '<input type="hidden" id="hdnNotificationId_' + tblCount + '" value="' + v.NotificationId + '" />' +
                                '</td>' +
                                '<td style="width:400px;">' +
                                '<p class="red-color NotiNumber" data-toggle="tooltip" data-placement="bottom" title="' + v.Description + '"><a href="/WTO/NotificationView/' + v.NotificationId + '" target="_blank" class="red-color ">' + v.NotificationNumber + '</a></p>' +
                                '<p style="word-wrap:break-word; width:400px;">' + v.Title + '</p>' +
                                '</td>' +
                                '<td class="text-center">' + v.FinalDateofComments + '</td>' +
                                '<td class="text-center">' + v.Country + '</td>';

                            //Discussion
                            if (result.NotificationProcessDots != null) {
                                var NotificationProcessDot = $.map(result.NotificationProcessDots, function (item, i) {
                                    if (item.NotificationId == v.NotificationId)
                                        return item;
                                });

                                NotificationProcessDot.sort(function (a, b) {
                                    return a.Sequence - b.Sequence;
                                });

                                html += '<td class="tooltiprelative">';
                                $.each(NotificationProcessDot, function (indx, val) {
                                    html += '<div class="small-circle" style="background:' + val.ColorCode + '" data-toggle="tooltip" data-placement="bottom" title="' + val.TooltipText + '"></div>';
                                });
                                html += '</td>';
                            }
                            html += '<td>' +
                                '<textarea class="form-control textboxcontrol AutoHeight" cols="30" rows="1" id="txtMeetingNote_' + tblCount + '"></textarea>' +
                                '</td>' +
                                '<td>' +
                                '<input type="text" class="form-control textboxcontrol" maxlength="5" id="txtNotificationGroup_' + tblCount + '" value="' + v.NotificationGroup + '" />' +
                                '</td>' +
                                '</tr>';
                            tblCount++;
                        });
                    }
                    else {
                        html += '<tr><td colspan="6"> No record found</td></tr>';
                    }
                    $('[id$=tblNotificationMoM] tr> td').each(function (index, value) {

                        if ($(value).attr('id') == 'tdNorecordFound') {
                            $('#tblNotificationMoM > tbody').html('');
                        }
                    })
                    $('#tblNotificationMoM > tbody').empty();
                    $('#tblNotificationMoM > tbody').append(html);
                    $("#ddlCountry").val("");
                    $("#txtNotificationNo").val('');
                    $("#chkSelectAll").prop('checked', false);
                    CheckHeaderCheckbox();
                },
                failure: function (result) {
                    Alert("Alert", "Something went wrong.<br/>", "Ok");
                },
                error: function (result) {
                    Alert("Alert", "Something went wrong.<br/>", "Ok");
                },
                complete: function () {
                    autosize($(".AutoHeight"));
                    $("#ModalAddNotiFication").modal('hide');
                    HideGlobalLodingPanel();
                }
            });
        }
    }
    else
        HideGlobalLodingPanel();
}

function GetFilterNotification(ctrl) {
    ShowGlobalLodingPanel();
    var NotificationId = "";
    var _CallFor = $(ctrl).attr('data-CallFor');
    var _CountryId = $("#ddlCountry").val() == "" ? 0 : $("#ddlCountry").val();
    var _NotificationNumber = $.trim($("#txtNotificationNo").val());
    var _ExistingNotifications = GetExistingNotifications();

    if ($.trim($('#txtSearch').val()) != "")
        _ExistingNotifications = $('#hdnExistingNotificationsId').val();
    else
        $('#hdnExistingNotificationsId').val(_ExistingNotifications);

    var obj = {
        callFor: _CallFor,
        CountryId: _CountryId,
        NotificationNumber: _NotificationNumber,
        ExistingNotifications: _ExistingNotifications
    }

    $.ajax({
        url: "/api/MoM/getNotifications",
        async: false,
        type: "POST",
        data: JSON.stringify(obj),
        contentType: "application/json; charset=utf-8",
        success: function (result) {
            $('#tblFilterResult > tbody').empty();
            $('#tblFilterResult').css("display", "block");
            var html = '';
            if (result.Notification_MomList != null) {
                if (result.Notification_MomList.length > 0) {
                    $.each(result.Notification_MomList, function (i, v) {
                        html += '<tr>';
                        html += '<td> <input type="hidden" id="hdnNotifId_' + v.ItemNumber + '" value="' + v.NotificationId + '" />';
                        html += '<div class="checkbox radio-margin" style="margin-top:2px;">';
                        html += '<label>';
                        html += '<input type="checkbox" name="Action" id="chkNot_' + v.ItemNumber + '" onchange="return CheckList();"/>';
                        html += '<span class="cr"><i class="cr-icon glyphicon glyphicon-ok cr-small"></i></span>';
                        html += '</label>';
                        html += '</div >';
                        html += '</td>';
                        html += '<td style="cursor:pointer" ><p data-toggle="tooltip" data-placement="bottom" title="' + v.Description + '">' + v.NotificationNumber + '</p></td>';
                        html += '<td>' + v.FinalDateofComments + '</td>';
                        html += '<td>' + v.Country + '</td>';

                        //Discussion
                        if (result.NotificationProcessDots != null) {
                            var NotificationProcessDot = $.map(result.NotificationProcessDots, function (item, i) {
                                if (item.NotificationId == v.NotificationId)
                                    return item;
                            });

                            NotificationProcessDot.sort(function (a, b) {
                                return a.Sequence - b.Sequence;
                            });

                            html += '<td class="tooltiprelative">';
                            $.each(NotificationProcessDot, function (indx, val) {
                                html += '<div class="small-circle" style="background:' + val.ColorCode + '" data-toggle="tooltip" data-placement="bottom" title="' + val.TooltipText + '"></div>';
                            });
                            html += '</td>';
                        }
                        html += '</tr>';
                    });
                }
            }
            else {
                html += '<tr><td colspan="5"> No record found</td></tr>';
            }
            $('#chkSelectAll').prop('checked', false);
            $('#tblFilterResult tbody').empty();
            $('#tblFilterResult tbody').append(html);
        },
        failure: function (result) {
            Alert("Alert", "Something went wrong.<br/>", "Ok");
        },
        error: function (result) {
            Alert("Alert", "Something went wrong.<br/>", "Ok");
        },
        complete: function () {
            HideGlobalLodingPanel();
            autosize($(".AutoHeight"));
        }
    });
}

function SelectAllNotification_Popup(ctrl) {
    var IsCheckedAll = ctrl.checked;
    $.each($(ctrl).closest('table').find('tbody > tr > td input[type=checkbox]'), function () {
        $(this).prop('checked', IsCheckedAll);
    });
}
//***********************Add Meeting End**********************************

function SearchMeetingNotifications(ctrl) {
    var _SearchFor = $(ctrl).attr('data-SearchFor');
    if (_SearchFor == 'Clear') {
        $('#txtSearch_EditMoM').val('');
        $('#clearSearch_EditMoM').addClass('hidden')
    }
    else
        $('#clearSearch_EditMoM').removeClass('hidden')

    var _SearchText = $.trim($('#txtSearch_EditMoM').val());
    var obj = {
        callFor: $.trim($(ctrl).attr('data-CallFor')),
        SearchText: _SearchText
    }

    $.ajax({
        url: "/api/MoM/Edit/" + myWTOAPP.id,
        async: false,
        type: "POST",
        data: JSON.stringify(obj),
        contentType: "application/json; charset=utf-8",
        success: function (result) {
            var html = '';
            var _MomId = result.MoMId;
            if (result.Notifications != null && result.Notifications.length > 0) {
                $.each(result.Notifications, function (i, v) {
                    html += '<tr>' +
                        '<td class="width-35">' +
                        '<a href="/WTO/NotificationView/' + v.NotificationId + '?MOMId=' + _MomId + '&amp;R=' + v.RowNum + '&amp;Total=' + v.TotalRow + '" class="red-color" target="_blank"><p class="red-color NotiNumber" data-toggle="tooltip" data-placement="bottom" title="' + v.Description + '">' + v.NotificationNumber + '</p></a>' +
                        '<p style="word-wrap:break-word; width:450px;">' +
                        '<b>Title: </b>' + v.Title +
                        '</p>' +
                        '<p>';

                    if (v.NotificationGroup != "")
                        html += '<b>Group : </b><span>' + v.NotificationGroup + '</span>';

                    html += '</p>' +
                        '<p>';

                    if (v.NotificationGroup != "")
                        html += '<b><a onclick="OpenAddNote(35)">Note</a></b><span id="MeetingNoteId">: Test Meeting</span>';

                    html += '<input id="hdnNotificationId" name="hdnNotificationId" type="hidden" value="35">' +
                        '</p>' +
                        '</td >' +
                        '<td class="text-center width-10">Brazil</td>';

                    //Discussion
                    if (result.Actions != null) {
                        $.each(result.Actions, function (indx, val) {
                            var Action = $.map(result.NotificationActions, function (item, i) {
                                if (item.NotificationId == v.NotificationId && item.ActionId == val.ActionId)
                                    return item;
                            });

                            if (Action.length > 0) {
                                debugger;
                                var _Action = Action[0];
                                if (_Action != null && (_Action.ActionId > 4 || (_Action.MailId > 0 && _Action < 4))) {
                                    html += '<td class="text-center width-10">' +
                                        '<span class="glyphicon glyphicon-ok dark-green-color font-20" id="span_' + v.ItemNumber + '"></span>' +
                                        '</td>';
                                }
                                else if (_Action != null) {
                                    html += '<td class="text-center width-10">' +
                                        '<span class="glyphicon glyphicon-hourglass dark-green-color font-20" id="span_' + v.ItemNumber + '"></span>' +
                                        '</td>';
                                }
                                else {
                                    html += '<td class="text-center width-10">' +
                                        '<span id="span_' + v.ItemNumber + '"></span>' +
                                        '</td>';
                                }
                            }
                            else
                                html += '<td class="text-center width-10"></td>';
                        });
                    }
                    if (v.IsUpdate)
                        html += '<td class="text-center width-5"><a data-searchfor="35" onclick="EditNotificationActions(this);"><img src="/contents/img/bedit.png"></a></td>';
                    else
                        html += '<td class="width-10"></td>';

                    html += '</tr>';
                });
            }
            else {
                html += '<tr><td colspan="6"> No record found</td></tr>';
            }

            $('#tblNotificationMoM > tbody').empty();
            $('#tblNotificationMoM > tbody').append(html);
        },
        failure: function (result) {
            Alert("Alert", "Something went wrong.<br/>", "Ok");
        },
        error: function (result) {
            Alert("Alert", "Something went wrong.<br/>", "Ok");
        },
        complete: function () {
            HideGlobalLodingPanel();
            autosize($(".AutoHeight"));
        }
    });
    return false;
}

function OpenPopup() {
    $("#ModalAddNotiFication").modal('show');
    $("#btnSearchPopUp").click();
}

function ClosePopup() {
    $('#tblFilterResult tbody').empty();
    $("#chkSelectAll").prop('checked', false);
    $("#ddlCountry").val("");
    $("#txtNotificationNo").val('');
    $('#tblFilterResult').css("display", "none");
    $("#ModalAddNotiFication").modal('hide');
    return false;
}

function CheckList() {

    var IsCheckedAll = true;
    $.each($('#tblFilterResult tbody tr').find('input[type=checkbox]'), function () {
        if (!this.checked)
            IsCheckedAll = false;
    });

    if (IsCheckedAll)
        $('#chkSelectAll').prop('checked', true);
    else
        $('#chkSelectAll').prop('checked', false);
    return false;
}

function CheckSelectAll(ctrl) {
    $('[id$=tblNotificationMoM]').find('[id^=chkNotification_]').prop('checked', $(ctrl).is(':checked'));
    $('.chkSelectAllNotification').prop('checked', $(ctrl).is(':checked'));
}

function CheckHeaderCheckbox() {
    var totalrowcount = 0;
    var checkrowcount = 0;
    $('[id$=tblNotificationMoM]').find('[id^=chkNotification_]').each(function (index, value) {
        totalrowcount++;
        if ($(value).prop('checked'))
            checkrowcount++;
    })
    if (checkrowcount == totalrowcount)
        $('.chkSelectAllNotification').prop('checked', true);
    else
        $('.chkSelectAllNotification').prop('checked', false);
}

function GetExistingNotifications() {
    var Notifications = "";
    var len = $('#tblNotificationMoM tbody td').length;
    if (len > 0) {
        $('#tblNotificationMoM tbody tr').each(function () {
            var row = $(this);
            if (row.find('[id^=hdnNotificationId_]').val() != undefined)
                Notifications += (row.find('[id^=hdnNotificationId_]')).val() + ",";
        });
    }
    return Notifications;
}

function OpenMeetingDatePopup() {
    $('#txtmeetingdate').val($('#lblMeetingDate').text());
    $("#ModelMeetingDate").modal('show');
    return false;
}

function CloseMeetingDatepopup() {
    $('#txtmeetingdate').val($('#lblMeetingDate').text());
    $("#ModelMeetingDate").modal('hide');
    return false;
}

function UpdateMeetingDate() {

    var MoMId = 0;
    var msg = "";
    MoMId = $("#hdnMomId").val();
    var MeetingDate = $.trim($("#txtmeetingdate").val());
    if (MeetingDate == "")
        msg += "Please Enter Meeting Date \n";
    if (msg.length > 0) {
        alert(msg);
        return false;
    }
    else if (Date.parse($("#lblMeetingDate").text()) == Date.parse(MeetingDate)) {
        CloseMeetingDatepopup();

    }
    else {
        $.ajax({
            url: "/api/MoM/UpdateMeetingDate?Id=" + MoMId + "&MeetingDate=" + MeetingDate,
            async: false,
            type: "POST",
            contentType: "application/json; charset=utf-8",
            success: function (result) {

                if (result != "") {
                    if (result.IsExistFlag == 0) {
                        Alert("Alert", "Meeting Date Already exists", "Ok");

                    }
                    else {
                        $("#lblMeetingDate").text(result.MeetingDate);
                        Alert("Alert", "Meeting Date has been updated", "Ok");
                    }
                }
            },
            failure: function (result) {
                Alert("Alert", "Something went wrong.<br/>", "Ok");
            },
            error: function (result) {
                Alert("Alert", "Something went wrong.<br/>", "Ok");
            },
            complete: function () {
                CloseMeetingDatepopup();
                autosize($(".AutoHeight"));
            }
        })
    }
    return false;
}

//===================================================

function closeActionMail(ctrl) {
    var callfor = $(ctrl).attr('data-callfor');
    if (callfor == "close") {
        $('.closeplanAction').attr('data-callfor', 'close');
        $('#ActionMail').modal('hide');
    }
    else {
        $('#ActionMail').modal('hide');
        BindNotificationActions();
        $('.closeplanAction').attr('data-callfor', 'closeaction');
    }
}

function closeModalAddAction(ctrl) {
    var callfor = $(ctrl).attr('data-callfor');
    if (callfor == "close") {
        $('#ModalAddAction').modal('hide');
    }
    else {
        window.location.reload();
    }
}

function BindRegulatoryBodies() {
    $.ajax({
        url: "/API/Masters/GetRegulatoryBodies",
        async: false,
        type: "GET",
        contentType: "application/json; charset=utf-8",
        success: function (result) {
            // $('[Id$=ddlTo]').multiselect('destroy');
            //$('[Id$=ddlTo]').multiselect('refresh');
            $('[Id$=ddlTo]').empty();
            $.each(result, function (i, v) {
                $('[Id$=ddlTo]').append('<option value="' + v.Email + '"> ' + v.Name + ' </option>');
            });
        },
        complete: function () {
            $('[Id$=ddlTo]').multiselect({
                maxHeight: 150,
                enableFiltering: true,
                numberDisplayed: 1,
                nonSelectedText: '--Select Recipients--'
            });
        },
        failure: function (result) {
            Alert("Alert", "Something went wrong.<br/>", "Ok");
        },
        error: function (result) {
            Alert("Alert", "Something went wrong.<br/>", "Ok");
        }

    });
}

function BindInternalStakeholders() {
    $.ajax({
        url: "/API/Masters/GetInternalStakeHolder",
        async: false,
        type: "GET",
        contentType: "application/json; charset=utf-8",
        success: function (result) {
            $('[Id$=ddlTo]').multiselect('destroy');
            $('[Id$=ddlTo]').empty();
            $.each(result, function (i, v) {
                $('[Id$=ddlTo]').append('<option value="' + v.Email + '"> ' + v.Name + ' </option>');
            });
        },
        failure: function (result) {
            Alert("Alert", "Something went wrong.<br/>", "Ok");
        },
        error: function (result) {
            Alert("Alert", "Something went wrong.<br/>", "Ok");
        },
        complete: function () {
            $('[Id$=ddlTo]').multiselect({
                maxHeight: 150,
                enableFiltering: true,
                numberDisplayed: 1,
                nonSelectedText: '--Select Recipients--'
            });
        }
    });
}

function OpenAddNote(NotificationId) {
    $("#AddNote").modal('show');
    GetMeetingNote(NotificationId);
    setInterval("autosize.update($('#NoteId'));", 100);
}

function CloseAddNote() {
    $('#NoteId').val('');
    $("#AddNote").modal('hide');
    RefreshAutoheight();
}

function GetMeetingNote(NotificationId) {
    $.ajax({
        url: "/api/AddUpdateNotification/GetMeetingNotes/" + NotificationId,
        async: false,
        type: "GET",
        contentType: "application/json; charset=utf-8",
        success: function (result) {
            if (result != null) {
                $('#NoteId').val(result);
                $('#hdnNotificationId').val(NotificationId);
            }
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

function BindNotificationActions() {
    var takeaction = 0;
    $.ajax({
        url: "/api/MoM/EditAction/" + $('#hdnNotificationId').val(),
        async: false,
        type: "GET",
        contentType: "application/json; charset=utf-8",
        success: function (result) {
            if (result != null) {
                $('#lblNotificationNumber').text(result.NotificationNumber);
                $('#lblNotificationTitle').text(result.Title);
                $('#lblNotificationStage').text(result.Status);
                $('#lblMeetingDate').text($('#txtmeetingdate').val());
                $('#txtMeetingNote').val(result.MeetingNote);

                $('#divPlanActions').empty();
                $('#divTakeActionHeader').addClass('hidden');
                var HTML = '';
                $.each(result.Actions, function (i, v) {
                    HTML += '<div class="row seprater">' +
                        '<input type="hidden" id="hdnNotificationActionId" value="' + v.NotificationActionId + '" />' +
                        '<div class="col-sm-1 pdright">';

                    if (v.NotificationActionId > 0) {
                        if (v.UpdatedOn == "") {
                            HTML += '<div class="checkbox radio-margin" style="margin-top: 0;float: left; margin-left:0;">' +
                                '<label style="padding-left: 0;">' +
                                '<input type="checkbox" value="' + v.ActionId + '" onchange="AddRemoveActions(this);" checked/>' +
                                '<span class="cr insertcheckbox" style="margin-top: 2px;">' +
                                '<i class="cr-icon glyphicon glyphicon-ok"></i>' +
                                '</span>' +
                                '</label>' +
                                '</div>';
                        }
                        else {
                            HTML += '<div class="checkbox radio-margin" style="margin-top: 0;float: left; margin-left:0;">' +
                                '<label style="padding-left: 0;">' +
                                '<input type="checkbox" value="' + v.ActionId + '" onchange="AddRemoveActions(this);" checked disabled="disabled"/>' +
                                '<span class="cr insertcheckbox" style="margin-top: 2px;">' +
                                '<i class="cr-icon glyphicon glyphicon-ok"></i>' +
                                '</span>' +
                                '</label>' +
                                '</div>';
                        }
                    }
                    else {
                        if (result.RetainedForNextDiscussion && v.ActionId == 5) {
                            HTML += '<div class="checkbox radio-margin" style="margin-top: 0;float: left; margin-left:0;">' +
                                '<label style="padding-left: 0;">' +
                                '<input type="checkbox" value="' + v.ActionId + '" onchange="AddRemoveActions(this);" checked="checked" disabled="disabled"/>' +
                                '<span class="cr insertcheckbox" style="margin-top: 2px;">' +
                                '<i class="cr-icon glyphicon glyphicon-ok"></i>' +
                                '</span>' +
                                '</label>' +
                                '</div>';
                        }
                        else if (result.RetainedForNextDiscussion) {
                            HTML += '<div class="checkbox radio-margin" style="margin-top: 0;float: left; margin-left:0;">' +
                                '<label style="padding-left: 0;">' +
                                '<input type="checkbox" value="' + v.ActionId + '" onchange="AddRemoveActions(this);" disabled="disabled" />' +
                                '<span class="cr insertcheckbox" style="margin-top: 2px;">' +
                                '<i class="cr-icon glyphicon glyphicon-ok"></i>' +
                                '</span>' +
                                '</label>' +
                                '</div>';
                        }
                        else {
                            HTML += '<div class="checkbox radio-margin" style="margin-top: 0;float: left; margin-left:0;">' +
                                '<label style="padding-left: 0;">' +
                                '<input type="checkbox" value="' + v.ActionId + '" onchange="AddRemoveActions(this);" />' +
                                '<span class="cr insertcheckbox" style="margin-top: 2px;">' +
                                '<i class="cr-icon glyphicon glyphicon-ok"></i>' +
                                '</span>' +
                                '</label>' +
                                '</div>';
                        }
                    }

                    HTML += '</div>' +
                        '<div class="col-sm-5">' + v.ActionName + '</div>';

                    if (v.ActionId < 4) {
                        HTML += '<div class="col-sm-4">' +
                            '<div class="form-group has-feedback" style="margin-bottom:0px;">' +
                            '<input type="text" id="RequiredOnId_' + v.ActionId + '" class="form-control date-picker ' + (v.UpdatedOn != "" ? "disabled" : "") + '" onkeydown="return false;" data-SearchFor="' + v.ActionId + '" value="' + v.RequiredOn + '" />' +
                            '<i class="glyphicon glyphicon-calendar form-control-feedback blue-color" style="right: 0;"></i>' +
                            '</div>' +
                            '</div>';
                        if (v.NotificationActionId != 0 && v.MailId == 0) {
                            var callfor = "takeaction";
                            takeaction++;
                            HTML += "<div class='col-sm-2'><a data-SearchFor='" + v.ActionName + "' data-callfor='" + callfor + "' onclick='EditAction(" + v.NotificationActionId + ",this);'>Take Action</a><input type='hidden' id='hdnActionId_" + v.NotificationActionId + "' value='" + v.ActionId + "'/></div>";
                        }
                    }
                    HTML += '</div>';
                    if (takeaction > 0) {
                        $('#divTakeActionHeader').removeClass('hidden');
                    }
                });
                $('#divPlanActions').append(HTML);
                $('#ModalAddAction').modal('show');

                if (result.RetainedForNextDiscussion)
                    $("#btnSaveActions").addClass("hidden");
                else
                    $("#btnSaveActions").removeClass("hidden");
            }
        },
        failure: function (result) {
            Alert("Alert", "Something went wrong.<br/>", "Ok");
        },
        error: function (result) {
            Alert("Alert", "Something went wrong.<br/>", "Ok");
        },
        complete: function () {
            $(".date-picker").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: 'dd M yy'
            });
            HideGlobalLodingPanel();
        }
    });
}

function EditNotificationActions(ctrl) {
    ShowGlobalLodingPanel();
    if (typeof ctrl != "undefined")
        $('#hdnNotificationId').val($(ctrl).attr('data-searchfor'));
    BindNotificationActions();
}

function EndMeeting() {
    Confirm('End meeting', 'Do you want to end current meeting and retain notifications for next meeting, if pending for action ?', 'Yes', 'No', 'SaveEndMeeting()');
}

function SaveEndMeeting() {
    var MoMId = 0;
    MoMId = $("#hdnMomId").val();
    $.ajax({
        url: "/api/MoM/EndMeeting/" + MoMId,
        async: false,
        type: "POST",
        contentType: "application/json; charset=utf-8",
        success: function (result) {
            if (result) {
                Alert("Alert", "Meeting has been successfully ended.<br/>", "Ok");
                location.href = window.location.origin + "/MoM/Add/0";
            }
            else
                Alert("Alert", "Error occured", "Ok");
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