var NotificationDoc = [], TranslatedDoc = [], NotificationAttachment = [];
var UploadedDocument = [];
var TempMailAttachments = [], MailAttachments = [];
var ResponseMailAttachments = [];
var SelectedLanguage = '';
var UntranslatedDocs = '';
var MailIdResponse = '';
var DocumentRowNumber;

function setScrollTop() {
    $('.selecthrbodyinner').scrollTop(0);
}

$(document).ready(function () {
    GetHSCode();
    autosize($(".AutoHeight"));

    $('#cbNoHSCode').click(function () {
        if (document.getElementById('cbNoHSCode').checked) {
            var element = document.getElementsByClassName("jstree-anchor");
            element[0].classList.remove("jstree-clicked");
            var index;
            for (index = 0; index < element.length; ++index) {
                element[index].classList.remove("jstree-clicked");
            }
        }
    });

    $("#HSCodeTree").bind(
        "select_node.jstree", function (evt, data) {
            $('#cbNoHSCode').prop('checked', false);
        });

    $('#chkSelectAll').change(function () {
        var IsCheckedAll = this.checked;
        $.each($(this).closest('table').find('tbody > tr > td > input[type=checkbox]'), function () {
            this.checked = IsCheckedAll;
        });
    });

    $('#chkSelectAll_AddStakeholders').change(function () {
        var IsCheckedAll = this.checked;
        $.each($('#divStakholder').find('input[type=checkbox]'), function () {
            this.checked = IsCheckedAll;
        });
    });

    $('#divStakholder').find('input[type=checkbox]').change(function () {
        var IsCheckedAll = true;
        var IsChecked = this.checked;
        $.each($('#divStakholder').find('input[type=checkbox]'), function () {
            if (!this.checked)
                IsCheckedAll = false;
        });

        if (IsCheckedAll)
            $('#chkSelectAll_AddStakeholders').prop('checked', true);
        else
            $('#chkSelectAll_AddStakeholders').prop('checked', false);
    });

    $('#ddlLanguage').change(function () {
        if ($(this).val() == '1' || $(this).val() == '') {
            $('.translatDoc').addClass("hidden");
            $('.translatedocument').addClass("hidden");
        }
        else {
            $('.translatDoc').removeClass("hidden");
            $('.translatedocument').removeClass("hidden");
            BindTranslater($(this).val());

            var FinalDateforComments = $('#FinalDateforCommentsId').val();
            var _FinalDate = new Date(FinalDateforComments);
            var DateofNotification = $('#DateofNotificationId').val();
            var _DateofNotification = new Date(DateofNotification);

            //Set Reminder for translation if pending  i.e, Final date for comments – 53 days 
            var TranslationReminderOn = new Date(FinalDateforComments);
            TranslationReminderOn.setDate(TranslationReminderOn.getDate() - 53);
            var _TranslationReminderOn = TranslationReminderOn.getDate() + ' ' + Months[TranslationReminderOn.getMonth()] + ' ' + TranslationReminderOn.getFullYear();
            $("#remainder").val(_TranslationReminderOn);

            if (_DateofNotification > TranslationReminderOn) {
                $("#RemainderforTranslationError").removeClass("hidden");
            }
            else {
                $("#RemainderforTranslationError").addClass("hidden");
            }

            //Set Due date for translation i.e. Final date for comments – 50 days 
            var TranslationDueOn = new Date(_FinalDate);
            TranslationDueOn.setDate(TranslationDueOn.getDate() - 50);
            var _TranslationDueOn = TranslationDueOn.getDate() + ' ' + Months[TranslationDueOn.getMonth()] + ' ' + TranslationDueOn.getFullYear();
            $("#duedate").val(_TranslationDueOn);
            if (_DateofNotification > TranslationDueOn) {
                $("#DueDateError").removeClass("hidden");
            }
            else {
                $("#DueDateError").addClass("hidden");
            }
        }
    });

    $('#NotificationNumberId').blur(function () {
        var NotificationNo = $('#NotificationNumberId').val().trim();
        if (NotificationNo.length > 0) {
            var Notifi_parts = NotificationNo.split('/');
            if (!(Notifi_parts.length > 3 && Notifi_parts.length <= 5)) {
                Alert('Alert', 'Please enter valid notification number.<br/>', 'Ok');
                ClearNotificationForm();
            }
            else {
                $.ajax({
                    url: "/api/AddUpdateNotification/ValidateNotification",
                    async: false,
                    type: "POST",
                    data: JSON.stringify({
                        NotificationId: $('#hdnNotificationId').val(),
                        NotificationNumber: NotificationNo
                    }),
                    contentType: "application/json; charset=utf-8",
                    success: function (result) {
                        if (result.IsExists) {
                            Alert("Alert", $.trim($('#NotificationNumberId').val()) + " already exists.<br/>", "Ok");
                            ClearNotificationForm();
                            $('#NotificationFile').val('');
                            $('#NotificationFileName').text('No File Choosen');
                        }
                        else {
                            if (result.CountryId == null || result.CountryId == '' || result.CountryId < 1)
                                Alert("Alert", "No country found in our masters w.r.t. notification country code.", "Ok");
                            else {
                                $('#NotifyingCountryId').text(result.Country + '(' + $.trim(result.CountryCode) + ')');
                                $('#NotifyingCountryId').attr('data-SearchFor', result.CountryId);
                                if ($.trim($('#EnquiryPointId').val()) == "")
                                    $('#EnquiryPointId').val(result.EnquiryDeskEmail);
                            }
                        }
                    }
                });
            }
        }
    });

    $('#NotificationFile').change(function (e) {
        ShowGlobalLodingPanel();
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
                $(this).val('');
                $('#NotificationFileName').text('No File Choosen');
                NotificationAttachment = [];
                HideGlobalLodingPanel();
                ClearNotificationForm();
                return false;
            }
            else if (ext != "docx" && ext != "doc") {
                Alert("Alert", "You can upload only doc files.<br/>", "Ok");
                $(this).val('');
                NotificationAttachment = [];
                $("#Loader").hide();
                $('#NotificationFileName').text('No File Choosen');
                ClearNotificationForm();
                HideGlobalLodingPanel();
                return false;
            }
            else {
                $.each($(this)[0].files, function (index, value) {
                    var reader = new FileReader();
                    reader.onload = function (e) {
                        NotificationAttachment = { "FileName": value.name, "Content": e.target.result };
                        $('#NotificationFileName').text(value.name);
                        $.ajax({
                            url: "/api/AddUpdateNotification/ReadNotificationDetails/",
                            async: false,
                            type: "POST",
                            data: JSON.stringify(NotificationAttachment),
                            contentType: "application/json; charset=utf-8",
                            success: function (result) {
                                ClearNotificationForm();
                                if (result != null) {
                                    if ($.trim(result.NotificationType) == "1")
                                        $('#TypeSPS').prop('checked', true);
                                    else if ($.trim(result.NotificationType) == "2")
                                        $('#TypeTBT').prop('checked', true);

                                    if ($.trim(result.NotificationNumber) != '' && $.trim(result.NotificationNumber).indexOf('/') > 0)
                                        $('#NotificationNumberId').val(result.NotificationNumber);

                                    if (!isNaN(new Date($.trim(result.DateofNotification)).valueOf())) {
                                        var _DateofNotification = new Date($.trim(result.DateofNotification));
                                        $('#DateofNotificationId').val($.datepicker.formatDate("d M yy", _DateofNotification));
                                    }

                                    if (!isNaN(new Date($.trim(result.FinalDateOfComments)).valueOf())) {
                                        var _FinalDateOfComments = new Date($.trim(result.FinalDateOfComments));
                                        $('#FinalDateforCommentsId').val($.datepicker.formatDate("d M yy", _FinalDateOfComments));
                                    }

                                    if (!isNaN(new Date($.trim(result.SendResponseBy)).valueOf())) {
                                        var _SendResponseBy = new Date($.trim(result.SendResponseBy));
                                        $('#SendResponseById').val($.datepicker.formatDate("d M yy", _SendResponseBy));
                                    }

                                    debugger;
                                    if (!isNaN(new Date($.trim(result.StakeholderResponseDueBy)).valueOf())) {
                                        var _StakeholderResponseDueBy = new Date($.trim(result.StakeholderResponseDueBy));
                                        $('#hdnStakeholderResponseDueBy').val($.datepicker.formatDate("d M yy", _StakeholderResponseDueBy));
                                    }

                                    if ($.trim(result.Articles) != '')
                                        $('#NotificationUnderArticleId').val($.trim(result.Articles).replace(/,\s*$/, ""));

                                    if ($.trim(result.Title) != '')
                                        $('#TitleId').val($.trim(result.Title));

                                    if ($.trim(result.ResponsibleAgency) != '')
                                        $('#AgencyResponsibleId').val($.trim(result.ResponsibleAgency));

                                    if ($.trim(result.ProductsCovered) != '')
                                        $('#ProductsCoveredId').val($.trim(result.ProductsCovered));

                                    if ($.trim(result.Description) != '')
                                        $('#DescriptionofContentId').val($.trim(result.Description));

                                    if ($.trim(result.HSCodes) != "") {
                                        $('[id$=hdnSelectedHSCodes]').val($.trim(result.HSCodes));
                                        var numbersArray = $('[id$=hdnSelectedHSCodes]').val().trim().split(',');
                                        $('#HSCodeTree').jstree(true).select_node(numbersArray);
                                        if ($('.jstree-anchor.jstree-clicked').length > 0)
                                            SaveHSCode();
                                    }

                                    if ($.trim(result.EnquiryEmailId) != "")
                                        $("#EnquiryPointId").val($.trim(result.EnquiryEmailId));

                                    $("#NotificationNumberId").trigger("blur");
                                    $("#AgencyResponsibleId").trigger('keyup');
                                    $("#DescriptionofContentId").trigger('keyup');
                                    $("#ProductsCoveredId").trigger('keyup');
                                    $("#TitleId").trigger('keyup');
                                }
                            },
                            failure: function (result) {
                                Alert("Alert", "Something went wrong.<br/>", "Ok");
                                ClearNotificationForm();
                            },
                            error: function (result) {
                                if (result.status == "500")
                                    Alert("Alert", "Please upload valid file.", "Ok");
                                else
                                    Alert("Alert", result.status, "Ok");
                                $(this).val('');
                                NotificationAttachment = [];
                                $("#Loader").hide();
                                $('#NotificationFileName').text('No File Choosen');
                                HideGlobalLodingPanel();
                            },
                            complete: function () {
                                //HideGlobalLodingPanel();
                                $.each($('.AutoHeight'), function (i, v) {
                                    var evt = document.createEvent('Event');
                                    evt.initEvent('autosize:update', true, false);
                                    this.dispatchEvent(evt);
                                });
                            }
                        });
                    };
                    reader.readAsDataURL(fileToLoad);
                });
            }
        }
        else {
            $(this).prev().text('Upload File');
            NotificationAttachment = [];
            $('#NotificationFileName').text('No File Choosen');
            ClearNotificationForm();
            HideGlobalLodingPanel();
        }
    });

    $(".yes").click(function () {
        $(".yes").addClass("switch-blue active");
        $(".yes").removeClass("switch-white");
        $(".No").removeClass("switch-red active");
        $(".No").addClass("switch-white");
        $(".upload").removeClass("hidden");
        $(".mailenquiry, .ObtainDocument").addClass("hidden");
    });

    $(".No").click(function () {
        $(".yes").removeClass("switch-blue active");
        $(".yes").addClass("switch-white");
        $(".No").addClass("switch-red active");
        $(".No").removeClass("switch-white");
        $(".divLanguage").addClass("hidden");
        $(".upload, .translatedocument, .translatDoc").addClass("hidden");
        $(".mailenquiry, .ObtainDocument").removeClass("hidden");
        $('#FUP_Notifi_Attach').val('');
        NotificationDoc = [];
    });

    $('#UploadDocumentId').change(function (e) {
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
                UploadedDocument = [];
                $('#FileUploadedId').val('');
                return false;
            }
            else if (ext != "docx" && ext != "doc" && ext != "pdf") {
                Alert("Alert", "You can upload only word and pdf files.<br/>", "Ok");
                $(this).val('');
                $("#Loader").hide();
                UploadedDocument = [];
                $('#FileUploadedId').val('');
                return false;
            }
            else {
                $.each($(this)[0].files, function (index, value) {
                    var reader = new FileReader();
                    reader.onload = function (e) {
                        var IsExists = false;
                        var callFor = $('#hdnDocumentFor').val();
                        if (callFor == "NotificationDoc") {
                            $.each(NotificationDoc, function (i, v) {
                                if (v.FileName == value.name)
                                    IsExists = true;
                            });
                        }
                        else if (callFor == "TranslatedDoc") {
                            $.each(TranslatedDoc, function (i, v) {
                                if (v.FileName == value.name)
                                    IsExists = true;
                            });
                        }

                        if (IsExists) {
                            Alert("Alert", "File already uploaded.<br/>", "Ok");
                            $(this).val('');
                            $("#Loader").hide();
                            UploadedDocument = [];
                            $('#FileUploadedId').val('');
                        }
                        else {
                            //var _DisplayName = value.name.substr(0, value.name.lastIndexOf('.')).replace(/[^a-z0-9\s\w_]/gi, '').replace(/[_\s]/g, '-');
                            var _DisplayName = value.name.substr(0, value.name.lastIndexOf('.')).replace(/[^a-z0-9\s\w_ ]/gi, '');
                            UploadedDocument = { "FileName": value.name, "Content": e.target.result };
                            $('#FileUploadedId').val(value.name);
                            $('#DocumentDisplayNameId').val(_DisplayName);
                        }
                    };
                    reader.readAsDataURL(fileToLoad);
                });
            }
        }
        else {
            UploadedDocument = [];
            $('#FileUploadedId').val('');
        }
    });

    $('#UploadDocumentModal').on('hidden.bs.modal', function () {
        var CallFor = $.trim($("[id$=hdnDocumentFor]").val());
        if (UploadedDocument.FileName == null && CallFor.toLowerCase() == "notificationdoc") {
            NotificationDoc = [];
            $('#Notifi_Attach_Name').text('');
            $("#btntxtNotificationDoc").text("Upload File");
        }
        else if (UploadedDocument.FileName == null && CallFor.toLowerCase() == "translateddoc") {
            TranslatedDoc = [];
            $('#Notifi_Attach_Name_Translated').text('');
            $("#btntxtTranslatedDoc").text("Upload File");
        }

        $('[id$=UploadDocumentId]').val('');
        $('#FileUploadedId').val('');
        $('[id$=DocumentDisplayNameId]').val('');
    });

    $('#EnquiryPointId').change(function () {
        Email = $.trim($(this).val());
        if (Email != '') {
            if (validateEmail(Email))
                $('[id$=EnquiryMailDeskId]').val($(this).val());
            else {
                $(this).val('');
                $('[id$=EnquiryMailDeskId]').val('');
                Alert("Alert", "Please enter valid email id.<br/>", "Ok");
            }
        }
    });

    if ($('[id$=hdnLanguageId]').val() > 0) {
        BindLanguages();
    }

    if ($('[id$=hdnNotificationId]').val() > 0) {
        //$("#FinalDateforCommentsId").trigger("change");

        $("#AgencyResponsibleId").trigger('keyup');
        $("#DescriptionofContentId").trigger('keyup');
        $("#ProductsCoveredId").trigger('keyup');
        $("#TitleId").trigger('keyup');

        $.each($('#RegulationsBody > tr'), function () {
            var _DocumentId = $(this).find('input[id^=hdnDocId_]').val();
            var _DisplayName = $(this).find('a[id^=lblRegulation_]').text();
            var _FileName = $(this).find('a[id^=lblRegulation_]').attr('download');
            var _Path = $(this).find('a[id^=lblRegulation_]').attr("href");
            var _LanguageId = $(this).find('select[id^=LangDrp_]').val();
            var _TranslaterId = $(this).find('label[id^=lblTranslator_]').attr('data-SearchFor');
            var _LanguageName = $(this).find('select[id^=LangDrp_] option:selected').text();
            if (_LanguageId > 1 && _TranslaterId <= 0)
                NotificationDoc.push({ DocumentId: _DocumentId, "DisplayName": _DisplayName, "FileName": _FileName, "Content": "", Path: _Path, LanguageId: _LanguageId, TranslaterId: _TranslaterId, LanguageName: _LanguageName, IsSelected: "1" });
            else
                NotificationDoc.push({ DocumentId: _DocumentId, "DisplayName": _DisplayName, "FileName": _FileName, "Content": "", Path: _Path, LanguageId: _LanguageId, TranslaterId: _TranslaterId, LanguageName: _LanguageName, IsSelected: "0" });
        });
    }

    $('#selecthr').on('hidden.bs.modal', function () {
        clearSearchtxt();
        $('#HSCodeTree').jstree().deselect_all();
    });
});

