
function GetRecordCount() {
    var toSend = GetValuesToSend();
    $.ajax({
        type: "Get",
        cache: false,
        async: true,
        data: toSend,
        url: urlpathGetRecordCount,
        success: function (data) { $('#GenerateData').html(data); },
        statusCode: {
            404: function (content) { alert('cannot find resource'); },
            500: function (content) { alert(content.responseText); }
        },
        beforeSend: function () {
            StartReportSpinner();
        },
        complete: function () {
            StopReportSpinner();
        },
        error: function (req, status, errorObj) {
            alert("Error");
            StopReportSpinner();
        }
    });
}

function GetValuesToSend() {
    var toSend = {};
    toSend.SignalId = $('#SignalID').val();
    toSend.StartDate = $('#StartDate').val();
    toSend.EndDate = $('#EndDate').val();
    toSend.EventParams = $('#EventParams').val();
    toSend.EventCodes = $('#EventCodes').val();
    return toSend;
}

function SetControlValues(signalID, selectedMetricID) {
    $("#SignalID").val(signalID);
    GetSignalLocation(selectedMetricID);
}

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
    });
}

function StartReportSpinner() {
    $("#RunReportSpinner").addClass("glyphicon-refresh spinning");
}

function StopReportSpinner() {
    $("#RunReportSpinner").removeClass("glyphicon-refresh spinning");
}
