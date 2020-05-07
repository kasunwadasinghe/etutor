etutorApp.factory('blogService', ['$http', '$q', function ($http, $q) {
    var blogServiceFactory = {};

    var GetBlogPost = function (Id) {

        var deferred = $q.defer();
        $http({
            method: 'GET',
            url: "/Blog/GetPost",
            params: {
                Id: Id
            }
        }).then(function (response) {

            deferred.resolve(response);

        },function (err) {

            deferred.reject(err);
        });

        return deferred.promise;
    };

    var GetBlogFeaturePost = function () {

        var deferred = $q.defer();
        $http({
            method: 'GET',
            url: "/Blog/GetFeaturePost"
        }).then(function (response) {

            deferred.resolve(response);

        }, function (err) {

            deferred.reject(err);
        });

        return deferred.promise;
    };

    var GetBlogPostList = function () {

        var deferred = $q.defer();
        $http({
            method: 'GET',
            url: "/Blog/GetPostList"
        }).then(function (response) {

            deferred.resolve(response);

        }, function (err) {

            deferred.reject(err);
        });

        return deferred.promise;
    };

    var CreateBlogPost = function (post) {
        var deferred = $q.defer();
        $http({
            method: 'POST',
            url: "/Blog/CreateBlogPost",
            params: {
                Title: post.title,
                BodyText: post.description
            }
        }).then(function (response) {

            deferred.resolve(response);

        }, function (err) {

            deferred.reject(err);
        });

        return deferred.promise;
    };

    var EditBlogPost = function (post) {

        var deferred = $q.defer();
        $http({
            method: 'POST',
            url: "/Blog/EditBlogPost",
            params: {
                Title: post.title,
                BodyText: post.description,
                Id: post.postId
            }
        }).then(function (response) {

            deferred.resolve(response);

        }, function (err) {

            deferred.reject(err);
        });

        return deferred.promise;
    };

    var DeleteBlogPost = function (postId) {
        var deferred = $q.defer();
        $http({
            method: 'GET',
            url: "/Blog/DeleteBlogPost",
            params: {
                postId: postId
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

        $http.post('/Blog/AddComment', comment)
            .then(function (response) {
                deferred.resolve(response);
            }, function (err) {
                deferred.reject(err);
            });

        return deferred.promise;
    };

    var EidtComment = function (comment) {
        var deferred = $q.defer();

        $http.post('/Blog/EidtComment', comment)
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
            url: "/Blog/DeleteComment",
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

    blogServiceFactory.GetBlogPost = GetBlogPost;
    blogServiceFactory.GetBlogFeaturePost = GetBlogFeaturePost;
    blogServiceFactory.GetBlogPostList = GetBlogPostList;
    blogServiceFactory.CreateBlogPost = CreateBlogPost;
    blogServiceFactory.EditBlogPost = EditBlogPost;
    blogServiceFactory.DeleteBlogPost = DeleteBlogPost;
    blogServiceFactory.AddComment = AddComment;
    blogServiceFactory.EidtComment = EidtComment;
    blogServiceFactory.DeleteComment = DeleteComment;

    return blogServiceFactory;

}]);