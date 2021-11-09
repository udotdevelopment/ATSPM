(function () {
    'use strict';

    angular
        .module('MyApp')
        .directive('EnhancedConfigDirective', EnhancedConfigDirective);

    EnhancedConfigDirective.$inject = ['$window'];

    function EnhancedConfigDirective($window) {
        // Usage:
        //     <directive></directive>
        // Creates:
        // 
        var directive = {
            link: link,
            restrict: 'EA',
            controller: 'EnhancedConfigController as vm',
            templateUrl: function (elem, attrs) {
                return '~/Views/EnhancedConfiguration/Index.cshtml';
            },
        };
        return directive;

        function link(scope, element, attrs) {
        }
    }

})();