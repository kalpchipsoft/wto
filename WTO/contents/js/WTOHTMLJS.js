$(document).ready(function () {
				
    // <!-- file js start -->
		
    $(".fileholder1").change(function(){
        var filename = $(this).val().substring(12);
        $("#choose1").text(filename);
    });
    $(".fileholder").change(function(){
        var filename = $(this).val().substring(12);
        $("#choose").text(filename);
    });
		
    $( "#fromdate, #todate" ).datepicker({
        changeMonth: true,
        changeYear: true
    });
		
    $(".fileholder2").change(function(){
        var filename = $(this).val().substring(12);
        $("#choose").text(filename);
    });
		
		
    $(".uploadfile").change(function(){
        var filename = $(this).val().substring(12);
        $(".btn-file > .puttext").text(filename);
    });
		
    // <!-- file js End -->
	
    // <!-- tadatable js -->
	
    // <!-- $('#notificationlist').DataTable({ -->
    // <!-- "scrollY":"400px", -->
    // <!-- "scrollCollapse": true, -->
    // <!-- "ordering": false, -->
    // <!-- "paging":false, -->
    // <!-- }); -->
	
    // <!-- tadatable js End-->

    // <!-- js tree js -->
	
	
    $('#html').jstree({
        "checkbox" : {
            "keep_selected_style" : false
        },
        "plugins" : [ "checkbox"]
    });
		
    // <!-- js tree js End-->	
	
    // <!-- translatename js -->
	
    $(".translatename").click(function(){
      //  <!-- var name=$(this).parent().parent().find('.name').text(); -->
        var name=$(this).find('.name').text();
        $('.value').text(name).focus();
    });
			
    // <!-- translatename js End-->
	
    // <!-- datepicker js -->
	
    $("#DateofNotificationId, #FinalDateforCommentsId, #SendResponseById, #remainder, #duedate, #discussed, #Action, #txtfromdate,#txttodate").keydown(function (e) {
        if (e.which == 8) {
            return false;
        }
        else{
            return false;
        }
    });

    $("#DateofNotificationId, #FinalDateforCommentsId, #SendResponseById, #remainder, #duedate, #discussed, #Action, #Obtaindate, #txtfromdate,#txttodate").datepicker({
        changeMonth: true,
        changeYear: true,
        dateFormat: 'dd-MM-yy'
    });
	
    $("#DateofNotificationId").datepicker("setDate", "new Date()");
    //$("#FinalDateforCommentsId").datepicker({
    //    changeMonth: true,
    //    changeYear: true,
    //    minDate: 0,
    //    dateFormat: 'dd-MM-yy'
    //});
    //$("#SendResponseById").datepicker({
    //    changeMonth: true,
    //    changeYear: true,
    //    minDate: 0,
    //    dateFormat: 'dd-MM-yy'
    //});

    //$("#FinalDateforCommentsId").datepicker("minDate", new Date());
    //$("#SendResponseById").datepicker("setDate", "8-November-2017");
    //$( "#remainder" ).datepicker( "setDate", "6-October-2017" );
    //$( "#duedate" ).datepicker( "setDate", "9-October-2017" );
    $("#Obtaindate").datepicker("setDate", "9-October-2017");
    $( "#discussed" ).datepicker( "setDate", "20-November-2017" );
    $( "#Action" ).datepicker( "setDate", "20-Novembers-2017" );
	
    //Remainder date pick code 
    $("#FinalDateforCommentsId").change(function () {
        var tt = $('#FinalDateforCommentsId').val();
        var date = new Date(tt);
        var newdate = new Date(date);
        newdate.setDate(newdate.getDate() - 20);
        var dd = newdate.getDate();
        var month = new Array();
        month[1] = "January";
        month[2] = "February";
        month[3] = "March";
        month[4] = "April";
        month[5] = "May";
        month[6] = "June";
        month[7] = "July";
        month[8] = "August";
        month[9] = "September";
        month[10] = "October";
        month[11] = "November";
        month[12] = "December";
        var MM = month[newdate.getMonth() + 1];
        var y = newdate.getFullYear();
        var someFormattedDate = dd + '-' + MM + '-' + y;
        $("#SendResponseById").val(someFormattedDate);
    });

    var StartDate = $('#DateofNotificationId').val();

    // Send response by check
    $("#FinalDateforCommentsId").change(function () {
        var EndDate = document.getElementById('SendResponseById').value;
        var SendResponseDate = new Date(EndDate);
        var NotificationDateDate = new Date(StartDate);
        if (NotificationDateDate > SendResponseDate) {
            $("#SendResponseError").removeClass("hidden");
            return false;
        }
        else {
            $("#SendResponseError").addClass("hidden");
            return true;
        }
    });


    //Remainder date pick code 
    $("#FinalDateforCommentsId").change(function () {
        var tt = $('#FinalDateforCommentsId').val();
        var date = new Date(tt);
        var newdate = new Date(date);
        newdate.setDate(newdate.getDate() - 53);
        var dd = newdate.getDate();
        var month = new Array();
        month[1] = "January";
        month[2] = "February";
        month[3] = "March";
        month[4] = "April";
        month[5] = "May";
        month[6] = "June";
        month[7] = "July";
        month[8] = "August";
        month[9] = "September";
        month[10] = "October";
        month[11] = "November";
        month[12] = "December";
        var MM = month[newdate.getMonth() + 1];
        var y = newdate.getFullYear();
        var someFormattedDate = dd + '-' + MM + '-' + y;
        $("#remainder").val(someFormattedDate);

        
        var NotificationDateDate = new Date(StartDate);
        var RemainderDate = new Date(someFormattedDate);
        if (NotificationDateDate > RemainderDate) {
            $("#RemainderforTranslationError").removeClass("hidden");
            return false;
        }
        else {
            $("#RemainderforTranslationError").addClass("hidden");
            return true;
        }
    });
    
    //Duedate date pick code 
    $("#FinalDateforCommentsId").change(function () {
        var tt = $('#FinalDateforCommentsId').val();
        var date = new Date(tt);
        var newdate = new Date(date);
        newdate.setDate(newdate.getDate() - 50);
        var dd = newdate.getDate();
        var month = new Array();
        month[1] = "January";
        month[2] = "February";
        month[3] = "March";
        month[4] = "April";
        month[5] = "May";
        month[6] = "June";
        month[7] = "July";
        month[8] = "August";
        month[9] = "September";
        month[10] = "October";
        month[11] = "November";
        month[12] = "December";
        var MM = month[newdate.getMonth() + 1];
        var y = newdate.getFullYear();
        var someFormattedDate = dd + '-' + MM + '-' + y;
        $("#duedate").val(someFormattedDate);

        var NotificationDateDate = new Date(StartDate);
        var RemainderDate = new Date(someFormattedDate);
        if (NotificationDateDate > RemainderDate) {
            $("#DueDateError").removeClass("hidden");
            return false;
        }
        else {
            $("#DueDateError").addClass("hidden");
            return true;
        }

    });


    // <!-- datepicker js end-->
		

   
    // <!-- hamburger js -->
	
    //var trigger = $('.hamburger'),
    //isClosed = false;

    //function buttonSwitch() {
    //    if (isClosed === true) {
			
    //        trigger.removeClass('is-open');
    //        trigger.addClass('is-closed');
    //        isClosed = false;
    //    } else {
    //        trigger.removeClass('is-closed');
    //        trigger.addClass('is-open');
    //        isClosed = true;
    //    }
    //}

    //trigger.click(function () {
    //    buttonSwitch();
    //});

    //$('[data-toggle="offcanvas"]').click(function () {
    //    $('#wrapper').toggleClass('toggled');
    //});

    if (window.location.href.match(/([^\/]*)\/*$/)[1] == "WTODashboard") {
        $("#toggleOpenClose").addClass("is-open");
        $("#toggleOpenClose").removeClass("is-closed");
        $('#wrapper').toggleClass('toggled');
        var trigger = $('.hamburger'),
          isClosed = false;

        function buttonSwitch() {
            if (isClosed === true) {
                trigger.removeClass('is-closed');
                trigger.addClass('is-open');
                isClosed = false;
            } else {
                trigger.removeClass('is-open');
                trigger.addClass('is-closed');
                isClosed = true;
            }
        }

        trigger.click(function () {
            buttonSwitch();
        });
        $('[data-toggle="offcanvas"]').click(function () {
            $('#wrapper').toggleClass('toggled');
            $('#wrapper').addClass('transitioncss');
            $('#sidebar-wrapper').addClass('transitioncss');
        });
    }
    else {     
        $("#toggleOpenClose").removeClass("is-open");
        $("#toggleOpenClose").addClass("is-closed");

        var trigger = $('.hamburger'),
          isClosed = true;

        function buttonSwitch() {
            if (isClosed === true) {
                trigger.removeClass('is-closed');
                trigger.addClass('is-open');
                isClosed = false;
            } else {
                trigger.removeClass('is-open');
                trigger.addClass('is-closed');
                isClosed = true;
            }
        }

        trigger.click(function () {
            buttonSwitch();
        });
        $('[data-toggle="offcanvas"]').click(function () {
            $('#wrapper').toggleClass('toggled');
            $('#wrapper').addClass('transitioncss');
            $('#sidebar-wrapper').addClass('transitioncss');
        });
    }
    // <!-- hamburger js -->
	
    // <!-- image swaping -->

    var dashboardblack = "/contents/img/dashboard.png";
    var dashboardwhite = "/contents/img/dashboard1.png";
    var notificationblack = "/contents/img/notification1.png";
    var notificationwhite = "/contents/img/notification.png";
    var Addnotificationblack = "/contents/img/Addnotification.png";
    var Addnotificationwhite = "/contents/img/Addnotification1.png";
    var AddMOMListWhite = "/contents/img/MOMlistW.png";
    var AddMOMListBlack = "/contents/img/MOMlistB.png";
    var AddMOMWhite = "/contents/img/AddMOM.png";
    var AddMOMBlack = "/contents/img/AddMOMB.png";
    var ManageAccessB = "/contents/img/manageB.png";
    var ManageAccessW = "/contents/img/manageW.png";

    var location = document.location.href;

    if (location.indexOf("WTODashboard") >= 1) {
        $('.dashboard').addClass("active");
        if ($('.dashboard img').attr('src') == dashboardwhite) {
            $('.dashboard img').attr('src', dashboardblack);
        } else {
            $('.dashboard img').attr('src', dashboardwhite);
        }
    }
    else {
        $('.dashboard').removeClass("active");
        $('.dashboard').on('mouseenter mouseleave', function () {
            if ($('.dashboard img').attr('src') == dashboardwhite) {
                $('.dashboard img').attr('src', dashboardblack);
            } else {
                $('.dashboard img').attr('src', dashboardwhite);
            }
        });
    }
    if (location.indexOf("NotificationList") > -1) {
        $('.notification').addClass("active");
        if ($('.notification img').attr('src') == notificationwhite) {
            $('.notification img').attr('src', notificationblack);
        } else {
            $('.notification img').attr('src', notificationwhite);
        }
    }
    else {
        $('.notification').removeClass("active");
        $('.notification').on('mouseenter mouseleave', function () {
            if ($('.notification img').attr('src') == notificationwhite) {
                $('.notification img').attr('src', notificationblack);
            } else {
                $('.notification img').attr('src', notificationwhite);
            }
        });
    } 
    if (location.indexOf("AddNotification") > -1) {
        $('.Addnotification').addClass("active");
        if ($('.Addnotification img').attr('src') == Addnotificationwhite) {
            $('.Addnotification img').attr('src', Addnotificationblack);
        } else {
            $('.Addnotification img').attr('src', Addnotificationwhite);
        }
    }
    else {
        $('.Addnotification').removeClass("active");
        $('.Addnotification').on('mouseenter mouseleave', function () {
            if ($('.Addnotification img').attr('src') == Addnotificationwhite) {
                $('.Addnotification img').attr('src', Addnotificationblack);
            } else {
                $('.Addnotification img').attr('src', Addnotificationwhite);
            }
        });
    }
    if (location.indexOf("MOMList") > -1) {
        $('.AddMOMList').addClass("active");
        if ($('.AddMOMList img').attr('src') == AddMOMListWhite) {
            $('.AddMOMList img').attr('src', AddMOMListBlack);
        } else {
            $('.AddMOMList img').attr('src', AddMOMListWhite);
        }
    }
    else {
        $('.AddMOMList').removeClass("active");
        $('.AddMOMList').on('mouseenter mouseleave', function () {
            if ($('.AddMOMList img').attr('src') == AddMOMListWhite) {
                $('.AddMOMList img').attr('src', AddMOMListBlack);
            } else {
                $('.AddMOMList img').attr('src', AddMOMListWhite);
            }
        });
    }
    if (location.indexOf("AddMOM") > -1) {
        $('.AddMOM').addClass("active");
        if ($('.AddMOM img').attr('src') == AddMOMWhite) {
            $('.AddMOM img').attr('src', AddMOMBlack);
        } else {
            $('.AddMOM img').attr('src', AddMOMWhite);
        }
    }
    else {
        $('.AddMOM').removeClass("active");
        $('.AddMOM').on('mouseenter mouseleave', function () {
            if ($('.AddMOM img').attr('src') == AddMOMWhite) {
                $('.AddMOM img').attr('src', AddMOMBlack);
            } else {
                $('.AddMOM img').attr('src', AddMOMWhite);
            }
        });
    }
    if (location.indexOf("ManageAccess") > -1) {
        $('.ManageAccess').addClass("active");
        if ($('.ManageAccess img').attr('src') == ManageAccessW) {
            $('.ManageAccess img').attr('src', ManageAccessB);
        } else {
            $('.ManageAccess img').attr('src', ManageAccessW);
        }
    }
    else {
        $('.ManageAccess').removeClass("active");
        $('.ManageAccess').on('mouseenter mouseleave', function () {
            if ($('.ManageAccess img').attr('src') == ManageAccessW) {
                $('.ManageAccess img').attr('src', ManageAccessB);
            } else {
                $('.ManageAccess img').attr('src', ManageAccessW);
            }
        });
    }

    // <!-- image swaping End-->
	
    // <!-- switch button js -->
    $(".mailenquiry, .ObtainDocument, .translatedocument").addClass("hidden");
    $(".yes").click(function(){
        $(".yes").addClass("switch-blue active");
        $(".yes").removeClass("switch-white");
        $(".No").removeClass("switch-red active");
        $(".No").addClass("switch-white");
        $(".selectlangusge").css("display","block");
        $(".upload, .translatedocument").removeClass("hidden");
        $(".mailenquiry, .ObtainDocument").addClass("hidden");
    });

    $(".No").click(function(){
        $(".yes").removeClass("switch-blue active");
        $(".yes").addClass("switch-white");
        $(".No").addClass("switch-red active");
        $(".No").removeClass("switch-white");
        $(".selectlangusge").css("display","none");
        $(".upload, .translatedocument").addClass("hidden");
        $(".mailenquiry, .ObtainDocument").removeClass("hidden");
    });

    // <!-- switch button js End -->

    // <!-- $(".addsavenotification").click(function(){ -->
    // <!-- $(".translatedocument").css("display","block").focus(); -->
    // <!-- $(".duedate").css("display","none"); -->
    // <!-- }); -->
	
    

    $('.panel-group').on('hidden.bs.collapse', toggleIcon);
    $('.panel-group').on('shown.bs.collapse', toggleIcon);

});

function toggleIcon(e) {
    $(e.target)
		.prev('.panel-heading')
		.find(".more-less")
		.toggleClass('glyphicon-chevron-right glyphicon-chevron-down');
}


$(function () {
    $('[data-toggle="tooltip"]').tooltip()
})


function GetreminerDate() {
    
}
