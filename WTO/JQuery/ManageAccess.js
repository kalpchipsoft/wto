
//Password display code
//function ShowPassword(ctrl) {
//    $(this).find(".eye").addClass("hidden");
//    $(this).find(".pwd").css("padding", "5px").removeClass("hidden").mouseout(function () {
//        $(this).addClass("hidden");
//        $(".eye").removeClass("hidden");
//    });
//}

$(document).ready(function () {
    debugger;
    GetHSCode();
    $("#hidden").val("Avinash");
})

function ShowPassword(ctrl) {
    $("td a").click(function (ctrl) {
        debugger;
        $(this).find(".eye").addClass("hidden");
        $(this).find(".pwd").css("padding", "5px").removeClass("hidden").mouseout(function () {
            $(this).addClass("hidden");
            $(".eye").removeClass("hidden");
        });
    })
}

//Add User PopUp
function AddUserPopup() {
    $("#UserSaveUpdate").text('Save');
    $("#AddUserpopupHead").text('Add User');
    $('[id$=UserFirstName]').val("");
    $('[id$=UserLastName]').val("");
    $('[id$=UserEmailId]').val("");
    $('[id$=UserMobileNo]').val("");
    $('[id$=UserSelectActiveInactive]').val("1");

    $("#UserFirstName, #UserLastName, #UserEmailId, #UserMobileNo, #UserSelectActiveInactive").removeClass("error");

}

//Add User Save button validation
function Uservalidate() {
    var msg = "";
    if ($('[id$=UserFirstName]').val().trim().length == 0) {
        $('[id$=UserFirstName]').addClass("error");
        msg += "Enter First Name \n" + "<br/>";
    }
    else {
        $('[id$=UserFirstName]').removeClass("error");
    }
    if ($('[id$=UserLastName]').val().trim().length == 0) {
        $('[id$=UserLastName]').addClass("error");
        msg += "Enter Last Name \n" + "<br/>";
    }
    else {
        $('[id$=UserLastName]').removeClass("error");
    }
    if ($('[id$=UserEmailId]').val().trim().length > 0) {
        if (!validateEmail($('[id$=UserEmailId]').val().trim())) {
            $('[id$=UserEmailId]').addClass("error");
            msg += "Email Id Not Valid \n" + "<br/>";
        }
        else {
            $('[id$=UserEmailId]').removeClass("error");
        }
    }
    else {
        $('[id$=UserEmailId]').addClass("error");
        msg += "Enter Valid Email Id \n" + "<br/>";
    }

    if ($('[id$=UserMobileNo]').val().trim().length > 0) {
        if (!IsMobileNumberReg($('[id$=UserMobileNo]').val().trim())) {
            $('[id$=UserMobileNo]').addClass("error");
            msg += "Mobile Number Not Valid \n" + "<br/>";
        }
        else {
            $('[id$=UserMobileNo]').removeClass("error");
        }
    }
    else {
        $('[id$=UserMobileNo]').addClass("error");
        msg += "Enter Valid Mobile Number \n" + "<br/>";
    }

    if ($('[id$=UserSelectActiveInactive]').val() == -1) {
        $('[id$=UserSelectActiveInactive]').addClass("error");
        msg += "Please select one status \n"
    }
    else {
        $('[id$=UserSelectActiveInactive]').removeClass("error");
    }
    if (msg.length > 0) {
        ShowAlert();
        $("#AlertBodyheading").text("Mandetory Fields")
        $("#AlertBody").append(msg);
    }
    else
        AddNewUser();
}

function EditUserData(id) {
    $("#AddUser").modal("show");
    $("#AddUserpopupHead").text('Update User');
    $("#UserSaveUpdate").text('Update');
    $.ajax({
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
        },
        failure: function (result) {
            alert("Something went wrong.");
        },
        error: function (result) {
            alert("Something went wrong.");
        },
        complete: function () {
            //HideGlobalLodingPanel();
        }
    });
}

