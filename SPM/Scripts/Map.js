var infobox;
function openWindow(url) {
    var w = window.open(url, '',
    'width=800,height=600,toolbar=0,status=0,location=0,menubar=0,directories=0,resizable=1,scrollbars=1');
    w.focus();
}

function GetMap() 
{
    map = new Microsoft.Maps.Map(document.getElementById('mapDiv'), { credentials: 'ArDqSVBgLAcobelrUlW6yVPIyL-UGPwVKTE0ce2_tAxvrZr5YFnSEFds7I1CNy5O',
        center: new Microsoft.Maps.Location(39.50, -111.00),
        mapTypeId: Microsoft.Maps.MapTypeId.road,
        showDashboard: true,
        showScalebar: false,
        enableSearchLogo: false,
        showMapTypeSelector: false,
        zoom: 6,
        customizeOverlays: false
    });
    var dataLayer = AddData();
    //map.entities.push(dataLayer);
    Microsoft.Maps.loadModule("Microsoft.Maps.Clustering", function () {
        var clusterLayer = new Microsoft.Maps.ClusterLayer(dataLayer, {
            clusteredPinCallback: customizeClusteredPin
        });
        map.layers.insert(clusterLayer);

    });
}

function GetRouteMap() {
    map = new Microsoft.Maps.Map(document.getElementById('mapDiv'), {
        credentials: 'ArDqSVBgLAcobelrUlW6yVPIyL-UGPwVKTE0ce2_tAxvrZr5YFnSEFds7I1CNy5O',
        center: new Microsoft.Maps.Location(39.50, -111.00),
        mapTypeId: Microsoft.Maps.MapTypeId.road,
        showDashboard: true,
        showScalebar: false,
        enableSearchLogo: false,
        showMapTypeSelector: false,
        zoom: 6,
        customizeOverlays: false
    });
    var dataLayer = AddData();
    //map.entities.push(dataLayer);
    Microsoft.Maps.loadModule("Microsoft.Maps.Clustering", function () {
        var clusterLayer = new Microsoft.Maps.ClusterLayer(dataLayer, {
            clusteredPinCallback: customizeClusteredPin
        });
        map.layers.insert(clusterLayer);

    });
}


function ReportTypeChange() {
    var regionDdl = $("#Regions")[0];
    var regionMy = document.getElementById('Regions');
    CenterMap(regionDdl.options[regionDdl.selectedIndex].value);
}

function RegionChange(e) {
    CenterMap(e.options[e.selectedIndex].value);
    var metricsMy = document.getElementById('MetricTypes');

}

function CenterMap(region) {
    if (region == 0) {
        GetMapWithCenter(39.777584, -111.719971, 6);
    }
    else if (region == 1) {
        GetMapWithCenter(41.510213, -112.015501, 8);
    }
    else if (region == 2) {
        GetMapWithCenter(40.653381, -112.040634, 10);
    }
    else if (region == 3) {
        GetMapWithCenter(40.354719, -110.710757, 8);
    }
    else if (region == 4) {
        GetMapWithCenter(38.268951, -111.417847, 7);
    }
}


function GetMapWithCenter(lat, long, zoom) {
    map = new Microsoft.Maps.Map(document.getElementById('mapDiv'), {
        credentials: 'ArDqSVBgLAcobelrUlW6yVPIyL-UGPwVKTE0ce2_tAxvrZr5YFnSEFds7I1CNy5O',
        center: new Microsoft.Maps.Location(lat, long),
        mapTypeId: Microsoft.Maps.MapTypeId.road,
        showDashboard: true,
        showScalebar: false,
        enableSearchLogo: false,
        showMapTypeSelector: false,
        zoom: zoom,
        customizeOverlays: false
    });
    var dataLayer = AddData();
    //map.entities.push(dataLayer);
    Microsoft.Maps.loadModule("Microsoft.Maps.Clustering", function () {
        var clusterLayer = new Microsoft.Maps.ClusterLayer(dataLayer, {
            clusteredPinCallback: customizeClusteredPin
        });
        map.layers.insert(clusterLayer);

    });
}


function customizeClusteredPin(cluster) {
    //Add click event to clustered pushpin
    Microsoft.Maps.Events.addHandler(cluster, 'click', clusterClicked);
}

function clusterClicked(e) {
    if (e.target.containedPushpins) {
        var locs = [];
        for (var i = 0, len = e.target.containedPushpins.length; i < len; i++) {
            //Get the location of each pushpin.
            locs.push(e.target.containedPushpins[i].getLocation());
        }

        //Create a bounding box for the pushpins.
        var bounds = Microsoft.Maps.LocationRect.fromLocations(locs);

        //Zoom into the bounding box of the cluster. 
        //Add a padding to compensate for the pixel area of the pushpins.
        map.setView({ bounds: bounds, padding: 100 });
    }
}

function closeInfobox() {
    if (infobox != null) {
        infobox.setMap(null);
    }
}




function get_type(thing) {
    if (thing === null) return "[object Null]"; // special case
    return Object.prototype.toString.call(thing);
}

function Log10(val) {
    return Math.log(val) / Math.LN10;
}


