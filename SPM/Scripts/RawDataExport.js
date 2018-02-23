$(function (ready) {
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
});

function GetRecordCount() {
    var toSend = GetValuesToSend();
    $.ajax({
        type: "POST",
        contentType:"application/json",
        cache: false,
        async: true,
        data: JSON.stringify(toSend),
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
    var DateTimePickerViewModel = {};
    DateTimePickerViewModel.StartDateDay = $('#DateTimePickerViewModel_StartDateDay').val();
    DateTimePickerViewModel.EndDateDay = $('#DateTimePickerViewModel_EndDateDay').val();
    DateTimePickerViewModel.StartTime = $('#DateTimePickerViewModel_StartTime').val();
    DateTimePickerViewModel.EndTime = $('#DateTimePickerViewModel_EndTime').val();
    DateTimePickerViewModel.StartTime = $('#DateTimePickerViewModel_StartTime').val();
    DateTimePickerViewModel.SelectedStartAMPM = $('#DateTimePickerViewModel_SelectedStartAMPM').val();
    DateTimePickerViewModel.SelectedEndAMPM = $('#DateTimePickerViewModel_SelectedEndAMPM').val();
    var toSend = {};
    toSend.DateTimePickerViewModel = DateTimePickerViewModel;
    toSend.SignalId = $('#SignalID').val();
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
