(function () {
    'use strict';

    angular
        .module('MyApp')
        .controller('approachAddController', approachAddController);

    /** @ngInject */
    function approachAddController($rootScope, $scope, $state, $mdDialog, lookups, signal, $http, $mdToast) {
        var vm = this;
        vm.approach = {};
        vm.close = close;
        vm.signal = signal;
        vm.saveApproach = saveApproach;
        vm.lookups = lookups;
        vm.checkPhases = checkPhases;
        vm.samePhases = false;
        vm.message = "";
        vm.approach.DirectionTypeID = 1;

        function checkPhases(protectedPhase, premisivePhase) {
            if (protectedPhase == premisivePhase) {
                vm.samePhases = true;
                vm.message = "Protected and Premesive Phase Number are same, please change!"
            }
            else {
                vm.samePhases = false;
                vm.message = ""
            }
        }

        function close() {
            $mdDialog.cancel(false);
        };

        $scope.ok = function () {
            $mdDialog.hide({ message: 'Success' });
        };

        function errorToast() {
            $mdToast.show(
                $mdToast.simple()
                    .parent(document.querySelectorAll('#content'))
                    .textContent('Ooops something went wrong! Please try again...')
                    .position("top right")
                    .hideDelay(3000)
            );
        }

        function showApproachSavedToast() {
            $mdToast.show(
                $mdToast.simple()
                    .parent(document.querySelectorAll('#content'))
                    .textContent('Approach saved successfully')
                    .position("top right")
                    .hideDelay(3000)
            );
        }

        function saveApproach() {
            vm.approach.SignalID = vm.signal.SignalID;
            $http({
                method: 'POST',
                url: 'EnhancedConfiguration/CreateUpdateApproach', //signal?&SignalID=' + vm.signal.SignalID + '&PrimaryName=' + vm.signal.PrimaryName + '&SecondaryName=' + vm.signal.SecondaryName + '&IPAddress=' + vm.signal.IPAddress + '&Latitude=' + vm.signal.Latitude + '&Longitude=' + vm.signal.Longitude + '&ControllerTypeID=' + vm.signal.ControllerTypeID + '&Enabled=' + vm.signal.Enabled,
                dataType: 'json',
                data: { approach: vm.approach },
                headers: {
                    "Content-Type": "application/json , Access-Control-Allow-Headers: Content-Type Access- Control - Allow - Methods: GET, POST, OPTIONS Access- Control - Allow - Origin: *"
                }
            }).then(function (res, status, headers, config) {
                vm.errorMessage = "";
                showApproachSavedToast();
                $scope.ok();
            }).catch(function (e, status, headers, config) {
                errorToast();
                $scope.ok();
                vm.errorMessage = 'Error: ' + e.data + '!';
            });
        };
    }
})();