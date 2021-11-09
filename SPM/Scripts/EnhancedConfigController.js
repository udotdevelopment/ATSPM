(function () {
    'use strict';

    angular
        .module('MyApp')
        .controller('EnhancedConfigController', EnhancedConfigController);

    function EnhancedConfigController($location,$scope) {
        /* jshint validthis:true */
        var vm = this;
        vm.title = "Hello world";
        vm.fname = $scope.fname;
        vm.lname = $scope.fname + " Skrijelj";
        vm.test = test;
        vm.totalCount = "111";

        function test() {
            console.log("Clicked");
        }

        console.log("Controller init....." + vm.title);
    }

})();

//app.controller("EnhancedConfigController", function ($scope) {
//    vm = this;
//    vm.title = "My Controller";
//})