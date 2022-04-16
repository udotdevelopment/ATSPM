using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MOE.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace SPM.Models
{
    public enum BinSizeOptions
    {
        [Display(Name = "5")]
        n1 = 5,
        [Display(Name = "15")]
        n2 = 15,
    }

    public class MetricTypesDefaultValuesViewModel
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
        [Display(Name = "Selected Consecutive Count")]
        [Range(1, 5)]
        public int SelectedConsecutiveCount { get; set; }
        [Display(Name = "Show Ped Activity")]
        public bool ShowPedActivity { get; set; }
        [Display(Name = "Show Plan Stripes")]
        public bool ShowPlanStripes { get; set; }
        [Display(Name = "Y Axis Max")]
        public int? YAxisMax { get; set; }
    }

    public class AoRDefaultValuesViewModel
    {
        [Display(Name = "Selected Bin Size")]
        public BinSizeOptions SelectedBinSize { get; set; }
        [Display(Name = "Show Plan Statistics")]
        public bool ShowPlanStatistics { get; set; }
    }

    public class ApproachDelayDefaultValuesViewModel
    {
        [Display(Name = "Selected Bin Size")]
        public BinSizeOptions SelectedBinSize { get; set; }
        [Display(Name = "Show Delay Per Vehicle")]
        public bool ShowDelayPerVehicle { get; set; }
        [Display(Name = "Show Plan Statistics")]
        public bool ShowPlanStatistics { get; set; }
        [Display(Name = "Show Total Delay Per Hour")]
        public bool ShowTotalDelayPerHour { get; set; }
        [Display(Name = "Y Axis Max")]
        public int YAxisMax { get; set; }
        [Display(Name = "Y2 Axis Max")]
        public int Y2AxisMax { get; set; }

    }

    public class ApproachSpeedDefaultValuesViewModel
    {
        [Display(Name = "Bin Size")]
        public BinSizeOptions BinSize { get; set; }
        [Display(Name = "Show 15 Percentile")]
        public bool Show15Percentile { get; set; }
        [Display(Name = "Show 85 Percentile")]
        public bool Show85Percentile { get; set; }
        [Display(Name = "Show Average Speed")]
        public bool ShowAverageSpeed { get; set; }
        [Display(Name = "Show Plan Statistics")]
        public bool ShowPlanStatistics { get; set; }
        [Display(Name = "Show Posted Speed")]
        public bool ShowPostedSpeed { get; set; }
        [Display(Name = "Y Axis Max")]
        public int YAxisMax { get; set; }
        [Display(Name = "Y2 Axis Max")]
        public int YAxisMin { get; set; }
    }

    public class ApproachVolumeDefaultValuesViewModel
    {
        [Display(Name = "Selected Bin Size")]
        public BinSizeOptions SelectedBinSize { get; set; }
        [Display(Name = "Show Advance Detection")]
        public bool ShowAdvanceDetection { get; set; }
        [Display(Name = "Show Directional Splits")]
        public bool ShowDirectionalSplits { get; set; }
        [Display(Name = "Show Northbound/Eastbound Volume")]
        public bool ShowNbEbVolume { get; set; }
        [Display(Name = "Show Southbound/Westbound Volume")]
        public bool ShowSbWbVolume { get; set; }
        [Display(Name = "Show Turning Movement Counts Detection")]
        public bool ShowTMCDetection { get; set; }
        [Display(Name = "Show Total Volume")]
        public bool ShowTotalVolume { get; set; }
        [Display(Name = "Y Axis Min")]
        public int YAxisMin { get; set; }

    }

    public class LeftTurnGapAnalysisDefaulValuesViewModel
    {
        [Display(Name = "Bin Size")]
        public BinSizeOptions BinSize { get; set;}
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
        public enum LineSizeOptions
        {
            Small,
            Large,
        }

        public BinSizeOptions SelectedBinSize { get; set; }          
        public int SelectedDotSize { get; set; }
        public int SelectedLineSize { get; set; }
        public bool ShowPlanStatistics { get; set; }
        public bool ShowVolumes { get; set; }
        public int Y2AxisMax { get; set; }
        public int YAxisMax { get; set; }
    }       
    
    public class PedDelayDefaulValuesViewModel
    {
        public int YAxisMax { get; set; }
    }

    public class SplitFailDefaultValuesViewModel
    {
        public int FirstSecondsOfRed { get; set; }
        public bool ShowAvgLines { get; set; }
        public bool ShowFailLines { get; set; }
        public bool ShowPercentFailLines { get; set; }
    }

    public class SplitMonitorDefaultValuesViewModel
    {
        [Display(Name = "Y Axis Max")]
        public int YAxisMax { get; set; }
        [Display(Name = "Selected Percentile Split")]
        public int SelectedPercentileSplit { get; set; }
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
        public int AdvancedOffset { get; set; }
        public bool CombineLanesForEachGroup { get; set; }
        public int DotAndBarSize { get; set; }
        public int ExtendStartStopSearch { get; set; }
        public int ExtendVsdSearch { get; set; }
        public bool ShowAdvancedCount { get; set; }
        public bool ShowAdvancedDilemmaZone { get; set; }
        public bool ShowAllLanesInfo { get; set; }
        public bool ShowEventPairs { get; set; }
        public bool ShowHeaderForEachPhase { get; set; }
        public bool ShowLaneByLaneCount { get; set; }
        public bool ShowLegend { get; set; }
        public bool ShowLinesStartEnd { get; set; }
        public bool ShowPedestrianActuation { get; set; }
        public bool ShowPedestrianboolervals { get; set; }
        public bool ShowPermissivePhases { get; set; }
        public bool ShowRawEventData { get; set; }
        public bool ShowStopBarPresence { get; set; }
        public bool ShowVehicleSignalDisplay { get; set; }
    }

    public class TMCDefaultValuesViewModel
    {
        public BinSizeOptions SelectedBinSize { get; set; }
        public bool ShowDataTable { get; set; }
        public bool ShowLaneVolumes { get; set; }
        public bool ShowTotalVolumes { get; set; }
        public int Y2AxisMax { get; set; }
        public int YAxisMax { get; set; }
    }

    public class WaitTimeDefaultValuesViewModel
    {
        public bool ShowPlanStripes { get; set; }
    }

    public class YellowAndRedDefaultValuesViewModel
    {
        public bool ShowAverageTimeRedLightViolations { get; set; }
        public bool ShowAverageTimeYellowOccurences { get; set; }
        public bool ShowPercentRedLightViolations { get; set; }
        public bool ShowPercentSevereRedLightViolations { get; set; }
        public bool ShowPercentYellowLightOccurrences { get; set; }
        public bool ShowRedLightViolations { get; set; }
        public bool ShowSevereRedLightViolations { get; set; }
        public bool ShowYellowLightOccurrences { get; set; }
        public int YAxisMax { get; set; }
    }
}