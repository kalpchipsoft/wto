$(document).ready(function () {
    var RowNum = $("#hdnRowNum").val();
    var TotalRow = $("#hdnTotalRow").val();
    if (RowNum != "" && TotalRow != "") {
        if (parseInt(TotalRow) == 1) {
            $("#lnkPrevoius").addClass('hidden');
            $("#lnlNext").addClass('hidden');
        }
        else if (parseInt(TotalRow) > 1) {
            if (parseInt(RowNum) == 1) {
                $("#lnkPrevoius").addClass('hidden');
            }
            else if (parseInt(RowNum) == parseInt(TotalRow)) {
                $("#lnlNext").addClass('hidden');
            }
        }
    }
    else {
        $("#lnkPrevoius").addClass('hidden');
        $("#lnlNext").addClass('hidden');
    }
});

function OpenAttachementViewer(Path, FileName) {
    if (Path != "") {
        //var FulllPath = "https://docs.google.com/gview?url=" + "http://testwto.chipsoftindia.in/" + Path.substring(2, Path.length) + "&embedded=true";
        var FulllPath = "https://docs.google.com/gview?url=" + "http://" + window.location.host + "/" + Path.substring(2, Path.length) + "&embedded=true";
        $("#iFrame").attr("src", FulllPath);
        $("#lblFileName").text(FileName);
        $("#AttachmentViewer").modal('show');
    }
    return false;
}

function CloseAttachmentViewer() {
    $("#iFrame").attr("src", "");
    $("#AttachmentViewer").modal('hide');
    return false;
}

function OpenAddNote() {
    $("#AddNote").modal('show');
    setInterval("autosize.update($('#NoteId'));", 100);
    GetMeetingNote();
}

function CloseAddNote() {
    $('#NoteId').val('');
    $("#AddNote").modal('hide');
    RefreshAutoheight();
}

function GetMeetingNote() {
    var NotificationId = myWTOAPP.id;
    $.ajax({
        url: "/api/AddUpdateNotification/GetMeetingNotes/" + NotificationId,//@Model.NotificationId,
        async: false,
        type: "GET",
        contentType: "application/json; charset=utf-8",
        success: function (result) {
            if (result != null) {
                $('#NoteId').val(result);
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
        url: "/api/MoM/EditAction/" + myWTOAPP.id,
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
    $('#hdnNotificationId').val(myWTOAPP.id);
    BindNotificationActions();
}