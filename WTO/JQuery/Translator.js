var myWTOAPP = {
    LoginCount: null,
    TranslatorId: null,
    Role:null
};

var TranslatedDocument = [];

(function (myWTOAPP) {
    myWTOAPP.init = function (LoginCount, TransId,Role) {
        myWTOAPP.LoginCount = (LoginCount == "" ? null : LoginCount);
        myWTOAPP.TranslatorId = (TransId == "" ? null : TransId);
        myWTOAPP.Role = (Role == "" ? null : Role);
    }
})(myWTOAPP);

$(document).ready(function () {
    if (myWTOAPP.LoginCount != null && myWTOAPP.LoginCount == 0) {
        $("#changepassword").modal("show");
    }

    $('[data-toggle="tooltip"]').tooltip();

    $(".date").datepicker({
        changeMonth: true,
        changeYear: true,
        dateFormat: 'dd M yy'
    });

    $(".date").keydown(function (e) {
        return false;
    });

    // We can watch for our custom `fileselect` event like this
    $(':file').on('fileselect', function (event, numFiles, label) {
        var input = $(this).parents('.input-group').find(':text'),
        log = numFiles > 1 ? numFiles + ' files selected' : label;

        if (input.length) {
            input.val(log);
        } else {
            if (log)
                Alert("Translator", log, "Ok");
        }
    });

    $("[id$=txtConfirmPassword]").change(function () {
        var ConfirmPassword = $(this).val();
        var Password = $('[id$=txtNewPassword]').val();
        if (Password != "" && ConfirmPassword != Password) {
            Alert("Change Password", "Password didn't Match.<br/>", "Ok");
        }
    });

    $('#changepassword').on('hidden.bs.modal', function () {
        $('[id$=txtOldPassword]').val('');
        $('[id$=txtNewPassword]').val('');
        $('[id$=txtConfirmPassword]').val('');
    });

    $("[id$=txtNewPassword]").change(function () {
        var NewPassword = $(this).val();
        var Password = $('[id$=txtOldPassword]').val();
        if (Password != "" && NewPassword == Password) {
            Alert("Change Password", "New password can't be same as the old password.<br/>", "Ok");
            $(this).val('');
        }
    });

    $('#TranslatedDocumentId').change(function (e) {
        var totfilesize = 0;
        if ($(this)[0].files.length != 0) {
            var fileToLoad = $(this)[0].files[0];
            var ext = $(this)[0].files[0].name.split(".")[$(this)[0].files[0].name.split(".").length - 1];
            $.each($(this)[0].files, function (index, value) {
                totfilesize += value.size;
            });

            if (totfilesize > 10485760) {
                Alert("Translator", "Total attachment files size should not be greater than 10 MB.<br/>", "Ok");
                $(this).val('');
                TranslatedDocument = [];
                $('[id$=TranslatedDocumentName_txt]').val('');
                return false;
            }
            else if (ext != "docx" && ext != "doc" && ext != "pdf") {
                Alert("Translator", "You can upload only word and pdf files.<br/>", "Ok");
                $(this).val('');
                TranslatedDocument = [];
                $('[id$=TranslatedDocumentName_txt]').val('');
                return false;
            }
            else {
                $.each($(this)[0].files, function (index, value) {
                    var reader = new FileReader();
                    reader.onload = function (e) {
                        TranslatedDocument = { "FileName": value.name, "Content": e.target.result };
                        $('[id$=TranslatedDocumentName_txt]').val(value.name);
                    };
                    reader.readAsDataURL(fileToLoad);
                });
            }
        }
        else {
            TranslatedDocument = [];
            $('[id$=TranslatedDocumentName_txt]').val('');
        }
    });

    $('#UploadDocumentModal').on('hidden.bs.modal', function () {
        $('[id$=TranslatedDocumentNameId]').val('');
        $('[id$=hdnNotificationId]').val('');
    });

    $("[id$=txtNotificationNumber]").on('keypress', function () {
        if ($.trim($(this).val()) == "") {
            $('[id$=clearNotificationNumber]').hide();
        }
        else
            $('[id$=clearNotificationNumber]').show();
    });

    $("[id$=txtNotificationNumber]").change(function () {
        if ($.trim($(this).val()) == "") {
            $('[id$=clearNotificationNumber]').hide();
        }
        else
            $('[id$=clearNotificationNumber]').show();
    });

    $("[id$=clearNotificationNumber]").click(function () {
        $('[id$=txtNotificationNumber]').val('');
        $(this).hide();
    });
});

function UploadTranslatedDocument(NotificationId) {
    $('[id$=hdnNotificationId]').val(NotificationId);
    $('[id$=TranslatedDocumentId]').val('');
    $('[id$=TranslatedDocumentNameId]').val('');
    $('[id$=TranslatedDocumentName_txt]').val('');
    TranslatedDocument = [];
    $('[id$=UploadDocumentModal]').modal('show');
}

function ChangePassword() {
    var ErrorMsg = '';
    var OldPasswrd = $('[id$=txtOldPassword]').val();
    var NewPasswrd = $('[id$=txtNewPassword]').val();
    var ConfirmPassword = $('[id$=txtConfirmPassword]').val();

    if (OldPasswrd == "") {
        ErrorMsg += "Please enter your existing password.<br/>";
    }

    if (NewPasswrd == "") {
        ErrorMsg += "Please enter a new password.<br/>";
    }
    else if (OldPasswrd == NewPasswrd) {
        ErrorMsg += "New password should not be same as old password.<br/>";
    }
    else if (ConfirmPassword == "") {
        ErrorMsg += "Please confirm your new password.<br/>";
    }
    else if (NewPasswrd != ConfirmPassword) {
        ErrorMsg += "New password and confirm password didn't match.<br/>";
    }

    if (ErrorMsg.length > 0) {
        Alert("Change Password", ErrorMsg, "Ok");
        return false;
    }
    else
        UpdatePassword();
}

