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
    LoadFromUrl();
});

function SetCommonValues(signalId, startDateDay, startTime, startAmPmDdl, endDateDay, endTime, endAmPmDdl, yAxisMax, y2AxisMax) {
    $("#SignalID").val(signalId);
    $("#StartDateDay").val(startDateDay);
    $("#StartTime").val(startTime);
    $("#StartAMPMddl").val(startAmPmDdl);
    $("#EndDateDay").val(endDateDay);
    $("#EndTime").val(endTime);
    $("#EndAMPMddl").val(endAmPmDdl);
    if (yAxisMax != null) {
        $("#YAxisMax").val(yAxisMax);
    }
    if (y2AxisMax != null) {
        $("#Y2AxisMax").val(y2AxisMax);
    }
}



function SetPhaseTerminationMetric(consecutiveCount, showPlans, showPed) {
    $("#SelectedConsecutiveCount").val(consecutiveCount);
    $("#ShowPlanStripes").val(showPlans);
    $("#ShowPedActivity").val(showPed);
}

function SetSplitMonitorMetric(selectedPercentileSplit, showPlanStripes, showPedActivity, showAverageSplit, showPercentMaxOutForceOff,
    showPercentGapOuts, showPercentSkip)
{
    $("#SelectedPercentileSplit").val(selectedPercentileSplit);
    $("#ShowPlanStripes").val(showPlanStripes);
    $("#ShowPedActivity").val(showPedActivity);
    $("#ShowAverageSplit").val(showAverageSplit);
    $("#ShowPercentMaxOutForceOff").val(showPercentMaxOutForceOff);
    $("#ShowPercentGapOuts").val(showPercentGapOuts);
    $("#ShowPercentSkip").val(showPercentSkip);

}

function SetPedDelayMetric() {

}

function SetPreemptionDetailsMetric() {

}

function SetTMCMetric(selectedBinSize, showLaneVolumes, showTotalVolumes, showDataTable) {
    $("#SelectedBinSize").val(selectedBinSize);
    $("#ShowLaneVolumes").val(showLaneVolumes);
    $("#ShowTotalVolumes").val(showTotalVolumes);
    $("#ShowDataTable").val(showDataTable);
}

function SetPCDMetric(selectedBinSize, selectedDotSize, showPlanStatistics, showVolumes) {
    $("#SelectedBinSize").val(selectedBinSize);
    $("#SelectedDotSize").val(selectedDotSize);
    $("#ShowPlanStatistics").val(showPlanStatistics);
    $("#ShowVolumes").val(showVolumes);
}

function SetApproachVolumeMetric(selectedBinSize, showDirectionalSplits, showTotalVolumes, showSBEBVolume, showNBWBVolume,
    showTMCDetection, showAdvanceDetection) {
    $("#SelectedBinSize").val(selectedBinSize);
    $("#ShowDirectionalSplits").val(showDirectionalSplits);
    $("#ShowTotalVolumes").val(showTotalVolumes);
    $("#ShowSBEBVolume").val(showSBEBVolume);
    $("#ShowNBWBVolume").val(showNBWBVolume);
    $("#ShowTMCDetection").val(showTMCDetection);
    $("#ShowAdvanceDetection").val(showAdvanceDetection);
}

function SetApproachDelayMetric(selectedBinSize, showPlanStatistics, showTotalDelayPerHour, showDelayPerVehicle) {
    $("#SelectedBinSize").val(selectedBinSize);
    $("#ShowPlanStatistics").val(showPlanStatistics);
    $("#ShowTotalDelayPerHour").val(showTotalDelayPerHour);
    $("#ShowDelayPerVehicle").val(showDelayPerVehicle);

}

function SetAORMetric(selectedBinSize, showPlanStatistics) {
    $("#SelectedBinSize").val(selectedBinSize);
    $("#ShowPlanStatistics").val(showPlanStatistics);
}

