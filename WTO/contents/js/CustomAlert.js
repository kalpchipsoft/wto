function Confirm(title, msg, $true, $false, $link) {
    var $content = "<div class='dialog-ovelay'>" +
                    "<div class='dialog'><header>" +
                     " <h4> " + title + " </h4> " +
                     "<i class='fa fa-close'></i>" +
                 "</header>" +
                 "<div class='dialog-msg'>" +
                     " <p> " + msg + " </p> " +
                 "</div>" +
                 "<footer>" +
                     "<div class='controls'>" +
                         "<input type='button' class='btn btn-danger btn-padding cancelAction' value='" + $false + "'/>" +
                         "<input type='button' class='btn btn-primary btn-padding doAction' value='" + $true + "'/>" +
                     "</div>" +
                 "</footer>" +
              "</div>" +
            "</div>";
    if ($('.dialog-ovelay').length == 0 || $('.dialog-ovelay .dialog-msg').find('p').text().trim() != msg.replace(/(<|&lt;)br\s*\/*(>|&gt;)/g, '').trim() || $('.dialog-ovelay').find('h4').text().trim() != title)
        $('body').prepend($content);
    $('.doAction').click(function () {
        var tmpFunc = new Function($link);
        tmpFunc();
        $(this).parents('.dialog-ovelay').fadeOut(500, function () {
            $(this).remove();
        });
    });
    $('.cancelAction, .fa-close').click(function () {
        $(this).parents('.dialog-ovelay').fadeOut(500, function () {
            $(this).remove();
        });
    });
    $('div').keypress(function (e) {
        if (e.keyCode == '13') {
            $('.doAction').click();
            return false;
        }
    });
}

function Alert(title, msg, $true) {
    var $content = "<div class='dialog-ovelay'>" +
                     "<div class='dialog'><header>" +
                      " <h4> " + title + " </h4> " +
                  "</header>" +
                  "<div class='dialog-msg'>" +
                      " <p> " + msg + " </p> " +
                  "</div>" +
                  "<footer>" +
                      "<div class='controls'>" +
                          "<input type='button' class='btn btn-primary btn-padding doAction' value='" + $true + "'/>" +
                      "</div>" +
                  "</footer>" +
               "</div>" +
             "</div>";
    if ($('.dialog-ovelay').length == 0 || $('.dialog-ovelay .dialog-msg').find('p').text().trim() != msg.replace(/(<|&lt;)br\s*\/*(>|&gt;)/g, '').trim() || $('.dialog-ovelay').find('h4').text().trim() != title)
        $('body').prepend($content);
    $('.doAction').click(function () {
        $(this).parents('.dialog-ovelay').fadeOut(500, function () {
            $(this).remove();
        });
    });
    $('div').keypress(function (e) {
        if (e.keyCode == '13') {
            $('.doAction').click();
            return false;
        }
    });
}