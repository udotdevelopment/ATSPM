(function () {
    'use strict';

    angular
        .module('MyApp')
        .directive('directive', directive);

    directive.$inject = ['$window'];

    function directive($window) {
        // Usage:
        //     <directive></directive>
        // Creates:
        // 
        var directive = {
            link: link,
            restrict: 'EA',
            controller: 'signalListController as vm',
            templateUrl: function (elem, attrs) {
                return '~/Views/FAQs/Index.cshtml';
            },
        };
        return directive;

        function link(scope, element, attrs) {
        }
    }

})();