//Add New User Manage Access Section
function AddNewUser() {
    var id = $('[id$=hdnUserId]').val();
    var obj = {
        UserId: id,
        FirstName: $('[id$=UserFirstName]').val().trim(),
        LastName: $('[id$=UserLastName]').val().trim(),
        Email: $('[id$=UserEmailId]').val().trim(),
        Mobile: $('[id$=UserMobileNo]').val().trim(),
        Status: $('[id$=UserSelectActiveInactive]').val().trim()
    };
    $.ajax({
        url: "/api/ManageAccess/AddUser",
        async: false,
        type: "POST",
        data: JSON.stringify(obj),
        contentType: "application/json; charset=utf-8",
        success: function (result) {
            if (result) {
                if (id > 0)
                    alert('User has been Update successfully.');
                else
                    alert('User has been saved successfully.');
            }
            else
                alert('Something went wrong. Please try again');

            $("#Section1").load('ManageAccess/GetUserList');
        },
        failure: function (result) {
            alert("Something went wrong.");
        },
        error: function (result) {
            alert("Something went wrong.");
        },
        complete: function () {
            //HideGlobalLodingPanel();
        }
    });
    $("#AddUser").modal("hide");
}

//Delete User From User List In Manage Access
function DeleteUser(id) {
    if (confirm('Are you sure ?')) {
        $(this).prev('span.text').remove();
        $.ajax({
            url: "/api/ManageAccess/DeleteUser?Id=" + id + "",
            async: false,
            type: "POST",
            data: JSON,
            contentType: "application/json; charset=utf-8",
            success: function (result) {
                if (result)
                    alert('User has been Delete successfully.');
                else
                    alert('Something went wrong. Please try again');
                $("#Section1").load('ManageAccess/GetUserList');
            },
            failure: function (result) {
                alert("Something went wrong.");
            },
            error: function (result) {
                alert("Something went wrong.");
            },
            complete: function () {
                //HideGlobalLodingPanel();
            }
        });
    }
    else
        return false;
}

//////////////////////////////////////////////////////Country Add, Delete, Edit part Start///////////////////////////////////////////////////////

function CountryTabClick() {
    $("#Section2").load('ManageAccess/GetCountryList');
}

//Add Country Validation
function CountryValidation() {
    var msg = "";

    if ($('[id$=CountryName]').val().trim().length == 0) {
        $('[id$=CountryName]').addClass("error");
        msg += "Enter Country Name \n" + "<br/>";
    }
    else {
        $('[id$=CountryName]').removeClass("error");
    }

    if ($('[id$=CountryStatus]').val() == -1) {
        $('[id$=CountryStatus]').addClass("error");
        msg += "Please select one status \n" + "<br/>";
    }
    else {
        $('[id$=CountryStatus]').removeClass("error");
    }

    if (msg.length > 0) {
        ShowAlert();
        $("#AlertBodyheading").text("Mandetory Fields")
        $("#AlertBody").append(msg);
    }
    else
        AddNewCountry();
}

//Add Country PopUp
function AddCountryPopup() {
    $("#CountrySaveUpdate").text('Save');
    $("#AddCountrypopupHead").text('Add Country');
    $('[id$=CountryName]').val("");
    $('[id$=CountryStatus]').val("1");
   
    $("#CountryName, #CountryStatus").removeClass("error");

}

//Add New Country Manage Access Section
function AddNewCountry() {
    debugger;
    var id = $('[id$=hdnCountryId]').val();
    var obj = {
        CountryId: id,
        CountryName: $('[id$=CountryName]').val().trim(),
        Status: $('[id$=CountryStatus]').val().trim()
    };
    $.ajax({
        url: "/api/ManageAccess/AddCountry",
        async: false,
        type: "POST",
        data: JSON.stringify(obj),
        contentType: "application/json; charset=utf-8",
        success: function (result) {
            debugger;
            if (result) {
                if (id > 0)
                    alert('Country has been Update successfully.');
                else
                    alert('Country has been saved successfully.');
            }
            else
                alert('Something went wrong. Please try again');

            $("#Section2").load('ManageAccess/GetCountryList');
        },
        failure: function (result) {
            alert("Something went wrong.");
        },
        error: function (result) {
            alert("Something went wrong.");
        },
        complete: function () {
            //HideGlobalLodingPanel();
        }
    });
    $("#AddCountry").modal("hide");
}

