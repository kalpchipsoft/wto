
//Alpha character only
function isAlpha(evt) {
    var keyCode = (evt.which) ? evt.which : event.keyCode;
    if (!((keyCode >= 65 && keyCode <= 90) || (keyCode == 8) || (keyCode == 9) || (keyCode >= 97 && keyCode <= 122) || (keyCode == 32)))
    { alert('Only alphabets are allowed.'); return false; }
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
    if (!((keyCode >= 65 && keyCode <= 90) || (keyCode == 8) || (keyCode == 9) || (keyCode >= 97 && keyCode <= 122) || (keyCode == 32) || (keyCode == 45) || (keyCode >= 48 && keyCode <= 57))) {
        alert('Only alphabets and numbers are allowed.');
        return false;
    }
}

//Numeric Validation
function isNumeric(evt) {
    //if the letter is not digit then display error and don't type anything
    if (evt.which != 8 && evt.which != 0 && (evt.which < 48 || evt.which > 57)) {
        alert('Only numbers are allowed.');
        return false;
    }
};

//Add Country Validation
function CountryValidation() {
    var msg = "";

    if ($('[id$=CountryName]').val().trim().length == 0) {
        msg += "Enter Country Name \n";
    }

    if ($('[id$=CountryStatus]').val() == -1) {
        msg += "Please select one status \n"
    }

    if (msg.length > 0) {
        alert(msg);
    }
}

//Add HSCode Save button validation
function HSCodeValidation() {
    var msg = "";

    if ($('[id$=HSCodetextbox]').val().trim().length == 0) {
        msg += "Enter HSCode \n";
    }
    
    if ($('[id$=HSCodeProductName]').val().trim().length == 0) {
        msg += "Enter Product Name \n";
    }

    if ($('[id$=HSCodeStatus]').val() == -1) {
        msg += "Please select status \n"
    }

    if ($('[id$=HSCodeDescription]').val().trim().length == 0) {
        msg += "Enter Product Name \n";
    }
    
    if (msg.length > 0) {
        alert(msg);
    }

}