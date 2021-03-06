﻿var etutorApp = angular.module('etutorApp', ['notificationModule', 'busyIndicatorModule', 'ui.bootstrap', 'angularjs-dropdown-multiselect']);
etutorApp.filter('join', function () {
    return function join(array, separator, prop) {
        if (!Array.isArray(array)) {
            return array; // if not array return original - can also throw error
        }

        return (!!prop ? array.map(function (item) {
            return item[prop];
        }) : array).join(separator);
    };
});