//-------------------------------------Basic Details Start---------------------------------------
function ClearNotificationForm() {
    $('#TypeSPS').prop('checked', false);
    $('#TypeTBT').prop('checked', false)
    $('#NotificationNumberId').val('');
    $('#DateofNotificationId').val('');
    $("#FinalDateforCommentsId").val('');
    $("#EnquiryPointId").val("");
    $('#SendResponseById').val('');
    $('#txtStakeholderResponseDueBy').val('');
    $("#NotifyingCountryId").text('');
    $("#NotifyingCountryId").attr('data-SearchFor', '');
    $('.combobox-container').find('input[type=text]').val('');

    $(".hs-code-table").addClass("hidden");
    $("#hdnSelectedHSCodes").val('');

    $('#TitleId').val('');
    $('#AgencyResponsibleId').val('');
    $('#ProductsCoveredId').val('');
    $('#DescriptionofContentId').val('');
    $('#NotificationUnderArticleId').val('');

    $('[id$=hdnSelectedHSCodes]').val('');
    $('#HSCodeTree').jstree().deselect_all();
    $('#HSCodesId > tbody').empty();

    $("#AgencyResponsibleId").trigger('keyup');
    $("#DescriptionofContentId").trigger('keyup');
    $("#ProductsCoveredId").trigger('keyup');
    $("#TitleId").trigger('keyup');
}

function ClearFile(ctrl) {
    //ClearNotificationForm();
    $(ctrl).val('');
}

//Validate Notification Section
function Validate() {
    var MSG = "";
    //Notification Type Id validation
    if ($('#NotificationTypeId').find('input[type=checkbox]:checked').length == 0) {
        MSG += 'Please select notification type.<br/>';
    }

    //Notification Number id Validation
    if ($('[id$=NotificationNumberId]').val().trim().length == 0) {
        $('[id$=NotificationNumberId]').addClass("error");
        MSG += "Please enter notification number.<br/>";
    }
    else {
        $('[id$=NotificationNumberId]').removeClass("error");
    }

    //Notification status Validation
    if ($('[id$=NotificationStatusId]').val() == '') {
        $('[id$=NotificationStatusId]').addClass("error");
        MSG += "Please select notification status.<br/>";
    }
    else {
        $('[id$=NotificationStatusId]').removeClass("error");
    }

    //Date of Notification id Validation
    if ($('[id$=DateofNotificationId]').val().trim().length == 0) {
        $('[id$=DateofNotificationId]').addClass("error");
        MSG += "Please select date of notification.<br/>";
    }
    else {
        $('[id$=DateofNotificationId]').removeClass("error");
    }

    //Final Date for Comments id Validation
    if ($('[id$=FinalDateforCommentsId]').val().trim().length == 0) {
        $('[id$=FinalDateforCommentsId]').addClass("error");
        MSG += "Please select final date of comments.<br/>";
    }
    else {
        $('[id$=FinalDateforCommentsId]').removeClass("error");
    }

    //Send Response By id Validation
    if ($('[id$=SendResponseById]').val().trim().length == 0) {
        $('[id$=SendResponseById]').addClass("error");
        MSG += "Please select date for send responce by.<br/>";
    }
    else {
        $('[id$=SendResponseById]').removeClass("error");
    }

    //Notifying Country validation
    if ($.trim($('[id$=NotifyingCountryId]').attr('data-SearchFor')).length == 0) {
        $('.combobox-container input').addClass("error");
        $('.combobox-container .input-group .input-group-addon').addClass("error");
        MSG += "Please select notifying country.<br/>";
    }
    else {
        $('.combobox-container input').removeClass("error");
        $('.combobox-container .input-group .input-group-addon').removeClass("error");
    }

    if ($('#NotifyingCountryId').attr('data-SearchFor').trim() != null && $('#NotifyingCountryId').attr('data-SearchFor').trim() <= 0) {
        MSG += "No country found in our masters w.r.t. notification country code.<br/>";
    }

    //$("#")
    if ($.trim($('[id$=EnquiryPointId]').val()) == "") {
        MSG += "Please enter enquiry desk email-id.<br/>";
        $('[id$=EnquiryPointId]').addClass('error');
    }
    else
        $('[id$=EnquiryPointId]').removeClass('error');

    //Title Id Validation
    if ($('[id$=TitleId]').val().trim().length == 0) {
        $('[id$=TitleId]').addClass("error");
        MSG += "Please enter title of notification.<br/>";
    }
    else {
        $('[id$=TitleId]').removeClass("error");
    }

    //Agency Responsible Id Validation
    if ($('[id$=AgencyResponsibleId]').val().trim().length == 0) {
        $('[id$=AgencyResponsibleId]').addClass("error");
        MSG += "Please enter responsible agency details.<br/>";
    }
    else {
        $('[id$=AgencyResponsibleId]').removeClass("error");
    }

    //Notification under Artical validation
    //if ($('[id$=NotificationUnderArticleId]').val().trim().length == 0) {
    //    $('[id$=NotificationUnderArticleId]').addClass("error");
    //    MSG += "Please enter notification under article.<br/>";
    //}
    //else {
    //    $('[id$=NotificationUnderArticleId]').removeClass("error");
    //}

    //Products Covered Id validation
    if ($('[id$=ProductsCoveredId]').val().trim().length == 0) {
        $('[id$=ProductsCoveredId]').addClass("error");
        MSG += "Please enter product covered.<br/>";
    }
    else {
        $('[id$=ProductsCoveredId]').removeClass("error");
    }

    //Hs code table validation
    var rowCount = $('#HSCodesId>tbody>tr').length;
    if ($('#HSCodesId>tbody>tr').length == 0) {
        MSG += "Please select HS Codes.<br/>";
    }

    //Description of Content Id validation
    if ($('[id$=DescriptionofContentId]').val().trim().length == 0) {
        $('[id$=DescriptionofContentId]').addClass("error");
        MSG += "Please enter description.<br/>";
    }
    else {
        $('[id$=DescriptionofContentId]').removeClass("error");
    }

    if ($('[id$=hdnNotificationId]').val() > 0) {
        //Document Accordian Validation    
        if ($("input[name='Documents']:checked").length == 0) {
            MSG += "Please specify whether you have detailed notification or not.<br/>";
        }
        else if ($.trim($("input[name='Documents']:checked").val()) == "0") {
            var DocumentObtainedOn = $.trim($("#Obtaindate").val());
            if (DocumentObtainedOn == '') {
                $("#Obtaindate").addClass("error");
                MSG += "Please specify the date by when the document will be obtained by you.<br/>";
            }
            else
                $("#Obtaindate").removeClass("error");
        }
        else {

            //upload file validation (for document section)
            if (NotificationDoc.length <= 0) {
                MSG += "Provide full text(s) of regulation.<br/>";
            }
            else {
                var IsLanguageSelected = true;

                $.each(NotificationDoc, function (i, v) {
                    if (v.LanguageId == "")
                        IsLanguageSelected = false;
                });

                if (!IsLanguageSelected)
                    MSG += "Please select the language of the document.<br/>";
            }

            if ($('#Notifi_Attach_Name').text() != '' && !$('[id$=chkSkipToDiscussion]').is(':checked') && $.trim($("#txtStakeholderResponseDueBy").val()) == '') {
                MSG += "Please specify the date by when the stakeholders will send response.<br/>";
                $("#txtStakeholderResponseDueBy").addClass('error');
            }
            else
                $("#txtStakeholderResponseDueBy").removeClass('error');
        }
    }

    if (MSG.length > 0) {
        Alert("Alert", MSG, "Ok");
        $($('.error')).eq(0).focus();
    }
    else
        SaveUpdateNotification();
}

