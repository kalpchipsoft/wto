var UserImage = [];
$(document).ready(function () {
    GetHSCode();

    $('#fileUploadId').change(function (e) {
        debugger;
        var totfilesize = 0;
        if ($(this)[0].files.length != 0) {
            var fileToLoad = $(this)[0].files[0];
            var ext = $(this)[0].files[0].name.split(".")[$(this)[0].files[0].name.split(".").length - 1];
            $.each($(this)[0].files, function (index, value) {
                totfilesize += value.size;
            });

            if (totfilesize > 10485760) {
                Alert("User", "Total attachment files size should not be greater than 10 MB.<br/>", "Ok");
                $("#Loader").hide();
                UserImage = [];
                $('[id$=ImgPhotograph]').attr('src', '../contents/img/Add-Image.jpg');
                $('[id$=lnkRemoveImage]').hide();
                return false;
            }
            else if (ext != "png" && ext != "jpg" && ext != "jpeg") {
                Alert("User", "You can upload only jpg,jpeg and png files.<br/>", "Ok");
                $(this).val('');
                $("#Loader").hide();
                UserImage = [];
                $('[id$=ImgPhotograph]').attr('src', '../contents/img/Add-Image.jpg');
                $('[id$=lnkRemoveImage]').hide();
                return false;
            }
            else {
                $.each($(this)[0].files, function (index, value) {
                    var reader = new FileReader();
                    reader.onload = function (e) {
                        UserImage = { "FileName": value.name, "Content": e.target.result, "Path": "" };
                        $('[id$=ImgPhotograph]').attr('src', e.target.result);
                        $('[id$=lnkUploadImage]').text("Change");
                        $('[id$=lnkRemoveImage]').show();
                    };
                    reader.readAsDataURL(fileToLoad);
                });

            }
        }
        else {
            UserImage = [];
            $('[id$=ImgPhotograph]').attr('src', '../contents/img/Add-Image.jpg');
            $('[id$=lnkRemoveImage]').hide();
        }
    });
});

/******************************User Section Start****************/
function setNoImage(ctrl) {
    $(ctrl).attr('src', '../contents/img/NoImage.png');
}

function UploadImage() {
    $('input[type=file]').trigger('click');
    return false;
}

function RemoveImage() {
    $('[id$=ImgPhotograph]').attr('src', '../contents/img/Add-Image.jpg');
    $('[id$=fileupload]').val('');
    $('[id$=lnkUploadImage]').text("Upload");
    $('[id$=lnkRemoveImage]').hide();
    UserImage = [];
    return false;
}

function AddUserPopup() {
    $("#UserSaveUpdate").text('Save');
    $("#AddUserpopupHead").text('Add User');
    $('[id$=UserFirstName]').val("");
    $('[id$=UserLastName]').val("");
    $('[id$=UserEmailId]').val("");
    $('[id$=UserMobileNo]').val("");
    $('[id$=UserSelectActiveInactive]').val("1");
    $('[id$=hdnUserId]').val('0');
    $('[id$=ImgPhotograph]').attr('src', '../contents/img/Add-Image.jpg');
    $('[id$=lnkUploadImage]').text("Upload");
    $('[id$=lnkRemoveImage]').hide();
    UserImage = [];
    $('[id$=UserRoleId]').val('');
    $('.error').removeClass('error');
}

function Uservalidate() {
    var msg = "";

    if (UserImage.FileName == null)
        msg += "Please upload user image.<br/>";

    if ($('[id$=UserFirstName]').val().trim().length == 0) {
        $('[id$=UserFirstName]').addClass("error");
        msg += "Please enter first name.<br/>";
    }
    else
        $('[id$=UserFirstName]').removeClass("error");

    if ($('[id$=UserLastName]').val().trim().length == 0) {
        $('[id$=UserLastName]').addClass("error");
        msg += "Please enter last name.<br/>";
    }
    else
        $('[id$=UserLastName]').removeClass("error");

    if ($('[id$=UserEmailId]').val().trim().length > 0) {
        if (!validateEmail($('[id$=UserEmailId]').val().trim())) {
            $('[id$=UserEmailId]').addClass("error");
            msg += "Please enter valid email-id.<br/>";
        }
        else {
            var IsEmailExist = IsEmailExists($('[id$=UserEmailId]').val().trim(), "User");
            if (IsEmailExist && $('[id$=hdnUserId]').val() == 0) {
                msg += "An User already exists with same email-id.<br/>";
                $('[id$=UserEmailId]').addClass("error");
            }
            else
                $('[id$=UserEmailId]').removeClass("error");
        }
    }
    else {
        $('[id$=UserEmailId]').addClass("error");
        msg += "Please enter email-id.<br/>";
    }

    if ($('[id$=UserMobileNo]').val().trim().length > 0) {
        if (!IsMobileNumberReg($('[id$=UserMobileNo]').val().trim())) {
            $('[id$=UserMobileNo]').addClass("error");
            msg += "Please enter valid mobile number.<br/>";
        }
        else
            $('[id$=UserMobileNo]').removeClass("error");
    }
    else {
        $('[id$=UserMobileNo]').addClass("error");
        msg += "Please enter mobile number.<br/>";
    }

    if ($('[id$=UserSelectActiveInactive]').val() == -1) {
        $('[id$=UserSelectActiveInactive]').addClass("error");
        msg += "Please select user status.<br/>";
    }
    else
        $('[id$=UserSelectActiveInactive]').removeClass("error");

    if ($.trim($('[id$=UserRoleId]').val()) == '') {
        $('[id$=UserRoleId]').addClass("error");
        msg += "Please select user role.<br/>";
    }
    else
        $('[id$=UserRoleId]').removeClass("error");


    if (msg.length > 0) {
        Alert("User", msg, "Ok");
    }
    else
        AddNewUser();
}