function SetYRAMetric(severeLevelSeconds, showRedLightViolations, showSevereRedLightViolations, showPercentRedLightViolations,
    showPercentSevereRedLightViolations, showAverageTimeRedLightViolations, showYellowLightOccurrences, showPercentYellowLightOccurrences, showAverageTimeYellowOccurences)
{
    $("#SevereLevelSeconds").val(severeLevelSeconds);
    $("#ShowRedLightViolations").val(showRedLightViolations);
    $("#ShowSevereRedLightViolations").val(showSevereRedLightViolations);
    $("#ShowPercentRedLightViolations").val(showPercentRedLightViolations);
    $("#ShowPercentSevereRedLightViolations").val(showPercentSevereRedLightViolations);
    $("#ShowPercentRedLightViolations").val(showPercentRedLightViolations);
    $("#ShowPercentSevereRedLightViolations").val(showPercentSevereRedLightViolations);
    $("#ShowAverageTimeRedLightViolations").val(showAverageTimeRedLightViolations);
    $("#ShowYellowLightOccurrences").val(showYellowLightOccurrences);
    $("#ShowPercentYellowLightOccurrences").val(showPercentYellowLightOccurrences);
    $("#ShowAverageTimeYellowOccurences").val(showAverageTimeYellowOccurences);
}

function SetSpeedMetric(selectedBinSize, showPlanStatistics, showAverageSpeed, showPostedSpeed, show85Percentile)
{
    $("#SelectedBinSize").val(selectedBinSize);
    $("#ShowPlanStatistics").val(showPlanStatistics);
    $("#ShowAverageSpeed").val(showAverageSpeed);
    $("#ShowPostedSpeed").val(showPostedSpeed);
    $("#Show85Percentile").val(show85Percentile);
}

function SetSplitFailMetric(firstSecondsOfRed, showFailLines, showAverageLines, showPercentLines) {
    $("#FirstSecondsOfRed").val(firstSecondsOfRed);
    $("#ShowFailLines").prop('checked', showFailLines);
    $("#ShowAvgLines").prop('checked', showAverageLines);
    $("#ShowPercentFailLines").prop('checked', showPercentLines);
}


function SetBaseOptions() {
    $("#SignalID").val(7062);
    $("#StartDateDay").val('10/17/2017');
    $("#EndDateDay").val('10/17/2017');
}

function SetControlValues(signalID, selectedMetricID) {
    $("#SignalID").val(signalID);    
    GetSignalLocation(selectedMetricID);
}

function GetMetricsList(signalID, selectedMetricID)
{
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



function GetSignalLocation(selectedMetricID)
{
    if (selectedMetricID == null) {
        var metricsList = $("#MetricsList");
        if (metricsList != null) {
            selectedMetricID = metricsList.val();
        }
    }
    var signalID = $("#SignalID").val();
    var tosend = {};
    tosend.signalID = signalID;
    $.get(urlpathGetSignalLocation, tosend,function (data) {
        $('#SignalLocation').text(data);
        if (data != "Signal Not Found") {
            GetMetricsList(signalID, selectedMetricID);
        }
    });    
}

function ResetDates()
{
    var d = new Date();
    var month = d.getMonth()+1;
    var day = d.getDate();

    var output =month  + '/' +
         + day + '/' +
         + d.getFullYear();
    $("#StartDateDay").val(output);
    $("#EndDateDay").val(output);
    $("#StartTime").val("12:00");
    $("#EndTime").val("11:59");
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
    
    var metricPath = urlOptions + "/" + selectedID;
    $.ajax({
        url: metricPath,
        type: "POST",
        cache: false,
        async: false,
        datatype: "json",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            $('#Options').html(data);
            $.validator.unobtrusive.parse($("#Options"));
        },
        onerror: function () { alert("Error"); }
    });
    //$.get(metricPath, function (data) {
    //$('#Options').html(data);
    //$.validator.unobtrusive.parse($("#Options"));
    //});
}