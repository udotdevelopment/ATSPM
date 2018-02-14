$(function (ready) {
    SetDateTextBoxes();
    LoadDataAggregateTypes();
});

function SetDateTextBoxes (){
    $(".datepicker").attr('type', 'text');
    $("#StartDateDay").val($.datepicker.formatDate('mm/dd/yy', new Date("2/1/2018")));
    $("#EndDateDay").val($.datepicker.formatDate('mm/dd/yy', new Date("2/2/2018")));
    $("#StartEndDaySelector").datepicker({
        onSelect: function (dateText) {
            $("#StartDateDay").val(dateText);
            $("#EndDateDay").val(dateText);
        }
    });
    $(".datepicker").datepicker();
}

function LoadRoute() {
    var RouteId = $("#SelectedRouteId").val();
    $.ajax({
        url: urlpathGetRouteSignals + "/" + RouteId,
        type: "GET",
        cache: false,
        async: true,
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            $('#RouteSignals').html(data);
            $.validator.unobtrusive.parse($("#RouteSignals"));
        },
        onerror: function () { alert("Error"); }
    });
}

function LoadDataAggregateTypes() {
    var metricId = $("#SelectedMetricTypeId").val();
    $.ajax({
        url: urlpathGetAggregateDataTypesSignals + "/" + metricId,
        type: "GET",
        cache: false,
        async: true,
        contentType: "application/json; charset=utf-8",
        success: function(data) {
            $('#AggregatedDataType').html(data);
            $.validator.unobtrusive.parse($("#AggregatedDataType"));
        },
        onerror: function() { alert("Error"); }
    });
}

function LoadSignal() {
    var signalId = $("#SignalID").val();
    $.ajax({
        url: urlpathGetSignal + "/" + signalId,
        type: "GET",
        cache: false,
        async: true,
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            $('#RouteSignals').append(data);
            $.validator.unobtrusive.parse($("#RouteSignals"));
        },
        onerror: function () { alert("Error"); }
    });
}




function SetControlValues(signalID, selectedMetricID) {
    $("#SignalID").val(signalID);
    GetSignalLocation(selectedMetricID);
}

function GetMetricsList(signalID, selectedMetricID) {
    if (selectedMetricID == null) {
        selectedMetricID = 1;
    }
    var tosend = {};
    tosend.signalID = signalID;
    tosend.selectedMetricID = selectedMetricID;

    GetOptionsByID(selectedMetricID);
    $.ajax({
        url: urlpathGetMetricsList,
        type: "POST",
        cache: false,
        async: false,
        datatype: "json",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(tosend),
        success: function (data) {
            $('#MetricsListContainer').html(data);
            $('#MetricsList').focus();
            $('#MetricsList').removeAttr('multiple');
        },
        onerror: function () { alert("Error"); }
    });
}

$("#ResetDate").click(function () { ResetDates(); });



function GetSignalLocation(selectedMetricID) {
    if (selectedMetricID == null) {
        var metricsList = $("#MetricsList");
        if (metricsList != null) {
            selectedMetricID = metricsList.val();
        }
    }
    var signalID = $("#SignalID").val();
    var tosend = {};
    tosend.signalID = signalID;
    $.get(urlpathGetSignalLocation, tosend, function (data) {
        $('#SignalLocation').text(data);
        if (data != "Signal Not Found") {
            LoadSignal();
        }
    });
}

function ClearSignals() {
    $('#RouteSignals').html('');
}

function ResetDates() {
    var d = new Date();
    var month = d.getMonth() + 1;
    var day = d.getDate();
    var endDay = d.getDate().addDays(1);

    var output = month + '/' +
        + day + '/' +
        + d.getFullYear();
    var endOutput = month + '/' +
        + endDay + '/' +
        + d.getFullYear();

    $("#StartDateDay").val(output);
    $("#EndDateDay").val(endOutput);
    $("#StartAMPMddl").val("AM");
    $("#EndAMPMddl").val("PM");
    $("#StartEndDaySelector").datepicker("setDate", d);
}

function StartReportSpinner() {
    $("#RunReportSpinner").addClass("glyphicon-refresh spinning");
}

function StopReportSpinner() {
    $("#RunReportSpinner").removeClass("glyphicon-refresh spinning");
}


function GetOptions() {
    var selectedID = $("#MetricsList").val();
    GetOptionsByID(selectedID);
}


function GetOptionsByID(selectedID) {

    //var metricPath = urlOptions + "/" + selectedID;
    //$.ajax({
    //    url: metricPath,
    //    type: "POST",
    //    cache: false,
    //    async: false,
    //    datatype: "json",
    //    contentType: "application/json; charset=utf-8",
    //    success: function (data) {
    //        $('#Options').html(data);
    //        $.validator.unobtrusive.parse($("#Options"));
    //    },
    //    onerror: function () { alert("Error"); }
    //});
    //$.get(metricPath, function (data) {
    //$('#Options').html(data);
    //$.validator.unobtrusive.parse($("#Options"));
    //});
}