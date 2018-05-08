var ActionAttachment = [];
$(document).ready(function () {
    $('#fileActionAttachment').change(function (e) {
        var totfilesize = 0;
        if ($(this)[0].files.length != 0) {
            var fileToLoad = $(this)[0].files[0];
            var ext = $(this)[0].files[0].name.split(".")[$(this)[0].files[0].name.split(".").length - 1];
            $.each($(this)[0].files, function (index, value) {
                totfilesize += value.size;
            });

            if (totfilesize > 10485760) {
                Alert("Notification", "Total attachment files size should not be greater than 10 MB.<br/>", "Ok");
                $("#Loader").hide();
                ActionAttachment = [];
                $('#ActionAttachmenName').text('No File Choose');
                return false;
            }
            else if (ext != "docx" && ext != "doc" && ext != "pdf") {
                Alert("Notification", "You can upload only word and pdf files.<br/>", "Ok");
                $(this).val('');
                $("#Loader").hide();
                ActionAttachment = [];
                $('#ActionAttachmenName').text('No File Choose');
                return false;
            }
            else {
                $.each($(this)[0].files, function (index, value) {
                    var reader = new FileReader();
                    reader.onload = function (e) {
                        ActionAttachment = { "FileName": value.name, "Content": e.target.result };
                        $('#ActionAttachmenName').text(value.name);
                    };
                    reader.readAsDataURL(fileToLoad);
                });

            }
        }
        else {
            ActionAttachment = [];
            $('#ActionAttachmenName').text('No File Choose');
        }
    });

    $('#actionneededId').on('hidden.bs.modal', function () {
        $.each($('.ActionName'), function () {
            $(this).text('');
        });
        $('[id$=txtActionDueOn]').val('');
        $('[id$=txtRemarks]').text('');
        $('[id$=ActionAttachmenName]').empty();
        $('[id$=ActionAttachmenName]').text('No File Choosen');
        $('[id$=hdnNotificationActionId]').val('0');
        $('[id$=hdnActionId]').val('0');

        $('[id$=txtActionDueOn]').removeAttr('disabled');
        $('[id$=txtRemarks]').removeAttr('disabled');
        $('.fileinput.fileinput-new').find('.btn').removeClass('hidden');
        $('[id$=btnUpdateStatus]').removeClass('hidden');
        $('[id$=btnCloseAction]').removeClass('hidden');
    });
});

//Open Modal for edit action
function EditAction(Id, ctrl) {
    var ActionName = $(ctrl).attr('data-searchfor');
    var ActionId = $('[id=hdnActionId_' + Id + ']').val();
    var DueOn = $('[id=tdRequiredOn_' + Id + ']').text();
    var Remarks = $('[id=hdnRemarks_' + Id + ']').val();
    var AttachmentName = $('[id=linkAttachment_' + Id + ']').text();
    var AttachmentPath = $('[id=linkAttachment_' + Id + ']').attr('href');

    $.each($('.ActionName'), function () {
        $(this).text(ActionName);
    });
    $('[id$=txtActionDueOn]').val(DueOn);
    $('[id$=txtRemarks]').text(Remarks);
    if (AttachmentName == "") {
        $('[id$=ActionAttachmenName]').empty();
        $('[id$=ActionAttachmenName]').text('No File Choosen');
    }
    else {
        $('[id$=ActionAttachmenName]').empty();
        $('[id$=ActionAttachmenName]').append('<a href="' + AttachmentPath + '" download="' + AttachmentName + '">' + AttachmentName + '</a>')
        ActionAttachment = { "FileName": AttachmentName, "Content": "" };
    }
    $('[id$=hdnNotificationActionId]').val(Id);
    $('[id$=hdnActionId]').val(ActionId);
    $('#actionneededId').modal('show');
}

function ValidateAction(Status) {
    var ErrorMsg = '';

    if (Status.toLowerCase() == 'insert' && $.trim($('[id$=txtMeetingDate]').val()) == '')
        ErrorMsg += 'Please enter meeting date.<br/>';

    if ($.trim($('[id$=txtActionDueOn]').val()) == '')
        ErrorMsg += 'Please enter due date of action.<br/>';

    if (ActionAttachment.length == 0)
        ErrorMsg += 'Please choose attachment for the action.<br/>';

    if (ErrorMsg.length > 0)
        Alert("Notification", ErrorMsg, "Ok");
    else {
        var Obj = {
            NotificationActionId: $('[id$=hdnNotificationActionId]').val(),
            Meetingdate: $('[id$=hdnNotificationActionId]').val() > 0 ? $('[id$=txtMeetingDate]').text() : $('[id$=txtMeetingDate]').val(),
            NotificationId: myWTOAPP.id,
            ActionId: $('[id$=hdnActionId]').val(),
            RequiredOn: $('[id$=txtActionDueOn]').val(),
            Attachment: ActionAttachment,
            Remarks: $.trim($('[id$=txtRemarks]').val()),
            Status: Status
        }
        InsertUpdateAction(Obj);
    }
}

function InsertUpdateAction(obj) {
    debugger;
    $.ajax({
        url: "/api/AddUpdateNotification/InsertUpdateAction/" + myWTOAPP.UserId,
        async: false,
        type: "POST",
        data: JSON.stringify(obj),
        contentType: "application/json; charset=utf-8",
        success: function (result) {
            debugger;
            Alert("Notification", "Action has been updated successfully.<br/>", "Ok");
            window.location.reload();
        },
        failure: function (result) {
            Alert("Notification", "Something went wrong.<br/>", "Ok");
        },
        error: function (result) {
            Alert("Notification", "Something went wrong.<br/>", "Ok");
        },
        complete: function () {
            //HideGlobalLodingPanel();
        }
    });
}

function ViewAction(Id, ctrl) {
    var ActionName = $(ctrl).attr('data-searchfor');
    var ActionId = $('[id=hdnActionId_' + Id + ']').val();
    var DueOn = $('[id=tdRequiredOn_' + Id + ']').text();
    var Remarks = $('[id=hdnRemarks_' + Id + ']').val();
    var AttachmentName = $('[id=linkAttachment_' + Id + ']').text();
    var AttachmentPath = $('[id=linkAttachment_' + Id + ']').attr('href');

    $.each($('.ActionName'), function () {
        $(this).text(ActionName);
    });
    $('[id$=txtActionDueOn]').val(DueOn);
    $('[id$=txtRemarks]').text(Remarks);
    if (AttachmentName == "") {
        $('[id$=ActionAttachmenName]').empty();
        $('[id$=ActionAttachmenName]').text('No File Choosen');
    }
    else {
        $('[id$=ActionAttachmenName]').empty();
        $('[id$=ActionAttachmenName]').append('<a href="' + AttachmentPath + '" download="' + AttachmentName + '">' + AttachmentName + '</a>')
        ActionAttachment = { "FileName": AttachmentName, "Content": "" };
    }
    
    $('[id$=txtActionDueOn]').attr('disabled', 'disabled');
    $('[id$=txtRemarks]').attr('disabled', 'disabled');
    $('.fileinput.fileinput-new').find('.btn').addClass('hidden');
    $('[id$=btnUpdateStatus]').addClass('hidden');
    $('[id$=btnCloseAction]').addClass('hidden');
    $('#actionneededId').modal('show');
}