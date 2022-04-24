function removeDateType() {
    $('input[type=date]').each(function () {
        this.type = "text";
    });
}

function GetMetric(urlPath, tosend)
{
    GetChartComment(tosend.MetricTypeID, tosend.SignalID);
    dataLayer.push({
        'MetricTypeId': tosend.MetricTypeID,
        'SignalId': tosend.SignalID
    });
    $.ajax({
        url: urlPath,
        type: "POST",
        cache: false,
        async: true,
        datatype: "json",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(tosend),
        success: function(data) {

            $('#ReportPlaceHolder').html(data);
            $('#ReportPlaceHolder').focus();
            $.validator.unobtrusive.parse($("#PCDOptionsPlaceHolder"));
        },
        beforeSend: function() {
            StartReportSpinner();
        },
        complete: function() {
            StopReportSpinner();
            ShowMetricUrl();
        }, 
        error: function(xhr, status, error) {
            StopReportSpinner();
            $('#ReportPlaceHolder').html(xhr.responseText);
        }
    });
}

function GetChartComment(metricID, signalID)
{
    var tosend = {};
    tosend.MetricID = metricID;
    tosend.SignalID = signalID;

    $.ajax({
        url: urlpathGetChartComment,
        type: "POST",
        cache: false,
        async: true,
        datatype: "json",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(tosend),
        success: function (data) {

            $('#ChartCommentPlaceHolder').html(data);

        },

        error: function (xhr, status, error) {
            StopReportSpinner();
            $('#ChartCommentPlaceHolder').html(xhr.responseText);
        }
    });
}

function CancelAddApproach() {
    $("#NewApproach").html("");
}

function GetCommonValues()
{
    var tosend = {};
    tosend.SignalID = $("#SignalID").val();
    tosend.StartDate = $("#StartDateDay").val() + " " + $("#StartTime").val() + " " + $("#StartAMPMddl").val();
    tosend.EndDate = $("#EndDateDay").val() + " " + $("#EndTime").val() + " " + $("#EndAMPMddl").val();    
    tosend.YAxisMax = $("#YAxisMax").val();
    tosend.Y2AxisMax = $("#Y2AxisMax").val();
    tosend.YAxisMin = $("#YAxisMin").val();
    tosend.Y2AxisMin = $("#Y2AxisMin").val();
    return tosend;
}

function GetPhaseTerminationMetric (metricTypeID) {
    var tosend = GetCommonValues();
    tosend.MetricTypeID = metricTypeID;
    tosend.SelectedConsecutiveCount = $("#SelectedConsecutiveCount").val();
    tosend.ShowPlanStripes = $("#ShowPlanStripes").is(":checked");
    tosend.ShowPedActivity = $("#ShowPedActivity").is(":checked");
    GetMetric(urlpathPhaseTermination, tosend);
}

function GetPreemptMetric(metricTypeID) {
    var tosend = GetCommonValues();
    tosend.MetricTypeID = metricTypeID;
    GetMetric(urlpathPreempt, tosend);
}

function GetPedDelayMetric(metricTypeID) {
    var tosend = GetCommonValues();
    tosend.MetricTypeID = metricTypeID;
    GetMetric(urlpathPedDelay, tosend);
}

function GetTMCMetric(metricTypeID) {
    var tosend = GetCommonValues();
    tosend.MetricTypeID = metricTypeID;
    tosend.SelectedBinSize = $("#SelectedBinSize").val();
    tosend.ShowLaneVolumes = $("#ShowLaneVolumes").is(":checked");
    tosend.ShowTotalVolumes = $("#ShowTotalVolumes").is(":checked");
    tosend.ShowDataTable = $("#ShowDataTable").is(":checked");
    
    GetMetric(urlpathTMC, tosend);
}

function GetPCDMetric(metricTypeID) {
    var tosend = GetCommonValues();
    tosend.MetricTypeID = metricTypeID;
    tosend.SelectedBinSize = $("#SelectedBinSize").val();
    tosend.SelectedDotSize = $("#SelectedDotSize").val();
    tosend.SelectedLineSize = $("#SelectedLineSize").val();
    tosend.ShowPlanStatistics = $("#ShowPlanStatistics").is(":checked");
    tosend.ShowVolumes = $("#ShowVolumes").is(":checked");
    GetMetric(urlpathPCD, tosend);
}

