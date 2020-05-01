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

    var GetAcademicLevelList = function (filters) {
        var deferred = $q.defer();

        $http.post('/AcademicLevel/GetAllAcademicLevels', filters)

            .success(function (response) {

                deferred.resolve(response);

            }).error(function (err, status, header, config) {

                deferred.reject(err);
            });

        return deferred.promise;
    };

    dashboardServiceFactory.GetAttachments = GetAttachments;

    return dashboardServiceFactory;

}]);