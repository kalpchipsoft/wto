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

function ViewAction(Id, ctrl) {
    $('[id$=lblMailMessage]').text('');
    $('[id$=lblMailMessage1]').text('');
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
            $('#hdnActionMailId').val(result.MailId);
            $('#lblRecipients1').text(result.MailTo);
            $('#lblActionDueOn1').text(result.RequiredOn);
            $('#lblActionTakenOn1').text(result.UpdatedOn);

            if (result.MailDetails != null) {
                $('[id$=lblMailSubject]').text(result.MailDetails.Subject);
                $('[id$=lblMailMessage]').append(result.MailDetails.Message);
                $('[id$=lblMailSubject1]').text(result.MailDetails.Subject);
                $('[id$=lblMailMessage1]').append(result.MailDetails.Message);
            }
            var Files = '';
            $.each(result.Attachments, function (i, v) {
                Files += "<a href='" + v.Path + "' download><p class='Attachmentfilename'>" + v.FileName + "</p></a>";
            })
            $('#lblMailAttachments').empty();
            $('#lblMailAttachments').append(Files);

            //  $('#lblMailAttachments1').empty();
            //  $('#lblMailAttachments1').append(Files);
            if (ActionId == "1") {
                $('#lnkAddActionResponse').addClass('hidden');
                //$('#divTradeData').removeClass('hidden');
                //$('.divBriefToRegulators').removeClass('hidden');
                //$('.divPolicyBrief').addClass('hidden');
            }
            else if (ActionId == '2') {
                $('#lnkAddActionResponse').addClass('hidden');
                //$('#divTradeData').removeClass('hidden');
                //$('.divPolicyBrief').removeClass('hidden');
                //$('.divBriefToRegulators').addClass('hidden');
                $("#tblPrinthead tbody tr td").css('width', "");
            }
            else {
                $('#lnkAddActionResponse').addClass('hidden');
                //$('#divDraftRegulationBrief').addClass('hidden');
                //$('.divPolicyBrief').addClass('hidden');
                //$('.divBriefToRegulators').addClass('hidden');
                if (ActionId == '3') {
                    if (result.ResponseId == "0")
                        $('#lnkAddActionResponse').removeClass('hidden');
                    else
                        $('#lnkAddActionResponse').addClass('hidden');
                }

            }
        },
        failure: function (result) {
            Alert("Meeting", "Something went wrong.<br/>", "Ok");
        },
        error: function (result) {
            Alert("Meeting", "Something went wrong.<br/>", "Ok");
        }
    });
} ``

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

function printDiv(elementId) {
    debugger;
    var date = new Date();
    $("#lblHeaderDate").text(FormatDate(date.getFullYear() + "-" + (date.getMonth() + 1) + "-" + date.getDate()));
    var printContent = document.getElementById(elementId);
    var windowUrl = '';
    var uniqueName = new Date();
    var windowName = '';  // 'Print' + uniqueName.getTime();
    var printWindow = window.open(windowUrl, windowName, '');
    printWindow.document.write(printContent.innerHTML);
    printWindow.document.close();
    printWindow.focus();
    printWindow.print();
    printWindow.close();
    return true;
}

function CloseViewAction() {
    $("#ViewActionModal").modal('hide');
    return false;
}

function FormatDate(val) {
    debugger;
    //if (val.length > 10) {
    var dt1 = val.split('T')[0];
    var mon = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'July', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];
    var d = dt1.split('-')[2];
    var m = dt1.split('-')[1];
    var y = dt1.split('-')[0];
    var MyDate = d + ' ' + mon[parseInt(m) - 1] + ' ' + y;
    return MyDate;
    // }
}

//--------------------------------- Action Response Start --------------------------------------
function OpenActionResponse() {
    $('#txtActionResponseReceivedOn').val('');
    $('#txtActionResponseMessage').val('');
    $('#txtResponseActionAttachment').val('');
    $('#ActionResponseModal').modal('show');
    $('#DivActionResponseAttachments').empty();
    ResponseActionMailAttachments = [];
}

function ClearActionResponse() {
    $('#ActionResponseModal').modal('hide');
}