function GetApproachVolumeMetric(metricTypeID) {
    var tosend = GetCommonValues();
    tosend.MetricTypeID = metricTypeID;
    tosend.SelectedBinSize = $("#SelectedBinSize").val();
    tosend.ShowTotalVolume = $("#ShowTotalVolume").is(":checked");
    tosend.ShowDirectionalSplits = $("#ShowDirectionalSplits").is(":checked");
    tosend.ShowNbEbVolume = $("#ShowNbEbVolume").is(":checked");
    tosend.ShowSbWbVolume = $("#ShowSbWbVolume").is(":checked");
    tosend.ShowTMCDetection = $("#ShowTMCDetection").is(":checked");
    tosend.ShowAdvanceDetection = $("#ShowAdvanceDetection").is(":checked");
    GetMetric(urlpathApproachVolume, tosend);
}
function GetTimingAndActuationsMetric(metricTypeID) {
    var tosend = GetCommonValues();
    tosend.MetricTypeID = metricTypeID;
    tosend.ShowLegend = $("#ShowLegend").is(":checked");
    tosend.ShowHeaderForEachPhase = $("#ShowHeaderForEachPhase").is(":checked");
    tosend.CombineLanesForEachGroup = $("#CombineLanesForEachGroup").is(":checked");
    tosend.DotAndBarSize = $("#DotAndBarSize").val();
    tosend.PhaseFilter = $("#PhaseFilter").val();
    tosend.PhaseEventCodes = $("#PhaseEventCodes").val();
    tosend.GlobalCustomEventCodes = $("#GlobalCustomEventCodes").val();
    tosend.GlobalCustomEventParams = $("#GlobalCustomEventParams").val();
    tosend.ShowAllLanesInfo = $("#ShowAllLanesInfo").is(":checked");
    tosend.ShowLinesStartEnd = $("#ShowLinesStartEnd").is(":checked");
    tosend.ShowEventPairs = $("#ShowEventPairs").is(":checked");
    tosend.ShowRawEventData = $("#ShowRawEventData").is(":checked");
    tosend.ShowPermissivePhases = $("#ShowPermissivePhases").is(":checked");
    tosend.ExtendVsdSearch = $("#ExtendVsdSearch").val();
    tosend.ShowVehicleSignalDisplay = $("#ShowVehicleSignalDisplay").is(":checked");
    tosend.ShowPedestrianIntervals = $("#ShowPedestrianIntervals").is(":checked");
    tosend.ExtendStartStopSearch = $("#ExtendStartStopSearch").val();
    tosend.ShowPedestrianActuation = $("#ShowPedestrianActuation").is(":checked");
    tosend.ShowStopBarPresence = $("#ShowStopBarPresence").is(":checked");
    tosend.ShowLaneByLaneCount = $("#ShowLaneByLaneCount").is(":checked");
    tosend.ShowAdvancedDilemmaZone = $("#ShowAdvancedDilemmaZone").is(":checked");
    tosend.ShowAdvancedCount = $("#ShowAdvancedCount").is(":checked");
    tosend.AdvancedOffset = $("#AdvancedOffset").val();
    GetMetric(urlpathTimingAndActuations, tosend);
}
function GetApproachDelayMetric(metricTypeID) {
    var tosend = GetCommonValues();
    tosend.MetricTypeID = metricTypeID;
    tosend.SelectedBinSize = $("#SelectedBinSize").val();
    tosend.ShowPlanStatistics = $("#ShowPlanStatistics").is(":checked");
    tosend.ShowTotalDelayPerHour = $("#ShowTotalDelayPerHour").is(":checked");
    tosend.ShowDelayPerVehicle = $("#ShowDelayPerVehicle").is(":checked");
    GetMetric(urlpathApproachDelay, tosend);
}
function GetAoRMetric(metricTypeID) {
    var tosend = GetCommonValues();
    tosend.MetricTypeID = metricTypeID;
    tosend.SelectedBinSize = $("#SelectedBinSize").val();
    tosend.ShowPlanStatistics = $("#ShowPlanStatistics").is(":checked");
    GetMetric(urlpathAoR, tosend);
}
function GetApproachSpeedMetric(metricTypeID) {
    var tosend = GetCommonValues();
    tosend.MetricTypeID = metricTypeID;
    tosend.SelectedBinSize = $("#SelectedBinSize").val();
    tosend.ShowPlanStatistics = $("#ShowPlanStatistics").is(":checked");
    tosend.ShowPostedSpeed = $("#ShowPostedSpeed").is(":checked");
    tosend.ShowAverageSpeed = $("#ShowAverageSpeed").is(":checked");
    tosend.Show85Percentile = $("#Show85Percentile").is(":checked");
    tosend.Show15Percentile = $("#Show15Percentile").is(":checked");
    GetMetric(urlpathApproachSpeed, tosend);
}
function GetYellowAndRedMetric(metricTypeID) {
    var tosend = GetCommonValues();
    tosend.MetricTypeID = metricTypeID;
    tosend.SeverLevel = $("#SeverLevel").val();
    tosend.ShowRedLightViolations = $("#ShowRedLightViolations").is(":checked");
    tosend.ShowSevereRedLightViolations = $("#ShowSevereRedLightViolations").is(":checked");
    tosend.ShowPercentRedLightViolations = $("#ShowPercentRedLightViolations").is(":checked");
    tosend.ShowPercentSevereRedLightViolations = $("#ShowPercentSevereRedLightViolations").is(":checked");
    tosend.ShowAverageTimeRedLightViolations = $("#ShowAverageTimeRedLightViolations").is(":checked");
    tosend.ShowYellowLightOccurrences = $("#ShowYellowLightOccurrences").is(":checked");
    tosend.ShowPercentYellowLightOccurrences = $("#ShowPercentYellowLightOccurrences").is(":checked");
    tosend.ShowAverageTimeYellowOccurences = $("#ShowAverageTimeYellowOccurences").is(":checked");
    GetMetric(urlpathYellowAndRed, tosend);
}
function GetSplitFailMetric(metricTypeID) {
    var tosend = GetCommonValues();
    tosend.MetricTypeID = metricTypeID;
    tosend.FirstSecondsOfRed = $("#FirstSecondsOfRed").val();
    tosend.ShowFailLines = $("#ShowFailLines").is(":checked");
    tosend.ShowAvgLines = $("#ShowAvgLines").is(":checked");
    tosend.ShowPercentFailLines = $("#ShowPercentFailLines").is(":checked");
    GetMetric(urlpathSplitFail, tosend);
}