function EditUserData(id) {
    $("#AddUser").modal("show");
    $("#AddUserpopupHead").text('Update User');
    $("#UserSaveUpdate").text('Update');
    $.ajax({
        beforeSend: function () {
            ShowGlobalLodingPanel();
        },
        url: "/api/ManageAccess/GetUserDetails?Id=" + id + "",
        async: false,
        type: "POST",
        data: JSON,
        contentType: "application/json; charset=utf-8",
        success: function (result) {
            $('[id$=hdnUserId]').val(result.UserId);
            $('[id$=UserFirstName]').val(result.FirstName);
            $('[id$=UserLastName]').val(result.LastName);
            $('[id$=UserEmailId]').val(result.Email);
            $('[id$=UserMobileNo]').val(result.Mobile);
            $('[id$=UserSelectActiveInactive]').val(result.Status);
            if (result.UserRole != null)
                $("[id$=UserRoleId]").val(result.UserRole.RoleId);

            if (result.UserImage != null) {
                $('[id$=ImgPhotograph]').attr('src', result.UserImage.Path);
                $('[id$=lnkUploadImage]').text("Change");
                $('[id$=lnkRemoveImage]').show();
                UserImage = { "FileName": result.UserImage.FileName, "Content": result.UserImage.Content, "Path": result.UserImage.Path }
            }
            else {
                $('[id$=ImgPhotograph]').attr('src', '../contents/img/Add-Image.jpg');
                $('[id$=fileupload]').val('');
                $('[id$=lnkUploadImage]').text("Upload");
                $('[id$=lnkRemoveImage]').hide();
                UserImage = [];
            }
        },
        failure: function (result) {
            Alert("User", "Something went wrong.<br/>", "Ok");
        },
        error: function (result) {
            Alert("User", "Something went wrong.<br/>", "Ok");
        },
        complete: function () {
            HideGlobalLodingPanel();
        }
    });
}

function AddNewUser() {
    var id = $('[id$=hdnUserId]').val();
    var obj = {
        UserId: id,
        FirstName: $('[id$=UserFirstName]').val().trim(),
        LastName: $('[id$=UserLastName]').val().trim(),
        Email: $('[id$=UserEmailId]').val().trim(),
        Mobile: $('[id$=UserMobileNo]').val().trim(),
        Status: $('[id$=UserSelectActiveInactive]').val().trim(),
        UserImage: UserImage,
        RoleId: $('[id$=UserRoleId]').val()
    };

    $.ajax({
        beforeSend: function () {
            ShowGlobalLodingPanel();
        },
        url: "/api/ManageAccess/AddUser/" + myWTOAPP.UserId,
        async: false,
        type: "POST",
        data: JSON.stringify(obj),
        contentType: "application/json; charset=utf-8",
        success: function (result) {
            if (result) {
                if (id > 0)
                    Alert("User", "User details has been updated successfully.<br/>", "Ok");
                else
                    Alert("User", "User has been added successfully.<br/>", "Ok");
            }
            else
                Alert("User", "Something went wrong. Please try again.<br/>", "Ok");
        },
        failure: function (result) {
            Alert("User", "Something went wrong.<br/>", "Ok");
        },
        error: function (result) {
            Alert("User", "Something went wrong.<br/>", "Ok");
        },
        complete: function () {
            $("#AddUser").modal("hide");
            HideGlobalLodingPanel();
            setTimeout(function () {
                $("#Section1").load('ManageAccess/GetUserList');
            }, 300);
        }
    });
}

function DeleteUser(id) {
    ShowGlobalLodingPanel();
    $.ajax({
        url: "/api/ManageAccess/DeleteUser?Id=" + id + "",
        async: false,
        type: "POST",
        data: JSON,
        contentType: "application/json; charset=utf-8",
        success: function (result) {
            if (result)
                Alert("User", "User has been deleted successfully.<br/>", "Ok");
            else
                Alert("User", "Something went wrong. Please try again.<br/>", "Ok");

            $("#Section1").load('ManageAccess/GetUserList');
        },
        failure: function (result) {
            Alert("User", "Something went wrong.<br/>", "Ok");
        },
        error: function (result) {
            Alert("User", "Something went wrong.<br/>", "Ok");
        },
        complete: function () {
            HideGlobalLodingPanel();
        }
    });
}
/******************************User Section End****************/

/******************************Country Section Start****************/
function CountryTabClick() {
    $("#Section2").load('ManageAccess/GetCountryList');
}

function CountryValidation() {
    var msg = "";

    if ($('[id$=CountryCodeId]').val().trim().length == 0) {
        $('[id$=CountryCodeId]').addClass("error");
        msg += "Please enter country code.<br/>";
    }
    else {
        $('[id$=CountryCodeId]').removeClass("error");
    }

    if ($('[id$=CountryName]').val().trim().length == 0) {
        $('[id$=CountryName]').addClass("error");
        msg += "Please enter country name.<br/>";
    }
    else {
        $('[id$=CountryName]').removeClass("error");
    }

    if ($('[id$=CountryStatus]').val() == -1) {
        $('[id$=CountryStatus]').addClass("error");
        msg += "Please select status.<br/>";
    }
    else {
        $('[id$=CountryStatus]').removeClass("error");
    }

    if (msg.length > 0) {
        Alert("Country", msg, "Ok");
    }
    else
        AddNewCountry();
}

function AddCountryPopup() {
    $("#CountrySaveUpdate").text('Save');
    $("#AddCountrypopupHead").text('Add Country');
    $('[id$=CountryName]').val("");
    $('[id$=CountryStatus]').val("1");
    $('[id$=CountryCodeId]').val('');

    $('.error').removeClass('error');
}

