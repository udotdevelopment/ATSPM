using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MOE.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace SPM.Models
{
    public class MeasuresDefaultsViewModel
    {
        public PhaseTerminationDefaultValuesViewModel PhaseTermination { get; set; }
        public AoRDefaultValuesViewModel AoR { get; set; }
        public ApproachDelayDefaultValuesViewModel ApproachDelay { get; set; }
        public ApproachSpeedDefaultValuesViewModel ApproachSpeed { get; set; }
        public ApproachVolumeDefaultValuesViewModel ApproachVolume { get; set; }
        public LeftTurnGapAnalysisDefaulValuesViewModel LeftTurnGapAnalysis { get; set; }
        public PCDefaulValuesViewModel PCD { get; set; }
        public PedDelayDefaulValuesViewModel PedDelay { get; set; }
        public SplitMonitorDefaultValuesViewModel SplitMonitor { get; set; }
        public SplitFailDefaultValuesViewModel SplitFail { get; set; }
        public TimingAndActuationsDefaulValuesViewModel TimingAndActuations { get; set; }
        public TMCDefaultValuesViewModel TMC { get; set; }
        public WaitTimeDefaultValuesViewModel WaitTime { get; set; }
        public YellowAndRedDefaultValuesViewModel YellowAndRed { get; set; }
    }
    public class PhaseTerminationDefaultValuesViewModel
    {
        [Display(Name = "Consecutive Count")]
        public int SelectedConsecutiveCount { get; set; }
        [Display(Name = "Show Ped Activity")]
        public bool ShowPedActivity { get; set; }
        [Display(Name = "Show Plans")]
        public bool ShowPlanStripes { get; set; }
        [Display(Name = "Y-axis Max")]
        public double? YAxisMax { get; set; }
    }

    public class AoRDefaultValuesViewModel
    {
        [Display(Name = "Volume Bin Size")]
        public int SelectedBinSize { get; set; }
        [Display(Name = "Y-axis Max")]
        public double? YAxisMax { get; set; }
        [Display(Name = "Show Plans")]
        public bool ShowPlanStatistics { get; set; }
    }

    public class ApproachDelayDefaultValuesViewModel
    {
        [Display(Name = "Volume Bin Size")]
        public int SelectedBinSize { get; set; }
        [Display(Name = "Show Delay Per Vehicle")]
        public bool ShowDelayPerVehicle { get; set; }
        [Display(Name = "Show Plans")]
        public bool ShowPlanStatistics { get; set; }
        [Display(Name = "Show Total Delay Per Hour")]
        public bool ShowTotalDelayPerHour { get; set; }
        [Display(Name = "Y-axis Max")]
        public double YAxisMax { get; set; }
        [Display(Name = "Secondary Y-Axis Max")]
        public double Y2AxisMax { get; set; }

    }

    public class ApproachSpeedDefaultValuesViewModel
    {
        [Display(Name = "Volume Bin Size")]
        public int SelectedBinSize { get; set; }
        [Display(Name = "Show 15% Speeds")]
        public bool Show15Percentile { get; set; }
        [Display(Name = "Show 85% Speeds")]
        public bool Show85Percentile { get; set; }
        [Display(Name = "Show Average Speed")]
        public bool ShowAverageSpeed { get; set; }
        [Display(Name = "Show Plans")]
        public bool ShowPlanStatistics { get; set; }
        [Display(Name = "Show Posted Speed")]
        public bool ShowPostedSpeed { get; set; }
        [Display(Name = "Y-axis Max")]
        public double YAxisMax { get; set; }
        [Display(Name = "Y-axis Min")]
        public double YAxisMin { get; set; }
    }

    public class ApproachVolumeDefaultValuesViewModel
    {
        [Display(Name = "Selected Bin Size")]
        public int SelectedBinSize { get; set; }
        [Display(Name = "Show Advance Detection")]
        public bool ShowAdvanceDetection { get; set; }
        [Display(Name = "Show Directional Splits")]
        public bool ShowDirectionalSplits { get; set; }
        [Display(Name = "Show NB/EB Volume")]
        public bool ShowNbEbVolume { get; set; }
        [Display(Name = "Show SB/WB Volume")]
        public bool ShowSbWbVolume { get; set; }
        [Display(Name = "Show TMC Detection")]
        public bool ShowTMCDetection { get; set; }
        [Display(Name = "Show Total Volume")]
        public bool ShowTotalVolume { get; set; }
        [Display(Name = "Y-axis Min")]
        public double YAxisMin { get; set; }
        [Display(Name = "Y-axis Max")]
        public double? YAxisMax { get; set; }

    }

    public class LeftTurnGapAnalysisDefaulValuesViewModel
    {
        [Display(Name = "Bin Size")]
        public int BinSize { get; set;}
        [Display(Name = "Gap 1 Max")]
        public double Gap1Max { get; set;}
        [Display(Name = "Gap 1 Min")]
        public double Gap1Min { get; set;}
        [Display(Name = "Gap 2 Max")]
        public double Gap2Max { get; set;}
        [Display(Name = "Gap 2 Min")]
        public double Gap2Min { get; set;}
        [Display(Name = "Gap 3 Max")]
        public double Gap3Max { get; set;}
        [Display(Name = "Gap 3 Min")]
        public double Gap3Min { get; set;}
        [Display(Name = "Gap 4 Min")]
        public double Gap4Min { get; set;}
        [Display(Name = "Trending Line Gap Threshhold")]
        public double TrendLineGapThreshold { get; set; }
    }

    public class PCDefaulValuesViewModel
    { 
        [Display(Name = "Volume Bin Size")]
        public int SelectedBinSize { get; set; }
        [Display(Name = "Dot Size")]
        public int SelectedDotSize { get; set; }
        [Display(Name = "Line Size")]
        public int SelectedLineSize { get; set; }
        [Display(Name = "Show Plans")]
        public bool ShowPlanStatistics { get; set; }
        [Display(Name = "Show Volumes")]
        public bool ShowVolumes { get; set; }
        [Display(Name = "Secondary Y-axis Max")]
        public double Y2AxisMax { get; set; }
        [Display(Name = "Y-axis Max")]
        public double YAxisMax { get; set; }
    }       
    
    public class PedDelayDefaulValuesViewModel
    {
        [Display(Name = "Y-axis Max")]
        public double? YAxisMax { get; set; }
    }

    public class SplitFailDefaultValuesViewModel
    {
        [Display(Name = "First Seconds Of Red")]
        public int FirstSecondsOfRed { get; set; }
        [Display(Name = "Show Average Lines")]
        public bool ShowAvgLines { get; set; }
        [Display(Name = " Show Fail Lines")]
        public bool ShowFailLines { get; set; }
        [Display(Name = "Show Percent Fail Lines")]
        public bool ShowPercentFailLines { get; set; }
    }

    public class SplitMonitorDefaultValuesViewModel
    {
        [Display(Name = "Y Axis Max")]
        public double? YAxisMax { get; set; }
        [Display(Name = "Selected Percentile Split")]
        public int? SelectedPercentileSplit { get; set; }
        [Display(Name = "Show Average Split")]
        public bool ShowAverageSplit { get; set; }
        [Display(Name = "Show Ped Activity")]
        public bool ShowPedActivity { get; set; }
        [Display(Name = "Show Percent of Gap Outs")]
        public bool ShowPercentGapOuts { get; set; }
        [Display(Name = "Show Percent of Max Out Force Offs")]
        public bool ShowPercentMaxOutForceOff { get; set; }
        [Display(Name = "Show Percent Skip")]
        public bool ShowPercentSkip { get; set; }
        [Display(Name = "Show Plan Stripes")]
        public bool ShowPlanStripes { get; set; }
    }

    public class TimingAndActuationsDefaulValuesViewModel
    {
        [Display(Name = " Advanced Count Offset")]
        public int AdvancedOffset { get; set; }
        [Display(Name = "Combine Lanes for Phase")]
        public bool CombineLanesForEachGroup { get; set; }
        [Display(Name = "Dot and Marker Size")]
        public int DotAndBarSize { get; set; }
        [Display(Name = "Extend Start/Stop Search Minutes.decimal")]
        public int ExtendStartStopSearch { get; set; }
        [Display(Name = "Extend Search (left) Minutes.decimal")]
        public int ExtendVsdSearch { get; set; }
        [Display(Name = "Advanced Count")]
        public bool ShowAdvancedCount { get; set; }
        [Display(Name = "Advanced Presence")]
        public bool ShowAdvancedDilemmaZone { get; set; }
        [Display(Name = "All Lanes For Each Phase")]
        public bool ShowAllLanesInfo { get; set; }
        [Display(Name = "Event Start Stop Pairs")]
        public bool ShowEventPairs { get; set; }
        [Display(Name = "Header For Each Phase")]
        public bool ShowHeaderForEachPhase { get; set; }
        [Display(Name = "Lane-by-lane Count")]
        public bool ShowLaneByLaneCount { get; set; }
        [Display(Name = "Legend")]
        public bool ShowLegend { get; set; }
        [Display(Name = "On/Off Lines")]
        public bool ShowLinesStartEnd { get; set; }
        [Display(Name = "Pedestrian Actuations")]
        public bool ShowPedestrianActuation { get; set; }
        [Display(Name = "Pedestrian Intervals")]
        public bool ShowPedestrianIntervals { get; set; }
        [Display(Name = "Show Permissive Phases")]
        public bool ShowPermissivePhases { get; set; }
        [Display(Name = "Raw Data Display")]
        public bool ShowRawEventData { get; set; }
        [Display(Name = "Stop Bar Presence")]
        public bool ShowStopBarPresence { get; set; }
        [Display(Name = " Vehicle Signal Display")]
        public bool ShowVehicleSignalDisplay { get; set; }
    }

    public class TMCDefaultValuesViewModel
    {
        [Display(Name = "Volume Bin Size")]
        public int SelectedBinSize { get; set; }
        [Display(Name = "Show Data Table")]
        public bool ShowDataTable { get; set; }
        [Display(Name = " Show Movement Type Volume")]
        public bool ShowLaneVolumes { get; set; }
        [Display(Name = " Show Total Volume")]
        public bool ShowTotalVolumes { get; set; }
        [Display(Name = "Turn Movement Y-axis Max")]
        public double Y2AxisMax { get; set; }
        [Display(Name = "Thru Movement Y-axis Max")]
        public double YAxisMax { get; set; }
    }

    public class WaitTimeDefaultValuesViewModel
    {
        [Display(Name = "Show Plan")]
        public bool ShowPlanStripes { get; set; }
    }

    public class YellowAndRedDefaultValuesViewModel
    {
        [Display(Name = "Average Time Red Light Violations")]
        public bool ShowAverageTimeRedLightViolations { get; set; }
        [Display(Name = "Severe Red Light Violations")]
        public double SevereLevelSeconds { get; set; }
        [Display(Name = "Average Time Yellow Occurences")]
        public bool ShowAverageTimeYellowOccurences { get; set; }
        [Display(Name = "Percent Red Light Violations")]
        public bool ShowPercentRedLightViolations { get; set; }
        [Display(Name = "Percent Severe Red Light Violations")]
        public bool ShowPercentSevereRedLightViolations { get; set; }
        [Display(Name = "Percent Yellow Light Occurrences")]
        public bool ShowPercentYellowLightOccurrences { get; set; }
        [Display(Name = "Red Light Violations")]
        public bool ShowRedLightViolations { get; set; }
        [Display(Name = "Severe Red Light Violations")]
        public bool ShowSevereRedLightViolations { get; set; }
        [Display(Name = "Yellow Light Occurrences")]
        public bool ShowYellowLightOccurrences { get; set; }
        [Display(Name = "Y-axis Max")]
        public double YAxisMax { get; set; }
    }
}