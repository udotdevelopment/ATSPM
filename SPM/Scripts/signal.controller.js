(function () {
    "use strict";

    angular
        .module("MyApp")
        .controller("signalController", signalController);

    function signalController($state, $rootScope, $element, $stateParams, $scope, $mdDialog, $http, $mdToast) {
        var vm = this;
        vm.signal = null;
        vm.signalID = 0;
        vm.lookups = undefined;
        vm.StartVersion = undefined; 


        if ($stateParams.inputParameters) {
            vm.lookups = $stateParams.inputParameters.lookups;
            if ($stateParams.inputParameters.signal) {
                vm.signalID = $stateParams.inputParameters.signal.SignalID;
                vm.signal = $stateParams.inputParameters.signal;
                vm.headerMessage = "Edit Signal Version: " + vm.signal.PrimaryName + " - " + vm.signal.SecondaryName;
                vm.signal.Start != null ? vm.StartVersion = new Date(vm.signal.Start) : vm.StartVersion = new Date();
            }
        }
        else {
            //go to signal list
            $state.go('MyApp');
            return;
        }
        //set options for the search bar
        vm.approachPromise = null;
        vm.totalApproaches = 0;
        vm.getApproaches = getApproaches;
        vm.setupDataTable = setupDataTable;
        vm.getSignal = getSignal;
        vm.goToAddApproach = goToAddApproach;
        vm.changeValues = changeValues;
        vm.goToApproachEdit = goToApproachEdit;
        vm.saveSignal = saveSignal;
        vm.deleteSignalVersion = deleteSignalVersion;
        vm.goToSignals = goToSignals;
        vm.deleteApproach = deleteApproach;
        vm.isFormValid = isFormValid;
        vm.isFormDirty = isFormDirty;
        vm.enableSaveButton = enableSaveButton;
        vm.setPristine = setPristine;
        vm.getVersionsOfSignal = getVersionsOfSignal;
        vm.getSignal();
        vm.setupDataTable();
        vm.getVersionsOfSignal();
        vm.comment = vm.signal.Comments != null && vm.signal.Comments.length > 0 ? vm.signal.Comments[vm.signal.Comments.length-1].CommentText : '';


        function changeValues() {
            //vm.getSignal();
            vm.setupDataTable();
            vm.getApproaches();
        }

        function getSignal() {
            $http({
                method: 'GET',
                url: 'EnhancedConfiguration/GetSignalBySignalId/',
                dataType: 'json',
                params: { signalID: vm.signalID },
                headers: {
                    "Content-Type": "application/json , Access-Control-Allow-Headers: Content-Type Access- Control - Allow - Methods: GET, POST, OPTIONS Access- Control - Allow - Origin: *"
                }
            }).then(function (res, status, headers, config) {
                vm.signal = res.data;
                vm.getApproaches();
            }).catch(function (data, status, headers, config) {
                errorToast();
            });
        }

        function getVersionsOfSignal() {
            $http({
                method: 'GET',
                url: 'EnhancedConfiguration/GetAllVersions/',
                dataType: 'json',
                params: { signalID: vm.signalID },
                headers: {
                    "Content-Type": "application/json , Access-Control-Allow-Headers: Content-Type Access- Control - Allow - Methods: GET, POST, OPTIONS Access- Control - Allow - Origin: *"
                }
            }).then(function (res, status, headers, config) {
                vm.versions = res.data;
            }).catch(function (data, status, headers, config) {
                errorToast();
            });
        }

        vm.selectChanged = function (verId) {
            vm.signal = vm.versions.find(x => x.VersionID == verId);
            vm.signal.Start != null ? vm.StartVersion = new Date(vm.signal.Start) : vm.StartVersion = new Date();
            vm.comment = vm.signal.Comments != null && vm.signal.Comments.length > 0 ? vm.signal.Comments[vm.signal.Comments.length - 1].CommentText : '';
            vm.getApproaches();
            vm.getVersionsOfSignal();
        };

        function getApproaches() {
            vm.queryData.Id = vm.signal.SignalID.toString();
            vm.queryData.VersionId = vm.signal.VersionID.toString();
            if (vm.queryData.VersionId != '')
            {
                $http({
                    method: 'POST',
                    url: 'EnhancedConfiguration/GetAllFromQueryApproaches', //query?&Filter=' + vm.queryData.Filter + '&Id=' + vm.queryData.ID + '&OrderBy=' + vm.queryData.OrderBy + '&PageIndex=' + vm.queryData.PageIndex + '&PageSize=' + vm.queryData.PageSize,
                    dataType: 'json',
                    data: { query: vm.queryData },
                    headers: {
                        "Content-Type": "application/json , Access-Control-Allow-Headers: Content-Type Access- Control - Allow - Methods: GET, POST, OPTIONS Access- Control - Allow - Origin: *"
                    }
                }).then(function (res, status, headers, config) {
                    vm.approaches = res.data.Approaches;
                    vm.totalApproaches = res.data.TotalCount;
                    for (var i = 0; i < vm.approaches.length; i++) {
                        for (var l = 0; l < vm.lookups.DirectionTypes.length; l++) {
                            if (vm.lookups.DirectionTypes[l].ID == vm.approaches[i].DirectionTypeID) {
                                vm.approaches[i].DirectionTypeDesc = vm.lookups.DirectionTypes[l].Description;
                                break;
                            }
                        }
                    };
                }).catch(function (data, status, headers, config) {
                    errorToast();
                });
            }
        };


        function deleteSignalVersion() {
            removeSignalVersion(vm.signal, vm.getVersionsOfSignal);
        }

        function removeSignalVersion(signal, callback) {
            // Appending dialog to document.body to cover sidenav in docs app
            var confirmText = "Are you sure you want to delete version " +
                signal.VersionID + " " +
                signal.PrimaryName +
                " - " +
                signal.SecondaryName + "?";

            var confirm = $mdDialog.confirm()
                .title(confirmText)
                .textContent('This action cannot be undone')
                .ok('Yes delete this version')
                .cancel('Cancel');

            $mdDialog.show(confirm)
                .then(function () {
                    removeSignalVersionNoPrompt(signal.VersionID)
                });
        }

        function removeSignalVersionNoPrompt(id) {
            var versionId = id;
            $http({
                method: 'POST',
                url: 'EnhancedConfiguration/DeleteVersion',
                dataType: 'json',
                data: { id: id },
                headers: {
                    "Content-Type": "application/json , Access-Control-Allow-Headers: Content-Type Access- Control - Allow - Methods: GET, POST, OPTIONS Access- Control - Allow - Origin: *"
                }
            }).then(function (res, status, headers, config) {
                showSignalVersionDeletedToast();
                vm.getVersionsOfSignal();
                vm.versions = vm.versions.filter(x => x.VersionID != versionId);
                vm.signal = vm.versions.length > 0 ? vm.versions[0] : $state.go('MyApp');
            }).catch(function (e, status, headers, config) {
                errorToast();
            });
        }

        function showSignalVersionDeletedToast() {
            $mdToast.show(
                $mdToast.simple()
                    .parent(document.querySelectorAll('#content'))
                    .textContent('Signal Version deleted successfully')
                    .position("top right")
                    .hideDelay(3000)
            );
        };

        function saveSignal() {
            vm.signal.Start = vm.StartVersion;
            updateSignal(vm.signal, vm.setPristine);
        }

        function updateSignal(signal, callback, isSignalDraged) {
            // Appending dialog to document.body to cover sidenav in docs app
            var confirmText = "Are you sure you want to save changes for " +
                signal.SignalID + " " +
                signal.PrimaryName +
                " - " +
                signal.SecondaryName + "?";

            var content = ''
            isSignalDraged ? content = 'Latitude and Longitude are changed. This action cannot be undone.' : content = 'This action cannot be undone';

            var confirm = $mdDialog.confirm()
                .title(confirmText)
                .textContent(content)
                .ok('Yes save changes')
                .cancel('Cancel');
            var selectedChoice = $mdDialog.show(confirm).then(function () {
                    updateSignalNoPrompt(signal);
            });

            return selectedChoice;
        }

        function updateSignalNoPrompt(signal) {

            if (vm.signal.Comments.length > 0 && vm.signal.Comments[vm.signal.Comments.length - 1].CommentText != vm.comment) {
                vm.signal.Comments[0].CommentText = vm.comment;
            }
            if (vm.signal.Comments.length == 0) {
                vm.signal.Comments = [{ CommentText: vm.comment }];
            }

            $http({
                method: 'POST',
                url: 'EnhancedConfiguration/UpdateSignalVersion',
                dataType: 'json',
                data: { signal: signal },
                headers: {
                    "Content-Type": "application/json , Access-Control-Allow-Headers: Content-Type Access- Control - Allow - Methods: GET, POST, OPTIONS Access- Control - Allow - Origin: *"
                }
            }).then(function (res, status, headers, config) {
                vm.errorMessage = "";
                showSignalSavedToast();
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

        function showSignalSavedToast() {
            $mdToast.show(
                $mdToast.simple()
                    .parent(document.querySelectorAll('#content'))
                    .textContent('Signal saved successfully')
                    .position("top right")
                    .hideDelay(3000)
            );
        }


        function goToSignals() {
            $state.go('MyApp', null);
            //breadcrumbNavigationService.navigateToStateWithoutBreadCrumb('app.spm.config.signals');
        };

        function goToAddApproach(ev) {
            var confirm = $mdDialog.show({
                locals: { lookups: vm.lookups, signal: vm.signal },
                controller: 'approachAddController as vm',
                templateUrl: 'Scripts/approach-add.html',
                parent: angular.element(document.body),
                targetEvent: ev,
            }).then(function () {
                vm.getApproaches();
            });
        };

        function goToApproachEdit(obj) {
            if (!obj) {
                obj = vm.selected[0];
            }
            //breadcrumbNavigationService.navigateToStateWithoutBreadCrumb('app.spm.config.approaches.edit', { approach: obj, signal: vm.signal, lookups: vm.lookups });
            var params = { signal: vm.signal, approach: obj, lookups: vm.lookups };
            $state.go('MyApp.edit.approach', { 'inputParameters': params });
        }

        //testing
        function deleteApproach(approach, callback) {
            // Appending dialog to document.body to cover sidenav in docs app
            var confirmText = "Are you sure you want to delete " +
                approach.ApproachID +
                ": " +
                approach.Description;

            var confirm = $mdDialog.confirm()
                .title(confirmText)
                .textContent('This action cannot be undone')
                .ok('Yes delete this approach')
                .cancel('Cancel');

            $mdDialog.show(confirm).then(function () {
                deleteApproachNoPrompt(approach.ApproachID).then(function () {
                    if (callback)
                        callback();
                });
            }, function () {
            });
        }


        function deleteApproachDialog(approach, callback) {
            // Appending dialog to document.body to cover sidenav in docs app
            var confirmText = "Are you sure you want to delete " +
                approach.ApproachID +
                ": " +
                approach.Description;

            var confirm = $mdDialog.confirm()
                .title(confirmText)
                .textContent('This action cannot be undone')
                .ok('Yes delete this approach')
                .cancel('Cancel');

            $mdDialog.show(confirm).then(function () {
                deleteApproachNoPrompt(approach.ApproachID).then(function () {
                    if (callback)
                        callback();
                });
            }, function () {
            });

        }

        // TODO: get signal returned when deleting the approach
        function deleteApproachNoPrompt(id) {
            $http({
                method: 'POST',
                url: 'EnhancedConfiguration/DeleteApproach',
                dataType: 'json',
                data: { id: id },
                headers: {
                    "Content-Type": "application/json , Access-Control-Allow-Headers: Content-Type Access- Control - Allow - Methods: GET, POST, OPTIONS Access- Control - Allow - Origin: *"
                }
            }).then(function (res, status, headers, config) {
                vm.getApproaches();
                vm.errorMessage = "";
                showApproachDeletedToast();
                $scope.ok();
            }).catch(function (e, status, headers, config) {
                errorToast();
            });

        }

        $scope.ok = function () {
            $mdDialog.hide({ message: 'Success' });
        };

        function showApproachDeletedToast() {
            $mdToast.show(
                $mdToast.simple()
                    .parent(document.querySelectorAll('#content'))
                    .textContent('Approach deleted successfully')
                    .position("top right")
                    .hideDelay(3000)
            );
        }


        function deleteApproach(obj) {
            if (!obj) {
                obj = vm.selected[0];
            }

            // Appending dialog to document.body to cover sidenav in docs app
            deleteApproachDialog(obj, getApproaches);
            vm.selected = [];
        }

        function isFormValid() {
            if (vm.signalEditForm) {
                return vm.signalEditForm.$valid;
            }
        }

        function setPristine() {
            vm.signalEditForm.$setPristine();
        }

        function enableSaveButton() {
            if (isFormValid() && isFormDirty()) {
                return true;
            }
            return false;
        }

        function isFormDirty() {
            if (vm.signalEditForm) {
                return vm.signalEditForm.$dirty;
            }
        }

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
                OrderBy: 'approachID',
                PageSize: 5,
                PageIndex: 1,
                Filter: ''
            };
        }
        var bookmark;
        vm.searchChange = function (newValue, oldValue) {
            if (!oldValue) {
                bookmark = vm.queryData.PageIndex;
            }

            if (newValue !== oldValue) {
                vm.queryData.PageIndex = 1;
            }

            if (!newValue) {
                vm.queryData.PageIndex = bookmark;
            }
            vm.getApproaches();
        };

        /*vm.getApproaches();*/
    }
}());