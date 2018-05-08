function AddRemoveActions(ctrl) {
    if (ctrl.checked && ctrl.value == "4") {
        $.each($(ctrl).closest('tr').find('input[type=checkbox]'), function () {
            if (this.id == '' && this.value != "4")
                this.checked = false;
        });
    }
    else if (ctrl.checked && ctrl.value != "4") {
        $.each($(ctrl).closest('tr').find('input[type=checkbox]'), function () {
            if (this.id == '' && this.value == "4")
                this.checked = false;

            //if (this.id == '' && this.value == "3")
            //    this.checked = true;
        });
    }
}

var rowsSelected = 0;
var rowWiseMsg = '';
function GetMoMDetails() {
    var _MoMDetails = [];
    $.each($("#tblNotificationMoM > tbody > tr"), function (i, v) {
        if ($(this).find("#chkNotification_" + (i + 1)).is(':checked')) {
            var _NotificationId = parseInt($.trim($(this).find("#hdnNotificationId_" + (i + 1)).val()));
            $.each($(this).find('input[type=checkbox]:checked'), function () {
                if (this.id == '') {
                    var MoMDetails = {
                        NotificationId: _NotificationId,
                        ActionId: parseInt($(this).val())
                    };
                    _MoMDetails.push(MoMDetails);
                }
            });
        }
    });
    return _MoMDetails;
}

function Validate() {
    var msg = "";

    if ($.trim($('[id$=txtmeetingdate]').val()) == "")
        msg += "Please enter meeting date. <br/>";

    var MoMDetails = GetMoMDetails();
    var IsRowSelected = false;
    $.each($("#tblNotificationMoM > tbody > tr"), function (index, value) {
        if ($(this).find("#chkNotification_" + (index + 1)).is(':checked')) {
            IsRowSelected = true;
            var _NotificationId = parseInt($.trim($(this).find("#hdnNotificationId_" + (index + 1)).val()));
            var IsExists = false;
            $.each(MoMDetails, function (i, v) {
                if (v.NotificationId == _NotificationId)
                    IsExist = true;
            });

            //if (IsExists == false)
            //    msg += "Please select action(s) for " + $.trim($(this).find('td>p.NotiNumber').text()) + ". <br/>";
        }
    });

    if (IsRowSelected == false && $('[id$=hdnMoMId]').val() == 0)
        msg += "Please select notification(s) to add MoM. <br/>";

    if (msg.length > 0) {
        Alert("MOM", msg, "Ok");
    }
    else
        AddUpdateMoM();
}

function AddUpdateMoM() {
    var obj = {
        MoMId: $('[id$=hdnMoMId]').val(),
        MeetingDate: $.trim($('[id$=txtmeetingdate]').val()),
        MoMDetails: GetMoMDetails()
    };

    $.ajax({
        url: "/api/MOM/InsertUpdate_MomData/" + myWTOAPP.UserId,
        async: false,
        type: "POST",
        data: JSON.stringify(obj),
        contentType: "application/json; charset=utf-8",
        success: function (result) {
            if (result != null) {
                if ($('[id$=hdnMoMId]').val() > 0)
                    Alert("MOM", "MoM has been updated successfully.<br/>", "Ok");
                else
                    Alert("MOM", "MoM has been saved successfully.<br/>", "Ok");
            }
            location.href = "../MoM/";
        },
        failure: function (result) {
            Alert("MOM", "Something went wrong.<br/>", "Ok");
        },
        error: function (result) {
            Alert("MOM", "Something went wrong.<br/>", "Ok");
        },
        complete: function () {
            //HideGlobalLodingPanel();
        }
    });
}

function EditNotification(ctrl) {
    debugger;
    $(ctrl).closest('tr').find('span.glyphicon.glyphicon-ok.dark-green-color').addClass('hidden');
    $(ctrl).closest('tr').find('.checkbox.radio-margin.hidden').removeClass('hidden');
    $('#txtmeetingdate').removeClass('disabled');
    $('#txtmeetingdate').next().removeClass('disabled');
    $('#btnSaveUpdate').removeClass('hidden');
    //$(ctrl).addClass('hidden');
}