function GetSplitMonitorMetric(metricTypeID) {
    var tosend = GetCommonValues();
    tosend.MetricTypeID = metricTypeID;
    tosend.SelectedPercentileSplit = $("#SelectedPercentileSplit").val();
    tosend.ShowPlanStripes = $("#ShowPlanStripes").is(":checked");
    tosend.ShowPedActivity = $("#ShowPedActivity").is(":checked");
    tosend.ShowAverageSplit = $("#ShowAverageSplit").is(":checked");
    tosend.ShowPercentMaxOutForceOff = $("#ShowPercentMaxOutForceOff").is(":checked");
    tosend.ShowPercentGapOuts = $("#ShowPercentGapOuts").is(":checked");
    tosend.ShowPercentSkip = $("#ShowPercentSkip").is(":checked");
    GetMetric(urlpathSplitMonitor, tosend);
}

function GetLeftTurnGapAnalysisMetric(metricTypeID) {
    var toSend = GetCommonValues();
    toSend.MetricTypeID = metricTypeID;
    toSend.Gap1Min = $("#Gap1Min").val();
    toSend.Gap1Max = $("#Gap1Max").val();
    toSend.Gap2Min = $("#Gap2Min").val();
    toSend.Gap2Max = $("#Gap2Max").val();
    toSend.Gap3Min = $("#Gap3Min").val();
    toSend.Gap3Max = $("#Gap3Max").val();
    toSend.Gap4Min = $("#Gap4Min").val();
    toSend.TrendLineGapThreshold = $("#TrendLineGapThreshold").val();
    toSend.BinSize = $("#BinSize").val();
    GetMetric(urlpathLeftTurnGapAnalysis, toSend);
}

function GetWaitTimeMetric(metricTypeID) {
    var toSend = GetCommonValues();
    toSend.MetricTypeID = metricTypeID;
    toSend.ShowPlanStripes = $("#ShowPlanStripes").is(":checked");
    GetMetric(urlpathWaitTime, toSend);
}

$('#CreateMetric').click(function() { CreateMetric(); });

function CreateMetric() {
    var defer = $.Deferred();
    var form = $("#MainForm")[0];
    $.validator.unobtrusive.parse(form);
    if ($(form).valid()) {
        var selectedMetricID = $("#MetricsList").val();
        if (selectedMetricID === "1") {
            GetPhaseTerminationMetric(1);
        }
        else if (selectedMetricID === "2") {
            GetSplitMonitorMetric(2);
        }
        else if (selectedMetricID === "3") {
            GetPedDelayMetric(3);
        }
        else if (selectedMetricID === "4") {
            GetPreemptMetric(4);
        }
        else if (selectedMetricID === "5") {
            GetTMCMetric(5);
        }
        else if (selectedMetricID === "6") {
            GetPCDMetric(6);
        }
        else if (selectedMetricID === "7") {
            GetApproachVolumeMetric(7);
        }
        else if (selectedMetricID === "8") {
            GetApproachDelayMetric(8);
        }
        else if (selectedMetricID === "9") {
            GetAoRMetric(9);
        }
        else if (selectedMetricID === "10") {
            GetApproachSpeedMetric(10);
        }
        else if (selectedMetricID === "11") {
            GetYellowAndRedMetric(11);
        }
        else if (selectedMetricID === "12") {
            GetSplitFailMetric(12);
        }
        else if (selectedMetricID === "31") {
            GetLeftTurnGapAnalysisMetric(31);
        }
        else if (selectedMetricID === "32") {
            GetWaitTimeMetric(32);
        }
        else if (selectedMetricID === "17") {
            GetTimingAndActuationsMetric(17);
        }
    }
    defer.resolve();
    return defer.promise();
}