function SaveActionResponse() {
    ShowGlobalLodingPanel();
    var NotificationId = $('[id$=hdnNotificationId]').val();
    var MailId = $('[id$=hdnActionMailId]').val();
    var Message = $.trim($('#txtActionResponseMessage').val());
    var ReceivedOn = $.trim($('#txtActionResponseReceivedOn').val());
    var ErrorMsg = '';

    if (ReceivedOn.trim() == '') {
        ErrorMsg += 'Mention response received on date.<br/>';
    }

    if (Message.trim() == '') {
        ErrorMsg += 'Enter message.<br/>';
    }

    if (ErrorMsg.length > 0) {
        HideGlobalLodingPanel();
        Alert("Alert", ErrorMsg, "Ok");
        return false;
    }
    else {
        var obj = {
            MailId: $('#hdnActionMailId').val(),
            NotificationId: NotificationId,
            Message: Message,
            ResponseReceivedOn: ReceivedOn,
            ResponseDocuments: ResponseActionMailAttachments,
        }
        $.ajax({
            url: "/api/AddUpdateNotification/SaveResponseActionMail",
            async: false,
            type: "POST",
            data: JSON.stringify(obj),
            contentType: "application/json; charset=utf-8",
            success: function (result) {
                AlertwithFunction("Alert", "Response have been added successfully.<br/>", "Ok", "window.location.reload()");
            },
            failure: function (result) {
                Alert("Alert", "Something went wrong.<br/>", "Ok");
            },
            error: function (result) {
                Alert("Alert", "Something went wrong.<br/>", "Ok");
            },
            complete: function () {
                TempResponseActionMailAttachments = [];
                ResponseActionMailAttachments = TempResponseActionMailAttachments;
            }
        });
    }
    HideGlobalLodingPanel();
    return false;
}

function UploadResponseAttachment() {
    $('#UploadActionResponseDoc').click();
    return false;
}

function AddMultipleDocAction(ctrl) {
    var totfilesize = 0;
    if ($(ctrl)[0].files.length != 0) {
        var fileToLoad = $(ctrl)[0].files[0];
        var ext = $(ctrl)[0].files[0].name.split(".")[$(ctrl)[0].files[0].name.split(".").length - 1];
        ext = ext.toLowerCase();
        $.each($(ctrl)[0].files, function (index, value) {
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
            $.each($(ctrl)[0].files, function (index, value) {
                var reader = new FileReader();
                reader.onload = function (e) {
                    var IsExist = false;
                    $.each(ResponseActionMailAttachments, function (i, v) {
                        var FileName = $.trim(v.FileName);
                        if (FileName.toLowerCase() == value.name.toLowerCase())
                            IsExist = true;
                    });

                    if (IsExist) {
                        Alert("Alert", "An attachment with same name has been already added.<br/>", "Ok")
                    }
                    else {
                        ResponseActionMailAttachments.push({ "FileName": value.name, "Content": e.target.result, "Path": "" });
                        var HTML = '';
                        HTML += '<p class="Attachmentfilename">' + value.name + '<a href="#" onclick="RemoveActionResponseAttachments(this);" class="fileremove"><span class="glyphicon glyphicon-remove"></span></a></p>';
                        $('[id$=DivActionResponseAttachments]').append(HTML);
                        $('#txtResponseActionAttachment').val(ResponseActionMailAttachments.length + ' file(s) selected');
                    }
                }
                reader.readAsDataURL(fileToLoad);
            });
        }
    }
    return false;
}

function RemoveActionResponseAttachments(ctrl) {
    var FileName = $.trim($(ctrl).parent().text());
    var FileCount = 0;
    var Files = '';
    var TempResponseActionMailAttachments = [];
    $.each(ResponseMailAttachments, function (i, v) {
        if (v.FileName != FileName) {
            TempResponseActionMailAttachments.push({ "FileName": v.FileName, "Content": v.Content, "Selected": true, "Path": "", "IsSelected": true });
            Files += '<p class="Attachmentfilename">' + v.FileName + '<a href="#" onclick="RemoveActionResponseAttachments(this);" class="fileremove"><span class="glyphicon glyphicon-remove"></span></a></p>';
            FileCount++;
        }
    });
    ResponseActionMailAttachments = TempResponseActionMailAttachments;

    $('#DivActionResponseAttachments').empty();
    $('#DivActionResponseAttachments').append(Files);
    $('#DivActionResponseAttachments').removeClass("hidden");
    $('#txtResponseActionAttachment').val(FileCount + ' file(s) selected');
}
//--------------------------------- Action Response End --------------------------------------
