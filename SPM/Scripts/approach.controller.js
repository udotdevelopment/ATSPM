(function () {
    "use strict";

    angular
        .module("MyApp")
        .controller("approachController", approachController);

    function approachController($state, $stateParams, $scope, $mdDialog, $http, $mdToast) {
        var vm = this;
        vm.dtInstance = {};
        vm.signal = undefined;
        vm.approach = undefined;
        vm.lookups = undefined;

        if ($stateParams.inputParameters) {
            vm.signal = $stateParams.inputParameters.signal;
            vm.approach = $stateParams.inputParameters.approach;
            vm.approachID = $stateParams.inputParameters.approach.ApproachID;
            vm.lookups = $stateParams.inputParameters.lookups;
        }
        else {
            //go to signal list
            $state.go('MyApp', null);
            return;
        }
        vm.setupDataTable = setupDataTable;
        vm.getDetectors = getDetectors;
        vm.goToSignal = goToSignal;
        vm.searchChange = searchChange;
        vm.totalDetectorCount = 0;
        vm.goToDetectorEdit = goToDetectorEdit;
        vm.deleteDetector = deleteDetector;
        vm.goToAddDetector = goToAddDetector;
        vm.isFormValid = isFormValid;
        vm.isFormDirty = isFormDirty;
        vm.enableSaveButton = enableSaveButton;
        vm.saveApproach = saveApproach;
        vm.detectorPromise = null;
        vm.detectors = [];
        vm.setPristine = setPristine;
        vm.deleteApproach = deleteApproach;
        vm.checkPhases = checkPhases;
        vm.samePhases = false;
        vm.message = "";

        vm.setupDataTable();
        vm.getDetectors();
        //set options for the search bar

        if (!vm.approach) {
            vm.addNewApproach = true;
            vm.headerMessage = "Add New Approach";
        }
        else {
            vm.addNewApproach = false
            vm.headerMessage = "Edit Approach " + vm.approach.Description;
        }

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

        function getDetectors() {
            vm.queryData.Id = vm.approachID.toString();
            $http({
                method: 'POST',
                url: 'EnhancedConfiguration/GetAllFromQueryDetectors',
                dataType: 'json',
                data: { query: vm.queryData },
                headers: {
                    "Content-Type": "application/json , Access-Control-Allow-Headers: Content-Type Access- Control - Allow - Methods: GET, POST, OPTIONS Access- Control - Allow - Origin: *"
                }
            }).then(function (res, status, headers, config) {
                vm.detectors = res.data.Detectors;
                vm.totalDetectorCount = res.data.TotalCount;
                for (var i = 0; i < vm.detectors.length; i++) {
                    for (var l = 0; l < vm.lookups.LaneTypes.length; l++) {
                        if (vm.lookups.LaneTypes[l].ID == vm.detectors[i].LaneTypeID) {
                            vm.detectors[i].LaneTypeDesc = vm.lookups.LaneTypes[l].Description;
                            break;
                        }
                    }
                    for (var m = 0; m < vm.lookups.MovementTypes.length; m++) {
                        if (vm.lookups.MovementTypes[m].ID == vm.detectors[i].MovementTypeID) {
                            vm.detectors[i].MovementTypeDesc = vm.lookups.MovementTypes[m].Description;
                            break;
                        }
                    }
                }
            }).catch(function (data, status, headers, config) {
                errorToast();
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

        function goToSignal() {
            $state.go('MyApp.edit', null);
        };


        function deleteApproach() {

        }

        function goToDetectorEdit(obj) {
            if (!obj) {
                obj = vm.selected[0];
            }
            //breadcrumbNavigationService.navigateToStateWithoutBreadCrumb('app.spm.config.detectors.edit', { signal: vm.signal, approach: vm.approach, detector: obj, lookups: vm.lookups });
            var params = { signal: vm.signal, detector: obj, approach: vm.approach, lookups: vm.lookups };
            $state.go('MyApp.edit.approach.detector', { 'inputParameters': params });
        }

        function deleteDetector(obj) {
            if (!obj) {
                obj = vm.selected[0];
            }
            deleteDetectorDialog(obj, getDetectors);
            vm.selected = [];

            var foundIndex = vm.approach.Detectors.findIndex(x => x.DetChannel == obj.DetChannel);
            if (foundIndex !== -1) vm.approach.Detectors.splice(foundIndex, 1);
        }

        function deleteDetectorDialog(detector, callback) {
            // Appending dialog to document.body to cover sidenav in docs app
            var confirmText = "Are you sure you want to delete " +
                detector.DetectorID +
                ": " +
                detector.Description;

            var confirm = $mdDialog.confirm()
                .title(confirmText)
                .textContent('This action cannot be undone')
                .ok('Yes delete this detector')
                .cancel('Cancel');

            $mdDialog.show(confirm).then(function () {
                deleteDetectorNoPrompt(detector.ID);
            });
        }

        function deleteDetectorNoPrompt(id) {
            $http({
                method: 'POST',
                url: 'EnhancedConfiguration/DeleteDetector',
                dataType: 'json',
                data: { id: id },
                headers: {
                    "Content-Type": "application/json , Access-Control-Allow-Headers: Content-Type Access- Control - Allow - Methods: GET, POST, OPTIONS Access- Control - Allow - Origin: *"
                }
            }).then(function (res, status, headers, config) {
                vm.getDetectors();
                vm.errorMessage = "";
                showDetectorDeletedToast();
                $scope.ok();
            }).catch(function (e, status, headers, config) {
                console.log("here is the error : " + e);
            });
        }

        function showDetectorDeletedToast() {
            $mdToast.show(
                $mdToast.simple()
                    .parent(document.querySelectorAll('#content'))
                    .textContent('Detector deleted successfully')
                    .position("top right")
                    .hideDelay(3000)
            );
        }


        function goToAddDetector(ev) {
            var confirm = $mdDialog.show({
                locals: { lookups: vm.lookups, signal: vm.signal, approach: vm.approach },
                controller: 'detectorAddController as vm',
                templateUrl: 'Scripts/detector-add.html',
                parent: angular.element(document.body),
                targetEvent: ev,
            }).then(function (response) {
                vm.getDetectors();
            });
        }

        function isFormValid() {
            if (vm.approachForm) {
                return vm.approachForm.$valid;
            }
        }

        function isFormDirty() {
            if (vm.approachForm) {
                return vm.approachForm.$dirty;
            }
        }

        function enableSaveButton() {
            if (isFormValid() && isFormDirty()) {
                return true;
            }
            return false;
        }

        function setPristine() {
            vm.approachForm.$setPristine();

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
                showApproachEditedToast();
                $scope.ok();
            }).catch(function (e, status, headers, config) {
                errorToast();
                vm.errorMessage = 'Error: ' + e.data + '!';
            });
        };

        function showApproachEditedToast() {
            $mdToast.show(
                $mdToast.simple()
                    .parent(document.querySelectorAll('#content'))
                    .textContent('Aprroach successfully edited.')
                    .position("top right")
                    .hideDelay(3000)
            );
        }

        $scope.ok = function () {
            $mdDialog.hide({ message: 'Success' });
        };

        function setupDataTable() {
            vm.selected = [];
            vm.totalCount = {};
            vm.options = {
                rowSelection: true,
                multiSelect: false,
                autoSelect: true,
                decapitate: false,
                largeEditDialog: false,
                boundaryLinks: false,
                limitSelect: true,
                pageSelect: true,
                filter: {
                    debounce: 500
                }
            };

            vm.queryData = {
                OrderBy: 'detectorID',
                PageSize: 5,
                PageIndex: 1,
                Filter: '',
                Id: vm.approach.ApproachID
            };
        }
        var bookmark;
        function searchChange(newValue, oldValue) {
            if (!oldValue) {
                bookmark = vm.queryData.pageIndex;
            }

            if (newValue !== oldValue) {
                vm.queryData.pageIndex = 1;
            }

            if (!newValue) {
                vm.queryData.pageIndex = bookmark;
            }
            vm.getDetectors();
        };
    }
}());