//Edit Country Manage Access Section
function EditCountryData(id) {
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
        },
        failure: function (result) {
            alert("Something went wrong.");
        },
        error: function (result) {
            alert("Something went wrong.");
        },
        complete: function () {
            //HideGlobalLodingPanel();
        }
    });
}

//Delete User From User List In Manage Access
function DeleteCountry(id) {
    if (confirm('Are you sure ?')) {
        $(this).prev('span.text').remove();
        $.ajax({
            url: "/api/ManageAccess/DeleteCountry?Id=" + id + "",
            async: false,
            type: "POST",
            data: JSON,
            contentType: "application/json; charset=utf-8",
            success: function (result) {
                if (result)
                    alert('Country has been Delete successfully.');
                else
                    alert('Something went wrong. Please try again');
                $("#Section2").load('ManageAccess/GetCountryList');
            },
            failure: function (result) {
                alert("Something went wrong.");
            },
            error: function (result) {
                alert("Something went wrong.");
            },
            complete: function () {
                //HideGlobalLodingPanel();
            }
        });
    }
    else
        return false;
}

//////////////////////////////////////////////////////Country Add, Delete, Edit part End///////////////////////////////////////////////////////

//////////////////////////////////////////////////////Stackholder Add, Delete, Edit part Start///////////////////////////////////////////////////////

//Add stakeholder PopUp
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
    SelectedHscode

    $('#stakeholderFirstName, #stakeholderLastName, #stackholderEmailId, #stackholderMobileNo, #stakeholderOrganization, #stakeholderselectstatus, #stackholderAddress, #stackholderState, #stackholderCity, #stackholderPIN').removeClass("error");
}

