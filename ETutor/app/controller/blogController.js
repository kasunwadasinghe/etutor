etutorApp.controller('blogController', ['$scope', '$timeout', '$filter', 'toastrService', 'busyIndicatorService', 'blogService', function PhoneListController($scope, $timeout, $filter, toastrService, busyIndicatorService, blogService) {

    $scope.ObjBlogPost = {
        postId:0,
        description: "",
        title: '',
    };

    $scope.BlogPost = {
        postId:0,
        description:'',
        title: '',
        isOwner: true
    };

    $scope.comment = {};
    $scope.Post = {};
    $scope.FeaturePost = [];
    $scope.BlogPostList = [];

    //On Load Events
    getBlogPost(null);
    getBlogFeaturePost();
    getBlogPostList();

    $scope.showBlogPost = function (postId) {
        getBlogPost(postId);
    }

    function getBlogPost(postId) {
        blogService.GetBlogPost(postId).then(function (data) {
            $scope.Post = data.data;
            if ($scope.Post != null) {
                $scope.comment = angular.copy($scope.Post.comment);
            }
        }, function (error) {
            toastrService.showErrorMessage("Error", 'Network error occured..Please try again later.');
        });
    }

    function getBlogFeaturePost() {
        blogService.GetBlogFeaturePost().then(function (data) {
            $scope.FeaturePost = data.data;
        }, function (error) {
            toastrService.showErrorMessage("Error", 'Network error occured..Please try again later.');
        });
    }

    function getBlogPostList() {
        blogService.GetBlogPostList().then(function (data) {
            $scope.BlogPostList = data.data;
        }, function (error) {
            toastrService.showErrorMessage("Error", 'Network error occured..Please try again later.');
        });
    }

    $scope.editBlogPost = function (BlogPost) {
        BlogPost.isEdit = !BlogPost.isEdit;
    }

    $scope.createBlogPost = function () {
        busyIndicatorService.showBusy("Update data...");
        blogService.CreateBlogPost($scope.BlogPost).then(function (data) {
            busyIndicatorService.stopBusy();
            if (data.data.isSuccess) {
                getBlogFeaturePost();
                getBlogPostList();
                $scope.BlogPost = $scope.ObjBlogPost;
                alert(data.data.message);
            } else {
                alert(data.data.message);
            }
        }, function (error) {
            toastrService.showErrorMessage("Error", 'Network error occured..Please try again later.');
            busyIndicatorService.stopBusy();
            BlogPost.isEdit = false;
        });
    }

    $scope.updateBlogPost = function (BlogPost) {
        busyIndicatorService.showBusy("Update data...");
        blogService.EditBlogPost(BlogPost).then(function (data) {
            busyIndicatorService.stopBusy();
            if (data.data.isSuccess) {
                alert(data.data.message);
            } else {
                alert(data.data.message);
            }
            BlogPost.isEdit = false;
        }, function (error) {
            toastrService.showErrorMessage("Error", 'Network error occured..Please try again later.');
            busyIndicatorService.stopBusy();
            BlogPost.isEdit = false;
        });
    }

    $scope.deleteBlogPost = function (BlogPostId) {
        busyIndicatorService.showBusy("Deleting data...");
        blogService.DeleteBlogPost(BlogPostId).then(function (data) {
            busyIndicatorService.stopBusy();
            if (data.data.isSuccess) {
                for (var i = 0; i < $scope.Post.length; i++) {
                    if ($scope.Post[i].postId == BlogPostId) {
                        $scope.Post.splice(i, 1);
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

    $scope.addComment = function (BlogPost) {
        busyIndicatorService.showBusy("Save data...");
        blogService.AddComment(BlogPost.comment).then(function (data) {
            busyIndicatorService.stopBusy();
            BlogPost.comments.push(data.data);
            BlogPost.comment = angular.copy($scope.comment);
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
        busyIndicatorService.showBusy("Update data...");
        blogService.EidtComment(comment).then(function (data) {
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

    $scope.deleteComment = function (commentId) {
        busyIndicatorService.showBusy("Deleting data...");
        blogService.DeleteComment(commentId).then(function (data) {
            busyIndicatorService.stopBusy();
            if (data.data.isSuccess) {
                for (var j = 0; j < $scope.Post.comments.length; j++) {
                    if ($scope.Post.comments[j].commentId == commentId) {
                        $scope.Post.comments.splice(j, 1);
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