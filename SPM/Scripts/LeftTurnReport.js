$(function (ready) {
    $(".datepicker").attr('type', 'text');
    $("#StartDate").val($.datepicker.formatDate('mm/dd/yy', new Date()));
    $("#EndDate").val($.datepicker.formatDate('mm/dd/yy', new Date()));
    $("#StartEndDaySelector").datepicker({
        onSelect: function (dateText) {
            $("#StartDate").val(dateText);
            $("#EndDate").val(dateText);
        }
    });
    $(".datepicker").datepicker();
});

$('#RunChecks').click(function () { RunChecks(); });

function RunChecks() {
    var tosend = {};
    tosend.parameters = null;
    $.get(urlpathGetSignalDataCheckReport,
        tosend,
        function(data) {
            $('#SignalDataCheckPlaceHolder').html(data);
        });
}

$('#RunReports').click(function () { RunReports(); });

function RunReports() {
    var tosend = {};
    tosend.parameters = null;
    $.get(urlpathGetFinalGapAnalysisReport, tosend, function (data) {
        $('#FinalGapAnalysisPlaceHolder').html(data);
    });
}

function customTimeClick() {
    var radiobuttons = document.getElementsByName('TimeOptions'); //$('#TimeOptions');
    for (var i = 0, length = radiobuttons.length; i < length; i++) {
        if (radiobuttons[i].value == 'customTimeRadiobutton') {
            if (radiobuttons[i].checked) {
                $('#customTimeDiv').removeClass('d-none');
            } else {
                $('#customTimeDiv').addClass('d-none');
            }
        }
    }

}

function signalDataCheckClick() {
    if ($("#signalDataCheck").is(":checked")) {
        $('#CyclesWithPedCallsDiv').removeClass('d-none');
        $('#CyclesWithGapOutsDiv').removeClass('d-none');
        $('#LeftTurnVolumeDiv').removeClass('d-none');
    } else {
        $('#CyclesWithPedCallsDiv').addClass('d-none');
        $('#CyclesWithGapOutsDiv').addClass('d-none');
        $('#LeftTurnVolumeDiv').addClass('d-none');
    }
}

function finalGapAnalysisCheckClick() {
    if ($("#finalGapAnalysisCheck").is(":checked")) {
        $('#AcceptableGapsDiv').removeClass('d-none');
    } else {
        $('#AcceptableGapsDiv').addClass('d-none');
    }

}

function splitFailAnalysisCheckClick() {
    if ($("#splitFailAnalysisCheck").is(":checked")) {
        $('#CyclesWithSplitFailDiv').removeClass('d-none');
    } else {
        $('#CyclesWithSplitFailDiv').addClass('d-none');
    }

}

function pedestrianCallAnalysisCheckClick() {
    if ($("#pedestrianCallAnalysisCheck").is(":checked")) {
        $('#LtCyclesWithPedCallsDiv').removeClass('d-none');
    } else {
        $('#LtCyclesWithPedCallsDiv').addClass('d-none');
    }

}

function GetSignalLocation() {
    //if (selectedMetricID === null || selectedMetricID === undefined) {
    //    var metricsList = $("#MetricsList");
    //    if (metricsList !== null) {
    //        selectedMetricID = metricsList.val();
    //    }
    //}

    var tosend = {};
    var signalID = $("#SignalID").val();
    tosend.signalID = signalID;
    $.get(urlpathGetLeftTurnCheckBoxes, tosend, function (data) {
        $('#LeftTurnCheckBoxesDiv').html(data);
    });
    $.get(urlpathGetSignalLocation, tosend, function (data) {
        $('#SignalLocation').text(data);
        if (data !== "Signal Not Found") {
            GetMetricsList(signalID, selectedMetricID);
        }
    });
}
$("#ResetDate").click(function () { ResetDates(); });
function ResetDates() {
    var d = new Date();
    var month = d.getMonth() + 1;
    var day = d.getDate();

    var output = month + '/' +
        + day + '/' +
        + d.getFullYear();
    $("#StartDate").val(output);
    $("#EndDate").val(output);
}


