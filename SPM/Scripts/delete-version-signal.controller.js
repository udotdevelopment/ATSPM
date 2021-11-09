(function () {
    'use strict';

    angular
        .module('MyApp')
        .controller('DialogController', DialogController);

    function DialogController($scope, $mdDialog) {
        var vm = this;
        vm.hide = function () {
            $mdDialog.hide();
        };

        vm.cancel = function () {
            $mdDialog.cancel();
        };

        vm.answer = function (answer) {
            $mdDialog.hide(answer);
        };
    }
})();
