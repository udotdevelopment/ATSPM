$(function (ready) {
    SetDateTextBoxes();
    LoadDataAggregateTypes();
});

function SetDateTextBoxes (){
    $(".datepicker").attr('type', 'text');
    $("#StartDateDay").val($.datepicker.formatDate('mm/dd/yy', new Date()));
    $("#EndDateDay").val($.datepicker.formatDate('mm/dd/yy', new Date()));
    $("#StartEndDaySelector").datepicker({
        onSelect: function (dateText) {
            $("#StartDateDay").val(dateText);
            $("#EndDateDay").val(dateText);
        }
    });
    $(".datepicker").datepicker();
}

function ResetDates() {
    var d = new Date();
    var month = d.getMonth() + 1;
    var day = d.getDate();

    var output = month + '/' +
        + day + '/' +
        + d.getFullYear();
    $("#StartDateDay").val(output);
    $("#EndDateDay").val(output);
    //$("#StartTime").val("12:00");
    //$("#EndTime").val("11:59");
    //$("#StartAMPMddl").val("AM");
    //$("#EndAMPMddl").val("PM");
    $("#StartEndDaySelector").datepicker("setDate", d);
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

function ExportChart() {
    var formData = $("#form0").serialize();
    $.ajax({
        url: urlpathExportData,
        type: "POST",
        cache: false,
        dataType: 'json',
        data: formData,
        async: true,
        success: function (data) {
        },
        onerror: function () { alert("Error"); }
    });
}

function GetRequestVerificationTokenObject() {
    var headers = {};
    var token = $('[name=__RequestVerificationToken]').val();
    headers['__RequestVerificationToken'] = token;
    return headers;
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
    var signalsContainer = $('#RouteSignals');
    var index = signalsContainer[0].children.length;
    var parameters = {};
    parameters.signalId = signalId;
    parameters.index = index;
    $.ajax({
        url: urlpathGetSignal,
        type: "GET",
        cache: false,
        async: true,
        data: parameters,
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            if (index > 0) {
                var newData = data.replaceAll("FilterSignals_0", "FilterSignals_" + index);
                var newData2 = newData.replaceAll("FilterSignals[0]", "FilterSignals[" + index + "]");
                $('#RouteSignals').append(newData2);
            }
            else {
                $('#RouteSignals').append(data);
            }
            $.validator.unobtrusive.parse($("#RouteSignals"));
        },
        onerror: function() {
             alert("Error");
        }
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
    $('#SelectedRouteId').val('');
    $('#RouteSignals').html('');
}


function CloseSignalList() {
    $("#RouteSignals").removeClass("in");
}

function StartReportSpinner() {
    $("#RunReportSpinner").addClass("fa fa-circle-o-notch fa-spin");
}

function StopReportSpinner() {
    $("#RunReportSpinner").removeClass("fa fa-circle-o-notch fa-spin");
    CloseSignalList();
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