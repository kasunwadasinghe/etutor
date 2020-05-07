etutorApp.factory('adminService', ['$http', '$q', function ($http, $q) {
    var adminServiceFactory = {};

    var GetAttachments = function () {

        var deferred = $q.defer();
        $http({
            method: 'GET',
            url: "/Admin/GetAttachments"
        }).then(function (response) {

            deferred.resolve(response);

        },function (err) {

            deferred.reject(err);
        });

        return deferred.promise;
    };

    var GetMeetings = function (UserId) {

        var deferred = $q.defer();
        $http({
            method: 'GET',
            url: "/Admin/GetMeetings",
            params: {
                UserId:UserId
            }

        }).then(function (response) {

            deferred.resolve(response);

        }, function (err) {

            deferred.reject(err);
        });

        return deferred.promise;
    };

    var GetUsers = function () {

        var deferred = $q.defer();
        $http({
            method: 'GET',
            url: "/Admin/GetUsers"
        }).then(function (response) {
            deferred.resolve(response);
        }, function (err) {
            deferred.reject(err);
        });

        return deferred.promise;
    };

    var SaveStudentTutor = function (studenttutor) {
        var deferred = $q.defer();

        $http.post('/Admin/SaveStudentTutor', studenttutor)
            .then(function (response) {
                deferred.resolve(response);
            }, function (err) {
                deferred.reject(err);
            });

        return deferred.promise;
    };

    var GetStudentWithOutTutor = function () {

        var deferred = $q.defer();
        $http({
            method: 'GET',
            url: "/Admin/GetStudentWithOutTutor"
        }).then(function (response) {

            deferred.resolve(response);

        }, function (err) {

            deferred.reject(err);
        });

        return deferred.promise;
    };

    var GetNonActiveStudents = function (from,to) {

        var deferred = $q.defer();
        $http({
            method: 'GET',
            url: "/Admin/GetNonActiveStudents",
            params: {
                from: from,
                to:to,
            }
        }).then(function (response) {

            deferred.resolve(response);

        }, function (err) {

            deferred.reject(err);
        });

        return deferred.promise;
    };

    var GetMessageHistory = function () {

        var deferred = $q.defer();
        $http({
            method: 'GET',
            url: "/Admin/GetMessageHistory"
        }).then(function (response) {

            deferred.resolve(response);

        }, function (err) {

            deferred.reject(err);
        });

        return deferred.promise;
    };

    var GetMessageForPersonalTutor = function () {

        var deferred = $q.defer();
        $http({
            method: 'GET',
            url: "/Admin/GetMessageForPersonalTutor"
        }).then(function (response) {

            deferred.resolve(response);

        }, function (err) {

            deferred.reject(err);
        });

        return deferred.promise;
    };

    var GetMeetings = function (user) {

        var deferred = $q.defer();
        $http({
            method: 'GET',
            url: "/Admin/GetMeetings",
            params: {
                selecteduser: user
            }
        }).then(function (response) {

            deferred.resolve(response);

        }, function (err) {

            deferred.reject(err);
        });

        return deferred.promise;
    };

    adminServiceFactory.GetAttachments = GetAttachments;
    adminServiceFactory.GetMeetings = GetMeetings;
    adminServiceFactory.GetUsers = GetUsers;
    adminServiceFactory.SaveStudentTutor = SaveStudentTutor;
    adminServiceFactory.GetStudentWithOutTutor = GetStudentWithOutTutor;
    adminServiceFactory.GetNonActiveStudents = GetNonActiveStudents;
    adminServiceFactory.GetMessageHistory = GetMessageHistory;
    adminServiceFactory.GetMessageForPersonalTutor = GetMessageForPersonalTutor;
    adminServiceFactory.GetMeetings = GetMeetings;

    return adminServiceFactory;

}]);