function AddNewCountry() {
    ShowGlobalLodingPanel();
    var id = $('[id$=hdnCountryId]').val();
    var obj = {
        CountryId: id,
        CountryName: $('[id$=CountryName]').val().trim(),
        Status: $('[id$=CountryStatus]').val().trim(),
        CountryCode: $('[id$=CountryCodeId]').val()
    };
    $.ajax({
        url: "/api/ManageAccess/AddCountry/" + myWTOAPP.UserId,
        async: false,
        type: "POST",
        data: JSON.stringify(obj),
        contentType: "application/json; charset=utf-8",
        success: function (result) {
            if (result) {
                if (id > 0) {
                    Alert("Country", "Country has been updated successfully.<br/>", "Ok");
                }
                else {
                    Alert("Country", "Country has been saved successfully.<br/>", "Ok");
                }
            }
            else
                Alert("Country", "Something went wrong. Please try again.<br/>", "Ok");
        },
        failure: function (result) {
            Alert("Country", "Something went wrong.<br/>", "Ok");
        },
        error: function (result) {
            Alert("Country", "Something went wrong.<br/>", "Ok");
        },
        complete: function () {
            $("#AddCountry").modal("hide");
            HideGlobalLodingPanel();
            setTimeout(function () {
                $("#Section2").load('ManageAccess/GetCountryList');
            }, 300);
        }
    });
}

function EditCountryData(id) {
    ShowGlobalLodingPanel();
    $("#AddCountry").modal("show");
    $("#AddCountrypopupHead").text('Update Country');
    $("#CountrySaveUpdate").text('Update');
    $.ajax({
        url: "/api/ManageAccess/GetCountryDetails?Id=" + id + "",
        async: false,
        type: "POST",
        data: JSON,
        contentType: "application/json; charset=utf-8",
        success: function (result) {
            $('[id$=hdnCountryId]').val(result.CountryId);
            $('[id$=CountryName]').val(result.CountryName);
            $('[id$=CountryStatus]').val(result.Status);
            $('[id$=CountryCodeId]').val(result.CountryCode);
        },
        failure: function (result) {
            Alert("Country", "Something went wrong.<br/>", "Ok");
        },
        error: function (result) {
            Alert("Country", "Something went wrong.<br/>", "Ok");
        },
        complete: function () {
            HideGlobalLodingPanel();
        }
    });
}

function DeleteCountry(id) {
    ShowGlobalLodingPanel();
    $(this).prev('span.text').remove();
    $.ajax({
        url: "/api/ManageAccess/DeleteCountry?Id=" + id + "",
        async: false,
        type: "POST",
        data: JSON,
        contentType: "application/json; charset=utf-8",
        success: function (result) {
            if (result) {
                Alert("Country", "Country has been deleted successfully.<br/>", "Ok");
            }
            else
                Alert("Country", "Something went wrong.Please try again.<br/>", "Ok");

            $("#Section2").load('ManageAccess/GetCountryList');
        },
        failure: function (result) {
            Alert("Country", "Something went wrong.<br/>", "Ok");
        },
        error: function (result) {
            Alert("Country", "Something went wrong.<br/>", "Ok");
        },
        complete: function () {
            HideGlobalLodingPanel();
        }
    });
}
/******************************Country Section End****************/

/******************************StakeHolder Section Start****************/
function AddStakeHolderPopup() {
    $("#StakeholderSaveUpdate").text('Save');
    $("#AddStackholderpopupHead").text('Add Stakeholder');
    $('[id$=stakeholderFirstName]').val("");
    $('[id$=stakeholderLastName]').val("");
    $('[id$=stackholderEmailId]').val("");
    $('[id$=stackholderMobileNo]').val("");
    $('[id$=stakeholderOrganization]').val("");
    $('[id$=stakeholderselectstatus]').val("1");
    $('[id$=stackholderAddress]').val("");
    $('[id$=stackholderState]').val("");
    $('[id$=stackholderCity]').val("");
    $('[id$=stackholderPIN]').val("");
    $('[id$=SelectedHscode]').text("");
    $('[id$=hdnstakeholderHscode]').val("");
    $('[id$=hdnStakeHolderId]').val('0');
    $('.error').removeClass('error');
}

function stakeholdervalidate() {
    var msg = "";

    if ($('[id$=stakeholderFirstName]').val().trim().length == 0) {
        $('[id$=stakeholderFirstName]').addClass("error");
        msg += "Please enter first name.<br/>";
    }
    else
        $('[id$=stakeholderFirstName]').removeClass("error");

    if ($('[id$=stakeholderLastName]').val().trim().length == 0) {
        $('[id$=stakeholderLastName]').addClass("error");
        msg += "Please enter last name.<br/>";
    }
    else
        $('[id$=stakeholderLastName]').removeClass("error");

    if ($('[id$=stackholderEmailId]').val().trim().length > 0) {
        if (!validateEmail($('[id$=stackholderEmailId]').val().trim())) {
            $('[id$=stackholderEmailId]').addClass("error");
            msg += "Please enter valid email-id.<br/>";
        }
        else {
            var IsEmailExist = IsEmailExists($('[id$=stackholderEmailId]').val().trim(), "Stakeholder");
            if (IsEmailExist && $('[id$=hdnStakeHolderId]').val() == 0) {
                msg += "A Stakeholder already exists with same email-id.<br/>";
                $('[id$=stackholderEmailId]').addClass("error");
            }
            else
                $('[id$=stackholderEmailId]').removeClass("error");
        }
    }
    else {
        $('[id$=stackholderEmailId]').addClass("error");
        msg += "Please enter email-id.<br/>";
    }

    if ($('[id$=stackholderMobileNo]').val().trim().length > 0) {
        if (!IsMobileNumberReg($('[id$=stackholderMobileNo]').val().trim())) {
            $('[id$=stackholderMobileNo]').addClass("error");
            msg += "Please enter valid mobile number.<br/>";
        }
        else
            $('[id$=stackholderMobileNo]').removeClass("error");
    }
    else {
        $('[id$=stackholderMobileNo]').addClass("error");
        msg += "Please enter mobile number.<br/>";
    }

    if ($('[id$=stakeholderOrganization]').val().trim().length == 0) {
        $('[id$=stakeholderOrganization]').addClass("error");
        msg += "Please enter organization name.<br/>";
    }
    else
        $('[id$=stakeholderOrganization]').removeClass("error");

    if ($('[id$=stakeholderselectstatus]').val() == -1) {
        $('[id$=stakeholderselectstatus]').addClass("error");
        msg += "Please select status.<br/>";
    }
    else
        $('[id$=stakeholderselectstatus]').removeClass("error");

    if ($('[id$=stackholderAddress]').val().trim().length == 0) {
        $('[id$=stackholderAddress]').addClass("error");
        msg += "Please enter address.<br/>";
    }
    else
        $('[id$=stackholderAddress]').removeClass("error");

    if ($('[id$=stackholderState]').val().trim().length == 0) {
        $('[id$=stackholderState]').addClass("error");
        msg += "Please enter state.<br/>";
    }
    else
        $('[id$=stackholderState]').removeClass("error");

    if ($('[id$=stackholderCity]').val().trim().length == 0) {
        $('[id$=stackholderCity]').addClass("error");
        msg += "Please enter city.<br/>";
    }
    else
        $('[id$=stackholderCity]').removeClass("error");

    if ($('[id$=stackholderPIN]').val().trim().length == 0) {
        $('[id$=stackholderPIN]').addClass("error");
        msg += "Please enter PIN code.<br/>";
    }
    else {
        $('[id$=stackholderPIN]').removeClass("error");
    }
    if ($('[id$=SelectedHscode]').text().trim().length == 0) {
        msg += "Please select atleast one HS code.<br/>";
    }

    if (msg.length > 0) {
        Alert("Stakeholder", msg, "Ok");
    }
    else
        AddNewStackholder();
}

