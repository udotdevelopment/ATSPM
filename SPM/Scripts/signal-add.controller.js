(function () {
    'use strict';

    angular
        .module('MyApp')
        .controller('signalAddController', signalAddController);

    /** @ngInject */
    function signalAddController($scope, $mdDialog, lookups, $http, $mdToast) {

        var vm = this;
        vm.signal = {};
        vm.close = close;
        vm.saveSignal = saveSignal;
        vm.notUsaDialog = notUsaDialog;
        vm.hasLatLng = false;
        vm.mapApi = {};
        vm.lookups = lookups;
        vm.mapOpen = false;
        vm.buttonText = 'auto locate';
        vm.errorMessage = "";
        vm.signalInfoText = ''
        vm.coordinatesHoverText = 'Enter latitude and longitude.';
        //set options for the search bar
        //const geocoder = new google.maps.Geocoder();
        vm.drawSignal = drawSignal;
        vm.signalDragendCallback = signalDragendCallback;
        vm.checkSignal = checkSignal;
        // vm.setPristine = setPristine;


        function checkSignal(signalID) {
            vm.signalExists = signalService.checkSignalExists(signalID);
        }

        function signalDragendCallback(signal) {
            $scope.$apply();
        }

        function drawSignal() {
            vm.coordinatesHoverText = '';
            vm.mapOpen = true;
            vm.buttonText = 'update';
            vm.signalInfoText = 'If you manually override the position or change either the coordinates or street names, click the button to update the map (and coordinates).'
            if (vm.signal && vm.mapApi.drawNewSignal) {
                vm.mapApi.drawNewSignal(vm.signal)
            }
        }

        function close() {
            $mdDialog.cancel(false);
        };

        $scope.ok = function () {
            $mdDialog.hide({ message: 'Success' });
        };
        // function setPristine() {
        //     vm.signalAddForm.$setPristine();
        // }

        function notUsaDialog() {
            $mdDialog.show(

                $mdDialog.confirm()
                    .parent(angular.element(document.querySelector('#popupContainer')))
                    .clickOutsideToClose(true)
                    .title('The signal you are trying to add is located outside of the United States borders')
                    .multiple(true)
                    .textContent('Are you sure you want to do that?')
                    .ariaLabel('Error during saving')
                    .ok('Yes, I\'m sure')
                    .cancel('Cancel')
            ).then(function () {
                signalService.createSignal(vm.signal)
                    .then(function (respSignal) {
                        vm.errorMessage = "";
                        $scope.ok();
                        breadcrumbNavigationService.navigateToStateWithoutBreadCrumb('app.spm.config.signals.edit', { signal: respSignal, lookups: vm.lookups });
                    }).catch(function (e) {
                        console.error(e);
                        vm.errorMessage = 'Error: ' + e.data + '!';
                    });
            }, function () { });
            return;
        }

        function errorToast() {
            $mdToast.show(
                $mdToast.simple()
                    .parent(document.querySelectorAll('#content'))
                    .textContent('Ooops something went wrong! Please try again...')
                    .position("top right")
                    .hideDelay(3000)
            );
        }

        function showSignalSavedToast() {
            $mdToast.show(
                $mdToast.simple()
                    .parent(document.querySelectorAll('#content'))
                    .textContent('Signal saved successfully')
                    .position("top right")
                    .hideDelay(3000)
            );
        }

        function saveSignal(lat, lng) {

            //query ?& fileType=& filter=& id=& orderBy=signalID & pageIndex=1 & pageSize=10
            $http({
                method: 'POST',
                url: 'EnhancedConfiguration/CreateUpdateSignal', //signal?&SignalID=' + vm.signal.SignalID + '&PrimaryName=' + vm.signal.PrimaryName + '&SecondaryName=' + vm.signal.SecondaryName + '&IPAddress=' + vm.signal.IPAddress + '&Latitude=' + vm.signal.Latitude + '&Longitude=' + vm.signal.Longitude + '&ControllerTypeID=' + vm.signal.ControllerTypeID + '&Enabled=' + vm.signal.Enabled,
                dataType: 'json',
                data: { signal: vm.signal },
                headers: {
                    "Content-Type": "application/json , Access-Control-Allow-Headers: Content-Type Access- Control - Allow - Methods: GET, POST, OPTIONS Access- Control - Allow - Origin: *"
                }
            }).then(function (res, status, headers, config) {
                vm.errorMessage = "";
                showSignalSavedToast();
                $scope.ok();
            }).catch(function (e, status, headers, config) {
                errorToast();
                $scope.ok();
                vm.errorMessage = 'Error: ' + e.data + '!';
            });
        };
    }
})();
