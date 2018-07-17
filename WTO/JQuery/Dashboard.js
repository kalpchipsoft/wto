var DateFrom = '', DateTo = '';
var DateChartFrom = '';
var DateChartTo = '';
var monthNames = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];
$(function () {
    $(window).scrollTop('0px');
    GetHSCode();
    $('#btnMonthly').addClass('btngroupactive');
    $('.rightArrow').addClass('hidden');
    var HSCodes = $('#hdnSelectedHSCodes').val();
    var start = moment().subtract(29, 'days');
    var end = moment();
    function cb(start, end) {
        $('.texthead span').html(start.format('MMMM D, YYYY') + ' - ' + end.format('MMMM D, YYYY'));
        start = new Date(start);
        end = new Date(end);
        BindHsCode(start, end, HSCodes);
        BindProcessingStatus();
        BindActions();
        BindRequestResponse();
    }
    $('#reportrange').daterangepicker({
        startDate: start,
        endDate: end,
        ranges: {
            // 'Today': [moment(), moment()],
            //'Yesterday': [moment().subtract(1, 'days'), moment().subtract(1, 'days')],
            'Last 7 Days': [moment().subtract(6, 'days'), moment()],
            'Last 30 Days': [moment().subtract(29, 'days'), moment()],
            'This Month': [moment().startOf('month'), moment().endOf('month')],
            'Last Month': [moment().subtract(1, 'month').startOf('month'), moment().subtract(1, 'month').endOf('month')]
        }
    }, cb);
    cb(start, end);

});
google.charts.load("visualization", "1", { packages: ["corechart"] });
google.charts.load('current', { 'packages': ['corechart'] });
google.charts.setOnLoadCallback(drawChart);
google.charts.setOnLoadCallback(drawPieChart);
$('#google-visualization-errors-0').css('display', 'none');

function SetCallFor(CallFor) {
    localStorage.setItem("CallFrom", "Dashboard");
    localStorage.setItem("Status", CallFor);
}

function BindHsCode(startdate, enddate, HSCodes) {
    var month = monthNames[startdate.getMonth()];
    var year = startdate.getFullYear();
    DateFrom = (startdate.getDate() > 9 ? startdate.getDate() : '0' + startdate.getDate()) + ' ' + month + ' ' + year;
    var month = monthNames[enddate.getMonth()];
    var year = enddate.getFullYear();
    DateTo = (enddate.getDate() > 9 ? enddate.getDate() : '0' + enddate.getDate()) + ' ' + month + ' ' + year;
    var divHSCode = '';
    $.ajax({
        url: "/api/Dashboard/WTOGetHSCodeData",
        async: false,
        type: "POST",
        data: JSON.stringify({
            HSCode: HSCodes,
            DateFrom: DateFrom,
            DateTo: DateTo
        }),
        contentType: "application/json; charset=utf-8",
        success: function (result) {
            $(result).each(function (index, value) {

                if (index % 3 == 0) {
                    divHSCode += "<div class='row top-offset-20'>";
                }
                divHSCode += '<div class="col-xs-12 col-ms-6 col-md-4 sm-top-offset-20">';
                var Hscode = value.HSCode;
                if (value.NotificationCount > 0) {
                    if (Hscode == "0") {
                        divHSCode += '<a class="hscodelink active" data-HScode="' + Hscode + '" onclick="callpiechart(this);">';
                    }
                    else {
                        divHSCode += '<a class="hscodelink " data-HScode="' + Hscode + '" onclick="callpiechart(this);">';
                    }
                }

                divHSCode += '<div class="hscodeheader text-center">';
                divHSCode += '<p>' + value.Text + '</p></div>';
                divHSCode += '<div class="hscodebody text-center">';
                divHSCode += '<p>' + value.NotificationCount + '</p></div> </a></div>';
                if (index % 3 == 2) {
                    divHSCode += "</div>";
                }

            })
            if (divHSCode.length > 0)
                $('#divHSCode').html(divHSCode);
            else {
                divHSCode += '<div class="row top-offset-20 text-center">';
                divHSCode += '<img src="/contents/img/NoDataAvailable.png" class="NoDataAvailable" alt="NoDataAvailable" /></div>';
                $('#divHSCode').html(divHSCode);
            }
            BindPieChart('0', DateFrom, DateTo);
        }
    });
}