function AddNewStackholder() {
    ShowGlobalLodingPanel();
    var id = $('[id$=hdnStakeHolderId]').val();
    var StakeHsCode = $('[id$=hdnstakeholderHscode]').val();
    var obj = {
        StakeHolderId: id,
        FirstName: $('[id$=stakeholderFirstName]').val().trim(),
        LastName: $('[id$=stakeholderLastName]').val().trim(),
        Email: $('[id$=stackholderEmailId]').val().trim(),
        Mobile: $('[id$=stackholderMobileNo]').val().trim(),
        OrgName: $('[id$=stakeholderOrganization]').val().trim(),
        Status: $('[id$=stakeholderselectstatus]').val().trim(),
        Address: $('[id$=stackholderAddress]').val().trim(),
        State: $('[id$=stackholderState]').val().trim(),
        City: $('[id$=stackholderCity]').val().trim(),
        PIN: $('[id$=stackholderPIN]').val().trim(),
        HSCodes: StakeHsCode
    };
    $.ajax({
        url: "/api/ManageAccess/AddStakeHolder/" + myWTOAPP.UserId,
        async: false,
        type: "POST",
        data: JSON.stringify(obj),
        contentType: "application/json; charset=utf-8",
        success: function (result) {
            if (result) {
                if (id > 0)
                    Alert("Stakeholder", "Stakeholder has been updated successfully.<br\>", "Ok");
                else
                    Alert("Stakeholder", "Stakeholder has been saved successfully.<br\>", "Ok");
            }
            else
                Alert("Stakeholder", "Something went wrong. Please try again.<br\>", "Ok");
        },
        failure: function (result) {
            Alert("Stakeholder", "Something went wrong.<br\>", "Ok");
        },
        error: function (result) {
            Alert("Stakeholder", "Something went wrong.<br\>", "Ok");
        },
        complete: function () {
            $("#AddStakeholder").modal("hide");
            HideGlobalLodingPanel();
            setTimeout(function () {
                $("#Section3").load('ManageAccess/GetStakeHolderList');
            }, 300);
        }
    });
}

function EditStakeHolderData(id) {
    ShowGlobalLodingPanel();
    $("#AddStakeholder").modal("show");
    $("#AddStackholderpopupHead").text('Update Stakeholder');
    $("#StakeholderSaveUpdate").text('Update');
    $.ajax({
        url: "/api/ManageAccess/GetStakeHolderDetails?Id=" + id + "",
        async: false,
        type: "POST",
        data: JSON,
        contentType: "application/json; charset=utf-8",
        success: function (result) {
            $('[id$=hdnStakeHolderId]').val(result.StakeHolderId);
            $('[id$=stakeholderFirstName]').val(result.FirstName);
            $('[id$=stakeholderLastName]').val(result.LastName);
            $('[id$=stackholderEmailId]').val(result.Email);
            $('[id$=stackholderMobileNo]').val(result.Mobile);
            $('[id$=stakeholderOrganization]').val(result.OrgName);
            $('[id$=stakeholderselectstatus]').val(result.Status);
            $('[id$=stackholderAddress]').val(result.Address);
            $('[id$=stackholderState]').val(result.State);
            $('[id$=stackholderCity]').val(result.City);
            $('[id$=stackholderPIN]').val(result.PIN);
            $('[id$=hdnstakeholderHscode]').val(result.HSCodes);
            $('[id$=SelectedHscode]').text($('[id$=hdnstakeholderHscode]').val());
        },
        failure: function (result) {
            Alert("Stakeholder", "Something went wrong.<br\>", "Ok");
        },
        error: function (result) {
            Alert("Stakeholder", "Something went wrong.<br\>", "Ok");
        },
        complete: function () {
            HideGlobalLodingPanel();
        }
    });
}