function UpdatePassword() {
    var obj = {
        TranslatorId: myWTOAPP.TranslatorId,
        OldPassword: $('[id$=txtOldPassword]').val(),
        NewPassword: $('[id$=txtNewPassword]').val()
    }

    $.ajax({
        url: "/api/Translator/ChangePassword",
        async: false,
        type: "POST",
        data: JSON.stringify(obj),
        contentType: "application/json; charset=utf-8",
        success: function (result) {
            if (result.Status.toLowerCase() == "success") {
                Alert("Change Password", "Your password has been updated successfully.<br/>", "Ok");
                $("#changepassword").modal("hide");
                window.location.href = window.location.origin + "/Translator/List";
            }
            else if (result.Status.toLowerCase() == "failure" && result.MessageType == "3")
                Alert("Change Password", "Existing password entered by you is incorrect.", "Ok");
            else
                Alert("Change Password", "Something went wrong. Please try again.<br/>", "Ok");
        },
        failure: function (result) {
            Alert("Change Password", "Something went wrong.<br/>", "Ok");
        },
        error: function (result) {
            Alert("Change Password", "Something went wrong.<br/>", "Ok");
        },
        complete: function () {
            //HideGlobalLodingPanel();
        }
    });
}

function SaveTranslatedDocument() {
    var ErrorMsg = '';

    if (TranslatedDocument.length == 0)
        ErrorMsg += "Please provide attachment. <br/>";

    if ($.trim($('[id$=TranslatedDocumentNameId]').val()) == "")
        ErrorMsg += "Please provide attachment name. <br/>";

    if (ErrorMsg.length > 0) {
        Alert("Translator", ErrorMsg, "Ok");
        return false;
    }
    else {
        var obj = {
            NotificationId: $('[id$=hdnNotificationId]').val(),
            DisplayName: $.trim($('[id$=TranslatedDocumentNameId]').val()),
            Document: TranslatedDocument
        }
        $.ajax({
            url: "/api/Translator/SaveTranslatedDocument/" + myWTOAPP.TranslatorId,
            async: false,
            type: "POST",
            data: JSON.stringify(obj),
            contentType: "application/json; charset=utf-8",
            success: function (result) {
                Alert("Translator", "Document has been saved successfully.<br/>", "Ok");
                window.location.reload();
            },
            failure: function (result) {
                Alert("Translator", "Something went wrong.<br/>", "Ok");
            },
            error: function (result) {
                Alert("Translator", "Something went wrong.<br/>", "Ok");
            },
            complete: function () {
                //HideGlobalLodingPanel();
            }
        });
    }
}

function ClearSearch() {
    $('[id$=txtNotificationNumber]').val('');
    $('[id$=txtDocumentReceivedOn]').val('');
    $('[id$=txtTranslationDueOn]').val('');
    $('[id$=ddlStatus]').val('');
    $('[id$=clearNotificationNumber]').hide();
    SearchDocument();
}

function SearchDocument() {
    var obj = {
        TranslatorId: myWTOAPP.TranslatorId,
        ReceivedOn: $('[id$=txtDocumentReceivedOn]').val(),
        SubmissionDueOn: $.trim($('[id$=txtTranslationDueOn]').val()),
        NotificationNumber: $.trim($('[id$=txtNotificationNumber]').val()),
        Status: $("[id$=ddlStatus]").val()
    }

    $.ajax({
        url: "/api/Translator/SearchDocuments/",
        async: false,
        type: "POST",
        data: JSON.stringify(obj),
        contentType: "application/json; charset=utf-8",
        success: function (result) {
            $('[id$=tblData]').find('tbody').empty();
            var HtmlData = '';
            if (result.length > 0) {
                $.each(result, function (i, v) {
                    HtmlData += '<tr>';
                    HtmlData += '<td>' + (i + 1) + '</td>';
                    HtmlData += '<td>' + v.NotificationNumber + '</td>';
                    HtmlData += '<td>' + v.SendToTranslaterOn + '</td>';
                    HtmlData += '<td>' + v.TranslationDueBy + '</td>';
                    HtmlData += '<td><a href="' + v.UntranslatedDocument.Path + '" download="' + v.UntranslatedDocument.FileName + '"><i class="material-icons download">&#xE2C4;</i>' + v.UntranslatedDocument.FileName + '</a></td>';
                    if (v.TranslatedDocument.FileName == "")
                        HtmlData += '<td><a href="#" onclick="UploadTranslatedDocument(' + v.NotificationId + ');" class="delete" data-toggle="modal">Upload</a></td>';
                    else {
                        HtmlData += '<td><a href="' + v.TranslatedDocument.Path + '" download="' + v.TranslatedDocument.FileName + '"><i class="material-icons download">&#xE2C4;</i> ' + v.TranslatedDocument.FileName + '</a></td>';
                    }
                    HtmlData += '</tr>';
                });
            }
            else {
                HtmlData += '<tr><td colspan="6">No record found</td></tr>';
            }

            $('[id$=tblData]').find('tbody').append(HtmlData);
        },
        failure: function (result) {
            Alert("Translator", "Something went wrong.<br/>", "Ok");
        },
        error: function (result) {
            Alert("Translator", "Something went wrong.<br/>", "Ok");
        },
        complete: function () {
            //HideGlobalLodingPanel();
        }
    });
}