//Save Notification Section
function SaveUpdateNotification() {
    ShowGlobalLodingPanel();
    var NotficationType = '';
    var Id = $('[id$=hdnNotificationId]').val();

    if ($('#NotificationTypeId').find('input[type=checkbox]:checked').length > 0) {
        if ($('#NotificationTypeId').find('input[type=checkbox]:checked').length > 1)
            NotficationType = '3';
        else
            NotficationType = $('#NotificationTypeId').find('input[type=checkbox]:checked').val();
    }

    var obj;
    var HSCodes = '';
    var NoHSCode = $('#pNoHSCode').find('input[type=checkbox]:checked').val();

    if (NoHSCode == 1)
        HSCodes = '-1';
    else
        HSCodes = $('[id$=hdnSelectedHSCodes]').val().trim();

    if ($.trim(Id) == 0) {
        obj = {
            UserId: myWTOAPP.UserId,
            Role: myWTOAPP.UserRole,
            NotificationId: Id,
            NotificationNumber: $('[id$=NotificationNumberId]').val().trim(),
            NotificationType: NotficationType,
            NotificationStatus: $('[id$=NotificationStatusId]').val(),
            DateofNotification: $('[id$=DateofNotificationId]').val().trim(),
            FinalDateOfComments: $('[id$=FinalDateforCommentsId]').val().trim(),
            SendResponseBy: $('[id$=SendResponseById]').val().trim(),
            CountryId: $('#NotifyingCountryId').attr('data-SearchFor').trim(),
            Title: $('[id$=TitleId]').val().trim(),
            ResponsibleAgency: $('[id$=AgencyResponsibleId]').val().trim(),
            UnderArticle: $('[id$=NotificationUnderArticleId]').val(),
            ProductsCovered: $('[id$=ProductsCoveredId]').val().trim(),
            HSCodes: HSCodes,
            Description: $('[id$=DescriptionofContentId]').val(),
            NotificationAttachment: NotificationAttachment,
            EnquiryEmail: $.trim($('[id$=EnquiryPointId]').val()),
            StakeholderResponseDueBy: $("#hdnStakeholderResponseDueBy").val()
        };
    }
    else {
        var IsDetailedNotifi = false;
        if ($("input[name='Documents']:checked").length != 0) {
            IsDetailedNotifi = $("input[name='Documents']:checked").val() == "0" ? false : true;
        }

        var _Regulations = [];
        $.each(NotificationDoc, function (i, v) {
            if (v.Path == "" && v.Content != "")
                _Regulations.push({ FileName: v.FileName, DisplayName: v.DisplayName, Content: v.Content, LanguageId: v.LanguageId });
        });

        var _Translations = [];
        $.each(TranslatedDoc, function (i, v) {
            if (v.Content != "")
                _Translations.push({ DocumentId: v.DocumentId, LanguageId: 0, FileName: v.FileName, DisplayName: v.DisplayName, Content: v.Content });
        });

        obj = {
            UserId: myWTOAPP.UserId,
            Role: myWTOAPP.UserRole,
            NotificationId: Id,
            NotificationNumber: $('[id$=NotificationNumberId]').val().trim(),
            NotificationType: NotficationType,
            NotificationStatus: $('[id$=NotificationStatusId]').val(),
            DateofNotification: $('[id$=DateofNotificationId]').val().trim(),
            FinalDateOfComments: $('[id$=FinalDateforCommentsId]').val().trim(),
            SendResponseBy: $('[id$=SendResponseById]').val().trim(),
            CountryId: $('#NotifyingCountryId').attr('data-SearchFor').trim(),
            Title: $('[id$=TitleId]').val().trim(),
            ResponsibleAgency: $('[id$=AgencyResponsibleId]').val().trim(),
            UnderArticle: $('[id$=NotificationUnderArticleId]').val(),
            ProductsCovered: $('[id$=ProductsCoveredId]').val().trim(),
            HSCodes: $('[id$=hdnSelectedHSCodes]').val().trim(),
            Description: $('[id$=DescriptionofContentId]').val(),
            NotificationAttachment: NotificationAttachment,
            EnquiryEmail: $.trim($('[id$=EnquiryPointId]').val()),
            DoesHaveDetails: IsDetailedNotifi,
            NotificationDoc: _Regulations,
            TranslationReminder: $('[id$=remainder]').val(),
            TranslationDueDate: $('[id$=duedate]').val(),
            ObtainDocBy: $("#Obtaindate").val(),
            TranslatedDoc: _Translations,
            StakeholderResponseDueBy: $("#txtStakeholderResponseDueBy").val(),
            Role: myWTOAPP.UserRole,
            SkippedToDiscussion: $('[id$=chkSkipToDiscussion]').is(':checked'),
            RetainedforNextDiscussion: $('[id$=chkRetainedForNextDiscussion]').is(':checked')
        };
    }
    $.ajax({
        url: "/api/AddUpdateNotification/InsertUpdate_Notification",
        async: false,
        type: "POST",
        data: JSON.stringify(obj),
        contentType: "application/json; charset=utf-8",
        success: function (result) {
            if (result.NotificationId > 0) {
                if ($.trim(Id) == 0)
                    AlertwithFunction("Alert", "Notification has been saved successfully.<br/>", "Ok", "AfterNotificationSaved(" + result.NotificationId + ")");
                else
                    AlertwithFunction("Alert", "Notification has been saved successfully.<br/>", "Ok", "AfterNotificationSaved(0)");
            }
            else if (result.Status == 'failure')
                Alert("Alert", "Something went wrong. Please try again.<br/>", "Ok");
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

//Function execute after notification save
function AfterNotificationSaved(NotificationId) {
    if (NotificationId != 0)
        location.href = window.location.origin + "/AddNotification/Edit_Notification/" + NotificationId;
    else
        location.reload();
}

//GetHSCode popup code
function SaveHSCode() {
    var seen = {};
    var HSCodeId = [];
    var LengthTwo = [];
    var LengthFour = [];
    var TotalIdArray = [];
    var FinalArray = [];
    var index = [];
    var index2 = [];
    var i, j, r = [];
    var row = '';
    if ($('.jstree-anchor.jstree-clicked').length == 0 && !$('#cbNoHSCode').is(':Checked')) {
        Alert("Alert", "Please select atleast one HSCode.<br/>", "Ok");
        $("#HSCodesId tbody > tr").remove();
        var rowCount = $('#HSCodesId>tbody>tr').length;
        if ($('#HSCodesId>tbody>tr').length == 0) {
            $(".hs-code-table").addClass("hidden");
            $("#hdnSelectedHSCodes").val('');
            return false;
        }
        return false;
    }
    else {
        $("#HSCodesId tbody > tr").remove();
        $.each($('.jstree-anchor.jstree-clicked'), function (i, nodeId) {
            HSCodeId.push(($('#HSCodeTree').jstree(true).get_selected()));
        });

        $.each(HSCodeId, function (i, nodeId) {
            for (var x = 0; x < nodeId.length; x++) {
                var id = $('#HSCodeTree').jstree(true).get_selected(true)[x].id;
                //var text = $('#HSCodeTree').jstree(true).get_selected(true)[x].text;
                if (id.length == 2)
                    if (LengthTwo.indexOf(id) < 0)
                        LengthTwo.push(id);
            }
        });

        $.each(HSCodeId[0], function (j, HSCode) {
            var IsIndexOf = false;
            var IsHScodeNodeSame = false;
            $.each(LengthTwo, function (i, nodeId) {
                var indx = HSCode.indexOf(nodeId);
                if (indx == 0) {
                    IsIndexOf = true;
                    if (HSCode == nodeId)
                        IsHScodeNodeSame = true;
                }
            });

            if (!IsIndexOf || IsHScodeNodeSame) {
                TotalIdArray.push(HSCode);
                if (HSCode.length == 4)
                    LengthFour.push(HSCode);
            }
        });

        $.each(TotalIdArray, function (j, HSCode) {
            var IsIndexOf = false;
            var IsHScodeNodeSame = false;
            $.each(LengthFour, function (i, nodeId) {
                var indx = HSCode.indexOf(nodeId);
                if (indx == 0) {
                    IsIndexOf = true;
                    if (HSCode == nodeId)
                        IsHScodeNodeSame = true;
                }
            });

            if (!IsIndexOf || IsHScodeNodeSame)
                FinalArray.push(HSCode);
        });

        var items = '';

        if (FinalArray.length == 0 && $('#cbNoHSCode').is(':Checked')) {
            var nodeId = "-1";
            items = '-1';
            var text = "No HS Code";
            var Onclick = 'RemoveHSCodeRow(this)';
            row += "<tr class='" + nodeId + "'><td colspan='2'>" + text + "</td><td><a id='" + nodeId + "' onclick='" + Onclick + "' class='remove-icon pull-right hidden'><span class='glyphicon glyphicon-remove'></span></a></td></tr>";
            $("#HSCodesId thead").addClass('hidden');
        }
        else {
            $.each(FinalArray, function (i, nodeId) {
                items += nodeId + ',';
                var text = $('#HSCodeTree').jstree().get_node(nodeId).text.split(':')[1].trim();
                var Onclick = 'RemoveHSCodeRow(this)';
                row += "<tr class='" + nodeId + "'><td>" + nodeId + "</td><td>" + text + "</td><td><a id='" + nodeId + "' onclick='" + Onclick + "' class='remove-icon'><span class='glyphicon glyphicon-remove'></span></a></td></tr>";
            });
            $("#HSCodesId thead").removeClass('hidden');
        }

        $("#hdnSelectedHSCodes").val(items);
        $("#HSCodesId tbody").append(row);
    }

    $("#HSCodesId tbody > tr").each(function () {
        var txt = $(this).text();
        if (seen[txt])
            $(this).remove();
        else
            seen[txt] = true;
    });
    $(".hs-code-table").removeClass("hidden");
    $('#selecthr').modal('hide');
}

//Remove HSCode table tr
function RemoveHSCodeRow(ClassName) {
    //if (ClassName == "-1")
    DeselectHSCodeNode(ClassName);
    //else
    //    Confirm('Delete', 'Do you want to remove this HS Code among selected HS Codes?', 'Yes', 'No', 'DeselectHSCodeNode("' + ClassName + '")');
}

//Remove HSCode table tr
function DeselectHSCodeNode(ctrl) {
    var id = '';
    id = ctrl.id;
    var ClassName = id;
    if (ClassName == "-1")
        $('#cbNoHSCode').prop('checked', false);
    else
        $('#HSCodeTree').jstree("uncheck_node", ClassName);

    $('.' + ClassName).remove();
    var UpdateHSCodeId = $("#hdnSelectedHSCodes").val().replace(ClassName, "").replace(",,", ",");
    $("#hdnSelectedHSCodes").val('');
    $("#hdnSelectedHSCodes").val(UpdateHSCodeId);

    var rowCount = $('#HSCodesId>tbody>tr').length;
    if ($('#HSCodesId>tbody>tr').length == 0) {
        $(".hs-code-table").addClass("hidden");
        return false;
    }
}

//Bind HSCode Api Below
function GetHSCode() {
    $.ajax({
        url: "/api/Masters/GetHSCode",
        async: false,
        type: "GET",
        contentType: "application/json; charset=utf-8",
        success: function (result) {
            $('#HSCodeTree').jstree({
                'core': {
                    'data': result
                },
                "checkbox": { "keep_selected_style": false },
                "search": {
                    "case_sensitive": false,
                    "show_only_matches": true
                },
                "plugins": ["checkbox", "search"]
            });
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

//Bind HSCode on Edit Mode
function bindhscodeUId() {
    document.getElementById('HSCodeSearchTxt').value = '';
    var numbersArray = $('[id$=hdnSelectedHSCodes]').val().trim().split(',');
    if (numbersArray == '-1')
        $('#cbNoHSCode').prop('checked', true);
    else
        $('#HSCodeTree').jstree(true).select_node(numbersArray);
}

//JSTree Search function
function SearchHSCode() {
    var searchString = $("#HSCodeSearchTxt").val();
    $('#HSCodeTree').jstree('search', searchString);
}

//JSTree clear Search function
function clearSearchtxt() {
    document.getElementById('HSCodeSearchTxt').value = '';
    $('#searchhscodebtn').click();
}

function FinalDateOfCommentsChanged() {//
    var _DateOfNotification = $('#DateofNotificationId').val();
    var _FinalDateOfComments = $('[id$=FinalDateforCommentsId]').val();

    if ($.trim(_FinalDateOfComments) != "") {
        var obj = {
            Id: myWTOAPP.id,
            DateOfNotification: $('#DateofNotificationId').val(),
            FinalDateOfComments: $('[id$=FinalDateforCommentsId]').val()
        }

        $.ajax({
            url: "/api/AddUpdateNotification/NotificationDate?Id=" + myWTOAPP.id + "&DateOfNotification=" + _DateOfNotification + "&FinalDateOfComments=" + _FinalDateOfComments,
            async: false,
            type: "GET",
            contentType: "application/json; charset=utf-8",
            success: function (result) {
                if (result != null) {
                    $('#txtStakeholderResponseDueBy').val(result.StakeholderResponsedueBy);
                    $('#SendResponseById').val(result.SendResponseBy);
                }
            },
            failure: function (result) {
                Alert("Alert", "Something went wrong.<br/>", "Ok");
            },
            error: function (result) {
                Alert("Alert", "Something went wrong.<br/>", "Ok");
            },
            complete: function () {
                setReminderDays('DueDate');
            }
        });
    }

    var DateOfNotification = new Date(_DateOfNotification);
    var FinalDateOfComments = new Date(_FinalDateOfComments);

    if (DateOfNotification >= FinalDateOfComments) {
        $('#FinalDateforCommentsError').removeClass('hidden');
        $('#SendResponseError').removeClass('hidden');
    }
}
//-------------------------------------Basic Details End---------------------------------------

//-------------------------------------Full text(s) of regulation Starts---------------------------------------

//Yes No Radio Button Function
function HaveDetaillNotifi(callfor) {
    if (callfor == 'Y') {
        $("#radioYesNo").val("1");
    }
    else if (callfor == 'N') {
        $("#radioYesNo").val("0");
    }
    else
        $("#radioYesNo").val("");
}

//Open Modal for send mail to enquiry desk
function OpenMailToEnquiryDeskModel(callfor) {
    if ($.trim($('#EnquiryPointId').val()) == '') {
        Alert("Alert", "Please enter enquiry desk email id.<br/>", "Ok");
        return false;
    }
    else if ($.trim($('#Obtaindate').val()) == '') {
        Alert("Alert", "Please enter date of obtain full text(s) of regulation by you.<br/>", "Ok");
        return false;
    }
    else {
        $('#EnquiryMailDeskId').val($.trim($('#EnquiryPointId').val()));
        var obj = {
            TemplateType: "Mail",
            TemplateFor: "EnquiryDesk"
        }
        $.ajax({
            url: "/api/AddUpdateNotification/GetMailSMSTemplate/" + myWTOAPP.id,
            async: false,
            type: "POST",
            data: JSON.stringify(obj),
            contentType: "application/json; charset=utf-8",
            success: function (result) {
                $('[id$=txtMailToEnqSubject]').val(result.Subject);
                CKEDITOR.instances.txtMailToEnqMailBody.setData(result.Message);
            },
            failure: function (result) {
                Alert("Alert", "Something went wrong.<br/>", "Ok");
            },
            error: function (result) {
                Alert("Alert", "Something went wrong.<br/>", "Ok");
            },
            complete: function () {
                $('#mailtoenquirydesk').modal('show');

                if (callfor == 'send') {
                    $('#btnsendmailtoenquirydesk').css('display', 'block');
                }
                if (callfor == 'view') {
                    $('#btnsendmailtoenquirydesk').css('display', 'none');
                }
                $.each($('.AutoHeight'), function (i, v) {
                    var evt = document.createEvent('Event');
                    evt.initEvent('autosize:update', true, false);
                    this.dispatchEvent(evt);
                });
            }
        });
    }
}

//Send Mail to enquiry desk
function SendMailToEnquiryDesk() {
    ShowGlobalLodingPanel();
    var NotificationId = $('[id$=hdnNotificationId]').val();
    var Subject = $.trim($('#txtMailToEnqSubject').val());
    var Message = $.trim(CKEDITOR.instances.txtMailToEnqMailBody.getData());
    var EnquiryEmail = $.trim($('#EnquiryMailDeskId').val());

    var ErrorMsg = '';

    if (EnquiryEmail == '') {
        ErrorMsg += 'Please enter enquiry desk email-id.<br/>';
    } else {
        var IsValidMail = true;
        if (EnquiryEmail.indexOf(';') > 0) {
            $.each(EnquiryEmail.split(';'), function (i, v) {
                if (!validateEmail(v))
                    IsValidMail = false;
            });
        }
        else if (!validateEmail(EnquiryEmail))
            IsValidMail = false;

        if (!IsValidMail)
            ErrorMsg += 'Please enter valid enquiry desk email-id.<br/>';
    }

    if (Subject == '') {
        ErrorMsg += 'Please enter subject.<br/>';
    }
    if (Message == '') {
        ErrorMsg += 'Please enter message.<br/>';
    }

    if (ErrorMsg.length > 0) {
        HideGlobalLodingPanel();
        Alert("Alert", ErrorMsg, "Ok");
        return false;
    }
    else {
        var obj = {
            UserId: myWTOAPP.UserId,
            Role: myWTOAPP.UserRole,
            EnquiryEmailId: EnquiryEmail,
            ObtainDocBy: $.trim($('#Obtaindate').val()),
            MailDetails: { MailId: 0, Subject: Subject, Body: Message }
        }
        $.ajax({
            url: "/api/AddUpdateNotification/SendMailToEnquiryDesk/" + myWTOAPP.id,
            async: false,
            type: "POST",
            data: JSON.stringify(obj),
            contentType: "application/json; charset=utf-8",
            success: function (result) {
                AlertwithFunction("Alert", "Mail has been sent successfully to enquiry desk.<br/>", "Ok", "window.location.reload()");
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
    HideGlobalLodingPanel();
}

//Open Modal for Upload document
function OpenModelForUploadDocument(CallFor, ctrl) {
    $("[id$=hdnDocumentFor]").val(CallFor);
    DocumentRowNumber = ctrl.id.replace('btnUploadTranslatedDoc_', '');
    if (CallFor.toLowerCase() == "notificationdoc") {
        var IsSelected = true;
        $.each(NotificationDoc, function (i, v) {
            if (v.LanguageId == "")
                IsSelected = false;
        });

        if (IsSelected) {
            $("[id$=lblModalHeading]").text("Upload full text(s) of regulation");
            $('[id$=UploadDocumentModal').modal('show');
        }
        else
            Alert("Alert", "Please select language of uploaded full text(s) of regulation.<br/>", "Ok");
    }
    else if (CallFor.toLowerCase() == "translateddoc") {//hdnDocId
        var DocId = $('#' + ctrl.id.replace('btnUploadTranslatedDoc', 'hdnDocId')).val();
        $("[id$=lblModalHeading]").text("Upload translated full text(s) of regulation");
        $("[id$=hdnDocumentId]").val(DocId);
        $("[id$=hdnDocumentFor]").val('translateddoc');
        $('[id$=UploadDocumentModal').modal('show');
    }
}

//Function for finalize document
function UploadDocumentOk() {
    var CallFor = $.trim($("[id$=hdnDocumentFor]").val());
    var DisplayName = $.trim($("[id$=DocumentDisplayNameId]").val());

    var ErrorMsg = '';
    if (UploadedDocument.length == 0)
        ErrorMsg += 'Please provide attachment.<br/>';

    if ($.trim($('[id$=DocumentDisplayNameId]').val()) == "") {
        ErrorMsg += "Please provide attachment name. <br/>";
        $("[id$=DocumentDisplayNameId]").addClass('Error');
    }
    else {
        var IsExists = false;
        if (CallFor == "NotificationDoc") {
            $.each(NotificationDoc, function (i, v) {
                if (v.DisplayName == DisplayName)
                    IsExists = true;
            });
        }
        else if (CallFor == "TranslatedDoc") {
            $.each(TranslatedDoc, function (i, v) {
                if (v.DisplayName == DisplayName)
                    IsExists = true;
            });
        }

        if (IsExists) {
            ErrorMsg += "Attachment with same name already exists. <br/>";
            $("[id$=DocumentDisplayNameId]").val('');
            $("[id$=DocumentDisplayNameId]").addClass('Error');
        }
        else
            $("[id$=DocumentDisplayNameId]").removeClass('Error');
    }

    if (ErrorMsg.length > 0) {
        Alert("Alert", ErrorMsg, "Ok");
        return false;
    }
    else {
        if (CallFor.toLowerCase() == "notificationdoc") {
            NotificationDoc.push({ DocumentId: "", "DisplayName": DisplayName, "FileName": UploadedDocument.FileName, "Content": UploadedDocument.Content, Path: "", LanguageId: "", TranslaterId: "", LanguageName: "", IsSelected: "0" });
            $('.No').addClass('disabled');
            $('.yes').addClass('disabled');
            $(".divLanguage").removeClass("hidden");
            $("#divRegulations").removeClass("hidden");

            var uploadedAttachCount = $('a[id^="lblRegulation_"]').length + 1;
            var html = '';
            var OnclickFunction = "OpenModelForUploadDocument('translateddoc',this);";
            html += '<tr>' +
                        '<td><a id="lblRegulation_' + uploadedAttachCount + '" class="default-pointer" download="' + UploadedDocument.FileName + '">' + DisplayName + '</a></td>' +
                        '<td><select class="form-control languageddl"></select><input id="hdnDocId_' + uploadedAttachCount + '" type="hidden" value="" /></td>' +
                        '<td class="translateddoc ' + ($('.translateddoc').eq(0).attr('class').indexOf('hidden') >= 0 ? "hidden" : "") + '"><label id="lblSentToTranslator_' + uploadedAttachCount + '"></label></td>' +
                        '<td class="translateddoc ' + ($('.translateddoc').eq(0).attr('class').indexOf('hidden') >= 0 ? "hidden" : "") + '"><label data-searchfor="1" id="lblTranslator_' + uploadedAttachCount + '"></label></td>' +
                        '<td class="translateddoc ' + ($('.translateddoc').eq(0).attr('class').indexOf('hidden') >= 0 ? "hidden" : "") + '">' +
                            '<span id="btnUploadTranslatedDoc_' + uploadedAttachCount + '" onclick="' + OnclickFunction + '" style="color:#E34724; text-decoration: underline; cursor:pointer;" class="forTranslation hidden">' +
                                '<span id="btntxtTranslatedDoc_' + uploadedAttachCount + '">Upload</span>' +
                            '</span>' +
                            '<span class="fileinput-filename"></span><span class="fileinput-new dark-blue underline"><a id="Notifi_Attach_Name_Translated_' + uploadedAttachCount + '" href=""></a></span>' +
                        '</td>' +
                        "<td><a style='float: right;' onclick='RemoveNotificationDoc(this)' class='remove-icon'><span class='glyphicon glyphicon-remove'></span></a></td>" +
                     '</tr>';
            $("#RegulationsBody").append(html);

            var IsForeignLanguageSelected = false;
            $.each(NotificationDoc, function (i, v) {
                if (v.LanguageId > 1)
                    IsForeignLanguageSelected = true;
            })

            if (IsForeignLanguageSelected) {
                var Options = '<option value="0"> --Select Language-- </option>';
                var _Languages = [];
                $.each($('#RegulationsBody > tr '), function (i, v) {
                    var Id = $(v).find('select').val();
                    var Text = $(v).find('select option:selected').text();

                    if (Id != null && $.inArray(Id, _Languages, 0) == -1) {
                        Options += "<option value='" + $.trim(Id) + "'>" + $.trim(Text) + "</option>";
                        _Languages.push(Id);
                    }
                });

                if ($.inArray('1', _Languages, 0) == -1)
                    Options += "<option value='1'>English</option>";

                $('.languageddl').append(Options);
                $('.languageddl').multiselect({
                    maxHeight: 150,
                    enableFiltering: true,
                    enableCaseInsensitiveFiltering: true,
                    nonSelectedText: '--Select Language--'
                });
                $('.languageddl').attr('onchange', 'BindRelatedTranslators(this)');
                $('.languageddl').removeClass('languageddl');
            }
            else
                BindGridLanguages();
        }
        else if (CallFor.toLowerCase() == "translateddoc") {
            var DocumentId = $("[id$=hdnDocumentId]").val();
            TranslatedDoc.push({ "DocumentId": DocumentId, "DisplayName": DisplayName, "FileName": UploadedDocument.FileName, "Content": UploadedDocument.Content });

            $('#Notifi_Attach_Name_Translated_' + DocumentRowNumber).text(DisplayName);
            $('#Notifi_Attach_Name_Translated_' + DocumentRowNumber).removeAttr('href');
            $('#Notifi_Attach_Name_Translated_' + DocumentRowNumber).parent().removeClass('underline')
            $('#btntxtTranslatedDoc_' + DocumentRowNumber).addClass('hidden');
            $('#divSkipToDiscussion').addClass('hidden');
        }
        $('[id$=UploadDocumentModal]').modal('hide');
    }
}

//Bind language for full text(s)
function BindGridLanguages() {
    $.ajax({
        url: "/api/Masters/GetLanguages",
        async: false,
        type: "GET",
        contentType: "application/json; charset=utf-8",
        success: function (result) {
            if ($('.languageddl').length > 0) {
                $.each($('.languageddl'), function (i, v) {
                    var $ddl = $(this);
                    $ddl.empty();
                    $ddl.multiselect('destroy');
                    if (result.length > 0) {
                        $ddl.append('<option value="0"> --Select Language-- </option>');
                        $.each(result, function (i, v) {
                            $ddl.append('<option value="' + v.LanguageId + '"> ' + v.LanguageName + ' </option>');
                        });
                        $ddl.attr('onchange', 'BindRelatedTranslators(this)');
                        //$(this).val($('[id$=hdnLanguageId]').val());
                    }
                });
            }
        },
        failure: function (result) {
            Alert("Alert", "Something went wrong.<br\>", "Ok");
        },
        error: function (result) {
            Alert("Alert", "Something went wrong.<br\>", "Ok");
        },
        complete: function () {
            $('.languageddl').multiselect({
                maxHeight: 150,
                enableFiltering: true,
                enableCaseInsensitiveFiltering: true,
                nonSelectedText: '--Select Language--'
            });

            $.each($('.languageddl'), function (i, v) {
                $(this).removeClass('languageddl');
            });
        }
    });
}

//Bind transltor of language
function BindTranslater(LanguageId) {
    $.ajax({
        url: "/api/Masters/GetTranslaters/" + LanguageId,
        async: false,
        type: "GET",
        contentType: "application/json; charset=utf-8",
        success: function (result) {
            $('#ddlTranslater').empty();
            if (result.length > 0) {
                Translators = result;
                $('#ddlTranslater').append('<option value="">--Select Translator--</option>');
                $.each(result, function (i, v) {
                    $('#ddlTranslater').append('<option value="' + v.TranslatorId + '"> ' + v.TranslatorName + ' </option>');
                });

                var TranslaterId = $('#ddlTranslater').attr('data-selectFor');
                $('#ddlTranslater').val(TranslaterId);
            }
            else {
                $('#ddlTranslater').append('<option value="">No Translator available for selected language.</option>');
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
            if ($('#ddlTranslater').val() == null)
                $('#ddlTranslater').val('');
        }
    });
}

//Remove Uploaded Notification Row
function RemoveNotificationDoc(ctrl) {
    var DocDisplayName = $(ctrl).closest('tr').find('a[id^=lblRegulation]').text();
    var FileName = $(ctrl).closest('tr').find('a[id^=lblRegulation]').attr('download');
    var _NotificationDoc = [];
    var IsNewFile = false;
    $.each(NotificationDoc, function (i, v) {

        if (!(v.DisplayName == DocDisplayName && FileName == v.FileName)) {
            _NotificationDoc.push(v);

            if (v.Content != "")
                IsNewFile = true;
        }
    });
    NotificationDoc = _NotificationDoc;
    $(ctrl).closest('tr').remove();
    if ($('#RegulationsBody > tr').length == 0) {
        $('#divRegulations').addClass('hidden');
    }


    if (IsNewFile)
        $('#btnSendToTranslator').removeClass('hidden');
    else
        $('#btnSendToTranslator').addClass('hidden');
}

//On change Fuunction of language dropdown
function BindRelatedTranslators(ctrl) {
    var LanguageId = $(ctrl).val();
    var Language = $(ctrl).find('option:selected').text();
    var FileName = $(ctrl).closest('tr').find('a[id^=lblRegulation]').attr('download');

    $.each(NotificationDoc, function (i, v) {
        if (FileName == v.FileName) {
            v.LanguageId = LanguageId;
            v.LanguageName = Language;
            v.IsSelected = (LanguageId > 1 && v.Content != "") ? "1" : "0";
        }
    });

    if (LanguageId > 1) {
        $('.translatDoc').removeClass('hidden');
        $('#btnSendToTranslator').removeClass('hidden');
        BindTranslater(LanguageId);
        var _DateOfNotification = $('#DateofNotificationId').val();
        var _FinalDateOfComments = $('[id$=FinalDateforCommentsId]').val();
        $.ajax({
            url: "/api/AddUpdateNotification/NotificationDate?Id=" + myWTOAPP.id + "&DateOfNotification=" + _DateOfNotification + "&FinalDateOfComments=" + _FinalDateOfComments,
            async: false,
            type: "GET",
            contentType: "application/json; charset=utf-8",
            success: function (result) {
                if (result != null) {
                    $('#remainder').val(result.TranslationReminder);
                    $('#duedate').val(result.TranslationDueBy);
                }
            },
            failure: function (result) {
                Alert("Alert", "Something went wrong.<br/>", "Ok");
            },
            error: function (result) {
                Alert("Alert", "Something went wrong.<br/>", "Ok");
            },
            complete: function () {
                setReminderDays('DueDate');
            }
        });
    }
    $(ctrl).multiselect('disable');
}

function setReminderDays(CallFor) {
    var start = $('#remainder').datepicker("getDate");
    var end = $('#duedate').datepicker("getDate");

    if (CallFor != null && CallFor != '' && CallFor == 'DueDate') {
        var newdate = new Date(end);
        newdate.setDate(newdate.getDate() - 2); // minus the date
        start = newdate;
        $("#remainder").val($.datepicker.formatDate('dd M yy', newdate));
    }

    // end - start returns difference in milliseconds 
    var diff = new Date(end - start);

    // get days
    var days = diff / 1000 / 60 / 60 / 24;
    $('#reminderdaysId').text(days);
}

function SetTranslatorName() {
    var translator = $('#ddlTranslater').val() == 0 ? "" : $('#ddlTranslater option:selected').text();
    var MailBody = CKEDITOR.instances.txtMailToTranslatorMailBody.getData();
    if (translator != "")
        CKEDITOR.instances.txtMailToTranslatorMailBody.setData(MailBody.replace("#Translator#", translator));

    $.each(NotificationDoc, function (i, v) {
        if (v.LanguageId > 1)
            v.TranslaterId = $('#ddlTranslater').val();
    });
}

//Open Modal for send mail to Translator
function OpenMailToTranslatorModel() {
    var IsSelected = true;
    var Lang_Id = 0;
    $.each(NotificationDoc, function (i, v) {
        if (v.LanguageId == "")
            IsSelected = false;
    });

    if (IsSelected) {
        $('#UploadedReg').empty();
        var Language = "";
        $.each(NotificationDoc, function (i, v) {
            if (v.LanguageId > 1 && v.IsSelected == "1") {
                $('#UploadedReg').append('<div class="insertfielddiv" style="width: 100%;margin-bottom: 2%;"><div class="checkbox radio-margin" style="margin-top: 2px;float: left;"><label><input type="checkbox" checked onchange="AddRemoveAttachmentTranslator(this);" value="' + v.FileName + '"><span class="cr insertcheckbox" style="margin-top: 2px;"><i class="cr-icon glyphicon glyphicon-ok" style="left: 10%;"></i></span>' + v.DisplayName + ' (' + $.trim(v.LanguageName) + ')' + '</label></div></div>');
                Language = $.trim(v.LanguageName);
                Lang_Id = v.LanguageId;
            }
            else
                $('#UploadedReg').append('<div class="insertfielddiv" style="width: 100%;margin-bottom: 2%;"><div class="checkbox radio-margin" style="margin-top: 2px;float: left;"><label><input type="checkbox" onchange="AddRemoveAttachmentTranslator(this);" value="' + v.FileName + '"><span class="cr insertcheckbox" style="margin-top: 2px;"><i class="cr-icon glyphicon glyphicon-ok" style="left: 10%;"></i></span>' + v.DisplayName + ' (' + $.trim(v.LanguageName) + ')' + '</label></div></div>');
        })

        $('#mailtoTranslator').modal('show');
        var obj = {
            TemplateType: "Mail",
            TemplateFor: "Translator"
        }
        $.ajax({
            url: "/api/AddUpdateNotification/GetMailSMSTemplate/" + myWTOAPP.id,
            async: false,
            type: "POST",
            data: JSON.stringify(obj),
            contentType: "application/json; charset=utf-8",
            success: function (result) {
                BindTranslater(Lang_Id);
                var translator = $('#ddlTranslater').val() == 0 ? "" : $('#ddlTranslater option:selected').text();
                var TranslationDueBy = $('#duedate').val();
                if ($.trim(translator) != "")
                    result.Message = result.Message.replace("#Translator#", $.trim(translator));

                if ($.trim(Language) != "")
                    result.Message = result.Message.replace("#Language#", $.trim(Language));

                if ($.trim(TranslationDueBy) != "")
                    result.Message = result.Message.replace("#TranslationDueBy#", $.trim(TranslationDueBy));

                $('[id$=txtMailToTranslatorSubject]').val(result.Subject);
                CKEDITOR.instances.txtMailToTranslatorMailBody.setData(result.Message);
            },
            failure: function (result) {
                Alert("Alert", "Something went wrong.<br/>", "Ok");
            },
            error: function (result) {
                Alert("Alert", "Something went wrong.<br/>", "Ok");
            },
            complete: function () {
                $('#mailtoTranslator').modal('show');
                $.each($('.AutoHeight'), function (i, v) {
                    var evt = document.createEvent('Event');
                    evt.initEvent('autosize:update', true, false);
                    this.dispatchEvent(evt);
                });
            }
        });
    }
    else
        Alert("Alert", "Please select language of uploaded full text(s) of regulation.<br/>", "Ok");
}

function AddRemoveAttachmentTranslator(ctrl) {
    var Filename = $(ctrl).val();
    var IsChecked = ctrl.checked;
    $.each(NotificationDoc, function (i, v) {
        if (v.FileName == Filename) {
            if (IsChecked)
                v.IsSelected = "1";
            else
                v.IsSelected = "0";
        }

    });
}

//Send Mail to tranlator 
function SendMailToTranslator() {
    var ErrorMsg = "";
    var _Attachments = [];

    var IsContainUntranslatedDoc = false;
    $.each(NotificationDoc, function (i, v) {
        if (v.IsSelected == "1") {
            if (v.LanguageId > 1 && v.TranslaterId > 0)
                IsContainUntranslatedDoc = true;
            _Attachments.push({ DocumentId: v.DocumentId, DisplayName: v.DisplayName, FileName: v.FileName, Content: v.Content, Path: v.Path, LanguageId: v.LanguageId, TranslaterId: v.TranslaterId });
        }
    })

    if ($.trim($('#ddlTranslater').val()) == 0)
        ErrorMsg += "Please select translator.<br/>";

    var MailSubject = $.trim($('#txtMailToTranslatorSubject').val());
    if (MailSubject == "")
        ErrorMsg += "Please enter mail subject.<br/>"

    var MailBody = CKEDITOR.instances.txtMailToTranslatorMailBody.getData();
    if ($.trim(MailBody) == "")
        ErrorMsg += "Please enter mail body.<br/>"

    if (_Attachments.length == 0)
        ErrorMsg += "Please select atleast one attachment.<br/>"
    else {
        if (!IsContainUntranslatedDoc)
            ErrorMsg += "Please select atleast one regulation with language other than English.<br/>"
    }
    if (ErrorMsg.length > 0) {
        Alert("Alert", ErrorMsg, "Ok");
        return false;
    }
    else {
        ShowGlobalLodingPanel();
        var Obj = {
            UserId: myWTOAPP.UserId,
            NotificationId: myWTOAPP.id,
            TranslaterId: $.trim($('#ddlTranslater').val()),
            TranslationDueOn: $.trim($('#duedate').val()),
            TranslationReminderOn: $.trim($('#remainder').val()),
            Attachments: _Attachments,
            MailDetails: { Subject: MailSubject, Message: MailBody }
        }

        $.ajax({
            url: "/api/AddUpdateNotification/SendToTranslater",
            async: false,
            type: "POST",
            data: JSON.stringify(Obj),
            contentType: "application/json; charset=utf-8",
            success: function (result) {
                if (result != null) {
                    $.each(result.Attachments, function (i, v) {
                        $.each(NotificationDoc, function (indx, val) {
                            if (v.FileName == val.FileName) {
                                val.Content = "";
                                val.Path = v.Path;
                                val.DocumentId = v.DocumentId
                                val.SentOn = v.SentToTranslatorOn
                            }
                        });
                    });

                    $.each($('#RegulationsBody > tr'), function () {
                        var $row = $(this);
                        var _Filename = $row.find('a[id^=lblRegulation_]').attr('download');
                        $.each(NotificationDoc, function (indx, val) {
                            if (val.FileName == _Filename) {
                                if ($row.find('.remove-icon').length > 0 && val.IsSelected == "1") {
                                    $row.find('.remove-icon').addClass('hidden');
                                    $row.find('input[id^=hdnDocId_]').val(val.DocumentId);
                                    $row.find('a[id^=lblRegulation_]').attr('download', val.DisplayName);
                                    $row.find('a[id^=lblRegulation_]').attr("href", val.Path);
                                    $row.find('label[id^=lblTranslator_]').attr('data-SearchFor', val.TranslaterId);

                                    $row.find('label[id^=lblSentToTranslator_]').text(val.SentOn);

                                    if (val.LanguageId > 1) {
                                        $row.find('.forTranslation').removeClass('hidden');
                                        $row.find('label[id^=lblTranslator_]').text($.trim($('#ddlTranslater option:selected').text()));
                                    }

                                    $('.translateddoc').removeClass('hidden');
                                }
                            }
                        });
                    });

                    Alert("Alert", "The document has been sent successfully for translation.<br/>", "Ok");
                    $('#btnSendToTranslator').addClass('hidden');
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
                $('#mailtoTranslator').modal('hide');
            }
        });
    }
}
//-------------------------------------Full text(s) of regulation End---------------------------------------



//-------------------------------------Stakeholders Start---------------------------------------

//clear stakeholder checkbox 
function clearStakhCheckbox() {
    $("#txtSearchStakeHolder").val('');
    $("#divStakholder").load('/AddNotification/GetStakeHoldersMaster', { SearchText: "" });
    $(".checked").prop("checked", false);
    if ($("#hdnSelectedStakeHolders").val() != null && $("#hdnSelectedStakeHolders").val() != '') {
        var hdnId = $("#hdnSelectedStakeHolders").val().split(',');
        var str = '';
        for (var i = 0; hdnId.length > i; i++) {
            str = hdnId[i];
            $("#" + str).prop("checked", true);
        }
    }
}

//Add Stakeholder Popup
function SaveStakeholder() {
    var Id = $('[id$=hdnNotificationId]').val();
    var stakeholderId = [];
    if ($('#stackholder').find('input[type=checkbox]:checked').length == 0) {
        Alert("Alert", "Please select atleast one stakeholder.<br/>", "Ok");
    }
    else {
        $.each($('#divStakholder').find('input[type=checkbox]:checked'), function () {
            stakeholderId.push($(this).val());
        });

        $.each(stakeholderId, function (index, value) {
            if (value != "") {
                if ($("#hdnSelectedStakeHolders").val().split(',').indexOf(value) < 0) {
                    $("#hdnSelectedStakeHolders").val($("#hdnSelectedStakeHolders").val() + "," + value);
                }
            }
        })
        // $("#hdnSelectedStakeHolders").val(stakeholderId);
        //stakeholderId = ["1", "2"];
        $.ajax({
            url: "/api/AddUpdateNotification/AddRelatedStackholders/" + Id,
            async: false,
            type: "POST",
            data: JSON.stringify({
                NotificationId: Id,
                StakeholderIds: $("#hdnSelectedStakeHolders").val()
                //   StakeholderIds: stakeholderId.toString()
            }),
            contentType: "application/json; charset=utf-8",
            success: function (result) {
                if (result.Status == "success") {
                    Alert("Alert", stakeholderId.length + " stakeholders have been added successfully.<br/>", "Ok");
                    stakeholderId = [];
                    $.each($("#hdnSelectedStakeHolders").val().split(','), function (index, value) {
                        if (value != "")
                            stakeholderId.push(value);
                    })
                    $('#lblstakehodercount').text(stakeholderId.length);

                }
                else
                    Alert("Alert", "Something went wrong. Please try again.<br/>", "Ok");
                $('#stackholder').modal('toggle');
                $("#RelatedStakeholderList").load('/AddNotification/GetNotificationStakeholders/' + Id);
            },
            failure: function (result) {
                Alert("Alert", "Something went wrong.<br/>", "Ok");
            },
            error: function (result) {
                Alert("Alert", "Something went wrong.<br/>", "Ok");
            },
            complete: function () {
                HideGlobalLodingPanel();
                $('#btnSendMailStakeHolder').show();
                $('#btnRemoveStakeholder').show();
            }
        });
    }
}

//Remove Stakeholder button click function
function RemoveStakeholder() {
    if ($('#notificationlist tbody').find('input[type=checkbox]:checked').length == 0)
        Alert("Alert", "Please select stakeholders to remove.<br/>", "Ok");
    else
        Confirm('Delete', 'Do you want to remove selected stakholders?', 'Yes', 'No', 'RemoveStakeholderOk()');
}

function RemoveStakeholderOk() {
    var Id = $('[id$=hdnNotificationId]').val();
    var stakeholderId = [];
    var ToRemove = [];
    $.each($('#notificationlist').find('input[type=checkbox]:checked'), function () {
        if ($(this).val() != "on")
            stakeholderId.push($(this).val());
    });
    $.each($('#notificationlist').find('input:checkbox:not(:checked)'), function () {
        ToRemove.push($(this).val());
    })
    $("#hdnSelectedStakeHolders").val(ToRemove);
    $.ajax({
        url: "/api/AddUpdateNotification/RemoveRelatedStackholders/" + Id,
        async: false,
        type: "POST",
        data: JSON.stringify({
            NotificationId: Id,
            StakeholderIds: stakeholderId.toString()
        }),
        contentType: "application/json; charset=utf-8",
        success: function (result) {
            if (result)
                Alert("Alert", "Selected Stakeholders have been removed successfully.<br/>", "Ok");
            else
                Alert("Alert", "Something went wrong. Please try again.<br/>", "Ok");

            $("#RelatedStakeholderList").load('/AddNotification/GetNotificationStakeholders/' + Id);
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

//Open model for send mail to stakeholders
function OpenSendMailModel() {
    $('#divAttachDetails').css('display', 'none');
    $('#divAttachments').css('display', '');
    $('#btnSendMail').css('display', '');
    $('#divStakeHolderList').css('display', 'none');
    $('#divStakeHolderCounts').css('display', '');
    $('#ddlAttachments').find('input[type=checkbox]').prop('checked', false);
    var stakeholderId = [];
    if ($('#notificationlist tbody').find('input[type=checkbox]:checked').length == 0) {
        Alert("Alert", "Please select stakeholders to send mail.<br/>", "Ok");
    }
    else {
        $.each($('#notificationlist tbody').find('input[type=checkbox]:checked'), function () {
            if (this.id.toLowerCase() != 'chkselectall')
                stakeholderId.push($(this).val());
        });
        $("#SelectedStackholdersCount").text(stakeholderId.length);
        $('#txtAttachments').addClass("hidden");
        $('#txtAttachmentCount').val('0 file(s) selected');
        MailAttachments = [];
        var obj = {
            TemplateType: "Mail",
            TemplateFor: "StakeholderMail",
            StakeHolderResponseDate: $('[id$=txtStakeholderResponseDueBy]').val()
        }
        $.ajax({
            url: "/api/AddUpdateNotification/GetMailSMSTemplate/" + myWTOAPP.id,
            async: false,
            type: "POST",
            data: JSON.stringify(obj),
            contentType: "application/json; charset=utf-8",
            success: function (result) {
                $('[id$=txtSubject]').val(result.Subject);
                CKEDITOR.instances.txtMessage.setData(result.Message);
                CKEDITOR.instances.txtMessage.focus();
            },
            failure: function (result) {
                Alert("Alert", "Something went wrong.<br/>", "Ok");
            },
            error: function (result) {
                Alert("Alert", "Something went wrong.<br/>", "Ok");
            },
            complete: function () {
                $.each($('[id$=tblAttachments] input[type=checkbox]'), function () {
                    this.checked = false;
                });
                BindStakeholdersMailAttachments();
                $('#sentmail').modal('show');
                $.each($('.AutoHeight'), function (i, v) {
                    var evt = document.createEvent('Event');
                    evt.initEvent('autosize:update', true, false);
                    this.dispatchEvent(evt);
                });
            }
        });

    }
}

function OpenSendMailModelFromStakehoderCount(MailId, Callfor) {
    $('#btnSendMail').css('display', 'none');
    $('#divStakeHolderList').css('display', '');
    $('#divStakeHolderCounts').css('display', 'none');
    $('#divStakholderSendMailList').empty();
    $('#divAttachDetails').css('display', '');
    $('#divAttachments').css('display', 'none');
    var obj = {
        MailId: MailId,
        callFor: Callfor
    }
    $.ajax({
        url: "/api/AddUpdateNotification/GetMailDetailsSendtoStakeholder?MailId=" + MailId + "&callFor=" + Callfor,
        async: false,
        type: "GET",
        contentType: "application/json; charset=utf-8",
        success: function (result) {
            $('[id$=txtSubject]').val(result.MailDetails.Subject);
            CKEDITOR.instances.txtMessage.setData(result.MailDetails.Message);
            var htmlstake = '';
            for (var i = 0; i < result.StakeHolders.length; i++) {
                htmlstake += "<div class='row seprater'> <div class='col-xs-12 col-sm-12 col-md-12'><div class='col-sm-2'>" + parseInt(i + 1) + "</div><div class='col-sm-5'>" + result.StakeHolders[i].OrgName + "</div><div class='col-sm-5'>" + result.StakeHolders[i].FullName + "</div></div></div>";
            }
            //MailAttachmentDetails
            $('#divStakholderSendMailList').append(htmlstake);
            if (result.MailAttachmentDetails.length > 0) {
                var FileCount = 0;
                var Files = '';
                $.each(result.MailAttachmentDetails, function (i, v) {
                    Files += '<a href="' + v.Path + '" download><p class="Attachmentfilename">' + v.FileName + '</p></a>';
                    FileCount++;
                });
                $('#divAttachFiles').empty();
                $('#divAttachFiles').append(Files);
                $('#divAttachFiles').removeClass("hidden");
            }
            else
                $('#divAttachDetails').hide();

            //divtblAttachments
        },
        failure: function (result) {
            Alert("Alert", "Something went wrong.<br/>", "Ok");
        },
        error: function (result) {
            Alert("Alert", "Something went wrong.<br/>", "Ok");
        }
    });
    $('#sentmail').modal('show');
}

function BindStakeholdersMailAttachments(Id) {
    $.ajax({
        url: "/api/AddUpdateNotification/NotificationDocuments/" + myWTOAPP.id,
        async: false,
        type: "GET",
        contentType: "application/json; charset=utf-8",
        success: function (result) {
            var FileCount = 0;
            var Files = '';
            $.each(result, function (i, v) {
                MailAttachments.push({ "FileName": v.FileName, "Content": "", "Path": v.Path, "IsSelected": true });
                Files += '<p class="Attachmentfilename">' + v.FileName + '<a onclick="RemoveStakeholdersMailAttachments(this);" class="fileremove"><span class="glyphicon glyphicon-remove"></span></a></p>';
                FileCount++;
            });
            TempMailAttachments = MailAttachments;
            $('#txtAttachments').empty();
            $('#txtAttachments').append(Files);
            $('#txtAttachments').removeClass("hidden");
            $('#txtAttachmentCount').val(FileCount + ' file(s) selected');
        },
        failure: function (result) {
            Alert("Alert", "Something went wrong.<br/>", "Ok");
        },
        error: function (result) {
            Alert("Alert", "Something went wrong.<br/>", "Ok");
        },
        complete: function () {
            $.each($('.AutoHeight'), function (i, v) {
                var evt = document.createEvent('Event');
                evt.initEvent('autosize:update', true, false);
                this.dispatchEvent(evt);
            });
        }
    })
}

function RemoveStakeholdersMailAttachments(ctrl) {
    var FileName = $.trim($(ctrl).parent().text());
    var FileCount = 0;
    var Files = '';

    $.each(MailAttachments, function (i, v) {
        if (v.FileName == FileName)
            v.IsSelected = false;
        else if (v.IsSelected) {
            Files += '<p class="Attachmentfilename">' + v.FileName + '<a href="#" onclick="RemoveStakeholdersMailAttachments(this);" class="fileremove"><span class="glyphicon glyphicon-remove"></span></a></p>';
            FileCount++;
        }
    });

    $('#txtAttachments').empty();
    $('#txtAttachments').append(Files);
    $('#txtAttachments').removeClass("hidden");
    $('#txtAttachmentCount').val(FileCount + ' file(s) selected');
}

//Open popup of add other attachments in Stakholders mail popup
function AddAttachment() {
    debugger;
    var HTML = '';
    $.each(TempMailAttachments, function (i, v) {
        //TempMailAttachments.push({ "FileName": v.FileName, "Content": v.FileContent, "Selected": true, "Path": v.Path, "IsSelected": v.IsSelected });
        HTML += '<tr>';
        HTML += '<td style="width: 4%; width:50px;">';
        HTML += '<div class="checkbox radio-margin">';
        HTML += '<label>';
        if (v.IsSelected)
            HTML += '<input type="checkbox" checked="checked" val="" onchange="AddRemoveMailAttachement(this);">';
        else
            HTML += '<input type="checkbox" val="" onchange="AddRemoveMailAttachement(this);">';
        HTML += '<span class="cr"><i class="cr-icon glyphicon glyphicon-ok"></i></span>';
        HTML += '</label>';
        HTML += '</div>';
        HTML += '</td>';
        HTML += '<td style="width: 96%">' + v.FileName + '</td>';
        HTML += '</tr>';
    });
    $('[id$=tblAttachments]').empty().append(HTML);
    $('[id$=divAddAttachmentOverlay]').show();
    $('[id$=divAddAttachment]').show();
}

//close popup of add other attachments in Stakholders mail popup
function CloseAddAttachment() {
    //TempMailAttachments = [];
    $('[id$=divAddAttachmentOverlay]').hide();
    $('[id$=divAddAttachment]').hide();
}

//ok function of add other attachments in Stakholders mail popup
function AddAttachmentOk() {
    var IsAttachmentSelected = false;
    $.each(TempMailAttachments, function (i, v) {
        if (v.IsSelected)
            IsAttachmentSelected = true;
    });

    if (TempMailAttachments.length == 0 || !IsAttachmentSelected) {
        Alert("Alert", "Please choose attachment.<br/>", "Ok");
        return false;
    }
    else {
        MailAttachments = [];
        var Files = "";
        var FileCount = 0;
        $.each(TempMailAttachments, function (i, v) {
            if (v.IsSelected) {
                MailAttachments.push({ "FileName": v.FileName, "Content": v.Content, "Path": v.Path, IsSelected: v.IsSelected });
                Files += '<p class="Attachmentfilename">' + v.FileName + '<a href="#" onclick="RemoveStakeholdersMailAttachments(this);" class="fileremove"><span class="glyphicon glyphicon-remove"></span></a></p>';
                FileCount++;
            }
        });
        $('#txtAttachments').empty().append(Files);
        $('#txtAttachments').removeClass("hidden");
        $('#txtAttachmentCount').val(FileCount + ' file(s) selected');

        CloseAddAttachment();
    }
}

//select/Unselect other attachments in Stakholders mail popup
function AddRemoveMailAttachement(ctrl) {
    var FileName = $.trim($(ctrl).closest('td').next().text());
    var FileContent = $(ctrl).val();
    if (ctrl.checked) {
        var IsChanged = false;
        $.each(TempMailAttachments, function (index, value) {
            if (value.FileName == FileName) {
                value.IsSelected = true;
                IsChanged = true;
            }
        });
        if (!IsChanged) {
            if (FileContent == "")
                TempMailAttachments.push({ "FileName": FileName, "Content": "", "Selected": true, "Path": "", IsSelected: true });
            else
                TempMailAttachments.push({ "FileName": FileName, "Content": "", "Selected": true, "Path": FileContent, IsSelected: true });
        }
    }
    else {
        $.each(TempMailAttachments, function (index, value) {
            if (value.FileName == FileName)
                value.IsSelected = false;
        });
    }
}

//Send Mail to stakeholders
function SendmailToStakeholders() {
    ShowGlobalLodingPanel();
    var NotificationId = $('[id$=hdnNotificationId]').val();
    var StackHolders = '';
    $.each($('#notificationlist tbody').find('input[type=checkbox]:checked'), function () {
        if (this.id.toLowerCase() != 'chkselectall')
            StackHolders += $(this).val() + ',';
    });

    var Subject = $.trim($('#txtSubject').val());
    var Message = $.trim(CKEDITOR.instances.txtMessage.getData());

    var ErrorMsg = '';
    if (Subject == '') {
        ErrorMsg += 'Subject cannot be blank.<br/>';
    }

    if (Message == '') {
        ErrorMsg += 'Message cannot be blank.<br/>';
    }

    if (ErrorMsg.length > 0) {
        HideGlobalLodingPanel();
        Alert("Alert", ErrorMsg, "Ok");
        return false;
    }
    else {
        var _Attachments = [];
        $.each(MailAttachments, function (i, v) {
            if (v.IsSelected == true)
                _Attachments.push(v);
        })
        var obj = {
            NotificationId: NotificationId,
            UserId: myWTOAPP.UserId,
            Role: myWTOAPP.UserRole,
            Stakeholders: StackHolders,
            Attachments: _Attachments,
            Subject: Subject,
            Message: Message
        }
        $.ajax({
            url: "/api/AddUpdateNotification/SendMailToStakeholders",
            async: false,
            type: "POST",
            data: JSON.stringify(obj),
            contentType: "application/json; charset=utf-8",
            success: function (result) {
                AlertwithFunction("Alert", "Mail has been sent successfully to selected stakeholders.<br/>", "Ok", "AfterMailSent(" + NotificationId + ")");
                //window.location.reload();                

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
    HideGlobalLodingPanel();
}

function AfterMailSent(NotificationId) {
    $("#divMailStatistics").load('/AddNotification/GetNotificationMails/' + NotificationId);
    $("#RelatedStakeholderList").load('/AddNotification/GetNotificationStakeholders/' + NotificationId);
    $('#sentmail').modal('hide');
    location.reload();
}

function CheckHeaderAll() {
    var Count = 0;
    var checkcount = 0;
    $('#divStakholder').find('input[type=checkbox]').each(function (index, value) {
        Count++;

        if ($(value).prop('checked') == true) {
            checkcount++;
        }
    })
    if (checkcount == Count && checkcount != 0) {
        $('#chkSelectAll_AddStakeholders').prop('checked', true);
    }
    else {
        $('#chkSelectAll_AddStakeholders').prop('checked', false);
    }
}

function AfterResponseSaved() {
    $("#MailConversationPopUp").load('/AddNotification/GetStakeholderConversation', { NotificationId: $('[id$=hdnNotificationId]').val(), StakeholderId: $('[id$=hdnStakeholderId]').val() });
    $("#RelatedStakeholderList").load('/AddNotification/GetNotificationStakeholders/' + myWTOAPP.id);
    $("#divMailStatistics").load('/AddNotification/GetNotificationMails/' + myWTOAPP.id);
}

function CheckUncheckSelectAll() {
    var IsSelectedAll = true;
    $.each($('#notificationlist > tbody').find('input[type=checkbox]'), function () {
        if (!this.checked)
            IsSelectedAll = false;
    });

    if (IsSelectedAll)
        $('#chkSelectAll').prop('checked', true);
    else
        $('#chkSelectAll').prop('checked', false);
}

function checkstakeholdercheckbox() {
    var IsSelectedAll = $('#chkSelectAll').prop('checked');
    $.each($('#notificationlist > tbody').find('input[type=checkbox]'), function () {
        this.checked = IsSelectedAll;
    });
}

function SetMailId(Id) {
    MailIdResponse = Id;
}

function ClearResponseForm() {
    MailIdResponse = '';
    $('#txtResponseMessage').val('');
    $('#txtResponseReceivedOn').val('');
    ResponseMailAttachments = [];
}

function ViewResponseFromStackHolder(ResponseMailId) {

    $.ajax({
        url: "/api/AddUpdateNotification/ViewResponseFromStackHolder/" + ResponseMailId,
        async: false,
        type: "GET",
        contentType: "application/json; charset=utf-8",
        success: function (result) {
            $('#ViewResponseModal').modal('show');
            if (result.objStackholderMail != null) {
                $('[id$=lblResponseDate]').text(result.objStackholderMail.Date);
                $('[id$=lblResponseMailSubject]').text(result.objStackholderMail.Subject);
                $('[id$=lblResponseMailMessage]').text(result.objStackholderMail.Message);
            }
            var Files = '';

            if (result.MailAttachmentDetails != null) {
                $.each(result.MailAttachmentDetails, function (i, v) {
                    Files += "<a href='" + v.Path + "' download><p class='Attachmentfilename'>" + v.FileName + "</p></a>";
                })
                $('#lblResponseMailAttachments').empty();
                $('#lblResponseMailAttachments').append(Files);
            }
            else {
                $('#divResponseAttachment').css('display', 'none');
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

function UploadResponseAttachment() {
    $('#UploadStackDoc').click();
    return false;
}

function AddMultipleDoc(ctrl) {
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
                    $.each(ResponseMailAttachments, function (i, v) {
                        var FileName = $.trim(v.FileName);
                        if (FileName.toLowerCase() == value.name.toLowerCase())
                            IsExist = true;
                    });

                    if (IsExist) {
                        Alert("Alert", "An attachment with same name has been already added.<br/>", "Ok")
                    }
                    else {
                        ResponseMailAttachments.push({ "FileName": value.name, "Content": e.target.result, "Path": "" });
                        var HTML = '';
                        HTML += '<p class="Attachmentfilename">' + value.name + '<a href="#" onclick="RemoveResponseAttachments(this);" class="fileremove"><span class="glyphicon glyphicon-remove"></span></a></p>';
                        $('[id$=DivStakeAttachments]').append(HTML);
                        $('#txtResponseAttachment').val(ResponseMailAttachments.length + ' file(s) selected');
                    }
                }
                reader.readAsDataURL(fileToLoad);
            });
        }
    }
    return false;
}

function RemoveResponseAttachments(ctrl) {
    var FileName = $.trim($(ctrl).parent().text());
    var FileCount = 0;
    var Files = '';
    var TempResponseMailAttachments = [];
    $.each(ResponseMailAttachments, function (i, v) {
        if (v.FileName != FileName) {
            TempResponseMailAttachments.push({ "FileName": v.FileName, "Content": v.Content, "Selected": true, "Path": "", "IsSelected": true });
            Files += '<p>' + v.FileName + '<a href="#" onclick="RemoveResponseAttachments(this);" class="fileremove"><span class="glyphicon glyphicon-remove"></span></a></p>';
            FileCount++;
        }
    });
    ResponseMailAttachments = TempResponseMailAttachments;

    $('#DivStakeAttachments').empty();
    $('#DivStakeAttachments').append(Files);
    $('#DivStakeAttachments').removeClass("hidden");
    $('#txtResponseAttachment').val(FileCount + ' file(s) selected');
}

function SaveStakeholderResponse() {
    ShowGlobalLodingPanel();
    var NotificationId = $('[id$=hdnNotificationId]').val();
    var StakeHolderId = $('[id$=hdnStakeholderId]').val();
    var Message = $.trim($('#txtResponseMessage').val());
    var ReceivedOn = $.trim($('#txtResponseReceivedOn').val());
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
            MailId: MailIdResponse,
            NotificationId: NotificationId,
            StakeholderId: StakeHolderId,
            Message: Message,
            ResponseReceivedOn: ReceivedOn,
            ResponseDocuments: ResponseMailAttachments,
        }
        $.ajax({
            url: "/api/AddUpdateNotification/SaveStakeholderResponse",
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
                //HideGlobalLodingPanel();
                TempResponseMailAttachments = [];
                ResponseMailAttachments = TempResponseMailAttachments;
            }
        });
    }
    HideGlobalLodingPanel();
    return false;
}
//-------------------------------------Stakeholders End---------------------------------------

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