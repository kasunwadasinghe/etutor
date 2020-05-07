etutorApp.factory('dashboardService', ['$http', '$q', function ($http, $q) {
    var dashboardServiceFactory = {};

    var GetAttachments = function () {

        var deferred = $q.defer();
        $http({
            method: 'GET',
            url: "/Home/GetAttachments"
        }).then(function (response) {

            deferred.resolve(response);

        },function (err) {

            deferred.reject(err);
        });

        return deferred.promise;
    };

    var GetUsers = function () {

        var deferred = $q.defer();
        $http({
            method: 'GET',
            url: "/Home/GetUsers"
        }).then(function (response) {
            deferred.resolve(response);
        }, function (err) {
            deferred.reject(err);
        });

        return deferred.promise;
    };

    var GetPermission = function () {

        var deferred = $q.defer();
        $http({
            method: 'GET',
            url: "/Home/GetPermission"
        }).then(function (response) {
            deferred.resolve(response);
        }, function (err) {
            deferred.reject(err);
        });

        return deferred.promise;
    };

    var EditAttachment = function (attachment) {
        var deferred = $q.defer();

        $http.post('/Home/EidtAttachment', attachment)

            .then(function (response) {
                deferred.resolve(response);
            },function (err) {
                deferred.reject(err);
            });

        return deferred.promise;
    };

    var DeleteAttachment = function (attachmentId) {
        var deferred = $q.defer();
        $http({
            method: 'GET',
            url: "/Home/DeleteAttachment",
            params: {
                attachmentId: attachmentId
            }
        }).then(function (response) {

            deferred.resolve(response);

        }, function (err) {

            deferred.reject(err);
        });

        return deferred.promise;
    };

    var AddComment = function (comment) {
        var deferred = $q.defer();

        $http.post('/Home/AddComment', comment)
            .then(function (response) {
                deferred.resolve(response);
            }, function (err) {
                deferred.reject(err);
            });

        return deferred.promise;
    };

    var EidtComment = function (comment) {
        var deferred = $q.defer();

        $http.post('/Home/EidtComment', comment)
            .then(function (response) {
                deferred.resolve(response);
            }, function (err) {
                deferred.reject(err);
            });

        return deferred.promise;
    };

    var DeleteComment = function (commentId) {
        var deferred = $q.defer();
        $http({
            method: 'GET',
            url: "/Home/DeleteComment",
            params: {
                commentId: commentId
            }
        }).then(function (response) {
            deferred.resolve(response);
        }, function (err) {
            deferred.reject(err);
        });

        return deferred.promise;
    };

    var AddMeeting = function (meeting) {
        var deferred = $q.defer();

        $http.post('/Home/AddMeeting', meeting)
            .then(function (response) {
                deferred.resolve(response);
            }, function (err) {
                deferred.reject(err);
            });

        return deferred.promise;
    };

    var GetMeetings = function () {

        var deferred = $q.defer();
        $http({
            method: 'GET',
            url: "/Home/GetMeetings"
        }).then(function (response) {

            deferred.resolve(response);

        }, function (err) {

            deferred.reject(err);
        });

        return deferred.promise;
    };

    var CancelMeetings = function (meetingId) {

        var deferred = $q.defer();
        $http({
            method: 'GET',
            url: "/Home/CancelMeetings",
            params: {
                Id: meetingId
            }
        }).then(function (response) {

            deferred.resolve(response);

        }, function (err) {

            deferred.reject(err);
        });

        return deferred.promise;
    };

    var GetMeetingMinutes = function (meetingId) {

        var deferred = $q.defer();
        $http({
            method: 'GET',
            url: "/Home/GetMeetingMinutes",
            params: {
                meetingId: meetingId
            }
        }).then(function (response) {

            deferred.resolve(response);

        }, function (err) {

            deferred.reject(err);
        });

        return deferred.promise;
    };

    var AddMeetingMinute = function (meetingmitute) {
        var deferred = $q.defer();

        $http.post('/Home/AddMeetingMinute', meetingmitute)
            .then(function (response) {
                deferred.resolve(response);
            }, function (err) {
                deferred.reject(err);
            });

        return deferred.promise;
    };

    var EidtMeetingMinute = function (meetingminute) {
        var deferred = $q.defer();

        $http.post('/Home/EidtMeetingMinute', meetingminute)
            .then(function (response) {
                deferred.resolve(response);
            }, function (err) {
                deferred.reject(err);
            });

        return deferred.promise;
    };

    var DeleteMeetingMinute = function (meetingminuteId) {
        var deferred = $q.defer();
        $http({
            method: 'GET',
            url: "/Home/DeleteMeetingMinute",
            params: {
                Id: meetingminuteId
            }
        }).then(function (response) {
            deferred.resolve(response);
        }, function (err) {
            deferred.reject(err);
        });

        return deferred.promise;
    };

    var GetPresenters = function (meetingId) {

        var deferred = $q.defer();
        $http({
            method: 'GET',
            url: "/Home/GetPresenters",
            params: {
                meetingId: meetingId
            }
        }).then(function (response) {

            deferred.resolve(response);

        }, function (err) {

            deferred.reject(err);
        });

        return deferred.promise;
    };

    dashboardServiceFactory.GetAttachments = GetAttachments;
    dashboardServiceFactory.EditAttachment = EditAttachment;
    dashboardServiceFactory.DeleteAttachment = DeleteAttachment;
    dashboardServiceFactory.AddComment = AddComment;
    dashboardServiceFactory.EidtComment = EidtComment;
    dashboardServiceFactory.DeleteComment = DeleteComment;
    dashboardServiceFactory.GetUsers = GetUsers;
    dashboardServiceFactory.GetPermission = GetPermission;
    dashboardServiceFactory.AddMeeting = AddMeeting;
    dashboardServiceFactory.GetMeetings = GetMeetings;
    dashboardServiceFactory.CancelMeetings = CancelMeetings;
    dashboardServiceFactory.GetMeetingMinutes = GetMeetingMinutes;
    dashboardServiceFactory.AddMeetingMinute = AddMeetingMinute;
    dashboardServiceFactory.EidtMeetingMinute = EidtMeetingMinute;
    dashboardServiceFactory.DeleteMeetingMinute = DeleteMeetingMinute;
    dashboardServiceFactory.GetPresenters = GetPresenters;

    return dashboardServiceFactory;

}]);