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
   

    if (yAxisMax !== null) {
        $("#YAxisMax").val(yAxisMax);
    }
    if (y2AxisMax !== null) {
        $("#Y2AxisMax").val(y2AxisMax);
    }
}



function SetPhaseTerminationMetric(consecutiveCount, showPlans, showPed) {
    $("#SelectedConsecutiveCount").val(consecutiveCount);
    $("#ShowPlanStripes").prop('checked',showPlans);
    $("#ShowPedActivity").prop('checked',showPed);
}

function SetSplitMonitorMetric(selectedPercentileSplit, showPlanStripes, showPedActivity, showAverageSplit, showPercentMaxOutForceOff,
    showPercentGapOuts, showPercentSkip)
{
    $("#SelectedPercentileSplit").val(selectedPercentileSplit);
    $("#ShowPlanStripes").prop('checked',showPlanStripes);
    $("#ShowPedActivity").prop(showPedActivity);
    $("#ShowAverageSplit").prop(showAverageSplit);
    $("#ShowPercentMaxOutForceOff").prop(showPercentMaxOutForceOff);
    $("#ShowPercentGapOuts").prop(showPercentGapOuts);
    $("#ShowPercentSkip").prop(showPercentSkip);

}

function SetPedDelayMetric(timeBuffer, showPedBeginWalk, showCycleLength, showPercentDelay, showPedRecall, pedRecallThreshold) {
    $("#TimeBuffer").val(timeBuffer);
    $("#ShowPedBeginWalk").prop('checked', showPedBeginWalk);
    $("#ShowCycleLength").prop('checked', showCycleLength);
    $("#ShowPercentDelay").prop('checked', showPercentDelay);
    $("#showPedRecall").prop('checked', showPedRecall);
    $("#pedRecallThreshold").val(pedRecallThreshold);
}

function SetPreemptionDetailsMetric() {

}

function SetTMCMetric(selectedBinSize, showLaneVolumes, showTotalVolumes, showDataTable) {
    $("#SelectedBinSize").val(selectedBinSize);
    $("#ShowLaneVolumes").prop('checked',showLaneVolumes);
    $("#ShowTotalVolumes").prop('checked',showTotalVolumes);
    $("#ShowDataTable").prop('checked',showDataTable);
}

function SetPCDMetric(selectedBinSize, selectedDotSize, selectedLineSize, showPlanStatistics, showVolumes) {
    $("#SelectedBinSize").val(selectedBinSize);
    $("#SelectedDotSize").val(selectedDotSize);
    $("#SelectedLineSize").val(selectedLineSize);
    $("#ShowPlanStatistics").prop('checked',showPlanStatistics);
    $("#ShowVolumes").prop('checked',showVolumes);
}

function SetApproachVolumeMetric(selectedBinSize, showDirectionalSplits, showTotalVolumes, ShowSbWbVolume, ShowNbEbVolume,
    showTMCDetection, showAdvanceDetection) {
    $("#SelectedBinSize").val(selectedBinSize);
    $("#ShowDirectionalSplits").prop('checked',showDirectionalSplits);
    $("#ShowTotalVolumes").prop('checked',showTotalVolumes);
    $("#ShowSbWbVolume").prop('checked',ShowSbWbVolume);
    $("#ShowNbEbVolume").prop('checked',ShowNbEbVolume);
    $("#ShowTMCDetection").prop('checked',showTMCDetection);
    $("#ShowAdvanceDetection").prop('checked',showAdvanceDetection);
}

function SetApproachDelayMetric(selectedBinSize, showPlanStatistics, showTotalDelayPerHour, showDelayPerVehicle) {
    $("#SelectedBinSize").val(selectedBinSize);
    $("#ShowPlanStatistics").prop('checked',showPlanStatistics);
    $("#ShowTotalDelayPerHour").prop('checked',showTotalDelayPerHour);
    $("#ShowDelayPerVehicle").prop('checked',showDelayPerVehicle);

}

function SetAoRMetric(selectedBinSize, showPlanStatistics) {
    $("#SelectedBinSize").val(selectedBinSize);
    $("#ShowPlanStatistics").prop('checked',showPlanStatistics);
}

function SetYRAMetric(severeLevelSeconds, showRedLightViolations, showSevereRedLightViolations, showPercentRedLightViolations,
    showPercentSevereRedLightViolations, showAverageTimeRedLightViolations, showYellowLightOccurrences, showPercentYellowLightOccurrences, showAverageTimeYellowOccurences)
{
    $("#SevereLevelSeconds").val(severeLevelSeconds);
    $("#ShowRedLightViolations").prop('checked',showRedLightViolations);
    $("#ShowSevereRedLightViolations").prop('checked',showSevereRedLightViolations);
    $("#ShowPercentRedLightViolations").prop('checked',showPercentRedLightViolations);
    $("#ShowPercentSevereRedLightViolations").prop('checked',showPercentSevereRedLightViolations);
    $("#ShowPercentRedLightViolations").prop('checked',showPercentRedLightViolations);
    $("#ShowPercentSevereRedLightViolations").prop('checked',showPercentSevereRedLightViolations);
    $("#ShowAverageTimeRedLightViolations").prop('checked',showAverageTimeRedLightViolations);
    $("#ShowYellowLightOccurrences").prop('checked',showYellowLightOccurrences);
    $("#ShowPercentYellowLightOccurrences").prop('checked',showPercentYellowLightOccurrences);
    $("#ShowAverageTimeYellowOccurences").prop('checked',showAverageTimeYellowOccurences);
}

function SetSpeedMetric(selectedBinSize, showPlanStatistics, showAverageSpeed, showPostedSpeed, show85Percentile, show15Percentile)
{
    $("#SelectedBinSize").val(selectedBinSize);
    $("#ShowPlanStatistics").prop('checked',showPlanStatistics);
    $("#ShowAverageSpeed").prop('checked',showAverageSpeed);
    $("#ShowPostedSpeed").prop('checked',showPostedSpeed);
    $("#Show85Percentile").prop('checked',show85Percentile);
    $("#Show15Percentile").prop('checked', show15Percentile);
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
    var metricTypeID = window.document.getElementById('MetricTypes').value;
    selectedMetricID = metricTypeID === "" ? selectedMetricID : metricTypeID;
    if (selectedMetricID === null || selectedMetricID === undefined)
    {
        selectedMetricID = metricTypeID === "" ? 1 : metricTypeID;
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
    if (selectedMetricID === null || selectedMetricID === undefined) {
        var metricsList = $("#MetricsList");
        if (metricsList !== null) {
            selectedMetricID = metricsList.val();
        }
    }
    var signalID = $("#SignalID").val();
    var tosend = {};
    tosend.signalID = signalID;
    $.get(urlpathGetSignalLocation, tosend,function (data) {
        $('#SignalLocation').text(data);
        if (data !== "Signal Not Found") {
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
    $("#RunReportSpinner").addClass("fa fa-circle-o-notch fa-spin");
}

function StopReportSpinner() {
    $("#RunReportSpinner").removeClass("fa fa-circle-o-notch fa-spin");
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