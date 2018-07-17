$(document).ready(function () {
    //Allow only numberic charaters
    $(document).on('keypress', '.numeric', function (e) {
        var unicode = e.charCode ? e.charCode : e.keyCode
        if (unicode != 8) {
            if ((unicode < 48 || unicode > 57)) {
                return false;
            }
        }
    });

    //allow numberic value on paste
    $(document).on('paste', '.numeric', function (e) {
        var str = e.originalEvent.clipboardData.getData('text');
        var regex = new RegExp("^[0-9]*$");
        if (!regex.test(str)) {
            Alert("", "Numeric value allowed only.<br/>", "Ok");
            $(this).val('');
            return false;
        }

    });

    //allows alphanumeric value only 
    $(document).on('keypress', '.alphanumericonly', function (e) {
        var regex = new RegExp("^[a-zA-Z0-9]+$");
        var str = String.fromCharCode(!e.charCode ? e.which : e.charCode);
        if (regex.test(str)) {
            return true;
        }

        e.preventDefault();
        return false;
    });

    $(document).on('paste', '.alphanumericonly', function (e) {
        var str = e.originalEvent.clipboardData.getData('text');
        var regex = new RegExp("^[a-zA-Z0-9]+$");
        if (!regex.test(str)) {
            Alert("", "Alphanumeric value allowed only.<br/>", "Ok");
            $(this).val('');
            return false;
        }
    });

    //allow decimal value only
    $(document).on('keypress', '.decimal', function (e) {
        var WordLength = $.trim($(this).val()).length;
        var IsDeciamlValue = $.trim($(this).val()).split('.').length > 1 ? false : true;
        var unicode = e.charCode ? e.charCode : e.keyCode

        if (unicode != 8) {
            if (((unicode < 48 || unicode > 57) && unicode != 46)) {
                return false;
            }
            else if (unicode == 46 && !IsDeciamlValue)
                return false;
        }

        if (WordLength > 5 && $.trim($(this).val()).indexOf('.') < 0 && unicode != 46)
            return false;
    });

    $(document).on('paste', '.decimal', function (e) {
        var WordLength = $.trim($(this).val()).length;
        var IsDecimalValue = $.trim($(this).val()).split('.').length > 1 ? false : true;

        var str = e.originalEvent.clipboardData.getData('text');
        var regex = new RegExp("^[0-9]+(\.[0-9]{1,2})?$");///^[0-9]+\.?[0-9]*$/
        if (!regex.test(str)) {
            Alert("", "Decimal value allowed only.<br/>", "Ok");
            $(this).val('');
            return false;
        }
        else if (str.split('.')[0].length > 6) {
            Alert("", "Maximum 6 digit allowed only.<br/>", "Ok");
            $(this).val('');
            return false;
        }

    });

    //decimal value upto 2 place only
    $(document).on('change', 'input[type=text].decimal', function (e) {
        if (!Number.isInteger(parseFloat($(this).val())) && !isNaN(parseFloat($(this).val()))) {
            $(this).val(parseFloat($(this).val()).toFixed(2));
        }
    });

    //No special Character
    $(document).on('keypress', '.NoSpecialChar', function (e) {
        var regex = new RegExp("^[a-zA-Z0-9_ ]+$");
        var str = String.fromCharCode(!e.charCode ? e.which : e.charCode);
        if (regex.test(str)) {
            return true;
        }

        e.preventDefault();
        return false;
    });

    $(document).on('paste', '.NoSpecialChar', function (e) {
        var str = e.originalEvent.clipboardData.getData('text');
        var regex = new RegExp("^[a-zA-Z0-9_ ]+$");
        if (!regex.test(str)) {
            Alert("", "Alphanumeric value allowed only.<br/>", "Ok");
            $(this).val('');
            return false;
        }
    });
});

//Alpha character only
function isAlpha(evt) {
    var keyCode = (evt.which) ? evt.which : event.keyCode;
    if (!((keyCode >= 65 && keyCode <= 90) || (keyCode == 8) || (keyCode == 9) || (keyCode >= 97 && keyCode <= 122) || (keyCode == 32)))
    { Alert("", "Only alphabets are allowed.<br/>", "Ok"); return false; }
}

//Mobile no validation
function IsMobileNumberReg(mobileno) {
    var mob = /^[5-9]{1}[0-9]{9}$/;
    if (mob.test(mobileno) == false)
    { return false; }
    return true;
}

//email validation
function validateEmail(email) {
    var re = /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    return re.test(email);
}

//Ailpha numeric validation
function IsAlphaNumber(evt) {
    var keyCode = (evt.which) ? evt.which : event.keyCode;
    if (!((keyCode >= 65 && keyCode <= 90) || (keyCode == 8) || (keyCode == 9) || (keyCode >= 97 && keyCode <= 122) || (keyCode == 32) || (keyCode == 45) || (keyCode >= 48 && keyCode <= 57) || keyCode == 13)) {
        Alert("", "Only alphabets and numbers are allowed.<br/>", "Ok");
        return false;
    }
}

//Numeric Validation
function isNumeric(evt) {
    //if the letter is not digit then display error and don't type anything
    if (evt.which != 8 && evt.which != 0 && (evt.which < 48 || evt.which > 57)) {
        Alert("", "Only numbers are allowed.<br/>", "Ok");
        return false;
    }
};

//Add HSCode Save button validation
function HSCodeValidation() {
    var msg = "";

    if ($('[id$=HSCodetextbox]').val().trim().length == 0) {
        msg += "Please enter HSCode.<br/>";
    }

    if ($('[id$=HSCodeProductName]').val().trim().length == 0) {
        msg += "Please enter product name.<br/>";
    }

    if ($('[id$=HSCodeStatus]').val() == -1) {
        msg += "Please select status.<br/>"
    }

    if ($('[id$=HSCodeDescription]').val().trim().length == 0) {
        msg += "Please enter product description.<br/>";
    }

    if (msg.length > 0) {
        Alert("HS Codes", msg, "Ok");
        return false;
    }
}

function onlyEmailsWithSemicolon(evt) {
    var keyCode = (evt.which) ? evt.which : event.keyCode;
    var IsInvalid = false;
    if (!((keyCode >= 65 && keyCode <= 90) || (keyCode == 8) || (keyCode == 9) || (keyCode >= 97 && keyCode <= 122) || (keyCode == 32) || (keyCode == 45) || (keyCode >= 48 && keyCode <= 57))) {
        IsInvalid = true;
    }

    if (keyCode == 46 || keyCode == 59 || keyCode == 64 || keyCode == 95)
        IsInvalid = false;

    if (IsInvalid) {
        //Alert("", "Only alphabets and numbers are allowed.<br/>", "Ok");
        return false;
    }
}