$(function () {
    var len = $('#tblNotificationMoM tbody tr').length;
    var Id = "";
    if (len > 0) {
        $.each($("#tblNotificationMoM > tbody > tr"), function (index, value) {
            Id += $(this).find("#hdnNotificationId_" + (index + 1)).val() + ",";
        });
        if (Id != "") {
            $("#hdnNId").val(Id);
        }
    }
    $('#chkSelectAll').change(function () {
        var IsCheckedAll = this.checked;
        $.each($(this).closest('table').find('tbody > tr > td > input[type=checkbox]'), function () {
            this.checked = IsCheckedAll;
        });
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
                AlertwithFunction("Alert", "Saved successfully<br/>", "Ok", "Afterdsave()");
            }

        },
        failure: function (result) {
            Alert("Alert", "Something went wrong.<br/>", "Ok");
        },
        error: function (result) {
            Alert("Alert", "Something went wrong.<br/>", "Ok");
        },
        complete: function () {
            //HideGlobalLodingPanel();
        }
    });
}

function Afterdsave() {
    location.href = window.location.origin + "/MoM";
}

function GetMoMDetails() {
    var _MoMDetails = [];
    $.each($("[id$=tblNotificationMoM] tr"), function (i, v) {
        if ($(this).find("[id*=chkNotification]").is(':checked')) {
            var _NotificationId = parseInt($.trim($(this).find("[id*=hdnNotificationId]").val()));
            var _MeetingNote = $.trim($(this).find("[id*=txtMeetingNote]").val());
            var MeetingDetails = {
                NotificationId: _NotificationId,
                MeetingNote: _MeetingNote
            };
            _MoMDetails.push(MeetingDetails);
        }
    });
    return _MoMDetails;
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
            $("#hdnNId").val($("#hdnNId").val() + NotificationId);
            var _CallFor = $(ctrl).attr('data-CallFor');
            var _CountryId = 0;
            var _NotificationNumber = $.trim($("#txtNotificationNo").val());
            var SelectedNotificationId = '';

            var obj = {
                callFor: _CallFor,
                NotificationNumber: _NotificationNumber,
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
                                '<textarea class="form-control textboxcontrol AutoHeight" cols="30" rows="2" id="txtMeetingNote_' + tblCount + '"></textarea>' +
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

                    HideGlobalLodingPanel();
                    autosize($(".AutoHeight"));
                    $("#ModalAddNotiFication").modal('hide');
                }
            });
        }
    }
    else {
        HideGlobalLodingPanel();
    }
}

function GetFilterNotification(ctrl) {
    ShowGlobalLodingPanel();
    var NotificationId = "";
    var _CallFor = $(ctrl).attr('data-CallFor');
    var _CountryId = $("#ddlCountry").val() == "" ? 0 : $("#ddlCountry").val();
    var _NotificationNumber = $.trim($("#txtNotificationNo").val());
    var _ExistingNotifications = GetExistingNotifications();

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
                var arr = $("#hdnNId").val().split(',');
                if (result.Notification_MomList.length > 0) {
                    $.each(result.Notification_MomList, function (i, v) {
                        html += '<tr>';
                        html += '<td> <input type="hidden" id="hdnNotifId_' + v.ItemNumber + '" value="' + v.NotificationId + '" /><input type="checkbox" name="Action" id="chkNot_' + v.ItemNumber + '" onchange="return CheckList();"/></td>';
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
//***********************Add Meeting End**********************************

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

function CheckSelectAll() {
    $('[id$=tblNotificationMoM]').find('[id*=chkNotification]').prop('checked', $('[id$=chkSelectAllNotification]').prop('checked'));
}

function CheckHeaderCheckbox() {
    var totalrowcount = 0;
    var checkrowcount = 0;
    $('[id$=tblNotificationMoM]').find('[id*=chkNotification]').each(function (index, value) {
        totalrowcount++;
        if ($(value).prop('checked'))
            checkrowcount++;
    })
    if (checkrowcount == totalrowcount)
        $('[id$=chkSelectAllNotification]').prop('checked', true);
    else
        $('[id$=chkSelectAllNotification]').prop('checked', false);
}

function GetExistingNotifications() {
    var Notifications = "";
    var len = $('#tblNotificationMoM tbody td').length;
    if (len > 0) {
        $('#tblNotificationMoM tbody tr').each(function () {
            debugger;
            var row = $(this);
            if (row.find('[id^=hdnNotificationId_]').val() != undefined)
                Notifications += (row.find('[id^=hdnNotificationId_]')).val() + ",";
        });
    }
    return Notifications;
}

function OpenMeetingDatePopup() {
    $("#ModelMeetingDate").modal('show');
    return false;
}

function CloseMeetingDatepopup() {
    $('#txtmeetingdate').val($('#txtMeetingNote').val());
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
                HideGlobalLodingPanel();
                CloseMeetingDatepopup();
                autosize($(".AutoHeight"));
            }
        })
    }
    return false;
}

//===================================================

//function GetSelectedNotification() {
//    var Notifications = "";
//    var len = $('#tblNotificationMoM tbody td').length;
//    if (len > 0) {
//        $('#tblNotificationMoM tbody tr').each(function () {
//            var row = $(this);
//            if (row.find('[id*=hdnNotificationId]').val() != undefined)
//                Notifications += (row.find('[id*=hdnNotificationId]')).val() + ",";
//        });
//    }
//    return Notifications;
//}

function closeActionMail(ctrl) {
    var callfor = $(ctrl).attr('data-callfor');
    if (callfor == "close") {
        $('#btncloseModalAction').attr('data-callfor', 'close');
        $('#ActionMail').modal('hide');
    }
    else {
        $('#ActionMail').modal('hide');
        BindNotificationActions();
        $('#btncloseModalAction').attr('data-callfor', 'closeaction');
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
            HideGlobalLodingPanel();
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
                                '<input type="checkbox" value="' + v.ActionId + '" onchange="AddRemoveActions(this);" checked="checked"/>' +
                                '<span class="cr insertcheckbox" style="margin-top: 2px;">' +
                                '<i class="cr-icon glyphicon glyphicon-ok"></i>' +
                                '</span>' +
                                '</label>' +
                                '</div>';
                        }
                        else {
                            HTML += '<div class="checkbox radio-margin" style="margin-top: 0;float: left; margin-left:0;">' +
                                '<label style="padding-left: 0;">' +
                                '<input type="checkbox" value="' + v.ActionId + '" onchange="AddRemoveActions(this);" checked="checked" disabled="disabled"/>' +
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
            HideGlobalLodingPanel();
            $(".date-picker").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: 'dd M yy'
            });
        }
    });
}

function EditNotificationActions(ctrl) {
    ShowGlobalLodingPanel();
    if (typeof ctrl != "undefined")
        $('#hdnNotificationId').val($(ctrl).attr('data-searchfor'));
    BindNotificationActions();
}