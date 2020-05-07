etutorApp.controller('adminController', ['$scope', '$timeout', '$filter', 'toastrService', 'busyIndicatorService', 'adminService', function PhoneListController($scope, $timeout, $filter, toastrService, busyIndicatorService, adminService) {

    $scope.format = 'dd-MM-yyyy';
    $scope.altInputFormats = ['M!/d!/yyyy'];
    $scope.today = new Date();

    $scope.AttachDocumentList = [];
    $scope.StudentWithOutTutor = [];
    $scope.Meetings = [];
    $scope.users = [];
    $scope.NonActiveStudents = [];
    $scope.MessageHistory = [];
    $scope.MessageSummary = [];
    $scope.MeetingArrangement = [];

    //On Load Events
    getAttachments();
    getMeeting();
    getUsers();

    $scope.SelectedUser = {};
    $scope.selectedStudents = {};
        
    $scope.navItemList = {
        isStudent: true,
        isStudentWithOutTutor: false,
        isNonActiveStudent: false,
        isMessageHistory: false,
        isMessageSummary: false,
        isUserMeetings: false
    }

    $scope.NonActiveStudentSearch = {
        from: new Date(),
        to: new Date($scope.today.setDate($scope.today.getDate() + 7)),
        fromopened: false,
        toopened: false
    }

    $scope.userDropdownSetting = {
        scrollable: true,
        enableSearch: true
    };

    $scope.opento = function () {
        $scope.NonActiveStudentSearch.toopened = true;
    };

    $scope.openfrom = function () {
        $scope.NonActiveStudentSearch.fromopened = true;
    };

    $scope.fromdateOptions = {
        dateDisabled: false,
        formatYear: 'yy',
        //maxDate: new Date(),
        //minDate: new Date(2018,01,01),
        startingDay: 1
    };

    $scope.fromdateOptions = {
        dateDisabled: false,
        formatYear: 'yy',
        //maxDate: new Date(),
        //minDate: $scope.NonActiveStudentSearch.from,
        startingDay: 1
    };

    function getAttachments() {
        adminService.GetAttachments().then(function (data) {
            $scope.AttachDocumentList = data.data;
            if ($scope.AttachDocumentList.length > 0) {
                $scope.comment = angular.copy($scope.AttachDocumentList[0].comment);
            }
        }, function (error) {
            toastrService.showErrorMessage("Error", 'Network error occured..Please try again later.');
        });
    }

    function getMeeting() {
        adminService.GetMeetings().then(function (data) {
            $scope.Meetings = data.data;
        }, function (error) {
            toastrService.showErrorMessage("Error", 'Network error occured..Please try again later.');
        });
    }

    function getUsers() {
        busyIndicatorService.showBusy("Please wait...");
        adminService.GetUsers().then(function (data) {
            busyIndicatorService.stopBusy();
            $scope.users = data.data;
        }, function (error) {
            toastrService.showErrorMessage("Error", 'Network error occured..Please try again later.');
            busyIndicatorService.stopBusy();
            });
    }

    $scope.saveStudentTutor = function () {
        busyIndicatorService.showBusy("Please wait...");
        $scope.users.selectedStudents = $scope.selectedStudents;
        adminService.SaveStudentTutor($scope.users).then(function (data) {
            busyIndicatorService.stopBusy();
            if (data.data.isSuccess) {
                alert(data.data.message);
            } else {
                alert(data.data.message);
            }
            $scope.selectedStudents = {};
        }, function (error) {
            toastrService.showErrorMessage("Error", 'Network error occured..Please try again later.');
            busyIndicatorService.stopBusy();
        });
    }

    $scope.NavClick = function (navitem) {

        $scope.navItemList.isStudent = false;
        $scope.navItemList.isStudentWithOutTutor = false;
        $scope.navItemList.isNonActiveStudent = false;
        $scope.navItemList.isMessageHistory = false;
        $scope.navItemList.isMessageSummary = false;
        $scope.navItemList.isUserMeetings = false;

        $scope.navItemList.studentclass = "nav-link";
        $scope.navItemList.studentwithouttutorclass = "nav-link";
        $scope.navItemList.isNonActiveStudentclass = "nav-link";

        switch (navitem) {
            case "student": {
                $scope.navItemList.isStudent = true;
                break;
            }
            case "studentwithouttutor": {
                $scope.navItemList.isStudentWithOutTutor = true;
                getStudentWithOutTutor();
                break;
            }
            case "nonactivestudents": {
                $scope.navItemList.isNonActiveStudent = true;
                getNonActiveStudent();
                break;
            }
            case "messagehistory": {
                $scope.navItemList.isMessageHistory = true;
                getMessageHistory();
                break;
            }
            case "messagesummary": {
                $scope.navItemList.isMessageSummary = true;
                getMessageSummaryForTutor();
                break;
            }
            case "meetingarrangement": {
                $scope.navItemList.isUserMeetings = true;
                break;
            }
            
        }
    }

    function getStudentWithOutTutor() {
        adminService.GetStudentWithOutTutor().then(function (data) {
            $scope.StudentWithOutTutor = data.data;

        }, function (error) {
            toastrService.showErrorMessage("Error", 'Network error occured..Please try again later.');
        });
    }

    $scope.serchNonActiveStudents = function() {
        getNonActiveStudent();
    }

function getNonActiveStudent() {
    adminService.GetNonActiveStudents($scope.NonActiveStudentSearch.from, $scope.NonActiveStudentSearch.to).then(function (data) {
        $scope.NonActiveStudents = data.data;

        }, function (error) {
            toastrService.showErrorMessage("Error", 'Network error occured..Please try again later.');
        });
    }

    function getMessageHistory() {
        adminService.GetMessageHistory().then(function (data) {
            $scope.MessageHistory = data.data;
        }, function (error) {
            toastrService.showErrorMessage("Error", 'Network error occured..Please try again later.');
        });
    }

    function getMessageSummaryForTutor() {
        adminService.GetMessageForPersonalTutor().then(function (data) {
            $scope.MessageSummary = data.data;
        }, function (error) {
            toastrService.showErrorMessage("Error", 'Network error occured..Please try again later.');
        });
    }

    function getMeetingArrengementByUser() {
        adminService.GetMeetings($scope.SelectedUser).then(function (data) {
            $scope.MeetingArrangement = data.data;
        }, function (error) {
            toastrService.showErrorMessage("Error", 'Network error occured..Please try again later.');
        });
    }

    $scope.serchMeetings = function () {
        getMeetingArrengementByUser();
    }
    function MessageHistoryChart(data) {
        var svg = d3.select("MessageSummary").append("svg"),
            margin = 200,
            width = svg.attr("width") - margin,
            height = svg.attr("height") - margin;


        var xScale = d3.scaleBand().range([0, width]).padding(0.4),
            yScale = d3.scaleLinear().range([height, 0]);

        var g = svg.append("g")
            .attr("transform", "translate(" + 100 + "," + 100 + ")");

            xScale.domain(data.map(function (d) { return d.year; }));
            yScale.domain([0, d3.max(data, function (d) { return d.value; })]);

            g.append("g")
                .attr("transform", "translate(0," + height + ")")
                .call(d3.axisBottom(xScale));

            g.append("g")
                .call(d3.axisLeft(yScale).tickFormat(function (d) {
                    return "$" + d;
                }).ticks(10))
                .append("text")
                .attr("y", 6)
                .attr("dy", "0.71em")
                .attr("text-anchor", "end")
                .text("value");
    }
}]);