//Add stakeholder Save button validation
function stakeholdervalidate() {
    var msg = "";

    if ($('[id$=stakeholderFirstName]').val().trim().length == 0) {
        $('[id$=stakeholderFirstName]').addClass("error");
        msg += "Enter first name \n" + "<br/>";
    }
    else {
        $('[id$=stakeholderFirstName]').removeClass("error");
    }

    if ($('[id$=stakeholderLastName]').val().trim().length == 0) {
        $('[id$=stakeholderLastName]').addClass("error");
        msg += "Enter last name \n" + "<br/>";
    }
    else {
        $('[id$=stakeholderLastName]').removeClass("error");
    }


    if ($('[id$=stackholderEmailId]').val().trim().length > 0) {
        if (!validateEmail($('[id$=stackholderEmailId]').val().trim())) {
            $('[id$=stackholderEmailId]').addClass("error");
            msg += "Email-id not valid \n" + "<br/>";
        }
        else {
            $('[id$=stackholderEmailId]').removeClass("error");
        }
    }
    else {
        $('[id$=stackholderEmailId]').addClass("error");
        msg += "Enter valid email-id \n" + "<br/>";
    }

    if ($('[id$=stackholderMobileNo]').val().trim().length > 0) {
        if (!IsMobileNumberReg($('[id$=stackholderMobileNo]').val().trim())) {
            $('[id$=stackholderMobileNo]').addClass("error");
            msg += "Mobile number not valid \n" + "<br/>";
        }
        else {
            $('[id$=stackholderMobileNo]').removeClass("error");
        }
    }
    else {
        $('[id$=stackholderMobileNo]').addClass("error");
        msg += "Enter valid mobile number \n" + "<br/>";
    }

    if ($('[id$=stakeholderOrganization]').val().trim().length == 0) {
        $('[id$=stakeholderOrganization]').addClass("error");
        msg += "Enter organization name \n" + "<br/>";
    }
    else {
        $('[id$=stakeholderOrganization]').removeClass("error");
    }

    if ($('[id$=stakeholderselectstatus]').val() == -1) {
        $('[id$=stakeholderselectstatus]').addClass("error");
        msg += "Please select one status \n" + "<br/>";
    }
    else {
        $('[id$=stakeholderselectstatus]').removeClass("error");
    }

    if ($('[id$=stackholderAddress]').val().trim().length == 0) {
        $('[id$=stackholderAddress]').addClass("error");
        msg += "Enter address \n" + "<br/>";
    }
    else {
        $('[id$=stackholderAddress]').removeClass("error");
    }

    if ($('[id$=stackholderState]').val().trim().length == 0) {
        $('[id$=stackholderState]').addClass("error");
        msg += "Enter state \n" + "<br/>";
    }
    else {
        $('[id$=stackholderState]').removeClass("error");
    }

    if ($('[id$=stackholderCity]').val().trim().length == 0) {
        $('[id$=stackholderCity]').addClass("error");
        msg += "Enter city \n" + "<br/>";
    }
    else {
        $('[id$=stackholderCity]').removeClass("error");
    }

    if ($('[id$=stackholderPIN]').val().trim().length == 0) {
        $('[id$=stackholderPIN]').addClass("error");
        msg += "Enter PIN code" + "<br/>";
    }
    else {
        $('[id$=stackholderPIN]').removeClass("error");
    }
    if ($('[id$=SelectedHscode]').text().trim().length == 0) {
        msg += "Select HSCode" + "<br/>";
    }
    
    if (msg.length > 0) {
        ShowAlert();
        $("#AlertBodyheading").text("Mandetory Fields")
        $("#AlertBody").append(msg);
    }
    else
        AddNewStackholder();
}

//Add New Stackholder popup Api
function AddNewStackholder() {
    var id = $('[id$=hdnStakeHolderId]').val();
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
        PIN: $('[id$=stackholderPIN]').val().trim()
    };
    $.ajax({
        url: "/api/ManageAccess/AddStakeHolder",
        async: false,
        type: "POST",
        data: JSON.stringify(obj),
        contentType: "application/json; charset=utf-8",
        success: function (result) {
            if (result) {
                if (id > 0)
                    alert('Stakeholder has been Update successfully.');
                else
                    alert('Stakeholder has been saved successfully.');
            }
            else
                alert('Something went wrong. Please try again');

            $("#Section3").load('ManageAccess/GetStakeHolderList');
        },
        failure: function (result) {
            alert("Something went wrong.");
        },
        error: function (result) {
            alert("Something went wrong.");
        },
        complete: function () {
            //HideGlobalLodingPanel();
        }
    });
    $("#AddStakeholder").modal("hide");
}

//Edit New Stackholder popup Api
function EditStakeHolderData(id) {
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
        },
        failure: function (result) {
            alert("Something went wrong.");
        },
        error: function (result) {
            alert("Something went wrong.");
        },
        complete: function () {
            //HideGlobalLodingPanel();
        }
    });
}

//Delete Stakeholder From Stakeholder List In Manage Access
function DeleteStakeHolder(id) {
    if (confirm('Are you sure ?')) {
        $(this).prev('span.text').remove();
        $.ajax({
            url: "/api/ManageAccess/DeleteStakeHolder?Id=" + id + "",
            async: false,
            type: "POST",
            data: JSON,
            contentType: "application/json; charset=utf-8",
            success: function (result) {
                if (result)
                    alert('Stakeholder has been deleted successfully.');
                else
                    alert('Something went wrong. Please try again');

                $("#Section3").load('ManageAccess/GetStakeHolderList');
            },
            failure: function (result) {
                alert("Something went wrong.");
            },
            error: function (result) {
                alert("Something went wrong.");
            },
            complete: function () {
                //HideGlobalLodingPanel();
            }
        });
    }
    else
        return false;
}

