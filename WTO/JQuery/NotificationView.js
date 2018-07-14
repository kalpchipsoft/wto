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

function EditNotificationActions(MeetingId) {
    var NotificationId = myWTOAPP.id;

    $('#divNotificationPlanTakeAction').load('/MoM/NotificationPlanTakeAction?Id=' + NotificationId + '&MeetingId=' + MeetingId, function (responseTxt, statusTxt, xhr) {
        if (statusTxt == "success")
            $('#ModalAddAction').modal('show');
    });
}