$(document).ready(function () {

    GetHSCode();

    $('#NotifyingCountryId').combobox();

    //Title Word Word validation
    $("#TitleId").on('keyup', function (e) {
        var words = this.value.match(/\S+/g).length;
        if (words > 100) {
            // Split the string on first 200 words and rejoin on spaces
            var trimmed = $(this).val().split(/\s+/, 100).join(" ");
            // Add a space at the end to keep new typing making new words
            $(this).val(trimmed + " ");
            if (e.which != 8) {
                return false;
            }
        }
        else {
            $('#display_count').text(words);
            $('#word_left').text(100 - words);
        }
    });

    //Agency Responsible Word Word validation
    $("#AgencyResponsibleId").on('keyup', function (e) {
        var words = this.value.match(/\S+/g).length;
        if (words > 150) {
            // Split the string on first 150 words and rejoin on spaces
            var trimmed = $(this).val().split(/\s+/, 150).join(" ");
            // Add a space at the end to keep new typing making new words
            $(this).val(trimmed + " ");
            if (e.which != 8) {
                return false;
            }
        }
        else {
            $('#display_count1').text(words);
            $('#word_left1').text(150 - words);
        }
    });

    //Agency Responsible Word Word validation
    $("#ProductsCoveredId").on('keyup', function (e) {
        var words = this.value.match(/\S+/g).length;
        if (words > 150) {
            // Split the string on first 150 words and rejoin on spaces
            var trimmed = $(this).val().split(/\s+/, 150).join(" ");
            // Add a space at the end to keep new typing making new words
            $(this).val(trimmed + " ");
            if (e.which != 8) {
                return false;
            }
        }
        else {
            $('#display_count2').text(words);
            $('#word_left2').text(150 - words);
        }
    });

    //Description of ContentId Word Word validation
    $("#DescriptionofContentId").on('keyup', function (e) {
        var words = this.value.match(/\S+/g).length;
        if (words > 300) {
            // Split the string on first 300 words and rejoin on spaces
            var trimmed = $(this).val().split(/\s+/, 300).join(" ");
            // Add a space at the end to keep new typing making new words
            $(this).val(trimmed + " ");
            if (e.which != 8) {
                return false;
            }
        }
        else {
            $('#display_count3').text(words);
            $('#word_left3').text(300 - words);
        }
    });

    //Do You Have a Detailed Notification file upload
    $(".fileholder1").change(function () {
        debugger;
        var filename = $(this).val().substring(12);
        $("#choose1").text(filename);
        var ext = $('#choose1').text().split('.').pop().toLowerCase();
        if ($.inArray(ext, ['docx', 'doc', 'pdf']) == -1) {
            $("#choose1").text('No File Choose');
            alert('Only Doc and Pdf file is valid');
            return false;
        }
    });

});

//validate Notification section

