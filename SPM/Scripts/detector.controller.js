(function () {
    "use strict";

    angular
        .module("MyApp")
        .controller("detectorController", detectorController);

    function detectorController($state, $stateParams, $scope, $mdDialog, $http, $mdToast) {
        var vm = this;
        vm.dtInstance = {};
        vm.signal = undefined;
        vm.approach = undefined;
        vm.detector = undefined;
        vm.lookups = undefined;

        if ($stateParams.inputParameters) {
            vm.signal = $stateParams.inputParameters.signal;
            vm.approach = $stateParams.inputParameters.approach;
            vm.detector = $stateParams.inputParameters.detector;
            vm.lookups = $stateParams.inputParameters.lookups;
        }
        else {
            //go to signal list
            breadcrumbNavigationService.navigateToStateWithoutBreadCrumb('app.spm.config.signals');
            return;
        }
        vm.isFormValid = isFormValid;
        vm.isFormDirty = isFormDirty;
        vm.enableSaveButton = enableSaveButton;
        vm.deleteDetector = deleteDetector;
        vm.saveDetector = saveDetector;
        vm.setPristine = setPristine;
        vm.checkDetectionType = checkDetectionType;
        vm.isRequired = false;
        vm.isDistanceDisabled = false;
        vm.isDisabled = false;
        vm.messageMinSpeed = "";
        vm.messageDecisionPoint = "";
        vm.notAllowed = "default";
        vm.checkLaneDetType = checkLaneDetType;
        vm.laneMessage = "";
        vm.laneDet = false;
        vm.showWarning = showWarning;
        vm.showDetChError = "";
        vm.detectorUpdatedSucceeded = detectorUpdatedSucceeded;
        vm.saveDetectorComments = saveDetectorComments;
        vm.getDetectorComments =  getDetectorComments;
        vm.detector.DetectionTypeID = (vm.detector.DetectionTypeID == 0) && vm.detector.DetectionTypes.length > 0 ? vm.detector.DetectionTypes[0].DetectionTypeID : vm.detector.DetectionTypeID;
        vm.checkDetectionType(vm.detector.DetectionTypeID);
        //set options for the search bar
        vm.searchBarConfig = {};

        if (!vm.detector) {
            vm.addNewdetector = true;
            vm.headerMessage = "Add New detector";
        }
        else {
            vm.addNewdetector = false
            vm.headerMessage = "Edit detector " + vm.detector.DetectorID + " - " + vm.detector.Description;
        }

        vm.goToApproach = goToApproach;

        vm.getDetectorComments(vm.detector.ID);

        function getDetectorComments(id) {
            $http({
                method: 'GET',
                url: 'EnhancedConfiguration/GetDetectorComments/',
                dataType: 'json',
                params: { id: id },
                headers: {
                    "Content-Type": "application/json , Access-Control-Allow-Headers: Content-Type Access- Control - Allow - Methods: GET, POST, OPTIONS Access- Control - Allow - Origin: *"
                }
            }).then(function (res, status, headers, config) {
                vm.detector.CommentText = res.data;
                vm.detectorCommentText = vm.detector.CommentText;
            }).catch(function (data, status, headers, config) {
                errorToast();
            });
        }

        function goToApproach() {
            var params = { signal: vm.signal, approach: vm.approach, lookups: vm.lookups };
            $state.go('MyApp.edit.approach', { 'inputParameters': params });
        };

        function isFormValid() {
            if (vm.detectorForm) {
                return vm.detectorForm.$valid;
            }
        }

        function isFormDirty() {
            if (vm.detectorForm) {
                return vm.detectorForm.$dirty;
            }
        }

        function setPristine() {
            vm.detectorForm.$setPristine();
        }

        function enableSaveButton() {
            if (isFormValid() && isFormDirty()) {
                return true;
            }
            return false;
        }

        function showWarning() {
            vm.laneMessage = "Detector Channel " + vm.checkDetector.DetChannel + " has the same detection type (" + vm.checkDetector.Description + ")"
                + " and is already assigned to this lane number " + vm.checkDetector.LaneNumber
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

        function checkLaneDetType(laneNumber, detectionTypeID, id) {
            vm.checkDetector = null;
            vm.approach.Detectors.forEach(function (det) {
                if (det.ID != id && det.LaneNumber == laneNumber && det.DetectionTypeID == detectionTypeID) {
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
            var laneType = vm.lookups.LaneTypes.find(x => x.ID == vm.detector.LaneTypeID);
            var movementType = vm.lookups.MovementTypes.find(x => x.ID == vm.detector.MovementTypeID);

            vm.detector.MovementType = {
                Abbreviation: movementType.ExtraData,
                Description: movementType.Description,
                MovementTypeID: movementType.ID
            }

            vm.detector.LaneType = {
                Abbreviation: laneType.ExtraData,
                Description: laneType.Description,
                MovementTypeID: laneType.ID
            }
            vm.LaneTypeDesc = laneType.Description;
            vm.MovementTypeDesc = movementType.Description;

            $http({
                method: 'POST',
                url: 'EnhancedConfiguration/AddUpdateDetector', //signal?&SignalID=' + vm.signal.SignalID + '&PrimaryName=' + vm.signal.PrimaryName + '&SecondaryName=' + vm.signal.SecondaryName + '&IPAddress=' + vm.signal.IPAddress + '&Latitude=' + vm.signal.Latitude + '&Longitude=' + vm.signal.Longitude + '&ControllerTypeID=' + vm.signal.ControllerTypeID + '&Enabled=' + vm.signal.Enabled,
                dataType: 'json',
                data: { det: vm.detector },
                headers: {
                    "Content-Type": "application/json , Access-Control-Allow-Headers: Content-Type Access- Control - Allow - Methods: GET, POST, OPTIONS Access- Control - Allow - Origin: *"
                }
            }).then(function (res, status, headers, config) {
                if (vm.detector.CommentText != vm.detectorCommentText) {
                    vm.saveDetectorComments(vm.detector.ID, vm.detectorCommentText);
                }
                else {
                    vm.goToApproach();
                    vm.errorMessage = "";
                }
                vm.errorMessage = "";
                $scope.ok();
            }).catch(function (e, status, headers, config) {
                errorToast();
                vm.errorMessage = 'Error: ' + e.data + '!';
            });
        }

        function saveDetectorComments(id, comment) {
            var detectorComment = {
                ID: id,
                CommentText: comment
            }
            $http({
                method: 'POST',
                url: 'EnhancedConfiguration/SaveDetectorComments',
                dataType: 'json',
                data: { detectorComment: detectorComment },
                headers: {
                    "Content-Type": "application/json , Access-Control-Allow-Headers: Content-Type Access- Control - Allow - Methods: GET, POST, OPTIONS Access- Control - Allow - Origin: *"
                }
            }).then(function (res, status, headers, config) {
                vm.goToApproach();
                vm.errorMessage = "";
                $scope.ok();
            }).catch(function (e, status, headers, config) {
                errorToast();
                vm.errorMessage = 'Error: ' + e.data + '!';
            });
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

        $scope.ok = function () {
            $mdDialog.hide({ message: 'Success' });
        };

        function detectorUpdatedSucceeded() {
            var foundIndex = vm.approach.Detectors.findIndex(x => x.DetChannel == vm.detector.DetChannel);
            if (foundIndex !== -1)
                vm.approach.Detectors[foundIndex] = vm.detector;
            vm.setPristine();
        }

        function deleteDetector(obj) {
            signalService.deleteDetector(vm.detector, goToApproach);
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
}());