function DeleteStakeHolder(id) {
    ShowGlobalLodingPanel();
    $(this).prev('span.text').remove();
    $.ajax({
        url: "/api/ManageAccess/DeleteStakeHolder?Id=" + id + "",
        async: false,
        type: "POST",
        data: JSON,
        contentType: "application/json; charset=utf-8",
        success: function (result) {
            if (result)
                Alert("Stakeholder", "Stakeholder has been deleted successfully.<br\>", "Ok");
            else
                Alert("Stakeholder", "Something went wrong. Please try again.<br\>", "Ok");

            $("#Section3").load('ManageAccess/GetStakeHolderList');
        },
        failure: function (result) {
            Alert("Stakeholder", "Something went wrong.<br\>", "Ok");
        },
        error: function (result) {
            Alert("Stakeholder", "Something went wrong.<br\>", "Ok");
        },
        complete: function () {
            HideGlobalLodingPanel();
        }
    });
}

function StakeHolderTabClick() {
    $("#Section3").load('ManageAccess/GetStakeHolderList');
}
/******************************StakeHolder Section End****************/

/******************************Translator Section Start****************/
function TranslatorTabClick() {
    $("#Section4").load('ManageAccess/GetTranslatorList');
}

function BindLanguages() {
    $.ajax({
        url: "/api/Masters/GetLanguages",
        async: false,
        type: "GET",
        contentType: "application/json; charset=utf-8",
        success: function (result) {
            if ($('[Id$=ddlLanguage]').length > 0) {
                $('[Id$=ddlLanguage]').multiselect('destroy');
                $('[Id$=ddlLanguage]').empty();
                if (result.length > 0) {
                    $.each(result, function (i, v) {
                        $('[Id$=ddlLanguage]').append('<option value="' + v.LanguageId + '"> ' + v.LanguageName + ' </option>');
                    });

                    $('[id$=ddlLanguage]').val($('[id$=ddlLanguage]').attr("data-SearchFor"));
                }
            }
        },
        failure: function (result) {
            Alert("Translator", "Something went wrong.<br\>", "Ok");
        },
        error: function (result) {
            Alert("Translator", "Something went wrong.<br\>", "Ok");
        },
        complete: function () {
            $('[Id$=ddlLanguage]').multiselect({
                maxHeight: 150,
                enableFiltering: true,
                nonSelectedText: '--Select Languages--'
            });
        }
    });
}

function AddTranslatorPopup() {
    $("#TranslatorSaveUpdate").text('Save');
    $("#AddTranslatorpopupHead").text('Add Translator');
    $('[id$=TranslatorFirstName]').val("");
    $('[id$=TranslatorLastName]').val("");
    $('[id$=TranslatorEmailid]').val("");
    $('[id$=TranslatorMobileNo]').val("");
    $('[id$=TranslatorStatus]').val("1");
    $('[id$=hdnTranslatorId]').val('0');
    $('[id$=divSendWelcomeMail]').show();
    $('[id$=ddlLanguage]').val('').multiselect('refresh');

    $('.error').removeClass('error');
}

function TranslatorValidate() {
    var msg = "";

    if ($('[id$=TranslatorFirstName]').val().trim().length == 0) {
        $('[id$=TranslatorFirstName]').addClass("error");
        msg += "Please enter first name.<br/>";
    }
    else
        $('[id$=TranslatorFirstName]').removeClass("error");

    if ($('[id$=TranslatorLastName]').val().trim().length == 0) {
        $('[id$=TranslatorLastName]').addClass("error");
        msg += "Please enter last name.<br/>";
    }
    else
        $('[id$=TranslatorLastName]').removeClass("error");

    if ($('[id$=TranslatorEmailid]').val().trim().length > 0) {
        if (!validateEmail($('[id$=TranslatorEmailid]').val().trim())) {
            $('[id$=TranslatorEmailid]').addClass("error");
            msg += "Please enter valid email-id.<br/>";
        }
        else {
            var IsEmailExist = IsEmailExists($('[id$=TranslatorEmailid]').val().trim(), "Translator");
            if (IsEmailExist && $('[id$=hdnTranslatorId]').val() == 0) {
                msg += "A translator already exists with the same email-id.<br/>";
                $('[id$=TranslatorEmailid]').addClass("error");
            }
            else
                $('[id$=TranslatorEmailid]').removeClass("error");
        }
    }
    else {
        $('[id$=TranslatorEmailid]').addClass("error");
        msg += "Please enter email-id.<br/>";
    }

    if ($.trim($('[id$=ddlLanguage]').val().toString()) == "") {
        msg += "Please select atleast one language.<br/>";
        $('[id$=ddlLanguage]').next().find('button.btn-block').addClass("error");
    }
    else
        $('[id$=ddlLanguage]').next().find('button.btn-block').removeClass("error");

    if ($('[id$=TranslatorMobileNo]').val().trim().length > 0) {
        if (!IsMobileNumberReg($('[id$=TranslatorMobileNo]').val().trim())) {
            $('[id$=TranslatorMobileNo]').addClass("error");
            msg += "Please enter the valid mobile number.<br/>";
        }
        else
            $('[id$=TranslatorMobileNo]').removeClass("error");
    }
    else {
        $('[id$=TranslatorMobileNo]').addClass("error");
        msg += "Please enter mobile number.<br/>";
    }

    if ($('[id$=TranslatorStatus]').val() == -1) {
        $('[id$=TranslatorStatus]').addClass("error");
        msg += "Please select status.<br/>";
    }
    else
        $('[id$=TranslatorStatus]').removeClass("error");

    if (msg.length > 0) {
        Alert("Translator", msg, "Ok");
    }
    else
        AddNewTranslator();
}