function Validate() {
    var MSG = "";

    //Notification Type Id validation
    if ($('#NotificationTypeId').find('input[type=checkbox]:checked').length == 0) {
        MSG += 'Please select atleast one Notification Type \n';
    }

    //Notification Number id Validation
    if ($('[id$=NotificationNumberId]').val().trim().length == 0) {
        $('[id$=NotificationNumberId]').addClass("error");
        MSG += "Notification Number Not be Empty \n";
    }
    else {
        $('[id$=NotificationNumberId]').removeClass("error");
    }

    //Notification status Validation
    if ($('[id$=NotificationStatusId]').val() == '') {
        $('[id$=NotificationStatusId]').addClass("error");
        MSG += "Please select one status \n";
    }
    else {
        $('[id$=NotificationStatusId]').removeClass("error");
    }

    //Date of Notification id Validation
    if ($('[id$=DateofNotificationId]').val().trim().length == 0) {
        $('[id$=DateofNotificationId]').addClass("error");
        MSG += "Please select Date of Notification \n";
    }
    else {
        $('[id$=DateofNotificationId]').removeClass("error");
    }

    //Final Date for Comments id Validation
    if ($('[id$=FinalDateforCommentsId]').val().trim().length == 0) {
        $('[id$=FinalDateforCommentsId]').addClass("error");
        MSG += "Please select Final Date OF Comments \n";
    }
    else {
        $('[id$=FinalDateforCommentsId]').removeClass("error");
    }

    //Send Response By id Validation
    if ($('[id$=SendResponseById]').val().trim().length == 0) {
        $('[id$=SendResponseById]').addClass("error");
        MSG += "Please Select Send Responce By Date \n";
    }
    else {
        $('[id$=SendResponseById]').removeClass("error");
    }

    //Notifying Country validation
    if ($('.combobox-container input').val() == 0) {
        $('.combobox-container input').addClass("error");
        $('.combobox-container .input-group .input-group-addon').addClass("error");
        MSG += "Please select one Country \n";
    }
    else {
        $('.combobox-container input').removeClass("error");
        $('.combobox-container .input-group .input-group-addon').removeClass("error");
    }

    //Title Id Validation
    if ($('[id$=TitleId]').val().trim().length == 0) {
        $('[id$=TitleId]').addClass("error");
        MSG += "Provide Suitable Title \n";
    }
    else {
        $('[id$=TitleId]').removeClass("error");
    }

    //Agency Responsible Id Validation
    if ($('[id$=AgencyResponsibleId]').val().trim().length == 0) {
        $('[id$=AgencyResponsibleId]').addClass("error");
        MSG += "Provide Agency Responsible Details \n";
    }
    else {
        $('[id$=AgencyResponsibleId]').removeClass("error");
    }

    //Notification under Artical validation
    if ($('[id$=NotificationUnderArticleId]').val().trim().length == 0) {
        $('[id$=NotificationUnderArticleId]').addClass("error");
        MSG += "Provide Notification Artical Code \n";
    }
    else {
        $('[id$=NotificationUnderArticleId]').removeClass("error");
    }

    //Products Covered Id validation
    if ($('[id$=ProductsCoveredId]').val().trim().length == 0) {
        $('[id$=ProductsCoveredId]').addClass("error");
        MSG += "Product Cover Details \n";
    }
    else {
        $('[id$=ProductsCoveredId]').removeClass("error");
    }

    //Hs code table validation
    var rowCount = $('#HSCodesId>tbody>tr').length;
    if ($('#HSCodesId>tbody>tr').length == 0) {
        MSG += "HSCodes not be Empty \n";
    }

    //Description of Content Id validation
    if ($('[id$=DescriptionofContentId]').val().trim().length == 0) {
        $('[id$=DescriptionofContentId]').addClass("error");
        MSG += "Provide Discription \n";
    }
    else {
        $('[id$=DescriptionofContentId]').removeClass("error");
    }

    //Document Accordian Validation    
    if ($('input[name=Documents]:checked').length <= 0) {
        alert("No radio checked")
    }
    else
        alert($('input[name=Documents]:checked').val());

    //upload file validation (for document section)
    if ($('[id$=choose1]').text().trim() == "No File Choose") {
        $('[id$=choose1]').removeClass("error");
        MSG += "Please Select atleast one File \n";
    }
    else {
            $('[id$=choose1]').removeClass("error");
    }

    //select Language Validation
    //if ($('[id$=SelectLanguageId]').val() == -1) {
    //    $('[id$=SelectLanguageId]').addClass("error");
    //    MSG += "Please select Language \n";
    //}
    //else{
    //    $('[id$=SelectLanguageId]').removeClass("error");
    //}

    //select Translator Validation
    //if ($('[id$=transletorDrop]').text() == "-- Select Translator --") {
    //    $('[id$=transletorDrop]').addClass("error");
    //    MSG += "Please select Translator \n";
    //}
    //else{
    //    $('[id$=transletorDrop]').removeClass("error");
    //}

    //remainder for translation id Validation

    //if ($('[id$=remainder]').val().trim().length == 0) {
    //    $('[id$=remainder]').addClass("error");
    //    MSG += "Please select Date of Remainder \n";
    //}
    //else{
    //    $('[id$=remainder]').removeClass("error");
    //}

    //Duedate of Translation id Validation

    //if ($('[id$=duedate]').val().trim().length == 0) {
    //    $('[id$=duedate]').addClass("error");
    //    MSG += "Please select duedate Date of Translation \n";
    //}
    //else{
    //    $('[id$=duedate]').removeClass("error");
    //}

    if (MSG.length > 0) {
        alert(MSG);
    }
    else
        SaveUpdateNotification();
}

