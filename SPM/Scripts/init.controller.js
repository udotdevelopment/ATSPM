(function () {
    'use strict';

    angular
        .module('MyApp')
        .controller('InitController', InitController);

    function InitController($location, $scope, $http, $mdDialog, $state) {
        /* jshint validthis:true */
        var vm = this;
/*        $state.go('/');*/
        $state.go('MyApp', null);
    }
})();