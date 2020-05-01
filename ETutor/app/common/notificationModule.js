var notificationModule = angular.module('notificationModule', []);

//Notification Service

notificationModule.factory("toastrService", function () {
    var toastrServiceFactory = {};

    var showSuccessMessage = function (title, msg) {

        var shortCutFunction = "success";
        toastr.options.showDuration = 500;;
        toastr.options.hideDuration = 300;
        toastr.options.timeOut = 5000;
        toastr.options.extendedTimeOut = 1000;
        toastr.options.showEasing = "swing";
        toastr.options.hideEasing = "linear";
        toastr.options.showMethod = "fadeIn";
        toastr.options.hideMethod = "fadeOut";

        var $toast = toastr[shortCutFunction](msg, title); // Wire up an event handler to a button in the toast, if it exists
        $toastlast = $toast;

        if (typeof $toast === 'undefined') {
            return;
        }

    };

    var showInfoMessage = function (title, msg) {

        var shortCutFunction = "info";
        toastr.options.showDuration = 500;;
        toastr.options.hideDuration = 300;
        toastr.options.timeOut = 5000;
        toastr.options.extendedTimeOut = 1000;
        toastr.options.showEasing = "swing";
        toastr.options.hideEasing = "linear";
        toastr.options.showMethod = "fadeIn";
        toastr.options.hideMethod = "fadeOut";

        var $toast = toastr[shortCutFunction](msg, title); // Wire up an event handler to a button in the toast, if it exists
        $toastlast = $toast;

        if (typeof $toast === 'undefined') {
            return;
        }
    };

    var showWarningMessage = function (title, msg) {

        var shortCutFunction = "warning";
        toastr.options.showDuration = 500;;
        toastr.options.hideDuration = 300;
        toastr.options.timeOut = 5000;
        toastr.options.extendedTimeOut = 1000;
        toastr.options.showEasing = "swing";
        toastr.options.hideEasing = "linear";
        toastr.options.showMethod = "fadeIn";
        toastr.options.hideMethod = "fadeOut";

        var $toast = toastr[shortCutFunction](msg, title); // Wire up an event handler to a button in the toast, if it exists
        $toastlast = $toast;

        if (typeof $toast === 'undefined') {
            return;
        }
    };

    var showErrorMessage = function (title, msg) {

        var shortCutFunction = "error";
        toastr.options.showDuration = 500;;
        toastr.options.hideDuration = 300;
        toastr.options.timeOut = 5000;
        toastr.options.extendedTimeOut = 1000;
        toastr.options.showEasing = "swing";
        toastr.options.hideEasing = "linear";
        toastr.options.showMethod = "fadeIn";
        toastr.options.hideMethod = "fadeOut";

        var $toast = toastr[shortCutFunction](msg, title); // Wire up an event handler to a button in the toast, if it exists
        $toastlast = $toast;

        if (typeof $toast === 'undefined') {
            return;
        }
    };

    var showConfirmationMessage = function (title, msg) {
        swal(
            {
                title: "Are you sure?",
                text: "You will not be able to recover this imaginary file!",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#1ab394",
                confirmButtonText: "Delete",
                cancelButtonText: "Cancel",
                cancelButtonColor: "#1ab394",
                closeOnConfirm: true,
                closeOnCancel: true
            }, function
                (isConfirm)
            {
                if (isConfirm)
                {
                    return true;
                    
                } else
                {
                    return false;
                }
            });

    };



    toastrServiceFactory.showSuccessMessage = showSuccessMessage;
    toastrServiceFactory.showInfoMessage = showInfoMessage;
    toastrServiceFactory.showWarningMessage = showWarningMessage;
    toastrServiceFactory.showErrorMessage = showErrorMessage;
    toastrServiceFactory.showConfirmationMessage = showConfirmationMessage;
    return toastrServiceFactory;

});