etutorApp.factory('messageService', ['$http', '$q', function ($http, $q) {
    var messageServiceFactory = {};

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

    var AddMessage = function (message) {
        var deferred = $q.defer();

        $http.post('/Home/AddMessage', message)
            .then(function (response) {
                deferred.resolve(response);
            }, function (err) {
                deferred.reject(err);
            });

        return deferred.promise;
    };

    var GetMessages = function () {

        var deferred = $q.defer();
        $http({
            method: 'GET',
            url: "/Home/GetMessages"
        }).then(function (response) {
            deferred.resolve(response);
        }, function (err) {
            deferred.reject(err);
        });

        return deferred.promise;
    };

    var DeleteMessage = function (messageId) {
        var deferred = $q.defer();
        $http({
            method: 'GET',
            url: "/Home/DeleteMessage",
            params: {
                Id: messageId
            }
        }).then(function (response) {
            deferred.resolve(response);
        }, function (err) {
            deferred.reject(err);
        });

        return deferred.promise;
    };

    messageServiceFactory.GetUsers = GetUsers;
    messageServiceFactory.AddMessage = AddMessage;
    messageServiceFactory.GetMessages = GetMessages;
    messageServiceFactory.DeleteMessage = DeleteMessage;

    return messageServiceFactory;

}]);