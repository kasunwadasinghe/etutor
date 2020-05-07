etutorApp.controller('dashboardController', ['$scope', '$timeout', '$filter', 'toastrService', 'busyIndicatorService', 'dashboardService', function PhoneListController($scope, $timeout, $filter, toastrService, busyIndicatorService, dashboardService) {

    $scope.format = 'dd-MM-yyyy';
    $scope.altInputFormats = ['M!/d!/yyyy'];
    $scope.isTutor = false;

    $scope.mytime = new Date();
    $scope.hstep = 1;
    $scope.mstep = 15;

    $scope.ismeridian = true;
    $scope.ObjAttachDocument = {
        description: "",
        name: 'No Files Selected',
        file: ""
    };

    $scope.AttachDocument = {
        description: "",
        name: 'No Files Selected',
        file: ""
    };

    $scope.ObjMeeting = {
        meetingId: 0,
        subject: '',
        description: '',
        place: '',
        members: [],
        date: new Date(),
        time: new Date(),
        isOwner: true
    }

    $scope.Meeting ={
        meetingId:0,
        subject:'',
        description: '',
        place:'',
        members:[],
        date: new Date(),
        time: new Date(),
        isOwner: true
    }

    $scope.MeetingMinuteSummary = {
        meetingId: 0,
        description: '',
        place: '',
        members: [],
        date: new Date()
    }

    $scope.ObjMeetingMinute = {
        meetingMinuteId:0,
        meetingId: 0,
        description: '',
        presenter: {},
        isEdit: false,
    }

    $scope.MeetingMinute = {}

    $scope.comment = {};

    $scope.AttachDocumentList = [];
    $scope.users = [];
    $scope.presenters = [];
    $scope.Meetings = [];
    $scope.MeetingMinutes = [];

    //On Load Events
    getAttachments();
    getUsers();
    getMeeting();
    getPermition();

    $scope.open = function () {
        $scope.MeatingDate.opened = true;
    };
    $scope.MeatingDate = {
        opened: false
    };

    $scope.dateOptions = {
        dateDisabled: false,
        formatYear: 'yy',
        //maxDate: new Date(2020, 5, 22),
        minDate: new Date(),
        startingDay: 1
    };

    $scope.userDropdownSetting = {
        scrollable: true,
        enableSearch: true
    };

    $scope.fileList = [];
    $scope.curFile;
    $scope.ImageProperty = {
        file: ''
    }
    $scope.setFile = function (element) {
        $scope.AttachDocument.file = element.files[0];
        $scope.AttachDocument.name = $scope.AttachDocument.file.name;
        $scope.$apply();
    }
    $scope.UploadFile = function () {
        $scope.UploadFileIndividual($scope.AttachDocument.file,
            $scope.AttachDocument.file.name,
            $scope.AttachDocument.file.type,
            $scope.AttachDocument.file.size,
            $scope.AttachDocument.description);
    }
    $scope.UploadFileIndividual = function (fileToUpload, name, type, size, description) {
        var reqObj = new XMLHttpRequest();
        reqObj.open("POST", "/Home/UploadFiles", true);
        reqObj.upload.addEventListener("load", complete, false)
        reqObj.addEventListener("error", error, false)
        reqObj.setRequestHeader("Content-Type", "multipart/form-data"); 
        reqObj.setRequestHeader('X-File-Name', name);
        reqObj.setRequestHeader('X-File-Type', type);
        reqObj.setRequestHeader('X-File-Size', size);
        reqObj.setRequestHeader('X-File-Description', description);
        reqObj.send(fileToUpload);
        function complete() {
            alert("Upload sucess")
            $scope.AttachDocument = angular.copy($scope.ObjAttachDocument);
            getAttachments();
        }

        function error() {
            alert("Upload fail")
        }
    }  

    function getAttachments() {
        dashboardService.GetAttachments().then(function (data) {
            $scope.AttachDocumentList = data.data;
            if ($scope.AttachDocumentList.length > 0) {
                $scope.comment = angular.copy($scope.AttachDocumentList[0].comment);
            }
        }, function (error) {
            toastrService.showErrorMessage("Error", 'Network error occured..Please try again later.');
        });
    }

    function getUsers() {
        dashboardService.GetUsers().then(function (data) {
            $scope.users = data.data;
        }, function (error) {
            toastrService.showErrorMessage("Error", 'Network error occured..Please try again later.');
        });
    }

    function getPermition() {
        dashboardService.GetPermission().then(function (data) {
            $scope.isTutor = data.data;
        }, function (error) {
            toastrService.showErrorMessage("Error", 'Network error occured..Please try again later.');
        });
    }

    $scope.editAttachment = function (attachment) {
        attachment.isEdit = true;
    }

    $scope.updateAttachment = function (attachment) {
        busyIndicatorService.showBusy("Please wait...");
        dashboardService.EditAttachment(attachment).then(function (data) {
            busyIndicatorService.stopBusy();
            if (data.data.isSuccess) {
                alert(data.data.message);
            } else {
                alert(data.data.message);
            }
            attachment.isEdit = false;
        }, function (error) {
            toastrService.showErrorMessage("Error", 'Network error occured..Please try again later.');
            busyIndicatorService.stopBusy();
            attachment.isEdit = false;
        });
    }

    $scope.deleteAttachment = function (attachmentId) {
        busyIndicatorService.showBusy("Please wait...");
        dashboardService.DeleteAttachment(attachmentId).then(function (data) {
            busyIndicatorService.stopBusy();
            if (data.data.isSuccess) {
                for (var i = 0; i < $scope.AttachDocumentList.length; i++) {
                    if ($scope.AttachDocumentList[i].attachmentId == attachmentId) {
                        $scope.AttachDocumentList.splice(i, 1);
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

    $scope.addComment = function (attachment) {
        busyIndicatorService.showBusy("Please wait...");
        dashboardService.AddComment(attachment.comment).then(function (data) {
            busyIndicatorService.stopBusy();
            attachment.comments.push(data.data);
            attachment.comment = angular.copy($scope.comment);
        }, function (error) {
            toastrService.showErrorMessage("Error", 'Network error occured..Please try again later.');
            busyIndicatorService.stopBusy();
            comment.isEdit = false;
        });
    }

    $scope.editComment = function (comment) {
        comment.isEdit = true;
    }

    $scope.updateComment = function (comment) {
        busyIndicatorService.showBusy("Please wait...");
        dashboardService.EidtComment(comment).then(function (data) {
            busyIndicatorService.stopBusy();
            if (data.data.isSuccess) {
                alert(data.data.message);
            } else {
                alert(data.data.message);
            }
            comment.isEdit = false;
        }, function (error) {
            toastrService.showErrorMessage("Error", 'Network error occured..Please try again later.');
            busyIndicatorService.stopBusy();
            comment.isEdit = false;
        });
    }

    $scope.deleteComment = function (attachment, commentId) {
        busyIndicatorService.showBusy("Please wait...");
        dashboardService.DeleteComment(commentId).then(function (data) {
            busyIndicatorService.stopBusy();
            if (data.data.isSuccess) {
                for (var i = 0; i < $scope.AttachDocumentList.length; i++) {
                    if ($scope.AttachDocumentList[i].attachmentId == attachment.attachmentId) {
                        for (var j = 0; j < $scope.AttachDocumentList[i].comments.length; j++) {
                            if ($scope.AttachDocumentList[i].comments[j].commentId == commentId) {
                                $scope.AttachDocumentList[i].comments.splice(j, 1);
                            }
                        }
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

    $scope.addMeeting = function () {
        busyIndicatorService.showBusy("Please wait...");
        dashboardService.AddMeeting($scope.Meeting).then(function (data) {
            busyIndicatorService.stopBusy();
            $scope.Meetings.push(data.data);
            $scope.Meeting = angular.copy($scope.ObjMeeting);
            alert("Save success");
        }, function (error) {
            toastrService.showErrorMessage("Error", 'Network error occured..Please try again later.');
            busyIndicatorService.stopBusy();
        });
    }

    $scope.cancelMeeting = function (meetingId) {
        busyIndicatorService.showBusy("Please wait...");
        dashboardService.CancelMeetings(meetingId).then(function (data) {
            busyIndicatorService.stopBusy();
            if (data.data.isSuccess) {
                for (var i = 0; i < $scope.Meetings.length; i++) {
                    if ($scope.Meetings[i].meetingId == meetingId) {
                        $scope.Meetings.splice(i, 1);
                    }
                }
                alert(data.data.message);
            } else {
                alert(data.data.message);
            }

        }, function (error) {
            toastrService.showErrorMessage("Error", 'Network error occured..Please try again later.');
            busyIndicatorService.stopBusy();
            comment.isEdit = false;
        });
    }

    $scope.editMeetingMinute = function (meetingminute) {
        meetingminute.isEdit = true;
    }

    $scope.addMeetingMinute = function () {
        busyIndicatorService.showBusy("Please wait...");
        dashboardService.AddMeetingMinute($scope.MeetingMinute).then(function (data) {
            busyIndicatorService.stopBusy();
            $scope.MeetingMinutes.push(data.data);
            $scope.MeetingMinute = angular.copy($scope.ObjMeetingMinute);
            alert("Save success");
        }, function (error) {
            toastrService.showErrorMessage("Error", 'Network error occured..Please try again later.');
            busyIndicatorService.stopBusy();
        });
    }

    $scope.updateMeetingMinute = function (meetingminute) {
        busyIndicatorService.showBusy("Please wait...");
        dashboardService.EidtMeetingMinute(meetingminute).then(function (data) {
            busyIndicatorService.stopBusy();
            meetingminute.isEdit = false;
            alert(data.data.message);
        }, function (error) {
            toastrService.showErrorMessage("Error", 'Network error occured..Please try again later.');
            busyIndicatorService.stopBusy();
            meetingminute.isEdit = false;
        });
    }

    $scope.deleteMeetingMinute = function (meetingminuteId) {
        busyIndicatorService.showBusy("Please wait...");
        dashboardService.DeleteMeetingMinute(meetingminuteId).then(function (data) {
            busyIndicatorService.stopBusy();
            for (var i = 0; i < $scope.MeetingMinutes.length; i++) {
                if ($scope.MeetingMinutes[i].meetingMinuteId = meetingminuteId) {
                    $scope.MeetingMinutes.splice(i, 1);
                }
            }
            alert(data.data.message);
        }, function (error) {
            toastrService.showErrorMessage("Error", 'Network error occured..Please try again later.');
            busyIndicatorService.stopBusy();
        });
    }

    function getMeeting() {
        dashboardService.GetMeetings().then(function (data) {
            $scope.Meetings = data.data;
        }, function (error) {
            toastrService.showErrorMessage("Error", 'Network error occured..Please try again later.');
        });
    }

    function getPresenters(meetingId) {
        dashboardService.GetPresenters(meetingId).then(function (data) {
            $scope.presenters = data.data;
        }, function (error) {
            toastrService.showErrorMessage("Error", 'Network error occured..Please try again later.');
        });
    }

    $scope.getMeetingMinutes = function (meeting) {
        $scope.MeetingMinuteSummary = {
            meetingId: meeting.meetingId,
            description: meeting.description,
            place: meeting.place,
            members: meeting.members,
            date: meeting.date
        }
        $scope.ObjMeetingMinute.meetingId= meeting.meetingId;
        $scope.MeetingMinute = angular.copy($scope.ObjMeetingMinute);

        getPresenters(meeting.meetingId);
        dashboardService.GetMeetingMinutes(meeting.meetingId).then(function (data) {
            $scope.MeetingMinutes = data.data;
        }, function (error) {
            toastrService.showErrorMessage("Error", 'Network error occured..Please try again later.');
        });
    }
}]);