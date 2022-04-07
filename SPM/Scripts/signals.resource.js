(function () {
    'use strict';

    angular
        .module('MyApp')
        .factory("signalsResource", ["$resource", function ($resource) {
            var signalsApiPath = "https://localhost/UDOT-WcfService/api/data/getAllSignals";
            var detApiPath = "";
            var apprApiPath = "";

            return {
                signals: $resource(signalApiPath, {}, {
                    getAllSignals: { method: "GET" }
                })
            };
        }]);
})();