//Save Notification Section
function SaveUpdateNotification() {
    var NotType = '';
    $.each($('#NotificationTypeId').find('input[type=checkbox]:checked'), function () {
        NotType += $(this).val() + ',';
    });
    var obj = {
        UserId: 1,
        NotificationId: $('[id$=hdnNotificationId]').val(),
        Noti_Number: $('[id$=NotificationNumberId]').val().trim(),
        Noti_Type: NotType,
        Noti_Status: $('[id$=NotificationStatusId]').val(),
        Noti_Date: $('[id$=DateofNotificationId]').val().trim(),
        FinalDateOfComments: $('[id$=FinalDateforCommentsId]').val().trim(),
        SendResponseBy: $('[id$=SendResponseById]').val().trim(),
        Noti_Country: $('[id$=NotifyingCountryId]').val(),
        Title: $('[id$=TitleId]').val().trim(),
        ResponsibleAgency: $('[id$=AgencyResponsibleId]').val().trim(),
        Noti_UnderArticle: $('[id$=NotificationUnderArticleId]').val(),
        ProductsCovered: $('[id$=ProductsCoveredId]').val().trim(),
        HSCodes: $('[id$=HiddenHSCode]').val().trim(),
        Description: $('[id$=DescriptionofContentId]').val()
    };
    $.ajax({
        url: "/api/AddUpdateNotification/InsertUpdate_Notification",
        async: false,
        type: "POST",
        data: JSON.stringify(obj),
        contentType: "application/json; charset=utf-8",
        success: function (result) {
            if (result.Status == 'success')
                alert('Notification has been saved successfully.');
            else if (result.Status == 'failure')
                alert('Something went wrong. Please try again');

            var url = $("#RedirectTo").val();
            location.href = url;
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
    if ($('.jstree-anchor.jstree-clicked').length == 0) {
        alert('Please select atleast one HSCode');
        $("#HSCodesId tbody > tr").remove();
        var rowCount = $('#HSCodesId>tbody>tr').length;
        if ($('#HSCodesId>tbody>tr').length == 0) {
            $(".hs-code-table").addClass("hidden");
            $("#HiddenHSCode").val('');
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
        $.each(FinalArray, function (i, nodeId) {
            items += nodeId + ',';
            //var text = $('#' + nodeId).text();
            var text = $('#' + nodeId).text().split(':')[1];
            row += "<tr class='" + nodeId + "'><td>" + nodeId + "</td><td>" + text + "</td><td><a href='#" + nodeId + "' id='" + nodeId + "' onclick='return RemoveHSCodeRow(this.id);' class='remove-icon'><span class='glyphicon glyphicon-remove'></span></a></td></tr>";
        });

        $("#HiddenHSCode").val(items);

        $("#HSCodesId tbody").append(row);
    }
    //$("#HiddenHSCode").val(id);
    console.log(HSCodeId);
    console.log(TotalIdArray);

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

    $('<div></div>').appendTo('body')
    .html('<div><h6> Do you want to remove this HSCode amoung selected HSCodes? </h6></div>')
    .dialog({
        modal: true, title: 'Delete message', zIndex: 10000, autoOpen: true,
        width: '350px', top: '50%', resizable: false,
        buttons: {
            Yes: function () {
                $('#HSCodeTree').jstree("uncheck_node", ClassName);
                $('.' + ClassName).remove();
                var UpdateHSCodeId = $("#HiddenHSCode").val().replace(ClassName, "").replace(",,", ",");
                $("#HiddenHSCode").val('');
                $("#HiddenHSCode").val(UpdateHSCodeId);
                $(this).dialog("close");
                var rowCount = $('#HSCodesId>tbody>tr').length;
                if ($('#HSCodesId>tbody>tr').length == 0) {
                    $(".hs-code-table").addClass("hidden");
                    return false;
                }
            },
            No: function () {

                $(this).dialog("close");
            }
        },
        close: function (event, ui) {
            $(this).remove();
        }
    });
}

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


//Bind HSCode on Edit Mode
function bindhscodeUId() {
    document.getElementById('HSCodeSearchTxt').value = '';
    var numbersArray = $('[id$=HiddenHSCode]').val().trim().split(',');
    $('#HSCodeTree').jstree(true).select_node(numbersArray);
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