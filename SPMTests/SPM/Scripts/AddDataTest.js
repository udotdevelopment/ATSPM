function AddData() {
    var pinColor = new Microsoft.Maps.Color(255, 238, 118, 35);
    var iconURL = window.location.protocol +"//"+window.location.hostname +"/Images/orangePin.png";
    var regionDdl = $("#Regions")[0];
    var regionFilter = 0;
    if (regionDdl.options[regionDdl.selectedIndex].value != "") {
        regionFilter = regionDdl.options[regionDdl.selectedIndex].value;
    }
    var reportType = $("#MetricTypes")[0];
    var reportTypeFilter = 0;
    var pins = [];
    if (reportType.options[reportType.selectedIndex].value != "") {
        reportTypeFilter = "," + reportType.options[reportType.selectedIndex].value;
    }
    if ((regionFilter == 0 && reportTypeFilter == 0) ||
        (reportTypeFilter == 0 && regionFilter == 2) ||
        (regionFilter == 0 && "1,2,3,4,14,15,12,6,7,8,9,13,10".indexOf(reportTypeFilter) > -1) ||
        ("1,2,3,4,14,15,12,6,7,8,9,13,10".indexOf(reportTypeFilter) > -1 && regionFilter == 2)) {
        var pin7063 = new Microsoft.Maps.Pushpin(new Microsoft.Maps.Location(40.65315905, -111.9814039),
            { color: 'orange' });
        pin7063.SignalID = "7063";
        pin7063.Region = "2";
        pin7063.Actions = "1,2,3,4,14,15,12,6,7,8,9,13,10";
        Microsoft.Maps.Events.addHandler(pin7063, "mouseup", ZoomIn);
        Microsoft.Maps.Events.addHandler(pin7063, "click", displayInfobox);
        pins.push(pin7063);
    }
    //dataLayer = pins;
    return pins;
}