function StakeHolderTabClick() {
    $("#Section3").load('ManageAccess/GetStakeHolderList');
}


/////////////////////////////////////////////////////Stackholder Add, Delete, Edit part End/////////////////////////////////////////////////////////////


//////////////////////////////////////////////////////Translator Add, Delete, Edit part Start///////////////////////////////////////////////////////////

//Add stakeholder PopUp
function AddTranslatorPopup() {
    $("#TranslatorSaveUpdate").text('Save');
    $("#AddTranslatorpopupHead").text('Add Translator');
    $('[id$=TranslatorFirstName]').val("");
    $('[id$=TranslatorLastName]').val("");
    $('[id$=TranslatorEmailid]').val("");
    $('[id$=TranslatorMobileNo]').val("");
    $('[id$=TranslatorStatus]').val("1");

    $("#TranslatorFirstName, #TranslatorLastName, #TranslatorEmailid, #TranslatorMobileNo, #TranslatorStatus").removeClass("error");
}

//Add Translator Save button validation
function TranslatorValidate() {
    debugger;
    var msg = "";

    if ($('[id$=TranslatorFirstName]').val().trim().length == 0) {
        $('[id$=TranslatorFirstName]').addClass("error");
        msg += "Enter First Name \n" + "<br/>";
    }
    else {
        $('[id$=TranslatorFirstName]').removeClass("error");
    }

    if ($('[id$=TranslatorLastName]').val().trim().length == 0) {
        $('[id$=TranslatorLastName]').addClass("error");
        msg += "Enter Last Name \n" + "<br/>";
    }
    else {
        $('[id$=TranslatorLastName]').removeClass("error");
    }

    if ($('[id$=TranslatorEmailid]').val().trim().length > 0) {
        if (!validateEmail($('[id$=TranslatorEmailid]').val().trim())) {
            $('[id$=TranslatorEmailid]').addClass("error");
            msg += "Email Id Not Valid \n" + "<br/>";
        }
        else {
            $('[id$=TranslatorEmailid]').removeClass("error");
        }
    }
    else {
        $('[id$=TranslatorEmailid]').addClass("error");
        msg += "Enter Valid Email Id \n" + "<br/>";
    }

    if ($('[id$=TranslatorMobileNo]').val().trim().length > 0) {
        if (!IsMobileNumberReg($('[id$=TranslatorMobileNo]').val().trim())) {
            $('[id$=TranslatorMobileNo]').addClass("error");
            msg += "Mobile Number Not Valid \n" + "<br/>";
        }
        else {
            $('[id$=TranslatorMobileNo]').removeClass("error");
        }
    }
    else {
        $('[id$=TranslatorMobileNo]').addClass("error");
        msg += "Enter Valid Mobile Number \n" + "<br/>";
    }

    if ($('[id$=TranslatorStatus]').val() == -1) {
        $('[id$=TranslatorStatus]').addClass("error");
        msg += "Please select one status \n" + "<br/>";
    }
    else {
        $('[id$=TranslatorStatus]').removeClass("error");
    }

    if (msg.length > 0) {
        ShowAlert();
        $("#AlertBodyheading").text("Mandetory Fields")
        $("#AlertBody").append(msg);
    }
    else
        AddNewTranslator();
}

//Translator tab click
function TranslatorTabClick() {
    $("#Section4").load('ManageAccess/GetTranslatorList');
}