function ZoomIn(e) {
    if (e.targetType == 'pushpin') {
        var location = e.target.getLocation();
        var pixelOffset = 0;
        var zoomLevel = map.getZoom();
        //The inforboxes are too large to fit in the map if the push-pin is dead center.
        //we need to apply an offset so the infobox is in frame.
        //The offset for a low zoom level is defferent than at a high zoom level
        //There is perhaps some equation that will work for all levels, but this method is pretty good.
        //switch (zoomLevel) {
        //    case 1:
        //        pixelOffset = .02;
        //        break;
        //    case 2:
        //        pixelOffset = .05;
        //        break;
        //    case 3:
        //        pixelOffset = .1;
        //        break;
        //    case 4:
        //        pixelOffset = .37;
        //        break;
        //    case 5:
        //        pixelOffset = .6;
        //        break;
        //    case 6:
        //        pixelOffset = 1;
        //        break;
        //    case 7:
        //        pixelOffset = 1.5;
        //        break;
        //    case 8:
        //        pixelOffset = 3;
        //        break;
        //    case 9:
        //        pixelOffset = 6;
        //        break;
        //    case 10:
        //        pixelOffset = 13;
        //        break;
        //    case 11:
        //        pixelOffset = 27;
        //        break;
        //    case 12:
        //        pixelOffset = 50;
        //        break;
        //    case 13:
        //        pixelOffset = 90;
        //        break;
        //    case 14:
        //        pixelOffset = 180;
        //        break;
        //    case 15:
        //        pixelOffset = 360;
        //        break;
        //    case 16:
        //        pixelOffset = 720;
        //        break;
        //    case 17:
        //        pixelOffset = 1440;
        //        break;
        //    case 18:
        //        pixelOffset = 2880;
        //        break;
        //    case 19:
        //        pixelOffset = 5760;
        //        break;
        //    case 20:
        //        pixelOffset = 11520;
        //        break;
        //    default:
        //        pixelOffset = 100;
        //}
        var centerpixel = map.tryLocationToPixel(location);
        centerpixel.y = centerpixel.y - pixelOffset;
        var newLocation = map.tryPixelToLocation(centerpixel)
        map.setView({
            zoom: 13,
            center: newLocation
        });
    }
}

function AddSignalFromPin(e) {
    if (e.targetType == 'pushpin') {
        var signalId = e.target.SignalID.toString();
        AddSignalToList(signalId);
    }
}




function displayInfobox(e) {
    if (e.targetType == 'pushpin') {
        actionArray = new Array();
        var SignalID = e.target.SignalID.toString();

        var tosend = {};
        tosend.signalID = SignalID;
        $.ajax({
            url: urlpathSignalInfoBox,
            type: "POST",
            cache: false,
            async: true,
            datatype: "json",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(tosend),
            success: function (data) {
                if (infobox != null) {
                    infobox.setOptions({ visible: false });
                }
                infobox = new Microsoft.Maps.Infobox(e.target.getLocation(),
                    { offset: new Microsoft.Maps.Point(-100, 0), htmlContent: data });
                infobox.setMap(map);
                SetControlValues(SignalID, null);
            },
            error: function (jqXHR, textStatus, errorThrown) {
                alert(textStatus);
            }
        });
    } 
}



function CancelAsyncPostBack() {
    if (prm.get_isInAsyncPostBack()) {
        prm.abortPostBack();
    }
}

function InitializeRequest(sender, args) {
    if (prm.get_isInAsyncPostBack()) {
        args.set_cancel(true);
    }
    postBackElement = args.get_postBackElement();
    if (postBackElement.id == 'uxCreateChartButton') {
        $get('UpdateProgress1').style.display = 'block';
    }
}
function EndRequest(sender, args) {
    if (postBackElement.id == 'uxCreateChartButton') {
        $get('UpdateProgress1').style.display = 'none';
    }
}

//function PinFilterCheck(regionFilter, reportTypeFilter, agencyFilter, pinRegion, pinAgency, pinMetricTypes) {
//    return ((regionFilter == -1 && reportTypeFilter == -1 && agencyFilter == -1) ||
//        (regionFilter == pinRegion && agencyFilter == pinAgency && pinMetricTypes.indexOf(reportTypeFilter) > -1) ||
//        (regionFilter == -1 && agencyFilter == -1 && pinMetricTypes.indexOf(reportTypeFilter) > -1) ||
//        (regionFilter == -1 && agencyFilter == pinAgency && reportTypeFilter == -1) ||
//        (regionFilter == pinRegion && agencyFilter == -1 && reportTypeFilter == -1) ||
//        (regionFilter == pinRegion && agencyFilter == pinAgency && reportTypeFilter == -1) ||
//        (regionFilter == pinRegion && agencyFilter == -1 && pinMetricTypes.indexOf(reportTypeFilter) > -1) ||
//        (regionFilter == -1 && agencyFilter == pinAgency && pinMetricTypes.indexOf(reportTypeFilter) > -1)
//        );
//}

function PinFilterCheck(regionFilter, reportTypeFilter, agencyFilter, areaFilter, pinRegion, pinAgency, areas, pinMetricTypes) {
    if (regionFilter != -1 && regionFilter != pinRegion) return false;
    if (agencyFilter != -1 && agencyFilter != pinAgency) return false;
    if (areaFilter != -1 && areas.indexOf(areaFilter) == -1) return false;
    if (reportTypeFilter != -1 && pinMetricTypes.indexOf(reportTypeFilter) == -1) return false;
    return true;
}


