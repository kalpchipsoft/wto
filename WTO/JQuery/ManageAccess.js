var UserImage = [];
$(document).ready(function () {
    GetHSCode();
});

/******************************User Section Start****************/
function setNoImage(ctrl) {
    $(ctrl).attr('src', '../contents/img/NoImage.png');
}

function ChangeFile(ctrl) {
    var totfilesize = 0;
    if ($(ctrl)[0].files.length != 0) {
        var fileToLoad = $(ctrl)[0].files[0];
        var ext = $(ctrl)[0].files[0].name.split(".")[$(ctrl)[0].files[0].name.split(".").length - 1].toLowerCase();
        ext = ext.toLowerCase();
        $.each($(ctrl)[0].files, function (index, value) {
            totfilesize += value.size;
        });

        if (totfilesize > 10485760) {
            Alert("Alert", "Total attachment files size should not be greater than 10 MB.<br/>", "Ok");
            $("#Loader").hide();
            UserImage = [];
            $('[id$=ImgPhotograph]').attr('src', '../contents/img/Add-Image.jpg');
            $('[id$=lnkRemoveImage]').hide();
            return false;
        }
        else if (ext != "png" && ext != "jpg" && ext != "jpeg") {
            Alert("Alert", "You can upload only jpg,jpeg and png files.<br/>", "Ok");
            $(ctrl).val('');
            $("#Loader").hide();
            UserImage = [];
            $('[id$=ImgPhotograph]').attr('src', '../contents/img/Add-Image.jpg');
            $('[id$=lnkRemoveImage]').hide();
            return false;
        }
        else {
            $.each($(ctrl)[0].files, function (index, value) {
                var reader = new FileReader();
                reader.onload = function (e) {
                    UserImage = { "FileName": value.name, "Content": e.target.result, "Path": "" };
                    $('[id$=ImgPhotograph]').attr('src', '');
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
}

function UploadImage() {
    $('#fileUploadId').click();
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
        Alert("Alert", msg, "Ok");
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
                    Alert("Alert", "User details has been updated successfully.<br/>", "Ok");
                else
                    Alert("Alert", "User has been added successfully.<br/>", "Ok");
            }
            else
                Alert("Alert", "Something went wrong. Please try again.<br/>", "Ok");
        },
        failure: function (result) {
            Alert("Alert", "Something went wrong.<br/>", "Ok");
        },
        error: function (result) {
            Alert("Alert", "Something went wrong.<br/>", "Ok");
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
                Alert("Alert", "User has been deleted successfully.<br/>", "Ok");
            else
                Alert("Alert", "Something went wrong. Please try again.<br/>", "Ok");

            $("#Section1").load('ManageAccess/GetUserList');
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

    if ($('[id$=CountryStatus]').val().trim() == "") {
        $('[id$=CountryStatus]').addClass("error");
        msg += "Please select status.<br/>";
    }
    else {
        $('[id$=CountryStatus]').removeClass("error");
    }

    if ($('[id$=txtSPSEnq]').val() != '') {
        var Emails = $('[id$=txtSPSEnq]').val().split(';');
        var IsValid = true;
        $.each(Emails, function (i, v) {
            if (!validateEmail(v))
                IsValid = false;
        });

        if (!IsValid) {
            $('[id$=txtSPSEnq]').addClass("error");
            msg += "Please enter valid SPS enquiry point email.<br/>";
        }
        else
            $('[id$=txtSPSEnq]').removeClass("error");
    }

    if ($('[id$=txtTBTEnq]').val() != '') {
        var Emails = $('[id$=txtTBTEnq]').val().split(';');
        var IsValid = true;
        $.each(Emails, function (i, v) {
            if (!validateEmail(v))
                IsValid = false;
        });

        if (!IsValid) {
            $('[id$=txtTBTEnq]').addClass("error");
            msg += "Please enter valid TBT enquiry point email.<br/>";
        }
        else
            $('[id$=txtTBTEnq]').removeClass("error");
    }

    if (msg.length > 0) {
        Alert("Alert", msg, "Ok");
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
    $('[id$=txtSPSEnq]').val('');
    $('[id$=txtTBTEnq]').val('');
    $('[id$=hdnCountryId]').val('0');
    $('.error').removeClass('error');
}

function AddNewCountry() {
    ShowGlobalLodingPanel();
    var id = $('[id$=hdnCountryId]').val();
    var obj = {
        CountryId: id,
        CountryName: $('[id$=CountryName]').val().trim(),
        Status: $('[id$=CountryStatus]').val().trim(),
        CountryCode: $.trim($('[id$=CountryCodeId]').val()),
        EnquiryEmail_TBT: $.trim($('[id$=txtTBTEnq]').val()),
        EnquiryEmail_SPS: $.trim($('[id$=txtSPSEnq]').val())
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
                    Alert("Alert", "Country has been updated successfully.<br/>", "Ok");
                }
                else {
                    Alert("Alert", "Country has been saved successfully.<br/>", "Ok");
                }
            }
            else
                Alert("Alert", "Something went wrong. Please try again.<br/>", "Ok");
        },
        failure: function (result) {
            Alert("Alert", "Something went wrong.<br/>", "Ok");
        },
        error: function (result) {
            Alert("Alert", "Something went wrong.<br/>", "Ok");
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
            $('[id$=txtSPSEnq]').val(result.EnquiryEmail_SPS);
            $('[id$=txtTBTEnq]').val(result.EnquiryEmail_TBT);
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
                Alert("Alert", "Country has been deleted successfully.<br/>", "Ok");
            }
            else
                Alert("Alert", "Something went wrong.Please try again.<br/>", "Ok");

            $("#Section2").load('ManageAccess/GetCountryList');
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

function CheckDuplicateEntryCodeName(Callfor) {
    var CountryName = "";
    if (Callfor == 'Code') {
        CountryName = $('[id$=CountryCodeId]').val().trim();
    }
    if (Callfor == 'Name') {
        CountryName = $('[id$=CountryName]').val().trim();
    }
    $.ajax({
        url: "/api/ManageAccess/CheckDuplicateCountry?id=" + $('[id$=hdnCountryId]').val() + "&&Callfor=" + Callfor + "&&text=" + CountryName,
        async: false,
        type: "GET",
        contentType: "application/json; charset=utf-8",
        success: function (result) {
            if (result.length > 0) {
                if (Callfor == 'Code') {
                    $('[id$=CountryCodeId]').val('');
                    Alert("Alert", 'Country code already exists!', "Ok");
                    $('[id$=CountryCodeId]').focus();
                }

                if (Callfor == 'Name') {
                    $('[id$=CountryName]').val('');
                    Alert("Alert", 'Country code already exists!', "Ok");
                    $('[id$=CountryName]').focus();
                }
            }
        }
    });
}

/******************************Country Section End****************/

/******************************StakeHolder Section Start****************/
function AddStakeHolderPopup() {
    $("#StakeholderSaveUpdate").text('Save');
    $("#AddStackholderpopupHead").text('Add Stakeholder');
    $('[id$=stakeholderName]').val("");
    //$('[id$=stakeholderLastName]').val("");
    $('[id$=stackholderEmailId]').val("");
    //$('[id$=stackholderMobileNo]').val("");
    $('[id$=stakeholderOrganization]').val("");
    $('[id$=stakeholderselectstatus]').val("1");
    //$('[id$=stackholderAddress]').val("");
    //$('[id$=stackholderState]').val("");
    //$('[id$=stackholderCity]').val("");
    //$('[id$=stackholderPIN]').val("");
    $('[id$=SelectedHscode]').text("");
    $('[id$=hdnstakeholderHscode]').val("");
    $('[id$=hdnStakeHolderId]').val('0');
    $('.error').removeClass('error');
}

function stakeholdervalidate() {
    var msg = "";

    if ($('[id$=stakeholderName]').val().trim().length == 0) {
        $('[id$=stakeholderName]').addClass("error");
        msg += "Please enter full name.<br/>";
    }
    else
        $('[id$=stakeholderName]').removeClass("error");

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

    //if ($('[id$=stackholderMobileNo]').val().trim().length > 0) {
    //    if (!IsMobileNumberReg($('[id$=stackholderMobileNo]').val().trim())) {
    //        $('[id$=stackholderMobileNo]').addClass("error");
    //        msg += "Please enter valid mobile number.<br/>";
    //    }
    //    else
    //        $('[id$=stackholderMobileNo]').removeClass("error");
    //}
    //else {
    //    $('[id$=stackholderMobileNo]').addClass("error");
    //    msg += "Please enter mobile number.<br/>";
    //}

    if ($('[id$=stakeholderOrganization]').val().trim().length == 0) {
        $('[id$=stakeholderOrganization]').addClass("error");
        msg += "Please enter organization name.<br/>";
    }
    else
        $('[id$=stakeholderOrganization]').removeClass("error");

    if ($('[id$=stakeholderDesignation]').val().trim().length == 0) {
        $('[id$=stakeholderDesignation]').addClass("error");
        msg += "Please enter designation.<br/>";
    }
    else
        $('[id$=stakeholderDesignation]').removeClass("error");

    if ($('[id$=stakeholderselectstatus]').val() == -1) {
        $('[id$=stakeholderselectstatus]').addClass("error");
        msg += "Please select status.<br/>";
    }
    else
        $('[id$=stakeholderselectstatus]').removeClass("error");
    if ($('[id$=SelectedHscode]').text().trim().length == 0) {
        msg += "Please select atleast one HS code.<br/>";
    }

    if (msg.length > 0) {
        Alert("Alert", msg, "Ok");
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
        StakeHolderName: $('[id$=stakeholderName]').val().trim(),
        Email: $('[id$=stackholderEmailId]').val().trim(),
        OrgName: $('[id$=stakeholderOrganization]').val().trim(),
        Designation: $('[id$=stakeholderDesignation]').val().trim(),
        Status: $('[id$=stakeholderselectstatus]').val().trim(),
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
                    Alert("Alert", "Stakeholder has been updated successfully.<br\>", "Ok");
                else
                    Alert("Alert", "Stakeholder has been saved successfully.<br\>", "Ok");
            }
            else
                Alert("Alert", "Something went wrong. Please try again.<br\>", "Ok");
        },
        failure: function (result) {
            Alert("Alert", "Something went wrong.<br\>", "Ok");
        },
        error: function (result) {
            Alert("Alert", "Something went wrong.<br\>", "Ok");
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
            $('[id$=stakeholderName]').val(result.StakeHolderName);
            //$('[id$=stakeholderLastName]').val(result.LastName);
            $('[id$=stackholderEmailId]').val(result.Email);
            //$('[id$=stackholderMobileNo]').val(result.Mobile);
            $('[id$=stakeholderOrganization]').val(result.OrgName);
            $('[id$=stakeholderselectstatus]').val(result.Status);
            //$('[id$=stackholderAddress]').val(result.Address);
            //$('[id$=stackholderState]').val(result.State);
            //$('[id$=stackholderCity]').val(result.City);
            //$('[id$=stackholderPIN]').val(result.PIN);
            $('[id$=hdnstakeholderHscode]').val(result.HSCodes);
            $('[id$=stakeholderDesignation]').val(result.Designation);
            $('[id$=SelectedHscode]').text($('[id$=hdnstakeholderHscode]').val());
        },
        failure: function (result) {
            Alert("Alert", "Something went wrong.<br\>", "Ok");
        },
        error: function (result) {
            Alert("Alert", "Something went wrong.<br\>", "Ok");
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
                Alert("Alert", "Stakeholder has been deleted successfully.<br\>", "Ok");
            else
                Alert("Alert", "Something went wrong. Please try again.<br\>", "Ok");

            $("#Section3").load('ManageAccess/GetStakeHolderList');
        },
        failure: function (result) {
            Alert("Alert", "Something went wrong.<br\>", "Ok");
        },
        error: function (result) {
            Alert("Alert", "Something went wrong.<br\>", "Ok");
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
            Alert("Alert", "Something went wrong.<br\>", "Ok");
        },
        error: function (result) {
            Alert("Alert", "Something went wrong.<br\>", "Ok");
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
        Alert("Alert", msg, "Ok");
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
                    Alert("Alert", "Translator has been updated successfully.<br\>", "Ok");
                else {
                    Alert("Alert", "Translator has been saved successfully.<br\>", "Ok");
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
                Alert("Alert", "Something went wrong. Please try again.<br\>", "Ok");
        },
        failure: function (result) {
            Alert("Alert", "Something went wrong..<br\>", "Ok");
        },
        error: function (result) {
            Alert("Alert", "Something went wrong..<br\>", "Ok");
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
            Alert("Alert", "Something went wrong..<br\>", "Ok");
        },
        error: function (result) {
            Alert("Alert", "Something went wrong..<br\>", "Ok");
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
                Alert("Alert", "Translator has been deleted successfully.<br\>", "Ok");
            }
            else
                Alert("Alert", "Something went wrong. Please try again.<br\>", "Ok");

            $("#Section4").load('ManageAccess/GetTranslatorList');
        },
        failure: function (result) {
            Alert("Alert", "Something went wrong.<br\>", "Ok");
        },
        error: function (result) {
            Alert("Alert", "Something went wrong.<br\>", "Ok");
        },
        complete: function () {
            HideGlobalLodingPanel();
        }
    });
}

function SendWelcomeMail(_Id) {
    $.ajax({
        url: "/api/ManageAccess/SendWelcomeMailToTranslator?id=" + _Id + "&UserId=" + myWTOAPP.UserId,
        async: false,
        type: "POST",
        contentType: "application/json; charset=utf-8",
        success: function (result) {
            Alert("Alert", "A welcome mail has been sent successfully to the translator.", "Ok");
            $("#Section4").load('ManageAccess/GetTranslatorList');
        },
        failure: function (result) {
            Alert("Alert", "Something went wrong.<br\>", "Ok");
        },
        error: function (result) {
            Alert("Alert", "Something went wrong.<br\>", "Ok");
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
                    "show_only_matches": false
                },
                "plugins": ["search"]
            });
        },
        failure: function (result) {
            Alert("Alert", "Something went wrong.<br\>", "Ok");
        },
        error: function (result) {
            Alert("Alert", "Something went wrong.<br\>", "Ok");
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
            Alert("Alert", "Something went wrong.<br\>", "Ok");
        },
        error: function (result) {
            Alert("Alert", "Something went wrong.<br\>", "Ok");
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
        Alert("Alert", "Please select atleast one HSCode.<br\>", "Ok");
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
    $('[id$=ddlNotificationType]').val("0");
    $('[id$=ddlTemplateFor]').val("");
    $('[id$=txtSubject]').val("");
    CKEDITOR.instances.txtMessage.setData('');
    $('[id$=ddlTemplateStatus]').val("true");
    $('[id$=hdnTemplateId]').val('0');

    $('.error').removeClass('error');
    $("#AddTemplate").modal("show");
}

function TemplateValidate() {
    debugger;
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
    if ($.trim($('[id$=ddlNotificationType]').val().toString()) == "0") {
        msg += "Please select notification type.<br/>";
        $('[id$=ddlNotificationType]').addClass("error");
    }
    else
        $('[id$=ddlNotificationType]').removeClass("error");

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
        Alert("Alert", msg, "Ok");
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
        NotificationType: $.trim($('[id$=ddlNotificationType]').val()),
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
            Alert("Alert", result.Message + "<br\>", "Ok");
        },
        failure: function (result) {
            Alert("Alert", "Something went wrong..<br\>", "Ok");
        },
        error: function (result) {
            Alert("Alert", "Something went wrong..<br\>", "Ok");
        },
        complete: function () {
            $("#AddTemplate").modal("hide");
            HideGlobalLodingPanel();
            setTimeout(function () {
                $("#Section6").load('ManageAccess/GetTemplateList');
            }, 300);
        }
    });
}

