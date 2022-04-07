(function () {
    'use strict';

    angular.module('MyApp')
        .directive('editContextMenu', editContextMenu);

    editContextMenu.$inject = ['$mdMenu'];
    /** @ngInject */
    function editContextMenu($mdMenu) {
        function link(scope, element, attrs) {

        }

        return {
            restrict: 'E',
            link: link,
            scope: {
                editAction: '&',
                deleteAction: '&',
                editObject: '=',
                noExcel: '='
            },
            controller: function ($scope) {
                $scope.editClicked = function (ev) {
                    if ($scope.editAction) {
                        $scope.editAction({ obj: $scope.editObject });
                    }
                }
                $scope.deleteClicked = function (ev) {
                    if ($scope.deleteAction) {
                        $scope.deleteAction({ obj: $scope.editObject });
                    }
                }
            },
            templateUrl: 'Scripts/edit-context-menu.html',
        };
    }
})();