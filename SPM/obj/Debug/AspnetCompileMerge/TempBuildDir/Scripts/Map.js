$(function (ready) {
    Microsoft.Maps.loadModule('Microsoft.Maps.Overlays.Style', { callback: GetMap });
});

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
    dataLayer = new Microsoft.Maps.EntityCollection();
    map.entities.push(dataLayer);
    var infoboxLayer = new Microsoft.Maps.EntityCollection();
    map.entities.push(infoboxLayer);
    infobox = new Microsoft.Maps.Infobox(new Microsoft.Maps.Location(0, 0), { visible: false, offset: new Microsoft.Maps.Point(0, 20) });
    infoboxLayer.push(infobox);
    AddData();
}


function ReportTypeChange() {
    var regionDdl = $("#Regions")[0];
    CenterMap(regionDdl.options[regionDdl.selectedIndex].value);
}

function RegionChange(e) {

    CenterMap(e.options[e.selectedIndex].value);

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
    map = new Microsoft.Maps.Map(document.getElementById('mapDiv'), { credentials: 'ArDqSVBgLAcobelrUlW6yVPIyL-UGPwVKTE0ce2_tAxvrZr5YFnSEFds7I1CNy5O',
        center: new Microsoft.Maps.Location(lat, long),
        mapTypeId: Microsoft.Maps.MapTypeId.road,
        showDashboard: false,
        showScalebar: false,
        enableSearchLogo: false,
        showMapTypeSelector: false,
        zoom: zoom
    });

    dataLayer = new Microsoft.Maps.EntityCollection();
    map.entities.push(dataLayer);

    var infoboxLayer = new Microsoft.Maps.EntityCollection();
    map.entities.push(infoboxLayer);

    infobox = new Microsoft.Maps.Infobox(new Microsoft.Maps.Location(0, 0), { visible: false, offset: new Microsoft.Maps.Point(0, 20) });
    infoboxLayer.push(infobox);

    AddData();
}


function closeInfobox() {
    infobox.setOptions({ visible: false });
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

function displayInfobox(e) {
    if (e.targetType == 'pushpin') {
        infobox.setLocation(e.target.getLocation());
        actionArray = new Array();
        var incString = e.target.Actions.toString()
        var SignalID = e.target.SignalID.toString();
        var Region = e.target.Region;

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
                infobox.setOptions({
                    visible: true,
                    offset: new Microsoft.Maps.Point(-100, 20),
                    htmlContent: data
                });
                SetControlValues(SignalID, null);
            },
        });
        

        

    }
}





//function rowClick(signalId) {
////    document.forms[0].AccordionPane1_content$uxEntityTextBox.value = signalId;
////    document.forms[0].AccordionPane1_content$uxEntityTextBox.value = "";
////    document.forms[0].AccordionPane1_content$uxEntityTextBox.value = signalId;
//   // return false;
//}


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

