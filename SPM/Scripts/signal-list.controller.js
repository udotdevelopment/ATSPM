(function () {
    'use strict';

    angular
        .module('MyApp')
        .controller('signalListController', signalListController);

    function signalListController($location, $scope, $http, $mdDialog, $state, $mdToast) {
        /* jshint validthis:true */
        var vm = this;
        vm.signals = [];
        vm.getSignals = getSignals;
        vm.getNumberOfSignals = getNumberOfSignals;
        setupDataTable();
        vm.goToSignals = goToSignals;
        //set options for the search bar

        vm.totalQueryCount = 0;
        vm.isCurrentStateEdit = $state.$current.self.name.includes("edit");


        function getSignals() {
            if (vm.signalDeleted) {
                vm.selected = [];
                vm.queryData.filter = "";
            }

            //query ?& fileType=& filter=& id=& orderBy=signalID & pageIndex=1 & pageSize=10
            $http({
                method: 'GET',
                url: 'EnhancedConfiguration/GetAllFromQuery/query?&Filter=' + vm.queryData.Filter + '&Id=' + vm.queryData.Id + '&OrderBy=' + vm.queryData.OrderBy + '&PageIndex=' + vm.queryData.PageIndex + '&PageSize=' + vm.queryData.PageSize,
                dataType: 'json',
                data: { query: vm.queryData },
                headers: {
                    "Content-Type": "application/json , Access-Control-Allow-Headers: Content-Type Access- Control - Allow - Methods: GET, POST, OPTIONS Access- Control - Allow - Origin: *"
                }
            }).then(function (res, status, headers, config) {
                getSignalsSuccess(res.data);
            }).catch(function (data, status, headers, config) {
                errorToast();
            });

            vm.signalDeleted = false;
        }

        function getNumberOfSignals() {
            $http({
                method: 'GET',
                url: 'EnhancedConfiguration/GetNumberOfSignals',
                dataType: 'json',
                /*            data: { name: $scope.Name },*/
                headers: {
                    "Content-Type": "application/json , Access-Control-Allow-Headers: Content-Type Access- Control - Allow - Methods: GET, POST, OPTIONS Access- Control - Allow - Origin: *"
                }
            }).then(function (res, status, headers, config) {
                vm.totalCount = res.data;
                vm.totalQueryCount = res.data;
                vm.lookups = "TEST";
            }).catch(function (data, status, headers, config) {
                errorToast();
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


        function getSignalsSuccess(data) {
            vm.totalCount = data.TotalCount;
            vm.totalQueryCount = data.TotalCountInQuery;
            vm.signals = data.Signals;
            vm.lookups = data.Lookups;
        }

        vm.goToAddSignal = function (ev) {
            var confirm = $mdDialog.show({
                locals: { lookups: vm.lookups },
                controller: 'signalAddController as vm',
                templateUrl: 'Scripts/signal-add.html',
                parent: angular.element(document.body),
                targetEvent: ev,
            }).then(function () {
                vm.getSignals();
            });
        };

        vm.goToAddVersion = function (ev, obj) {
            if (!obj) {
                obj = vm.selected[0];
            }
            var params = { signal: obj, lookups: vm.lookups };
            var confirm = $mdDialog.show({
                locals: { params: params },
                controller: 'versionAddController as vm',
                templateUrl: 'Scripts/version-add.html',
                parent: angular.element(document.body),
                targetEvent: ev,
            }).then(function () {
            });

        };

        

        vm.goToEditSignal = function (obj) {
            if (!obj) {
                obj = vm.selected[0];
            }
            //{ 'inputParameters': params }
            var params = { signal: obj, lookups: vm.lookups };
            $state.go('MyApp.edit', { 'inputParameters': params });
            //breadcrumbNavigationService.navigateToStateWithoutBreadCrumb('app.spm.config.signals.edit', { signal: obj, lookups: vm.lookups });
        };

        function showSignalDeletedToast() {
            $mdToast.show(
                $mdToast.simple()
                    .parent(document.querySelectorAll('#content'))
                    .textContent('Signal deleted successfully')
                    .position("top right")
                    .hideDelay(3000)
            );
        }

        function showVersionDeletedToast() {
            $mdToast.show(
                $mdToast.simple()
                    .parent(document.querySelectorAll('#content'))
                    .textContent('Version deleted successfully')
                    .position("top right")
                    .hideDelay(3000)
            );
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

        function removeSignalNoPrompt(id) {
            $http({
                method: 'POST',
                url: 'EnhancedConfiguration/DeleteSignal',
                dataType: 'json',
                data: { id: id },
                headers: {
                    "Content-Type": "application/json , Access-Control-Allow-Headers: Content-Type Access- Control - Allow - Methods: GET, POST, OPTIONS Access- Control - Allow - Origin: *"
                }
            }).then(function (res, status, headers, config) {
                showSignalDeletedToast();
                vm.getSignals();
            }).catch(function (e, status, headers, config) {
                errorToast();
            });
        }

        function removeVersionNoPrompt(id) {
            $http({
                method: 'POST',
                url: 'EnhancedConfiguration/DeleteVersion',
                dataType: 'json',
                data: { id: id },
                headers: {
                    "Content-Type": "application/json , Access-Control-Allow-Headers: Content-Type Access- Control - Allow - Methods: GET, POST, OPTIONS Access- Control - Allow - Origin: *"
                }
            }).then(function (res, status, headers, config) {
                showVersionDeletedToast();
                vm.getSignals();
            }).catch(function (e, status, headers, config) {
                errorToast();
            });
        }

        function removeSignal(signal, callback) {
            // Appending dialog to document.body to cover sidenav in docs app
            var confirmText = "Are you sure you want to delete " +
                signal.SignalID + " " +
                signal.PrimaryName +
                " - " +
                signal.SecondaryName + "?";

            var confirm = $mdDialog.confirm()
                .title(confirmText)
                .textContent('This action cannot be undone')
                .ok('Yes delete this signal')
                .cancel('Cancel');

            $mdDialog.show(confirm)
                .then(function () {
                    removeSignalNoPrompt(signal.SignalID)
                });
        }


        vm.deleteSignalVersion = function (ev,obj) {
            $mdDialog.show({
                controller: 'DialogController as vm',
                templateUrl: 'Scripts/delete-version-signal.html',
                parent: angular.element(document.body),
                targetEvent: ev,
                clickOutsideToClose: true,
                fullscreen: false // Only for -xs, -sm breakpoints.
            }).then(function (answer) {

                if (!obj) {
                    obj = vm.selected[0];
                }
                vm.signalDeleted = true;

                if (answer == 'signal') {
                    var signalId = obj.SignalID;
                    removeSignalNoPrompt(signalId);
                }
                if (answer == 'version') {
                    var versionId = obj.VersionID;
                    removeVersionNoPrompt(versionId);
                }
            });
        };


        vm.deleteSignal = function (obj) {
            if (!obj) {
                obj = vm.selected[0];
            }
            vm.signalDeleted = true;
            removeSignal(obj, vm.getSignals);
        }

        function goToSignals() {
            breadcrumbNavigationService.navigateToStateWithoutBreadCrumb('app.spm.config.signals');
        };

        var bookmark;
        vm.searchChange = function (newValue, oldValue) {
            if (!oldValue) {
                bookmark = vm.queryData.pageIndex;
            }

            if (newValue !== oldValue) {
                vm.queryData.pageIndex = 1;
            }

            if (!newValue) {
                vm.queryData.pageIndex = bookmark;
            }

            vm.getSignals();
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
                OrderBy: 'signalID',
                PageSize: 10,
                PageIndex: 1,
                Filter: '',
                Id: '',
            };
        }


        vm.getSignals();
        
    }
})();

