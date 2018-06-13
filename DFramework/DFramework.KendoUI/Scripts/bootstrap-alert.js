// bootstrap-alert.js
// modify  2015-02-12

window.alert = function (text, fn) {
    var alertId = "alertModal";
    var title = "提示";
    var alertHtml = "<div class='modal fade' id='" + alertId + "' tabindex='-1' role='dialog' aria-labelledby='myModalLabel' aria-hidden='true'>"
        + "<div class='modal-dialog'><div class='modal-content'>"
        + "<div class='modal-header'><button type='button' class='close' data-dismiss='modal'><span aria-hidden='true'>&times;</span><span class='sr-only'>Close</span></button>"
        + "<h4 class='modal-title  text-left' id='myModalLabel'>" + title + "</h4></div>"
        + "<div class='modal-body'>" + text + "</div><div class='modal-footer'>"
        + "<button data-alert-action='ok' type='button' class='btn btn-primary' data-dismiss='modal'>确定</button>"
        + "</div></div></div></div>";
    $("body").append(alertHtml);
    var alertModal = $("#" + alertId);
    alertModal.on('hidden.bs.modal', function (e) {
        alertModal.remove();
    })
    if (typeof fn === "function") {
        var btnOK = alertModal.find("button[data-alert-action=ok]");
        if (btnOK) {
            btnOK.click(function () {
                fn.call();
            })
        }
    }
    alertModal.modal('show');
}

window.confirm = function (text, fn, canelFn) {
    var confirmId = "confirmModal";
    var title = "提示";
    var confirmHtml = "<div class='modal fade' id='" + confirmId + "' tabindex='-1' role='dialog' aria-labelledby='myModalLabel' aria-hidden='true'>"
        + "<div class='modal-dialog'><div class='modal-content'>"
        + "<div class='modal-header'><button type='button' class='close' data-dismiss='modal'><span aria-hidden='true'>&times;</span><span class='sr-only'>Close</span></button>"
        + "<h4 class='modal-title text-left' id='myModalLabel'>" + title + "</h4></div>"
        + "<div class='modal-body'>" + text + "</div><div class='modal-footer'>"
        + "<button data-alert-action='ok' type='button' class='btn btn-primary' data-dismiss='modal'>确定</button>"
        + "<button data-alert-action='cancel' type='button' class='btn btn-default' data-dismiss='modal'>取消</button>"
        + "</div></div></div></div>";
    $("body").append(confirmHtml);
    var confirmModal = $("#" + confirmId);
    confirmModal.on('hidden.bs.modal', function (e) {
        confirmModal.remove();
    })
    if (typeof fn === "function") {
        var btnOK = confirmModal.find("button[data-alert-action=ok]");
        if (btnOK) {
            btnOK.click(function () {
                fn.call();
            })
        }
    }
    if (typeof fn === "function") {
        var btnCancel = confirmModal.find("button[data-alert-action=cancel]");
        if (btnCancel) {
            btnCancel.click(function () {
                canelFn && canelFn.call();
            })
        }
    }
    confirmModal.modal('show');
}