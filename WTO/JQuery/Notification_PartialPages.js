var MaterialAttachment = [], TempMaterialAttachment = [];

$(document).ready(function () {
    $('#fileActionAttachment').change(function (e) {
        var totfilesize = 0;
        if ($(this)[0].files.length != 0) {
            var fileToLoad = $(this)[0].files[0];
            var ext = $(this)[0].files[0].name.split(".")[$(this)[0].files[0].name.split(".").length - 1];
            ext = ext.toLowerCase();
            $.each($(this)[0].files, function (index, value) {
                totfilesize += value.size;
            });

            if (totfilesize > 10485760) {
                Alert("Alert", "Total attachment files size should not be greater than 10 MB.<br/>", "Ok");
                $("#Loader").hide();
                ActionAttachment = [];
                $('#ActionAttachmenName').text('No File Choose');
                return false;
            }
            else if (ext != "docx" && ext != "doc" && ext != "pdf") {
                Alert("Alert", "You can upload only word and pdf files.<br/>", "Ok");
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

    $('#UpdateActionModal').on('hidden.bs.modal', function () {
        $.each($('.ActionName'), function () {
            $(this).text('');
        });
        $('[id$=txtActionDueOn]').val('');
        $('[id$=txtRemarks]').text('');
        $('[id$=ActionAttachmenName]').empty();
        $('[id$=ActionAttachmenName]').text('No File Choosen');

        $('[id$=txtActionDueOn]').removeAttr('disabled');
        $('[id$=txtRemarks]').removeAttr('disabled');
        $('.fileinput.fileinput-new').find('.btn').removeClass('hidden');
        $('[id$=btnUpdateStatus]').removeClass('hidden');
        $('[id$=btnCloseAction]').removeClass('hidden');
    });

    $('#MeetingDateId').change(function () {
        var MeetingDate = $(this).val();
        var _Meetingdate = new Date(MeetingDate);
        var RequiredOn = new Date(_Meetingdate.setDate(_Meetingdate.getDate() + 7))
        $.each($('#divPlanActions').find('.date-picker'), function (i, v) {
            if ($(this).attr('data-SearchFor') != '3')
                $(this).val($.datepicker.formatDate('dd M yy', RequiredOn));
        });
    });

    $('#AddAttachment_UploadDocumentId').change(function (e) {
        var totfilesize = 0;
        if ($(this)[0].files.length != 0) {
            var fileToLoad = $(this)[0].files[0];
            var ext = $(this)[0].files[0].name.split(".")[$(this)[0].files[0].name.split(".").length - 1];
            ext = ext.toLowerCase();
            $.each($(this)[0].files, function (index, value) {
                totfilesize += value.size;
            });

            if (totfilesize > 10485760) {
                Alert("Alert", "Total attachment files size should not be greater than 10 MB.<br/>", "Ok");
                $("#Loader").hide();
                return false;
            }
            else if (ext != "docx" && ext != "doc" && ext != "pdf") {
                Alert("Alert", "You can upload only word and pdf files.<br/>", "Ok");
                $(this).val('');
                $("#Loader").hide();
                return false;
            }
            else {
                $.each($(this)[0].files, function (index, value) {
                    var reader = new FileReader();
                    reader.onload = function (e) {
                        var IsExist = false;
                        $.each($('[id$=tblAttachments] input[type=checkbox]'), function () {
                            var FileName = $.trim($(this).closest('td').next().text());
                            if (FileName.toLowerCase() == value.name.toLowerCase())
                                IsExist = true;
                        });
                        if (IsExist) {
                            Alert("Alert", "An attachment with same name has been already added.<br/>", "Ok")
                        }
                        else {
                            TempMailAttachments.push({ "FileName": value.name, "Content": e.target.result, "Selected": true, "Path": "", "IsSelected": true });
                            var HTML = '';
                            HTML += '<tr>';
                            HTML += '<td style="width: 4%; width:50px;">';
                            HTML += '<div class="checkbox radio-margin">';
                            HTML += '<label>';
                            HTML += '<input type="checkbox" checked="checked" val="" onchange="AddRemoveMailAttachement(this);">';
                            HTML += '<span class="cr"><i class="cr-icon glyphicon glyphicon-ok"></i></span>';
                            HTML += '</label>';
                            HTML += '</div>';
                            HTML += '</td>';
                            HTML += '<td style="width: 96%">' + value.name + '</td>';
                            HTML += '</tr>';
                            $('[id$=tblAttachments]').append(HTML);
                        }
                    }
                    reader.readAsDataURL(fileToLoad);
                });
            }
        }
    });

    $('#UploadMaterialAttachmentId').change(function (e) {
        var totfilesize = 0;
        if ($(this)[0].files.length != 0) {
            var fileToLoad = $(this)[0].files[0];
            var ext = $(this)[0].files[0].name.split(".")[$(this)[0].files[0].name.split(".").length - 1];
            ext = ext.toLowerCase();
            $.each($(this)[0].files, function (index, value) {
                totfilesize += value.size;
            });

            if (totfilesize > 10485760) {
                Alert("Alert", "Total attachment files size should not be greater than 10 MB.<br/>", "Ok");
                $("#Loader").hide();
                TempMaterialAttachment = [];
                $('#FileUploadedId').val('');
                return false;
            }
            else if (ext != "docx" && ext != "doc" && ext != "pdf") {
                Alert("Alert", "You can upload only word and pdf files.<br/>", "Ok");
                $(this).val('');
                $("#Loader").hide();
                TempMaterialAttachment = [];
                $('#MaterialAttachmentNameId').val('');
                return false;
            }
            else {
                $.each($(this)[0].files, function (index, value) {
                    var reader = new FileReader();
                    reader.onload = function (e) {
                        var _DisplayName = value.name.substr(0, value.name.lastIndexOf('.')).replace(/[^a-z0-9\s\w_ ]/gi, '');
                        TempMaterialAttachment = { "FileName": value.name, "Content": e.target.result, DisplayName: '' };
                        $('#FileMaterialAttachmentId').val(value.name);
                        $('#MaterialAttachmentNameId').val(_DisplayName);
                    };
                    reader.readAsDataURL(fileToLoad);
                });
            }
        }
        else {
            TempMaterialAttachment = [];
            $('#MaterialAttachmentNameId').val('');
        }
    });

    $('[name$=MaterialType]').change(function () {
        var MaterialType = this.value;
        $('.MaterialType').text(MaterialType);
        $('.divRelatedMatrialDetails').removeClass('hidden');
    });
});

//Open Modal for edit action
function _EditAction(Id, ctrl) {
    //var ActionName = $(ctrl).attr('data-searchfor');
    //var ActionId = $('[id=hdnActionId_' + Id + ']').val();
    //var DueOn = $('[id=tdRequiredOn_' + Id + ']').text();
    //var DraftRegulationBrief = $('[id=hdnDraftRegulationBrief_' + Id + ']').val();
    //var ReferencetoInternationalStandards = $('[id=hdnReferencetoInternationalStandards_' + Id + ']').val();
    //var TradeData = $('[id=hdnTradeData_' + Id + ']').val();
    //var Implications = $('[id=hdnImplications_' + Id + ']').val();
    //var AttachmentName = $('[id=linkAttachment_' + Id + ']').text();
    //var AttachmentPath = $('[id=linkAttachment_' + Id + ']').attr('href');

    //$.each($('.ActionName'), function () {
    //    $(this).text(ActionName);
    //});
    //$('[id$=txtActionDueOn]').text(DueOn);
    //$('[id$=txtDraftRegulationBrief]').text(DraftRegulationBrief);
    //$('[id$=txtReftoInternationalStandards]').text(ReferencetoInternationalStandards);
    //$('[id$=txtTradeData]').text(TradeData);
    //$('[id$=txtImplications]').text(Implications);
    //if (AttachmentName == "") {
    //    $('[id$=ActionAttachmenName]').empty();
    //    $('[id$=ActionAttachmenName]').text('No File Choosen');
    //}
    //else {
    //    $('[id$=ActionAttachmenName]').empty();
    //    $('[id$=ActionAttachmenName]').append('<a href="' + AttachmentPath + '" download="' + AttachmentName + '">' + AttachmentName + '</a>')
    //    ActionAttachment = { "FileName": AttachmentName, "Content": "" };
    //}
    //$('[id$=hdnNotificationActionId]').val(Id);
    //$('[id$=hdnActionId]').val(ActionId);
    //$('#UpdateActionModal').modal('show');

    //if (ActionId == "1") {
    //    $('#divDraftRegulationBrief').removeClass('hidden');
    //    $('.divBriefToRegulators').removeClass('hidden');
    //    $('.divPolicyBrief').addClass('hidden');
    //}
    //else if (ActionId == '2') {
    //    $('#divDraftRegulationBrief').removeClass('hidden');
    //    $('.divPolicyBrief').removeClass('hidden');
    //    $('.divBriefToRegulators').addClass('hidden');
    //}
    //else {
    //    $('#divDraftRegulationBrief').addClass('hidden');
    //    $('.divPolicyBrief').addClass('hidden');
    //    $('.divBriefToRegulators').addClass('hidden');
    //}

    //if (ActionId == 4) {
    //    $('#btnCloseAction').addClass('hidden');
    //}
    //else if (ActionId == 3) {
    //    $('#spnbtntxt').text('Send response to notifying country');
    //    $('#btnCloseAction').removeClass('hidden');
    //    $('#btnUpdateStatus').addClass('hidden');
    //}
    //else {
    //    $('#spnbtntxt').text('Update & Send ' + ActionName);
    //    $('#btnCloseAction').removeClass('hidden');
    //}
}

function ValidateActionStatus(Status) {
    var ErrorMsg = '';
    var _ActionId = $('[id$=hdnActionId]').val();

    if (_ActionId == 1 || _ActionId == 2) {
        if ($.trim($('#txtDraftRegulationBrief').val()) == "") {
            ErrorMsg += 'Please enter draft regulation brief.<br/>';
            $('#txtDraftRegulationBrief').addClass('error');
        }
        else
            $('#txtDraftRegulationBrief').removeClass('error');

        if ($.trim($('#txtReftoInternationalStandards').val()) == "" && _ActionId == 1) {
            ErrorMsg += 'Please enter reference to international standards.<br/>';
            $('#txtReftoInternationalStandards').addClass('error');
        }
        else
            $('#txtReftoInternationalStandards').removeClass('error');

        if ($.trim($('#txtTradeData').val()) == "" && _ActionId == 2) {
            ErrorMsg += 'Please enter trade data.<br/>';
            $('#txtTradeData').addClass('error');
        }
        else
            $('#txtTradeData').removeClass('error');

        if ($.trim($('#txtImplications').val()) == "" && _ActionId == 2) {
            ErrorMsg += 'Please enter implications.<br/>';
            $('#txtImplications').addClass('error');
        }
        else
            $('#txtImplications').removeClass('error');
    }

    if (ErrorMsg.length > 0)
        Alert("Alert", ErrorMsg, "Ok");
    else {
        var obj = {
            NotificationActionId: $('[id$=hdnNotificationActionId]').val(),
            NotificationId: myWTOAPP.id,
            DraftRegulationBrief: $.trim($('#txtDraftRegulationBrief').val()),
            ReferencetoInternationalStandards: $.trim($('#txtReftoInternationalStandards').val()),
            TradeData: $.trim($('#txtTradeData').val()),
            Implications: $.trim($('#txtImplications').val()),
        }
        UpdateActionStatus(obj, Status);
    }
}

function BindRegulatoryBodies() {
    $.ajax({
        url: "/API/Masters/GetRegulatoryBodies",
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

function ViewAction(Id, ctrl) {
    $('[id$=lblMailMessage]').text('');
    var ActionName = $(ctrl).attr('data-searchfor');
    var ActionId = $(ctrl).attr('data-SearchId');
    $.each($('.ActionName'), function () {
        $(this).text(ActionName);
    });

    $.ajax({
        url: "/api/AddUpdateNotification/ViewAction/" + Id,
        async: false,
        type: "GET",
        contentType: "application/json; charset=utf-8",
        success: function (result) {
            $('#ViewActionModal').modal('show');
            $('#lblRecipients').text(result.MailTo);
            $('#lblActionDueOn').text(result.RequiredOn);
            $('#lblActionTakenOn').text(result.UpdatedOn);
            $('#lblRecipients').text(result.MailTo);
            $('#lblDraftRegulationBrief').text(result.DraftRegulationBrief);
            $('#lblTradeData').text(result.TradeData);
            $('#lblImplications').text(result.Implications);
            $('#lblReftoInternationalStandards').text(result.ReferencetoInternationalStandards)

            if (result.MailDetails != null) {
                $('[id$=lblMailSubject]').text(result.MailDetails.Subject);
                $('[id$=lblMailMessage]').append(result.MailDetails.Message);
            }

            var Files = '';
            $.each(result.Attachments, function (i, v) {
                Files += "<a href='" + v.Path + "' download><p class='Attachmentfilename'>" + v.FileName + "</p></a>";
            })
            $('#lblMailAttachments').empty();
            $('#lblMailAttachments').append(Files);
            if (ActionId == "1") {
                $('#divTradeData').removeClass('hidden');
                $('.divBriefToRegulators').removeClass('hidden');
                $('.divPolicyBrief').addClass('hidden');
            }
            else if (ActionId == '2') {
                $('#divTradeData').removeClass('hidden');
                $('.divPolicyBrief').removeClass('hidden');
                $('.divBriefToRegulators').addClass('hidden');
            }
            else {
                $('#divDraftRegulationBrief').addClass('hidden');
                $('.divPolicyBrief').addClass('hidden');
                $('.divBriefToRegulators').addClass('hidden');
            }
        },
        failure: function (result) {
            Alert("Meeting", "Something went wrong.<br/>", "Ok");
        },
        error: function (result) {
            Alert("Meeting", "Something went wrong.<br/>", "Ok");
        }
    });
}

///-------------------------------------------------------------Attachments

function OpenAddNote() {
    $('#NoteId').val($('#MeetingNoteId').text());
    $("#AddNote").modal('show');
    GetMeetingNote();
    setInterval("autosize.update($('#NoteId'));", 100);
}

function CloseAddNote() {
    $('#NoteId').val('');
    $("#AddNote").modal('hide');
    RefreshAutoheight();
}

function GetMeetingNote() {
    var NotificationId = $('[id$=hdnNotificationId]').val();
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

////------------------------------------------------------------------------Notification related materials
//Open popup of add attachments in add material popup
function AddMaterialAttachment() {
    $('[id$=divAddMaterialAttachmentOverlay]').show();
    $('[id$=divAddMaterialAttachment]').show();

    $("[id$=MaterialAttachmentNameId]").val($.trim(MaterialAttachment.DisplayName));
    $("#FileMaterialAttachmentId").val(MaterialAttachment.FileName);
    $('#lblMaterialAttachment').text($.trim(MaterialAttachment.DisplayName));
}

//Close popup of add attachments in add material popup
function CloseMaterialAttachment() {
    TempMaterialAttachment = [];
    $('[id$=divAddMaterialAttachmentOverlay]').hide();
    $('[id$=divAddMaterialAttachment]').hide();

    $("[id$=MaterialAttachmentNameId]").val('');
    $("#FileMaterialAttachmentId").val('');
    $('#lblMaterialAttachment').text($.trim(MaterialAttachment.DisplayName));
    return false;
}

//ok function of add attachments in add material popup
function UploadMaterialAttachmentOk() {
    var ErrorMsg = '';
    if (TempMaterialAttachment.length == 0)
        ErrorMsg += 'Please provide attachment.<br/>';

    if ($.trim($('[id$=MaterialAttachmentNameId]').val()) == "") {
        ErrorMsg += "Please provide attachment name. <br/>";
        $("[id$=MaterialAttachmentNameId]").addClass('Error');
    }
    else
        $("[id$=MaterialAttachmentNameId]").removeClass('Error');

    if (ErrorMsg.length > 0) {
        Alert("Alert", ErrorMsg, "Ok");
        return false;
    }
    else {
        MaterialAttachment = TempMaterialAttachment;
        MaterialAttachment.DisplayName = $.trim($('[id$=MaterialAttachmentNameId]').val());
        $('#lblMaterialAttachment').text($.trim(MaterialAttachment.DisplayName));
        CloseMaterialAttachment();
    }
}

function OpenAddNotificationRelatedModal() {
    $('#AddNotificationRelatedModal').modal('show');
    return false;
}

function CloseAddNotificationRelatedModal() {
    $('#AddNotificationRelatedModal').modal('hide');
    $('[name$=MaterialType]:checked').prop('checked', false);
    $('.divRelatedMatrialDetails').addClass('hidden');
    $('#txtMaterialNumber').val('');
    $('#txtDateOfMaterial').val('');
    $('#txtMaterialDescription').val('');
    MaterialAttachment = [];
    $('#lblMaterialAttachment').text('');
    $('#MaterialAttachmentNameId').val('');
    $('#FileMaterialAttachmentId').val('');
    $('#UploadMaterialAttachmentId').val('');
    return false;
}

function ValidateNotificationRelatedMaterial() {
    ShowGlobalLodingPanel();
    var ErrorMsg = '';
    var NotificationNumber = $.trim($('#NotificationNumberId').val());
    var MaterialNumber = $.trim($('#txtMaterialNumber').val());
    var MaterialType = $.trim($('[name$=MaterialType]:checked').val());

    if (MaterialNumber == "")
        ErrorMsg += "Please enter " + MaterialType + " number.<br/>";
    else {
        if (MaterialNumber.indexOf('/') < 0)
            ErrorMsg += "Please enter valid " + MaterialType + " number.<br/>";
        else if (MaterialNumber.indexOf(NotificationNumber) < 0)
            ErrorMsg += "Please enter valid " + MaterialType + " number.<br/>";
    }

    if ($.trim($('#txtDateOfMaterial').val()) == "")
        ErrorMsg += "Please enter date of " + MaterialType + ".<br/>";

    if ($.trim($('#txtMaterialDescription').val()) == "")
        ErrorMsg += "Please enter " + MaterialType + " description.<br/>";

    if (MaterialAttachment.length == 0)
        ErrorMsg += "Please choose " + MaterialType + " attachment.<br/>";

    if (ErrorMsg.length > 0) {
        HideGlobalLodingPanel();
        Alert("Alert", ErrorMsg, "Ok");
        return false;
    }
    else
        SaveNotificationRelatedMaterial();
}

function SaveNotificationRelatedMaterial() {
    var obj = {
        NotificationId: myWTOAPP.id,
        MaterialType: $.trim($('[name$=MaterialType]:checked').val()),
        MaterialNumber: $.trim($('#txtMaterialNumber').val()),
        MaterialDescription: $.trim($('#txtMaterialDescription').val()),
        DateOfMaterial: $.trim($('#txtDateOfMaterial').val()),
        Attachment: MaterialAttachment
    }

    $.ajax({
        url: "/api/AddUpdateNotification/InsertNotificationRelatedMaterial/" + myWTOAPP.UserId,
        async: false,
        type: "POST",
        data: JSON.stringify(obj),
        contentType: "application/json; charset=utf-8",
        success: function (result) {
            AlertwithFunction("Alert", result.Message + "<br/>", "Ok", "AfterSaveNotificationRelatedMaterial()");
        },
        failure: function (result) {
            Alert("Alert", "Something went wrong.<br/>", "Ok");
        },
        error: function (result) {
            Alert("Alert", "Something went wrong.<br/>", "Ok");
        },
    });
}

function AfterSaveNotificationRelatedMaterial() {
    HideGlobalLodingPanel();
    $('#AddNotificationRelatedModal').modal('hide');
    MaterialAttachment = [];
    setTimeout(function () {
        $("#divNotificationRelatedMatrial").load('/AddNotification/NotificationRelatedMaterials/' + myWTOAPP.id);
    }, 300);
}