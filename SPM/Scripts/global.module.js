var app =
    angular.module("MyApp", [
        'ngMaterial',
        'ngRoute',
        'ngResource',
        'datatables',
        'ngAnimate',
        'ngAria',
        'ngCookies',
        'ngResource',
        'ngSanitize',
        'angularMoment',
        'ngFileSaver',
        'md-steppers',
        'md.data.table',
        'ui.router',
    ])
           .config(config);


// function
function config($stateProvider) {
    $stateProvider
        .state('MyApp', {
            url: '/signal-list',
            views: {
                "@": {
                    templateUrl: "Scripts/signal-list.html",
                    controller: 'signalListController as vm'
                },
            }
        })
        .state('MyApp.add', {
            url: '/detail/add',
            abstract: true,
            templateUrl: 'Scripts/signal-add.html',
            controller: 'signalAddController as vm',
            params: {
                inputParameters: undefined
            }
        })
        .state('MyApp.edit', {
            url: '/detail/:id',
            views: {
                "@": {
                    templateUrl: "Scripts/signal.html",
                    controller: 'signalController as vm'
                },
            },
            params: {
                inputParameters: undefined
            }
        })
        .state('MyApp.edit.approach', {
            url: '/approaches/edit',
            views: {
                "@": {
                    templateUrl: "Scripts/approach.html",
                    controller: 'approachController as vm'
                },
            },
            params: {
                inputParameters: undefined
            }
        })
        .state('MyApp.edit.approach.detector', {
            url: '/detectors/edit',
            views: {
                "@": {
                    templateUrl: "Scripts/detector.html",
                    controller: 'detectorController as vm'
                },
            },
            params: {
                inputParameters: undefined
            }
        });
}