//Add New Translator Manage Access Section
function AddNewTranslator() {
    var id = $('[id$=hdnTranslatorId]').val();
    var obj = {
        TranslatorId: id,
        FirstName: $('[id$=TranslatorFirstName]').val().trim(),
        LastName: $('[id$=TranslatorLastName]').val().trim(),
        Email: $('[id$=TranslatorEmailid]').val().trim(),
        Mobile: $('[id$=TranslatorMobileNo]').val().trim(),
        Status: $('[id$=TranslatorStatus]').val().trim()
    };
    $.ajax({
        url: "/api/ManageAccess/AddTranslator",
        async: false,
        type: "POST",
        data: JSON.stringify(obj),
        contentType: "application/json; charset=utf-8",
        success: function (result) {
            if (result) {
                if (id > 0)
                    alert('Translator has been Update successfully.');
                else
                    alert('Translator has been saved successfully.');
            }
            else
                alert('Something went wrong. Please try again');

            $("#Section4").load('ManageAccess/GetTranslatorList');
        },
        failure: function (result) {
            alert("Something went wrong.");
        },
        error: function (result) {
            alert("Something went wrong.");
        },
        complete: function () {
            //HideGlobalLodingPanel();
        }
    });
    $("#AddTranslator").modal("hide");
}

//Edit Existing Translator Manage Access Section
function EditTranslatorData(id) {
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
        },
        failure: function (result) {
            alert("Something went wrong.");
        },
        error: function (result) {
            alert("Something went wrong.");
        },
        complete: function () {
            //HideGlobalLodingPanel();
        }
    });
}

//Delete User From User List In Manage Access
function DeleteTranslator(id) {
    if (confirm('Are you sure ?')) {
        $(this).prev('span.text').remove();
        $.ajax({
            url: "/api/ManageAccess/DeleteTranslator?Id=" + id + "",
            async: false,
            type: "POST",
            data: JSON,
            contentType: "application/json; charset=utf-8",
            success: function (result) {
                if (result)
                    alert('Translator has been Delete successfully.');
                else
                    alert('Something went wrong. Please try again');

                $("#Section4").load('ManageAccess/GetTranslatorList');
            },
            failure: function (result) {
                alert("Something went wrong.");
            },
            error: function (result) {
                alert("Something went wrong.");
            },
            complete: function () {
                //HideGlobalLodingPanel();
            }
        });
    }
    else
        return false;
}

//////////////////////////////////////////////////////Translator Add, Delete, Edit part End///////////////////////////////////////////////////////////

///////////////////////////////////////////////////////HSCode Tree Bind start//////////////////////////////////////////////////////////////////////////////

//Bind HSCode Api Below
function GetHSCode() {
    debugger;
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
            alert("Something went wrong.");
        },
        error: function (result) {
            alert("Something went wrong.");
        },
        complete: function () {
            //HideGlobalLodingPanel();
        }
    });
}

//JSTree Search function
function SearchHSCode() {
    debugger;
    var searchString = $("#HSCodeSearchTxt").val();
    $('#HSCodeTree').jstree('search', searchString);
}

//JSTree clear Search function
function clearSearchtxt() {
    document.getElementById('HSCodeSearchTxt').value = '';
    $('#searchhscodebtn').click();
}

///////////////////////////////////////////////////////HSCode Tree Bind End//////////////////////////////////////////////////////////////////////////////

////////////////////////////////////////////////////// Stackholder HSCode Tree bind ////////////////////////////////////////////////////////////////////

//select stskeholder popup show code
function selectstakeholderHscode() {
    $("#AddStakeholder").modal("hide");
    $("#selectstakeholderHscode").modal("show");
}

//Open AddStakeholder popup
function openAddStakeholder() {
    $("#selectstakeholderHscode").modal("hide");
    $("#selectstakeholderHscode").on('hidden.bs.modal', function () {
        $("#AddStakeholder").modal("show");
    });
}

//GetStackholderHSCode popup code
function GetStackholderHSCode() {
    debugger;
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
            alert("Something went wrong.");
        },
        error: function (result) {
            alert("Something went wrong.");
        },
        complete: function () {
            //HideGlobalLodingPanel();
        }
    });
}

//selectstakeholderHscode select code popup code
function SelectHSCode() {
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
    if ($('.jstree-anchor.jstree-clicked').length == 0) {
        alert('Please select atleast one HSCode');
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
