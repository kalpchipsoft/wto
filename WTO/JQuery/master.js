﻿var myWTOAPP = {
    id: null,
    UserId: null
};
(function (myWTOAPP) {
    myWTOAPP.init = function (id, uid) {
        myWTOAPP.id = (id == "" ? null : id);
        myWTOAPP.UserId = (uid == "" ? null : uid);
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

//Loading panel starts
//window.onbeforeunload = function () {
//    if ($('[id$=hdnNoProgress]').val() == '0') {
//        ShowGlobalLodingPanel();
//    }
//    else
//        $('[id$=hdnNoProgress]').val('0');
//}
//function pageLoaded() {
//    HideGlobalLodingPanel();
//}
function ShowGlobalLodingPanel() {
    $("#loader-wrapper").css("display", "block");
}
function HideGlobalLodingPanel() {
    $("#loader-wrapper").css("display", "none");
}
//function SetHiddenFieldValue() {
//    $('[id$=hdnNoProgress]').val('1');
//}
//Loading panel

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
    Alert += "<h4 id='AlertBodyheading'></h4>";
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