function drawChart() {
    var data = google.visualization.arrayToDataTable(arrayNotificationGraph);

    var options = {
        chartArea: { width: "93%", height: "60%", top: '12%' },
        colors: ['#666766', '#f9c851', '#599bff', '#0b9a41'],
        legend: 'none',
        isStacked: true,
        hAxis: {
            direction: 1,
            slantedText: true,
            slantedTextAngle: 45 // here you can even use 180 
        },
    };
    var chart = new google.visualization.ColumnChart(document.getElementById('chart_div'));
    function GotoNotificationList() {
        var selectedItem = chart.getSelection()[0];
        if (selectedItem) {
            var value = data.getValue(selectedItem.row, selectedItem.column);
            var FromDate = arrayNotificationGraphDate[selectedItem.row + 1][1];

            var date = new Date(FromDate);
            var month = monthNames[date.getMonth()];
            var year = date.getFullYear();
            FromDate = (date.getDate() > 10 ? date.getDate() : '0' + date.getDate()) + ' ' + month + ' ' + year;

            var ToDate = arrayNotificationGraphDate[selectedItem.row + 1][2];

            var date = new Date(ToDate);
            var month = monthNames[date.getMonth()];
            var year = date.getFullYear();
            ToDate = date.getDate() + ' ' + month + ' ' + year;
            if (selectedItem.column == 1)
                window.location.href = '/NotificationList?PageIndex=1&PageSize=10&DateofNotification_From=' + FromDate + '&DateofNotification_To=' + ToDate + '&StatusId=20';
            if (selectedItem.column == 2)
                window.location.href = '/NotificationList?PageIndex=1&PageSize=10&DateofNotification_From=' + FromDate + '&DateofNotification_To=' + ToDate + '&StatusId=8';
            if (selectedItem.column == 3)
                window.location.href = '/NotificationList?PageIndex=1&PageSize=10&DateofNotification_From=' + FromDate + '&DateofNotification_To=' + ToDate + '&StatusId=5';
            if (selectedItem.column == 4)
                window.location.href = '/NotificationList?PageIndex=1&PageSize=10&DateofNotification_From=' + FromDate + '&DateofNotification_To=' + ToDate + '&StatusId=7';
        }
    }
    function AddHandlerCursor() {
        $('#chart_div').css('cursor', 'pointer')
    }
    function RemoveHandlerCursor() {
        $('#chart_div').css('cursor', 'default')
    }
    google.visualization.events.addListener(chart, 'click', GotoNotificationList);
    google.visualization.events.addListener(chart, 'onmouseover', AddHandlerCursor);
    google.visualization.events.addListener(chart, 'onmouseout', RemoveHandlerCursor);
    chart.draw(data, options);
}

function drawPieChart() {
    var colorpie = [];
    for (var i = 0; i < colors.length; i++) {
        colorpie.push(colors[i][1]);
    }
    var data = google.visualization.arrayToDataTable(arrayHScode);
    var options = {
        // is3D: true,
        legend: { position: 'none', alignment: 'center' },
        tooltip: { textStyle: { color: '#000', fontSize: 15 }, fill: 'black', showColorCode: true },
        //  colors: ['#f9c851' ,'#fdd470' ,'#5edca3' ,'#599bff' ,'#fbe1a1' ,'#9fc4fc' ,'#f5707a' ,'#5edca3' ,'#73fdbf' ,'#fd989f' ,'#ffc5c9']
        //  colors:[colors], 
        colors: colorpie,
    };
    var chart = new google.visualization.PieChart(document.getElementById('piechart'));
    // The select handler. Call the chart's getSelection() method
    function selectHandler() {
        debugger;
        var selectedItem = chart.getSelection()[0];
        if (selectedItem) {
            var selectedvalue = data.getValue(selectedItem.row, 0);
            $(arrayCountryCode).each(function (index, value) {
                if ($(value)[0] == selectedvalue)
                    window.location.href = "/WTO/NotifyingMemberList?PageIndex=1&PageSize=10&&Hscode=" + $('[id$=hdnHscode]').val() + "&FromDate=" + DateFrom + "&ToDate=" + DateTo + "&CountryId=" + value[1];
            })
        }
    }
    function AddHandlerCursor() {
        $('#chart_div').css('cursor', 'pointer')
    }
    function RemoveHandlerCursor() {
        $('#chart_div').css('cursor', 'default')
    }

    // Listen for the 'select' event, and call my function selectHandler() when
    // the user selects something on the chart.
    google.visualization.events.addListener(chart, 'select', selectHandler);
    google.visualization.events.addListener(chart, 'onmouseover', AddHandlerCursor);
    google.visualization.events.addListener(chart, 'onmouseout', RemoveHandlerCursor);
    chart.draw(data, options);
    $('#piechart').fadeIn();
}

