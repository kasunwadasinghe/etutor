etutorApp.controller('dashboardController', ['$scope', '$timeout', '$filter', 'toastrService', 'busyIndicatorService', 'dashboardService', function PhoneListController($scope, $timeout, $filter, toastrService, busyIndicatorService, dashboardService) {

    $scope.test = "angular test"
    $scope.file = null;

    $scope.AttachDocument = {
        description : "",
        name: 'No Files Selected',
        url : ""
    };

    $scope.AttachDocumentList = [];

    getAttachments();

    function getAttachments() {
        dashboardService.GetAttachments().then(function (data) {
            busyIndicatorService.stopBusy();
            $scope.AttachDocumentList = data;
        }, function (error) {
            toastrService.showErrorMessage("Error", 'Network error occured..Please try again later.');
            busyIndicatorService.stopBusy();
        });
    }

    $scope.openfileUpload = function() {
        var fileName = jQuery('.custom-file-input').val().split("\\").pop();
        $scope.AttachDocument.name = fileName;
    }

    $scope.setFiles = function (element) {
        $scope.$apply(function (scope) {
            $scope.AttachStatus = "";
            $scope.files = []
            for (var i = 0; i < element.files.length; i++) {
                $scope.files.push(element.files[i])
            }
            $scope.progressVisible = false
        });
    }
}]);