function EditTemplate(id) {
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
            $('[id$=ddlNotificationType]').val(result.NotificationType);
            $('[id$=ddlTemplateFor]').val(result.TemplateFor);
            $('[id$=txtSubject]').val(result.Subject);
            CKEDITOR.instances.txtMessage.setData(result.Body);
            $('[id$=ddlTemplateStatus]').val(result.TemplateStatus.toString());

            if (result.TemplateType.toLowerCase() == "sms")
                $('[id$=divSubject]').addClass('hidden');
            else
                $('[id$=divSubject]').removeClass('hidden');
        },
        failure: function (result) {
            Alert("Alert", "Something went wrong.<br\>", "Ok");
        },
        error: function (result) {
            Alert("Alert", "Something went wrong.<br\>", "Ok");
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
                Alert("Alert", "Template has been deleted successfully.<br\>", "Ok");
            }
            else
                Alert("Alert", "Something went wrong. Please try again.<br\>", "Ok");
        },
        failure: function (result) {
            Alert("Alert", "Something went wrong.<br\>", "Ok");
        },
        error: function (result) {
            Alert("Alert", "Something went wrong.<br\>", "Ok");
        },
        complete: function () {
            HideGlobalLodingPanel();
            setTimeout(function () {
                $("#Section6").load('ManageAccess/GetTemplateList');
            }, 300);
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
                if (typeof ctrlName == "object")
                    $('[id$=hdnTemplateFieldsFor]').val(ctrlName.name);
                else
                    $('[id$=hdnTemplateFieldsFor]').val(ctrlName);
            }
        },
        failure: function (result) {
            Alert("Alert", "Something went wrong.<br\>", "Ok");
        },
        error: function (result) {
            Alert("Alert", "Something went wrong.<br\>", "Ok");
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

/****************************** Internal StackHolder Section Start ****************/
function InternalStackHolderTabClick() {
    $("#Section7").load('ManageAccess/GetInternalStackHolderList');
}

function AddInternalStackHolderPopup() {
    $("#InternalStakeholderSaveUpdate").text('Save');
    $("#AddInternalStackholderpopupHead").text('Add Internal Stakeholder');
    $('[id$=internalstakeholderName]').val("");
    $('[id$=internalstakeholderdesignation]').val('');
    $('[id$=internalstackholderEmailId]').val('');
    $('[id$=internalstakeholderorganizationname]').val('');
    $('[id$=internalstakeholderselectstatus]').val("1");
    $('[id$=hdnInternalStackHolderId]').val('0');
    $('.error').removeClass('error');
}

function internalstakeholdervalidate() {
    var msg = "";
    if ($('[id$=internalstakeholderName]').val().trim().length == 0) {
        $('[id$=internalstakeholderName]').addClass("error");
        msg += "Please enter name.<br/>";
    }
    else
        $('[id$=internalstakeholderName]').removeClass("error");

    //if ($('[id$=internalstakeholderdesignation]').val().trim().length == 0) {
    //    $('[id$=internalstakeholderdesignation]').addClass("error");
    //    msg += "Please enter designation.<br/>";
    //}
    //else
    //    $('[id$=internalstakeholderdesignation]').removeClass("error");

    if ($('[id$=internalstackholderEmailId]').val().trim().length > 0) {
        if (!validateEmail($('[id$=internalstackholderEmailId]').val().trim())) {
            $('[id$=internalstackholderEmailId]').addClass("error");
            msg += "Please enter valid email-id.<br/>";
        }
        //else {
        //    var IsEmailExist = IsEmailExists($('[id$=internalstackholderEmailId]').val().trim(), "InternalStakeholder");
        //    if (IsEmailExist && $('[id$=hdnInternalStackHolderId]').val() == 0) {
        //        msg += "An internal stakeholder already exists with same email-id.<br/>";
        //        $('[id$=internalstackholderEmailId]').addClass("error");
        //    }
        //    else
        //        $('[id$=internalstackholderEmailId]').removeClass("error");
        //}
    }
    else {
        $('[id$=internalstackholderEmailId]').addClass("error");
        msg += "Please enter email-id.<br/>";
    }
    if ($('[id$=internalstakeholderselectstatus]').val() == "") {
        $('[id$=internalstakeholderselectstatus]').addClass("error");
        msg += "Please select status.<br/>";
    }
    else
        $('[id$=internalstakeholderselectstatus]').removeClass("error");

    if (msg.length > 0) {
        Alert("Alert", msg, "Ok");
    }
    else
        AddNewInternalStackholder();
}

function AddNewInternalStackholder() {
    ShowGlobalLodingPanel();
    var id = $('[id$=hdnInternalStackHolderId]').val();
    var obj = {
        InternalStakeholdersId: id,
        Name: $('[id$=internalstakeholderName]').val().trim(),
        Designation: $('[id$=internalstakeholderdesignation]').val().trim(),
        Emailid: $('[id$=internalstackholderEmailId]').val().trim(),
        OrgName: $('[id$=internalstakeholderorganizationname]').val().trim(),
        Status: $('[id$=internalstakeholderselectstatus]').val().trim(),
    };
    $.ajax({
        url: "/api/ManageAccess/AddInternalStackholder/" + myWTOAPP.UserId,
        async: false,
        type: "POST",
        data: JSON.stringify(obj),
        contentType: "application/json; charset=utf-8",
        success: function (result) {
            if (result > 0) {
                if (id > 0)
                    Alert("Alert", "Internal stakeholder has been updated successfully.<br\>", "Ok");
                else {
                    Alert("Alert", "Internal stakeholder  has been saved successfully.<br\>", "Ok");
                }
            }
            else
                Alert("Alert", "Something went wrong. Please try again.<br\>", "Ok");
        },
        failure: function (result) {

            Alert("Alert", "Something went wrong..<br\>", "Ok");
        },
        error: function (result) {
            Alert("Alert", "Something went wrong..<br\>", "Ok");
        },
        complete: function () {
            $("#AddInternalStakeholder").modal("hide");
            HideGlobalLodingPanel();
            setTimeout(function () {
                $("#Section7").load('ManageAccess/GetInternalStackHolderList');
            }, 300);
        }
    });
}

function EditInternalTranslatorData(id) {
    ShowGlobalLodingPanel();
    $("#AddInternalStakeholder").modal("show");
    $("#AddInternalStackholderpopupHead").text('Update Internal Stakeholder');
    $("#InternalStakeholderSaveUpdate").text('Update');
    $.ajax({
        url: "/api/ManageAccess/GetInternalStackHolderDetails?Id=" + id + "",
        async: false,
        type: "GET",
        data: JSON,
        contentType: "application/json; charset=utf-8",
        success: function (result) {
            $('[id$=hdnInternalStackHolderId]').val(result.InternalStakeholdersId);
            $('[id$=internalstakeholderName]').val(result.Name);
            $('[id$=internalstakeholderdesignation]').val(result.Designation);
            $('[id$=internalstakeholderorganizationname]').val(result.OrgName);
            $('[id$=internalstackholderEmailId]').val(result.Emailid);
            $('[id$=internalstakeholderselectstatus]').val(result.Status);
        },
        failure: function (result) {
            Alert("Alert", "Something went wrong..<br\>", "Ok");
        },
        error: function (result) {
            Alert("Alert", "Something went wrong..<br\>", "Ok");
        },
        complete: function () {
            HideGlobalLodingPanel();
        }
    });
}

function DeleteInternalTranslator(id) {
    $.ajax({
        url: "/api/ManageAccess/DeleteinternalStackHolder?Id=" + id + "",
        async: false,
        type: "GET",
        data: JSON,
        contentType: "application/json; charset=utf-8",
        success: function (result) {
            if (result) {
                Alert("Alert", "Internal Stackholder has been deleted successfully.<br\>", "Ok");
            }
            else
                Alert("Alert", "Something went wrong. Please try again.<br\>", "Ok");

            $("#Section7").load('ManageAccess/GetInternalStackHolderList');
        },
        failure: function (result) {
            Alert("Alert", "Something went wrong.<br\>", "Ok");
        },
        error: function (result) {
            Alert("Alert", "Something went wrong.<br\>", "Ok");
        },
        complete: function () {
            HideGlobalLodingPanel();
        }
    });
}

/******************************Internal StackHolder Section End****************/


/****************************** Regulatory Bodies Section Start ****************/
function RegulatoryBodiesTabClick() {
    $("#Section8").load('ManageAccess/GetRegulatoryBodiesList');
}

function AddRegulatoryBodyPopup() {
    $("#RegulatoryBodySaveUpdate").text('Save');
    $("#AddRegulatoryBodiespopupHead").text('Add Regulatory Body');
    $('[id$=RegulatoryBodyName]').val("");
    $('[id$=RegulatoryBodyEmailId]').val('');
    $('[id$=RegulatoryBodyContact]').val('');
    $('[id$=RegulatoryBodyAddress]').val("");
    $('[id$=hdnRegulatoryBodiesId]').val('0');
    $('.error').removeClass('error');
}

function RegulatoryBodiesvalidate() {
    var msg = "";
    if ($('[id$=RegulatoryBodyName]').val().trim().length == 0) {
        $('[id$=RegulatoryBodyName]').addClass("error");
        msg += "Please enter name.<br/>";
    }
    else
        $('[id$=RegulatoryBodyName]').removeClass("error");

    //if ($('[id$=RegulatoryBodyContact]').val().trim().length == 0) {
    //  $('[id$=RegulatoryBodyContact]').addClass("error");
    //  msg += "Please enter contact.<br/>";
    //  }
    //else
    //   $('[id$=RegulatoryBodyContact]').removeClass("error");

    if ($('[id$=RegulatoryBodyEmailId]').val().trim().length > 0) {
        if (!validateEmail($('[id$=RegulatoryBodyEmailId]').val().trim())) {
            $('[id$=RegulatoryBodyEmailId]').addClass("error");
            msg += "Please enter valid email-id.<br/>";
        }
        //else {
        //    var IsEmailExist = IsEmailExists($('[id$=RegulatoryBodyEmailId]').val().trim(), "RegulatoryBody");
        //    if (IsEmailExist && $('[id$=hdnRegulatoryBodiesId]').val() == 0) {
        //        msg += "A regulatory body already exists with same email-id.<br/>";
        //        $('[id$=internalstackholderEmailId]').addClass("error");
        //    }
        //    else
        //        $('[id$=RegulatoryBodyEmailId]').removeClass("error");
        //}
    }
    else {
        $('[id$=RegulatoryBodyEmailId]').addClass("error");
        msg += "Please enter email-id.<br/>";
    }
    if ($('[id$=RegulatoryBodyselectstatus]').val() == "") {
        $('[id$=RegulatoryBodyselectstatus]').addClass("error");
        msg += "Please select status.<br/>";
    }
    else
        $('[id$=RegulatoryBodyselectstatus]').removeClass("error");

    if (msg.length > 0) {
        Alert("Alert", msg, "Ok");
    }
    else
        AddNewRegulatoryBody();
}

function AddNewRegulatoryBody() {
    ShowGlobalLodingPanel();
    var id = $('[id$=hdnRegulatoryBodiesId]').val();
    var obj = {
        RegulatoryBodyId: id,
        Name: $('[id$=RegulatoryBodyName]').val().trim(),
        Address: $('[id$=RegulatoryBodyAddress]').val().trim(),
        Emailid: $('[id$=RegulatoryBodyEmailId]').val().trim(),
        Contact: $('[id$=RegulatoryBodyContact]').val().trim(),
        Status: $('[id$=RegulatoryBodyselectstatus]').val().trim(),
    };
    $.ajax({
        url: "/api/ManageAccess/AddRegulatoryBody/" + myWTOAPP.UserId,
        async: false,
        type: "POST",
        data: JSON.stringify(obj),
        contentType: "application/json; charset=utf-8",
        success: function (result) {
            if (result > 0) {
                if (id > 0)
                    Alert("Alert", "Regulatory Body has been updated successfully.<br\>", "Ok");
                else {
                    Alert("Alert", "Regulatory Body  has been saved successfully.<br\>", "Ok");
                }
            }
            else
                Alert("Alert", "Something went wrong. Please try again.<br\>", "Ok");
        },
        failure: function (result) {

            Alert("Alert", "Something went wrong..<br\>", "Ok");
        },
        error: function (result) {
            Alert("Alert", "Something went wrong..<br\>", "Ok");
        },
        complete: function () {
            $("#AddRegulatoryBodies").modal("hide");
            HideGlobalLodingPanel();
            setTimeout(function () {
                $("#Section8").load('ManageAccess/GetRegulatoryBodiesList');
            }, 300);
        }
    });
}

function EditRegulatoryBodyData(id) {
    ShowGlobalLodingPanel();
    $("#AddRegulatoryBodies").modal("show");
    $("#AddRegulatoryBodiespopupHead").text('Update Regulatory Body');
    $("#RegulatoryBodySaveUpdate").text('Update');
    $.ajax({
        url: "/api/ManageAccess/GetRegulatoryBodyDetails?Id=" + id + "",
        async: false,
        type: "GET",
        data: JSON,
        contentType: "application/json; charset=utf-8",
        success: function (result) {
            $('[id$=hdnRegulatoryBodiesId]').val(result.RegulatoryBodyId);
            $('[id$=RegulatoryBodyName]').val(result.Name);
            $('[id$=RegulatoryBodyAddress]').val(result.Address);
            $('[id$=RegulatoryBodyEmailId]').val(result.Emailid);
            $('[id$=RegulatoryBodyContact]').val(result.Contact);
            $('[id$=RegulatoryBodyselectstatus]').val(result.Status);
        },
        failure: function (result) {
            Alert("Alert", "Something went wrong..<br\>", "Ok");
        },
        error: function (result) {
            Alert("Alert", "Something went wrong..<br\>", "Ok");
        },
        complete: function () {
            HideGlobalLodingPanel();
        }
    });
}

function DeleteRegulatoryBody(id) {
    $.ajax({
        url: "/api/ManageAccess/DeleteRegulatoryBody?Id=" + id + "",
        async: false,
        type: "GET",
        data: JSON,
        contentType: "application/json; charset=utf-8",
        success: function (result) {
            if (result) {
                Alert("Alert", "Regulatory Body has been deleted successfully.<br\>", "Ok");
            }
            else
                Alert("Alert", "Something went wrong. Please try again.<br\>", "Ok");

            $("#Section8").load('ManageAccess/GetRegulatoryBodiesList');
        },
        failure: function (result) {
            Alert("Alert", "Something went wrong.<br\>", "Ok");
        },
        error: function (result) {
            Alert("Alert", "Something went wrong.<br\>", "Ok");
        },
        complete: function () {
            HideGlobalLodingPanel();
        }
    });
}

function ValidateBodyName(evt) {
    var keyCode = (evt.which) ? evt.which : event.keyCode;
    if ((keyCode >= 48 && keyCode <= 57)) {
        Alert("Alert", 'Numbers are not allowed.<br/>', "Ok");
        return false;
    }
}

function ValidateBodyContact(evt) {
    var keyCode = (evt.which) ? evt.which : event.keyCode;
    if (((keyCode >= 65 && keyCode <= 90) || (keyCode >= 97 && keyCode <= 122))) {
        Alert("Alert", 'Alphabets are not allowed.<br/>', "Ok");
        return false;
    }

}
/****************************** Regulatory Bodies Section End****************/


/****************************** Language Section Start****************/
function LanguageTabClick() {
    $("#Section9").load('ManageAccess/GetLanguageList');
}

function AddLanguagePopup() {
    $("#LanguageSaveUpdate").text('Save');
    $("#AddLanguaugepopupHead").text('Add Regulatory Body');
    $('[id$=LanguageName]').val("");
    $('[id$=hdnLanguageId]').val('0');
    $('#languageselectstatus').val("");
    $('.error').removeClass('error');
}

function Languagevalidate() {
    var msg = "";
    if ($('[id$=LanguageName]').val().trim().length == 0) {
        $('[id$=LanguageName]').addClass("error");
        msg += "Please enter language.<br/>";
    }
    else
        $('[id$=LanguageName]').removeClass("error");

    if ($('[id$=languageselectstatus]').val() == "") {
        $('[id$=languageselectstatus]').addClass("error");
        msg += "Please select status.<br/>";
    }
    else
        $('[id$=languageselectstatus]').removeClass("error");

    if (msg.length > 0) {
        Alert("Alert", msg, "Ok");
    }
    else
        AddNewLanguage();
}

function AddNewLanguage() {
    ShowGlobalLodingPanel();
    var id = $('[id$=hdnLanguageId]').val();
    var obj = {
        LanguageId: id,
        Language: $('[id$=LanguageName]').val().trim(),
        Status: $('[id$=languageselectstatus]').val().trim(),
    };
    $.ajax({
        url: "/api/ManageAccess/AddLanguage/" + myWTOAPP.UserId,
        async: false,
        type: "POST",
        data: JSON.stringify(obj),
        contentType: "application/json; charset=utf-8",
        success: function (result) {
            if (result > 0) {
                if (id > 0)
                    Alert("Alert", "Language has been updated successfully.<br\>", "Ok");
                else {
                    Alert("Alert", "Language has been saved successfully.<br\>", "Ok");
                }
            }
            else
                Alert("Alert", "Something went wrong. Please try again.<br\>", "Ok");
        },
        failure: function (result) {

            Alert("Alert", "Something went wrong..<br\>", "Ok");
        },
        error: function (result) {
            Alert("Alert", "Something went wrong..<br\>", "Ok");
        },
        complete: function () {
            $("#AddLanguage").modal("hide");
            HideGlobalLodingPanel();
            setTimeout(function () {
                $("#Section9").load('ManageAccess/GetLanguageList');
            }, 300);
        }
    });
}

function EditLanguageData(id) {
    ShowGlobalLodingPanel();
    $('.error').removeClass('error');
    $("#AddLanguage").modal("show");
    $("#AddLanguaugepopupHead").text('Update Language');
    $("#LanguageSaveUpdate").text('Update');
    $.ajax({
        url: "/api/ManageAccess/GetLanguageDetails?Id=" + id + "",
        async: false,
        type: "GET",
        data: JSON,
        contentType: "application/json; charset=utf-8",
        success: function (result) {
            $('[id$=hdnLanguageId]').val(result.LanguageId);
            $('[id$=LanguageName]').val(result.Language);
            $('[id$=languageselectstatus]').val(result.Status);
        },
        failure: function (result) {
            Alert("Alert", "Something went wrong..<br\>", "Ok");
        },
        error: function (result) {
            Alert("Alert", "Something went wrong..<br\>", "Ok");
        },
        complete: function () {
            HideGlobalLodingPanel();
        }
    });
}

function DeleteLanguage(id) {
    $.ajax({
        url: "/api/ManageAccess/DeleteLanguage?Id=" + id + "",
        async: false,
        type: "GET",
        data: JSON,
        contentType: "application/json; charset=utf-8",
        success: function (result) {
            if (result) {
                Alert("Alert", "Language has been deleted successfully.<br\>", "Ok");
            }
            else
                Alert("Alert", "Something went wrong. Please try again.<br\>", "Ok");
            $("#Section9").load('ManageAccess/GetLanguageList');
        },
        failure: function (result) {
            Alert("Alert", "Something went wrong.<br\>", "Ok");
        },
        error: function (result) {
            Alert("Alert", "Something went wrong.<br\>", "Ok");
        },
        complete: function () {
            HideGlobalLodingPanel();
        }
    });
}

function CheckDuplicateEntryLanguage() {
    var Language = "";
    Language = $('[id$=LanguageName]').val().trim();

    $.ajax({
        url: "/api/ManageAccess/CheckDuplicateLanguage?id=" + $('[id$=hdnLanguageId]').val() + "&&text=" + Language,
        async: false,
        type: "GET",
        contentType: "application/json; charset=utf-8",
        success: function (result) {
            if (result.length > 0) {
                $('[id$=LanguageName]').val('');
                alert('Language already exists!');
                $('[id$=LanguageName]').focus();
            }
        }
    });
}
/****************************** Language Section End****************/