function AddNewTranslator() {
    ShowGlobalLodingPanel();
    var id = $('[id$=hdnTranslatorId]').val();
    var obj = {
        TranslatorId: id,
        FirstName: $('[id$=TranslatorFirstName]').val().trim(),
        LastName: $('[id$=TranslatorLastName]').val().trim(),
        Email: $('[id$=TranslatorEmailid]').val().trim(),
        Mobile: $('[id$=TranslatorMobileNo]').val().trim(),
        Status: $('[id$=TranslatorStatus]').val().trim(),
        LanguageIds: $('[id$=ddlLanguage]').val().toString().trim(),
        IsWelcomeMailSent: $('[id$=IsSendWelcomeMailId]').is(':Checked')
    };
    $.ajax({
        url: "/api/ManageAccess/AddTranslator/" + myWTOAPP.UserId,
        async: false,
        type: "POST",
        data: JSON.stringify(obj),
        contentType: "application/json; charset=utf-8",
        success: function (result) {
            if (result > 0) {
                if (id > 0)
                    Alert("Translator", "Translator has been updated successfully.<br\>", "Ok");
                else {
                    Alert("Translator", "Translator has been saved successfully.<br\>", "Ok");
                    if (obj.IsWelcomeMailSent) {
                        $.ajax({
                            url: "/api/ManageAccess/SendWelcomeMailToTranslator/" + result,
                            async: false,
                            type: "POST",
                            contentType: "application/json; charset=utf-8"
                        });
                    }
                }
            }
            else
                Alert("Translator", "Something went wrong. Please try again.<br\>", "Ok");
        },
        failure: function (result) {
            Alert("Translator", "Something went wrong..<br\>", "Ok");
        },
        error: function (result) {
            Alert("Translator", "Something went wrong..<br\>", "Ok");
        },
        complete: function () {
            $("#AddTranslator").modal("hide");
            HideGlobalLodingPanel();
            setTimeout(function () {
                $("#Section4").load('ManageAccess/GetTranslatorList');
            }, 300);
        }
    });
}

function EditTranslatorData(id) {
    ShowGlobalLodingPanel();
    $("#AddTranslator").modal("show");
    $("#AddTranslatorpopupHead").text('Update Translator');
    $("#TranslatorSaveUpdate").text('Update');
    $.ajax({
        url: "/api/ManageAccess/GetTranslatorDetails?Id=" + id + "",
        async: false,
        type: "POST",
        data: JSON,
        contentType: "application/json; charset=utf-8",
        success: function (result) {
            $('[id$=hdnTranslatorId]').val(result.TranslatorId);
            $('[id$=TranslatorFirstName]').val(result.FirstName);
            $('[id$=TranslatorLastName]').val(result.LastName);
            $('[id$=TranslatorEmailid]').val(result.Email);
            $('[id$=TranslatorMobileNo]').val(result.Mobile);
            $('[id$=TranslatorStatus]').val(result.Status);
            if (result.LanguageIds != null) {
                var _LanguageIds = new Array;
                $.each(result.LanguageIds.split(','), function (i, v) {
                    if (v !== "")
                        _LanguageIds.push(v);
                });
                $('[id$=ddlLanguage]').val(_LanguageIds).multiselect('refresh');
            }
            else
                $('[id$=ddlLanguage]').val('').multiselect('refresh');

            $('[id$=divSendWelcomeMail]').hide();
        },
        failure: function (result) {
            Alert("Translator", "Something went wrong..<br\>", "Ok");
        },
        error: function (result) {
            Alert("Translator", "Something went wrong..<br\>", "Ok");
        },
        complete: function () {
            HideGlobalLodingPanel();
        }
    });
}

function DeleteTranslator(id) {
    $.ajax({
        url: "/api/ManageAccess/DeleteTranslator?Id=" + id + "",
        async: false,
        type: "POST",
        data: JSON,
        contentType: "application/json; charset=utf-8",
        success: function (result) {
            if (result) {
                Alert("Translator", "Translator has been deleted successfully.<br\>", "Ok");
            }
            else
                Alert("Translator", "Something went wrong. Please try again.<br\>", "Ok");

            $("#Section4").load('ManageAccess/GetTranslatorList');
        },
        failure: function (result) {
            Alert("Translator", "Something went wrong.<br\>", "Ok");
        },
        error: function (result) {
            Alert("Translator", "Something went wrong.<br\>", "Ok");
        },
        complete: function () {
            HideGlobalLodingPanel();
        }
    });
}

function SendWelcomeMail(_Id) {
    $.ajax({
        url: "/api/ManageAccess/SendWelcomeMailToTranslator/" + _Id,
        async: false,
        type: "POST",
        contentType: "application/json; charset=utf-8",
        success: function (result) {
            Alert("Send Welcome Mail to translator", "A welcome mail has been sent successfully to the translator.", "Ok");
            $("#Section4").load('ManageAccess/GetTranslatorList');
        },
        failure: function (result) {
            Alert("Translator", "Something went wrong.<br\>", "Ok");
        },
        error: function (result) {
            Alert("Translator", "Something went wrong.<br\>", "Ok");
        },
        complete: function () {
            HideGlobalLodingPanel();
        }
    });
}
/******************************Translator Section End****************/

/******************************HS Code Section Start****************/
function GetHSCode() {
    ShowGlobalLodingPanel();
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
                "search": {
                    "case_sensitive": false,
                    "show_only_matches": true
                },
                "plugins": ["search"]
            });
        },
        failure: function (result) {
            Alert("HS Code", "Something went wrong.<br\>", "Ok");
        },
        error: function (result) {
            Alert("HS Code", "Something went wrong.<br\>", "Ok");
        },
        complete: function () {
            HideGlobalLodingPanel();
        }
    });
}

function SearchHSCode() {
    var searchString = $("#HSCodeSearchTxt").val();
    $('#HSCodeTree').jstree('search', searchString);
}

function clearSearchtxt() {
    document.getElementById('HSCodeSearchTxt').value = '';
    $('#searchhscodebtn').click();
}
/******************************HS Code Section End****************/

/******************************Stakeholder HS Code Section Start****************/
function selectstakeholderHscode() {
    $("#AddStakeholder").modal("hide");
    $("#selectstakeholderHscode").modal("show");
    document.getElementById('HSCodeSearchTxt').value = '';
    var numbersArray = $('[id$=SelectedHscode]').text().trim().split(',');
    $('#HSCodeTree').jstree(true).select_node(numbersArray);
}

