var myWTOAPP = {
    id: null,
    UserId: null,
    UserRole:null
};

(function (myWTOAPP) {
    myWTOAPP.init = function (id, uid,role) {
        myWTOAPP.id = (id == "" ? null : id);
        myWTOAPP.UserId = (uid == "" ? null : uid);
        myWTOAPP.UserRole = (role == "" ? null : role);
    }
})(myWTOAPP);

function ShowDashboardLodingPanel() {
    $(".LoadingPanel").css("display", "block");
}

function HideDashboardLodingPanel() {
    $(".LoadingPanel").css("display", "none");
}

$(window).bind("load", function () {
    HideGlobalLodingPanel();
});

////Temp
//$(window).unload(function () {
//    ShowGlobalLodingPanel();
//});

$(document).ajaxStart(function () {
    ShowGlobalLodingPanel();
});

$(document).ajaxComplete(function () {
    HideGlobalLodingPanel();
});

////End temp

function ShowGlobalLodingPanel() {
    $("#loader-wrapper").css("display", "block");
}

function HideGlobalLodingPanel() {
    $("#loader-wrapper").css("display", "none");
}


function ShowAlert() {
    var Alert = '';
    Alert += "<div class='modal fade selecthrmodal' id='AlertPopup' role='dialog' data-backdrop='static' data-keyboard='false> ";
    Alert += "<div class='modal-dialog selecthrdialog'>";
    Alert += "<div class='modal-content selecthrcontent AlertPopup'>";
    Alert += "<div class='modal-header selecthrheader'>";
    Alert += "<button type='button' class='close selecthrclose' data-dismiss='modal'><span class='glyphicon glyphicon-remove' onclick='EmptyAlertBody();'></span> </button>";
    Alert += "</div>";
    Alert += "<div class='modal-body selecthrbody'>";
    Alert += "<div class='row'>";
    Alert += "<div class='col-xs-12 col-sm-12 col-md-12'>";
    Alert += "<div class='col-xs-12 col-sm-12 col-md-12 selecthrbodyinner'>";
    Alert += "<div class='col-xs-12 col-sm-12 col-md-12'>"
    Alert += "<h4 id='AlertBodyheading'>Mandatory Field</h4>";
    Alert += "</div>";
    Alert += "<div id='AlertBody' style='color:red;'></div>";
    Alert += "<a href='#' class='btn btn-blue btn-padding pull-right top-offset-20 bottom-offset-20' data-dismiss='modal' onclick='EmptyAlertBody();'>OK</a>";
    Alert += "</div>";
    Alert += "</div>";
    Alert += "</div>";
    Alert += "</div>";
    Alert += "</div>";
    Alert += "</div>";
    Alert += "</div>";
    $("body").append(Alert);
    $("#AlertPopup").modal("show");
}

function EmptyAlertBody() {
    $("#AlertBody").empty();
}

//Added by Ashvini
function IsEmailExists(Email, CallFor) {
    var IsExists = false;
    $.ajax({
        url: "/api/Masters/IsEmailExists?Email=" + Email + "&callFor=" + CallFor,
        async: false,
        type: "GET",
        data: JSON,
        contentType: "application/json; charset=utf-8",
        success: function (result) {
            IsExists = result;
        },
        failure: function (result) {
            Alert("", "Something went wrong.<br/>", "Ok");
        },
        error: function (result) {
            Alert("", "Something went wrong.<br/>", "Ok");
        },
        complete: function () {
            HideGlobalLodingPanel();
        }
    });
    return IsExists;
}

