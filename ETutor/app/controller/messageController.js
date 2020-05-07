etutorApp.controller('messageController', ['$scope', '$timeout', '$filter', 'toastrService', 'busyIndicatorService', 'messageService', function PhoneListController($scope, $timeout, $filter, toastrService, busyIndicatorService, messageService) {

    $scope.objMessage = {
        messageId: 0,
        description: '',
        members: [],
    }

    $scope.Message = {
        messageId: 0,
        description: '',
        members: [],
    }

    $scope.users = [];
    $scope.Messages = [];

    //On Load Events
    getUsers();
    getMessage();

    $scope.userDropdownSetting = {
        scrollable: true,
        enableSearch: true
    };

    function getUsers() {
        messageService.GetUsers().then(function (data) {
            $scope.users = data.data;
        }, function (error) {
            toastrService.showErrorMessage("Error", 'Network error occured..Please try again later.');
        });
    }

    $scope.addMessage = function () {
        busyIndicatorService.showBusy("Save data...");
        messageService.AddMessage($scope.Message).then(function (data) {
            busyIndicatorService.stopBusy();
            $scope.Messages.push(data.data);
            $scope.Message = angular.copy($scope.ObjMessage);
        }, function (error) {
            toastrService.showErrorMessage("Error", 'Network error occured..Please try again later.');
            busyIndicatorService.stopBusy();
            comment.isEdit = false;
        });
    }

    function getMessage() {
        messageService.GetMessages().then(function (data) {
            $scope.Messages = data.data;
        }, function (error) {
            toastrService.showErrorMessage("Error", 'Network error occured..Please try again later.');
        });
    }

    $scope.deleteMessage = function (messageId) {
        busyIndicatorService.showBusy("Deleting data...");
        messageService.DeleteMessage(messageId).then(function (data) {
            busyIndicatorService.stopBusy();
            if (data.data.isSuccess) {
                for (var i = 0; i < $scope.Messages.length; i++) {
                    if ($scope.Messages[i].messageId == messageId) {
                        $scope.Messages.splice(i, 1);
                    }
                }

                alert(data.data.message);
            } else {
                alert(data.data.message);
            }
        }, function (error) {
            toastrService.showErrorMessage("Error", 'Network error occured..Please try again later.');
            busyIndicatorService.stopBusy();
        });
    }
}]);