function openAddStakeholder() {
    $("#selectstakeholderHscode").modal("hide");
    $("#selectstakeholderHscode").on('hidden.bs.modal', function () {
        $("#AddStakeholder").modal("show");
    });
}

function GetStackholderHSCode() {
    ShowGlobalLodingPanel();
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
            $('.modal - backdrop').hide();
        },
        failure: function (result) {
            Alert("Stakeholder", "Something went wrong.<br\>", "Ok");
        },
        error: function (result) {
            Alert("Stakeholder", "Something went wrong.<br\>", "Ok");
        },
        complete: function () {
            HideGlobalLodingPanel();
        }
    });
}

function SelectHSCode() {
    //var seen = {};
    var HSCodeId = [];
    var LengthTwo = [];
    var LengthFour = [];
    var TotalIdArray = [];
    var FinalArray = [];
    var index = [];
    var index2 = [];
    var i, j, r = [];
    var row = '';
    if ($('.jstree-anchor.jstree-clicked').length == 0) {
        Alert("HS Code", "Please select atleast one HSCode.<br\>", "Ok");
        return false;
    }
    else {
        $.each($('.jstree-anchor.jstree-clicked'), function (i, nodeId) {
            HSCodeId.push(($('#HSCodeTree').jstree(true).get_selected()));
        });

        $.each(HSCodeId, function (i, nodeId) {
            for (var x = 0; x < nodeId.length; x++) {
                var id = $('#HSCodeTree').jstree(true).get_selected(true)[x].id;
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
        $.each(FinalArray, function (i, nodeId) {
            items += nodeId + ',';
        });

        $("#hdnstakeholderHscode").val(items);
        $("#SelectedHscode").text($("#hdnstakeholderHscode").val());
    }
    openAddStakeholder();
}
/******************************Stakeholder HS Code Section End****************/

/******************************Translator Section Start****************/
function TemplateTabClick() {
    $("#Section6").load('ManageAccess/GetTemplateList');
}

function AddTemplatePopup() {
    $("#TemplateSaveUpdate").text('Save');
    $("#AddTemplatepopupHead").text('Add Template');
    $('[id$=ddlTemplateType]').val("");
    $('[id$=ddlTemplateFor]').val("");
    $('[id$=txtSubject]').val("");
    $('[id$=txtMessage]').val("");
    $('[id$=ddlTemplateStatus]').val("1");
    $('[id$=hdnTemplateId]').val('0');

    $('.error').removeClass('error');
}

function TemplateValidate() {
    var msg = "";

    if ($.trim($('[id$=ddlTemplateType]').val().toString()) == "") {
        msg += "Please select template type.<br/>";
        $('[id$=ddlTemplateType]').addClass("error");
    }
    else
        $('[id$=ddlTemplateType]').removeClass("error");

    if ($.trim($('[id$=ddlTemplateFor]').val().toString()) == "") {
        msg += "Please select template for.<br/>";
        $('[id$=ddlTemplateFor]').addClass("error");
    }
    else
        $('[id$=ddlTemplateFor]').removeClass("error");

    if ($('[id$=txtSubject]').val().trim() == "" && $.trim($('[id$=ddlTemplateType]').val()) != "SMS") {
        $('[id$=txtSubject]').addClass("error");
        msg += "Please enter mail subject.<br/>";
    }
    else
        $('[id$=txtSubject]').removeClass("error");

    var MessageBody = CKEDITOR.instances.txtMessage.getData();
    if (MessageBody.trim() == "") {
        $('[id$=txtMessage]').addClass("error");
        msg += "Please enter message.<br/>";
    }
    else
        $('[id$=txtMessage]').removeClass("error");

    if ($('[id$=ddlTemplateStatus]').val().trim() == "") {
        $('[id$=ddlTemplateStatus]').addClass("error");
        msg += "Please select status.<br/>";
    } else
        $('[id$=ddlTemplateStatus]').removeClass("error");

    if (msg.length > 0) {
        Alert("Translator", msg, "Ok");
    }
    else
        AddUpdateTemplate();
}

function AddUpdateTemplate() {
    ShowGlobalLodingPanel();
    var id = $('[id$=hdnTemplateId]').val();
    var obj = {
        TemplateId: id,
        TemplateFor: $.trim($('[id$=ddlTemplateFor]').val()),
        TemplateType: $.trim($('[id$=ddlTemplateFor]').val()).toLowerCase() == 'sms' ? '' : $.trim($('[id$=ddlTemplateType]').val()),
        TemplateStatus: $.trim($('[id$=ddlTemplateStatus]').val()),
        Subject: $.trim($('[id$=txtSubject]').val()),
        Body: $.trim(CKEDITOR.instances.txtMessage.getData()),
    };
    $.ajax({
        url: "/api/ManageAccess/AddUpdateTemplate/" + myWTOAPP.UserId,
        async: false,
        type: "POST",
        data: JSON.stringify(obj),
        contentType: "application/json; charset=utf-8",
        success: function (result) {
            Alert("Template", result.Message + "<br\>", "Ok");
        },
        failure: function (result) {
            Alert("Template", "Something went wrong..<br\>", "Ok");
        },
        error: function (result) {
            Alert("Template", "Something went wrong..<br\>", "Ok");
        },
        complete: function () {
            $("#AddTemplate").modal("hide");
            HideGlobalLodingPanel();
            setTimeout(function () {
                $("#Section4").load('ManageAccess/GetTemplateList');
            }, 300);
        }
    });
}

function EditTemplateData(id) {
    ShowGlobalLodingPanel();
    $("#AddTemplate").modal("show");
    $("#AddTemplatepopupHead").text('Update Template');
    $("#TemplateSaveUpdate").text('Update');
    $.ajax({
        url: "/api/ManageAccess/GetTemplateDetails?Id=" + id + "",
        async: false,
        type: "POST",
        data: JSON,
        contentType: "application/json; charset=utf-8",
        success: function (result) {
            $('[id$=hdnTemplateId]').val(result.TemplateId);
            $('[id$=ddlTemplateType]').val(result.TemplateType);
            $('[id$=ddlTemplateFor]').val(result.TemplateFor);
            $('[id$=txtSubject]').val(result.Subject);
            CKEDITOR.instances.txtMessage.setData(result.Body);
            $('[id$=ddlTemplateStatus]').val(result.TemplateStatus.toString());
        },
        failure: function (result) {
            Alert("Template", "Something went wrong.<br\>", "Ok");
        },
        error: function (result) {
            Alert("Template", "Something went wrong.<br\>", "Ok");
        },
        complete: function () {
            HideGlobalLodingPanel();
        }
    });
}

function DeleteTemplate(id) {
    $.ajax({
        url: "/api/ManageAccess/DeleteTemplate/" + id,
        async: false,
        type: "POST",
        data: JSON,
        contentType: "application/json; charset=utf-8",
        success: function (result) {
            if (result) {
                Alert("Translator", "Translator has been deleted successfully.<br\>", "Ok");
            }
            else
                Alert("Template", "Something went wrong. Please try again.<br\>", "Ok");

            $("#Section4").load('ManageAccess/GetTemplateList');
        },
        failure: function (result) {
            Alert("Template", "Something went wrong.<br\>", "Ok");
        },
        error: function (result) {
            Alert("Template", "Something went wrong.<br\>", "Ok");
        },
        complete: function () {
            HideGlobalLodingPanel();
        }
    });
}

function AddFields(ctrlName) {
    $.ajax({
        url: "/api/ManageAccess/GetTemplateFields",
        async: false,
        type: "GET",
        data: JSON,
        contentType: "application/json; charset=utf-8",
        success: function (result) {
            debugger;
            $('#divFields').empty();
            if (result.length) {
                var html = '';
                $.each(result, function (i, v) {
                    html += '<div class="insertfielddiv">' +
                                '<div class="checkbox radio-margin" style="margin-top: 2px;float: left;">' +
                                    '<label>' +
                                        '<input type="checkbox" onchange="AddRemoveField(this);" value="' + v.Value + '">' +
                                        '<span class="cr insertcheckbox"><i class="cr-icon glyphicon glyphicon-ok"></i></span>' +
                                        v.Text +
                                    '</label>' +
                                '</div>' +
                            '</div>';
                });

                $('#divFields').append(html);

                $('[id$=divAddFieldsOverlay]').show();
                $('[id$=divAddFields]').show();
                $('[id$=hdnTemplateFieldsFor]').val(ctrlName);
            }
        },
        failure: function (result) {
            Alert("Template", "Something went wrong.<br\>", "Ok");
        },
        error: function (result) {
            Alert("Template", "Something went wrong.<br\>", "Ok");
        },
        complete: function () {
            HideGlobalLodingPanel();
        }
    });
    return false;
}

var MessageFields = [];
//close popup of add other attachments in Stakholders mail popup
function CloseAddField() {
    $('[id$=divAddFieldsOverlay]').hide();
    $('[id$=divAddFields]').hide();
    MessageFields = [];
    $.each($('[id$=tblFields]').find('input[type=checkbox]'), function () {
        this.checked = false;
    });
}

//ok function of add other attachments in Stakholders mail popup
function AddFieldOk() {
    var CntrlId = $('[id$=hdnTemplateFieldsFor]').val();
    var Fields = '';
    var IsMessageFieldSelected = false;
    $.each(MessageFields, function (i, v) {
        Fields += '#' + v + '#  ';
    });

    if (CntrlId.toLowerCase() == 'txtmessage')
        CKEDITOR.instances[CntrlId].insertText(Fields);
    else
        insertAtCaret(CntrlId, Fields);

    CloseAddField();
}

//select/Unselect other attachments in Stakholders mail popup
function AddRemoveField(ctrl) {
    var FieldValue = $(ctrl).val();
    if (ctrl.checked) {
        MessageFields.push(FieldValue);
    }
    else {
        var Index = -1;
        $.each(MessageFields, function (index, value) {
            if (value == FieldValue)
                Index = index;
        });

        if (Index > -1)
            MessageFields.splice(Index, 1);
    }
}

function insertAtCaret(areaId, text) {
    var txtarea = document.getElementById(areaId);
    if (!txtarea) {
        return;
    }

    var scrollPos = txtarea.scrollTop;
    var strPos = 0;
    var br = ((txtarea.selectionStart || txtarea.selectionStart == '0') ?
      "ff" : (document.selection ? "ie" : false));
    if (br == "ie") {
        txtarea.focus();
        var range = document.selection.createRange();
        range.moveStart('character', -txtarea.value.length);
        strPos = range.text.length;
    } else if (br == "ff") {
        strPos = txtarea.selectionStart;
    }

    var front = (txtarea.value).substring(0, strPos);
    var back = (txtarea.value).substring(strPos, txtarea.value.length);
    txtarea.value = front + text + back;
    strPos = strPos + text.length;
    if (br == "ie") {
        txtarea.focus();
        var ieRange = document.selection.createRange();
        ieRange.moveStart('character', -txtarea.value.length);
        ieRange.moveStart('character', strPos);
        ieRange.moveEnd('character', 0);
        ieRange.select();
    } else if (br == "ff") {
        txtarea.selectionStart = strPos;
        txtarea.selectionEnd = strPos;
        txtarea.focus();
    }

    txtarea.scrollTop = scrollPos;
}

function ShowHideSubject(ctrl) {
    if ($(ctrl).val().toLowerCase() == "sms")
        $('[id$=divSubject]').addClass('hidden');
    else
        $('[id$=divSubject]').removeClass('hidden');
}
/******************************Translator Section End****************/