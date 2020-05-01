var viewChangedNotificationModule = angular.module('viewChangedNotificationModule', []);

viewChangedNotificationModule.factory('checkStateChangeService', function ($rootScope, toastrService) {

    var addCheck = function ($scope,tabIndex,levelNo) {

        var removeListener = $rootScope.$on('$stateChangeStart'
            , function (event, toState, toParams, fromState, fromParams) {

                if ($scope.form1.$pristine) {
                    return;
                }
                else
                {
                    toastrService.showWarningMessage("Warning", "Please save/cancel your changes before move to new page.");
                    if (levelNo === 1)
                    {
                        $scope.$emit('statusNotChangeEvet', tabIndex);
                    }
                    else
                    {
                        $scope.$emit('secondLevelStatusNotChangeEvet', tabIndex);
                    }

                }


                event.preventDefault();
            });

        $scope.$on("$destroy", removeListener);
    };

    return { checkFormOnStateChange: addCheck };
});

viewChangedNotificationModule.factory('webAppStateChangeService', function ($rootScope, toastrService) {

    var addCheck = function ($scope, classId, studentId,tabIndex) {

        var removeListener = $rootScope.$on('$stateChangeStart'
            , function (event, toState, toParams, fromState, fromParams) {

                if ($scope.form1.$pristine) {
                    return;
                }
                else {
                    toastrService.showWarningMessage("Warning", "Please save/cancel your changes before move to new page.");
                    $scope.$emit('statusNotChangeEvet', { classId: classId, studentId: studentId, isDirty: true, tabIndex: tabIndex });

                }


                event.preventDefault();
            });

        $scope.$on("$destroy", removeListener);
    };

    return { checkFormOnStateChange: addCheck};
});