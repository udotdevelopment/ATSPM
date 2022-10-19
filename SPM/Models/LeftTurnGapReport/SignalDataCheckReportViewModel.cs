namespace SPM.Models
{
    public class SignalDataCheckReportViewModel
    {
        public bool LeftTurnVolumeOk { get; set; }
        public bool GapOutOk { get; set; }
        public bool PedCycleOk { get; set; }   
        public string ApproachDescriptions { get; set; }
        public bool InsufficientDetectorEventCount { get; set; } = true;
        public bool InsufficientCycleAggregation { get; set; } = true;
        public bool InsufficientPhaseTermination { get; set; } = true;
        public bool InsufficientPedAggregations { get; set; } = true;
        public bool InsufficientSplitFailAggregations { get; set; } = true;
        public bool InsufficientLeftTurnGapAggregations { get; set; } = true;
        public double VolumeThreshold { get; set; }
        public double GapOutThreshold { get; set; }
        public double PedThreshold { get; set; }

        public SignalDataCheckReportViewModel(bool leftTurnVolumeOk, bool gapOutOk, bool pedCycleOk, string approachDescriptions, bool insufficientDetectorEventCount,
            bool insufficientCycleAggregation, bool insufficientPhaseTermination, bool insufficientPedAggregations)
        {
            LeftTurnVolumeOk = leftTurnVolumeOk;
            GapOutOk = gapOutOk;
            PedCycleOk = pedCycleOk;
            ApproachDescriptions = approachDescriptions;
            InsufficientDetectorEventCount = insufficientDetectorEventCount;
            InsufficientCycleAggregation = insufficientCycleAggregation;
            InsufficientPhaseTermination = insufficientPhaseTermination;
            InsufficientPedAggregations = insufficientPedAggregations;
        }

        public SignalDataCheckReportViewModel()
        {
        }
    }
}