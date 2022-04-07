(function () {

    "use strict";

    angular
        .module("MyApp")
        .service("signalService", signalService);

    function signalService(signalsResource) {

        /*SIGNALS*/
        function getAllSignals() {
            return signalsResource.signals.getAllSignals(function (data) {
                return data;
            }).$promise;
        }

        return {
            getAllSignals: getAllSignals
        }

    }
})();