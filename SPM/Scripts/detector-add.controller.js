(function () {
    'use strict';

    angular
        .module('MyApp')
        .controller('detectorAddController', detectorAddController);

    /** @ngInject */
    function detectorAddController($rootScope, $scope, $state, $mdDialog, lookups, approach, signal, $http, $mdToast) {
        var vm = this;
        vm.detector = {};
        vm.approach = approach;
        vm.close = close;
        vm.signal = signal;
        vm.saveDetector = saveDetector;
        vm.lookups = lookups;
        vm.checkDetectionType = checkDetectionType;
        vm.isRequired = false;
        vm.isDistanceDisabled = false;
        vm.isDisabled = false;
        vm.messageMinSpeed = "";
        vm.messageDecisionPoint = "";
        vm.messageDistanceStopBar = "";
        vm.checkLaneDetType = checkLaneDetType;
        vm.laneMessage = "";
        vm.laneDet = false;
        vm.showWarning = showWarning;
        vm.showDetChError = "";

        vm.checkDetectionType(vm.detector.detectionTypeID);

        function close() {
            $mdDialog.cancel(false);
        };

        $scope.ok = function () {
            $mdDialog.hide({ message: 'Success' });
        };

        function checkLaneDetType(laneNumber, detectionTypeID) {
            vm.checkDetector = null;
            vm.approach.Detectors.forEach(function (det) {
                if (det.LaneNumber == laneNumber && det.DetectionTypeID == detectionTypeID) {
                    vm.laneDet = true;
                    vm.checkDetector = det;
                }
            });
            if (!vm.laneDet) vm.saveDetector();
            if (vm.laneDet) vm.showWarning();
        }

        function saveDetector() {
            vm.detector.ApproachID = vm.approach.ApproachID;
            vm.detector.DetectionTypeIDs = [vm.detector.DetectionTypeID];
            $http({
                method: 'POST',
                url: 'EnhancedConfiguration/AddUpdateDetector', //signal?&SignalID=' + vm.signal.SignalID + '&PrimaryName=' + vm.signal.PrimaryName + '&SecondaryName=' + vm.signal.SecondaryName + '&IPAddress=' + vm.signal.IPAddress + '&Latitude=' + vm.signal.Latitude + '&Longitude=' + vm.signal.Longitude + '&ControllerTypeID=' + vm.signal.ControllerTypeID + '&Enabled=' + vm.signal.Enabled,
                dataType: 'json',
                data: { det: vm.detector },
                headers: {
                    "Content-Type": "application/json , Access-Control-Allow-Headers: Content-Type Access- Control - Allow - Methods: GET, POST, OPTIONS Access- Control - Allow - Origin: *"
                }
            }).then(function (res, status, headers, config) {
                showDetectorSavedToast();
                vm.errorMessage = "";
                $scope.ok();
            }).catch(function (e, status, headers, config) {
                errorToast();
                $scope.ok();
                vm.errorMessage = 'Error: ' + e.data + '!';
            });
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

        function showDetectorSavedToast() {
            $mdToast.show(
                $mdToast.simple()
                    .parent(document.querySelectorAll('#content'))
                    .textContent('Detector saved successfully')
                    .position("top right")
                    .hideDelay(3000)
            );
        }

        function showWarning() {
            vm.laneMessage = "Detector Channel " + vm.checkDetector.detChannel + " has the same detection type (" + vm.checkDetector.description + ")"
                + " and is already assigned to this lane number " + vm.checkDetector.laneNumber
                + "\n" + ". Do you still want to save this detector? ";

            vm.laneDet = false;

            var confirm = $mdDialog.confirm()
                .multiple(true)
                .title("Confirm")
                .textContent(vm.laneMessage)
                .ok('Yes')
                .cancel('No');

            $mdDialog.show(confirm)
                .then(function () {
                    saveDetector();
                }, function () { }
                );
        }

        function checkDetectionType(detectionType) {
            switch (detectionType) {
                case 2: //Advanced Count
                    vm.isRequired = true;
                    vm.isDisabled = true;
                    vm.isDistanceDisabled = false;
                    vm.messageMinSpeed = "By selecting this Detection Type you are not able to set Min Speed Filter Field";
                    vm.messageDecisionPoint = "By selecting this Detection Type you are not able to set Decision Point";
                    vm.detector.MinSpeedFilter = "";
                    vm.detector.DecisionPoint = "";
                    break;
                case 6: // Set Bar Presence
                    vm.isRequired = false;
                    vm.isDisabled = true;
                    vm.isDistanceDisabled = true;
                    vm.messageMinSpeed = "By selecting this Detection Type you are not able to set  Min Speed Filter";
                    vm.messageDecisionPoint = "By selecting this Detection Type you are not able to set  Decision Point";
                    vm.messageDistanceStopBar = "By selecting this Detection Type you are not able to set  Distance from Stop Bar"
                    vm.detector.MinSpeedFilter = "";
                    vm.detector.DecisionPoint = "";
                    vm.detector.DistanceFromStopBar = "";
                    break;
                case 4: // Lane-by-lane Count
                    vm.isRequired = false;
                    vm.isDisabled = true;
                    vm.isDistanceDisabled = true;
                    vm.messageMinSpeed = "By selecting this Detection Type you are not able to set  Min Speed Filter";
                    vm.messageDecisionPoint = "By selecting this Detection Type you are not able to set  Decision Point";
                    vm.messageDistanceStopBar = "By selecting this Detection Type you are not able to set  Distance from Stop Bar"
                    vm.detector.MinSpeedFilter = "";
                    vm.detector.DecisionPoint = "";
                    vm.detector.DistanceFromStopBar = "";
                    break;
                default:
                    vm.isRequired = false;
                    vm.isDisabled = true;
                    vm.isDistanceDisabled = true;
                    vm.messageMinSpeed = "By selecting this Detection Type you are not able to set  Min Speed Filter";
                    vm.messageDecisionPoint = "By selecting this Detection Type you are not able to set  Decision Point";
                    vm.messageDistanceStopBar = "By selecting this Detection Type you are not able to set  Distance from Stop Bar"
                    vm.detector.MinSpeedFilter = "";
                    vm.detector.DecisionPoint = "";
                    vm.detector.DistanceFromStopBar = "";
                //code block
            }
        }
    }

})();