//Bind HSCode Api Below
function GetHSCode() {
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
            Alert("Alert", "Something went wrong.<br/>", "Ok");
        },
        error: function (result) {
            Alert("Alert", "Something went wrong.<br/>", "Ok");
        }
    });
}

//Bind HSCode on Edit Mode
function bindhscodeUId() {
    document.getElementById('HSCodeSearchTxt').value = '';
    var numbersArray = $('[id$=hdnSelectedHSCodes]').val().trim().split(',');
    $('#HSCodeTree').jstree(true).select_node(numbersArray);
}

//JSTree Search function
function SearchHSCode() {
    var searchString = $("#HSCodeSearchTxt").val();
    $('#HSCodeTree').jstree('search', searchString);
}

//JSTree clear Search function
function clearSearchtxt() {
    document.getElementById('HSCodeSearchTxt').value = '';
    $('#searchhscodebtn').click();
}

function callpiechart(ctrl) {
    $('.hscodelink').removeClass('active');
    $(ctrl).addClass('active');

    BindPieChart($(ctrl).attr('data-HScode'), DateFrom, DateTo);
}

function BindPieChart(HSCode, DateFrom, DateTo) {
    $.ajax({
        url: "/api/Dashboard/WTOGetHSCodeDataByCountry",
        async: false,
        type: "POST",
        data: JSON.stringify({
            HSCode: HSCode,
            DateFrom: DateFrom,
            DateTo: DateTo
        }),
        contentType: "application/json; charset=utf-8",
        success: function (result) {
            var TotalNotificationCount = 0;
            var hscodelegend = '<div class="col-xs-12 col-sm-12 col-md-12 padding-right-0">';
            hscodelegend += '<div class="row">';
            arrayHScode = [];
            colors = [];
            arrayCountryCode = [];
            arrayCountryCode.push(['CountryCode', 'CountryId']);
            arrayHScode.push(['Task', 'HSCode', { role: "style" }]);
            $(result.objHSCodeCountryData).each(function (index, value) {
                if (value.NotificationCount > 0) {
                    BindArray(value.Country, value.NotificationCount, value.ColorCode);
                    BindArrayCountryCode(value.Country, value.CountryId);
                    hscodelegend += '<div class="col-xs-6 col-sm-4">';
                    hscodelegend += '<div class="legend" style="background:' + value.ColorCode + '"></div>';
                    hscodelegend += '<div class="countryName">';
                    hscodelegend += '<p style="cursor:pointer" data-toggle="tooltip" data-placement="bottom" title="' + value.Country + '">' + value.CountryCode + '</p>';
                    hscodelegend += '</div></div>';
                    TotalNotificationCount += value.NotificationCount;
                }
            })
            hscodelegend + ' </div></div>';
            var divhscode = '<p>For HS Code ' + result.HSCode + '</p><p>Notifications &nbsp;&nbsp;&nbsp; <a href="/NotificationList?PageIndex=1&PageSize=10&&Hscode=' + result.HSCode + '&DateofNotification_From=' + DateFrom + '&DateofNotification_To=' + DateTo + '">' + TotalNotificationCount + '</a>&nbsp;&nbsp;&nbsp; from &nbsp;&nbsp;&nbsp;<a href="/WTO/NotifyingMemberList?PageIndex=1&PageSize=10&&Hscode=' + result.HSCode + '&FromDate=' + DateFrom + '&ToDate=' + DateTo + '"> ' + result.CountryCount + '</a> &nbsp;&nbsp;&nbsp; Countries  </p>  <input type="hidden"  id="hdnHscode" value="' + result.HSCode + '"/>';
            $('.hscodecount').html(divhscode);
            $('.hscodelegend').html(hscodelegend);

            google.charts.setOnLoadCallback(drawPieChart);
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
        Alert("Alert", "Please select atleast one HSCode.<br/>", "Ok");
        $("#HSCodesId tbody > tr").remove();
        var rowCount = $('#HSCodesId>tbody>tr').length;
        if ($('#HSCodesId>tbody>tr').length == 0) {
            $(".hs-code-table").addClass("hidden");
            $("#hdnSelectedHSCodes").val('');
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
            items += nodeId + '|';
            var text = $('#HSCodeTree').jstree().get_node(nodeId).text.split(':')[1].trim();
            var Onclick = 'RemoveHSCodeRow(this)';
            row += "<tr class='" + nodeId + "'><td>" + nodeId + "</td><td>" + text + "</td><td><a id='" + nodeId + "' onclick='" + Onclick + "' class='remove-icon'><span class='glyphicon glyphicon-remove'></span></a></td></tr>";
        });
        $("#HSCodesId thead").removeClass('hidden');

        $("#hdnSelectedHSCodes").val(items);
        $("#HSCodesId tbody").append(row);
    }

    $("#HSCodesId tbody > tr").each(function () {
        var txt = $(this).text();
        if (seen[txt])
            $(this).remove();
        else
            seen[txt] = true;
    });
    var hdnSelectedHscode = $('#hdnSelectedHSCodes').val().split("|");
    var hscodecount = 0;
    for (var i = 0; i < hdnSelectedHscode.length; i++) {
        if (hdnSelectedHscode[i].length > 0)
            hscodecount++;
    }

    if (hscodecount > 10) {
        Alert('Alert', 'You can select only 10 hs code.<br/>', 'Ok');
    }
    else {
        $(".hs-code-table").removeClass("hidden");
        $('#selecthr').modal('hide');
        var start = new Date(DateFrom);
        var end = new Date(DateTo);
        BindHsCode(start, end, $('#hdnSelectedHSCodes').val());
    }
}

//Remove HSCode table tr
function RemoveHSCodeRow(ClassName) {
    DeselectHSCodeNode(ClassName);
}

//Remove HSCode table tr
function DeselectHSCodeNode(ctrl) {
    var id = '';
    id = ctrl.id;
    var ClassName = id;
    if (ClassName == "-1")
        $('#cbNoHSCode').prop('checked', false);
    else
        $('#HSCodeTree').jstree("uncheck_node", ClassName);

    $('.' + ClassName).remove();
    var UpdateHSCodeId = $("#hdnSelectedHSCodes").val().replace(ClassName, "").replace("||", "|");
    $("#hdnSelectedHSCodes").val('');
    $("#hdnSelectedHSCodes").val(UpdateHSCodeId);

    var rowCount = $('#HSCodesId>tbody>tr').length;
    if ($('#HSCodesId>tbody>tr').length == 0) {
        $(".hs-code-table").addClass("hidden");
        return false;
    }
}

function GetNotificationGraphDataWeekly() {
    $('#btnMonthly').removeClass('btngroupactive');
    $('#btnWeekly').removeClass('btngroupactive');
    $('#btnWeekly').addClass('btngroupactive');
    $('.rightArrow').addClass('hidden');
    var date = new Date();
    date = new Date(date.getFullYear(), date.getMonth(), 0);
    var month = monthNames[date.getMonth()];
    var year = date.getFullYear();
    DateChartFrom = date.getDate() + ' ' + month + ' ' + year;
    BindNotificationGraph(DateChartFrom);
}

function BindNotificationGraph(DateFr) {
    $.ajax({
        url: "/api/Dashboard/WTOGetNotificationGraphDataWeekly",
        async: false,
        type: "POST",
        data: JSON.stringify({
            DateFrom: DateFr
        }),
        contentType: "application/json; charset=utf-8",
        success: function (result) {
            arrayNotificationGraph = [];
            arrayNotificationGraph.push(['Notification', 'Lapsed', 'In Process', 'Under Action', 'Closed', { role: 'annotation' }]);
            arrayNotificationGraphDate = [];
            arrayNotificationGraphDate.push(['SrNo', 'StartDate', 'EndDate']);
            $(result).each(function (index, value) {
                BindArrayNotificationGraph(value.MonthName, value.LapsedCount, value.InProcessCount, value.UnderActionCount, value.ClosedCount);
                BindarrayNotificationGraphDate(index, value.StartDate, value.EndDate);
            })
            google.charts.setOnLoadCallback(drawChart);
        }
    });
}

function BindGraphPrevious() {
    if (DateChartFrom == '' && $('#btnMonthly').hasClass('btngroupactive')) {
        var date = new Date();
        var month = monthNames[date.getMonth() - 1];
        var year = date.getFullYear();
        DateChartFrom = date.getDate() + ' ' + month + ' ' + year;
        BindNotificationGraphMonthly(DateChartFrom);
    }
    if (DateChartFrom != '' && $('#btnMonthly').hasClass('btngroupactive')) {
        var myVariable = DateChartFrom;
        var date = new Date(myVariable);
        date = new Date(date.setMonth(date.getMonth() - 1));
        var month = monthNames[date.getMonth()];
        var year = date.getFullYear();
        DateChartFrom = date.getDate() + ' ' + month + ' ' + year;
        BindNotificationGraphMonthly(DateChartFrom);
    }
    if (DateChartFrom == '' && $('#btnWeekly').hasClass('btngroupactive')) {
        var date = new Date();
        var month = monthNames[date.getMonth()];
        var year = date.getFullYear();
        DateChartFrom = date.getDate() + ' ' + month + ' ' + year;
        BindNotificationGraph(DateChartFrom);
    }
    if (DateChartFrom != '' && $('#btnWeekly').hasClass('btngroupactive')) {
        var myVariable = DateChartFrom;
        var date = new Date(myVariable);
        date = new Date(date.getFullYear(), date.getMonth(), date.getDate() - 7);
        var month = monthNames[date.getMonth()];
        var year = date.getFullYear();
        DateChartFrom = date.getDate() + ' ' + month + ' ' + year;
        BindNotificationGraph(DateChartFrom);
        $('.rightArrow').removeClass('hidden');
    }
    DisplayPrevNext();
}

function BindGraphNext() {
    if (DateChartFrom == '' && $('#btnMonthly').hasClass('btngroupactive')) {
        var date = new Date();
        var month = monthNames[date.getMonth()];
        var year = date.getFullYear();
        DateChartFrom = date.getDate() + ' ' + month + ' ' + year;
        BindNotificationGraphMonthly(DateChartFrom);
        DisplayPrevNext();
    }
    if (DateChartFrom != '' && $('#btnMonthly').hasClass('btngroupactive')) {
        var myVariable = DateChartFrom;
        var date = new Date(myVariable);
        date = new Date(date.setMonth(date.getMonth() + 1));
        var month = monthNames[date.getMonth()];
        var year = date.getFullYear();
        DateChartFrom = date.getDate() + ' ' + month + ' ' + year;
        BindNotificationGraphMonthly(DateChartFrom);
        DisplayPrevNext();
    }
    if (DateChartFrom == '' && $('#btnWeekly').hasClass('btngroupactive')) {
        var date = new Date();
        var month = monthNames[date.getMonth()];
        var year = date.getFullYear();
        DateChartFrom = date.getDate() + ' ' + month + ' ' + year;
        BindNotificationGraph(DateChartFrom);
    }
    if (DateChartFrom != '' && $('#btnWeekly').hasClass('btngroupactive')) {
        debugger
        var myVariable = DateChartFrom;
        var date = new Date(myVariable);
        var currentdate = new Date();
        date = new Date(date.getFullYear(), date.getMonth(), date.getDate() + 7);
        var month = monthNames[date.getMonth()];
        var year = date.getFullYear();
        DateChartFrom = date.getDate() + ' ' + month + ' ' + year;
        BindNotificationGraph(DateChartFrom);

        var lastdate = new Date();
        lastdate = new Date(lastdate.getFullYear(), lastdate.getMonth(), 0);
        var month = monthNames[lastdate.getMonth()];
        var year = lastdate.getFullYear();
        lastdate = lastdate.getDate() + ' ' + month + ' ' + year;
        if (DateChartFrom == lastdate) {
            $('.rightArrow').addClass('hidden');
        }
        else {
            $('.rightArrow').removeClass('hidden');
        }
    }

    //   $('.rightArrow').removeClass('hidden');
}

function GetNotificationGraphDataMonthly() {
    $('#btnMonthly').removeClass('btngroupactive');
    $('#btnWeekly').removeClass('btngroupactive');
    $('#btnMonthly').addClass('btngroupactive');
    $('.rightArrow').addClass('hidden');
    var date = new Date();
    var month = monthNames[date.getMonth()];
    var year = date.getFullYear();
    DateChartFrom = date.getDate() + ' ' + month + ' ' + year;
    BindNotificationGraphMonthly(DateChartFrom);
}

function BindNotificationGraphMonthly(DateFr) {
    $.ajax({
        url: "/api/Dashboard/WTOGetNotificationGraphDataMonthly",
        async: false,
        type: "POST",
        data: JSON.stringify({
            DateFrom: DateFr
        }),
        contentType: "application/json; charset=utf-8",
        success: function (result) {
            arrayNotificationGraph = [];
            arrayNotificationGraph.push(['Notification', 'Lapsed', 'In Process', 'Under Action', 'Closed', { role: 'annotation' }]);
            arrayNotificationGraphDate = [];
            arrayNotificationGraphDate.push(['SrNo', 'StartDate', 'EndDate']);
            $(result).each(function (index, value) {
                BindArrayNotificationGraph(value.MonthName, value.LapsedCount, value.InProcessCount, value.UnderActionCount, value.ClosedCount);
                BindarrayNotificationGraphDate(index, value.StartDate, value.EndDate);
            })
            google.charts.setOnLoadCallback(drawChart);
        }
    });
}

function DisplayPrevNext() {
    var datecurrent = new Date();
    datecurrent.setMonth(datecurrent.getMonth() - 1);
    var month = monthNames[datecurrent.getMonth()];
    var year = datecurrent.getFullYear();
    datecurrent = datecurrent.getDate() + ' ' + month + ' ' + year;
    var datechartfrom = new Date(DateChartFrom);
    var month = monthNames[datechartfrom.getMonth()];
    var year = datechartfrom.getFullYear();
    datechartfrom = datechartfrom.getDate() + ' ' + month + ' ' + year;
    if (datecurrent == datechartfrom) { $('.rightArrow').addClass('hidden'); }
    else { $('.rightArrow').removeClass('hidden'); }
}

function BindProcessingStatus() {
    $.ajax({
        url: "/WTO/WTODashboardProcessingStatus",
        async: false,
        type: "POST",
        data: JSON.stringify({
            DateFrom: DateFrom,
            DateTo: DateTo
        }),
        dataType: 'html',
        contentType: "application/json; charset=utf-8",
        success: function (result) {
            $('#divProcessingStatus').html('');
            $('#divProcessingStatus').html(result);
        }
    });
}

function BindActions() {
    $.ajax({
        url: "/WTO/GetDashboardAction",
        async: false,
        type: "POST",
        data: JSON.stringify({
            DateFrom: DateFrom,
            DateTo: DateTo
        }),
        dataType: 'html',
        contentType: "application/json; charset=utf-8",
        success: function (result) {
            $('#divAction').html('');
            $('#divAction').html(result);
        }
    });
}

function BindRequestResponse() {
    $.ajax({
        url: "/WTO/WTODashboardRequestResponse",
        async: false,
        type: "POST",
        data: JSON.stringify({
            DateFrom: DateFrom,
            DateTo: DateTo
        }),
        dataType: 'html',
        contentType: "application/json; charset=utf-8",
        success: function (result) {
            $('#divRequestResponse').html('');
            $('#divRequestResponse').html(result);
        }
    });
}