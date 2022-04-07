using System;
using System.Collections.Generic;

namespace SPM.Models
{

    public class FinalGapAnalysisReportViewModel
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string ApproachDescription { get; set; }
        public string SignalId { get; set; }
        public string Location { get; set; }
        public bool Get24HourPeriod { get; set; }
        public string PhaseType { get; set; }
        public string SignalType { get; set; }
        public string OpposingApproach { get; set; }
        //public int NumberOfThruLanes { get; set; }
        public int? SpeedLimit { get; set; }
        public string PeakPeriodDescription { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public int CyclesWithSplitFailNum { get; set; }
        public double CyclesWithSplitFailPercent { get; set; }
        public int CyclesWithPedCallNum { get; set; }
        public double CyclesWithPedCallPercent { get; set; }
        public double CrossProductValue { get; set; }
        public double CalculatedVolumeBoundary { get; set; }
        public bool? GapDurationConsiderForStudy { get; set; }
        public bool? SplitFailsConsiderForStudy { get; set; }
        public bool? PedActuationsConsiderForStudy { get; set; }
        public bool? VolumesConsiderForStudy { get; set; }
        public double Capacity { get; set; }
        public double Demand { get; set; }
        public double GapOutPercent { get; internal set; }
        public int OpposingLanes { get; internal set; }
        public bool CrossProductReview { get; internal set; }
        public bool DecisionBoundariesReview { get; internal set; }
        public double LeftTurnVolume { get; internal set; }
        public double OpposingThroughVolume { get; internal set; }
        public bool? CrossProductConsiderForStudy { get; internal set; }
        public Dictionary<DateTime, double> AcceptableGapList { get; internal set; }
        public Dictionary<DateTime, double> PercentCyclesWithPedsList { get; internal set; }
        public Dictionary<DateTime, double> DemandList { get; internal set; }
        public Dictionary<DateTime, double> PercentCyclesWithSplitFailList { get; internal set; }
        public String GapDemandChartImg { get; set; }
        public String PedSplitFailChartImg { get; set; }
        public string Direction { get; internal set; }
        public string OpposingDirection { get; internal set; }

        public FinalGapAnalysisReportViewModel(bool? gapDurationConsiderForStudy, 
            bool? splitFailsConsiderForStudy, bool? pedActuationsConsiderForStudy,
            bool? volumesConsiderForStudy)
        {
            GapDurationConsiderForStudy = gapDurationConsiderForStudy;
            SplitFailsConsiderForStudy = splitFailsConsiderForStudy;
            PedActuationsConsiderForStudy = pedActuationsConsiderForStudy;
            VolumesConsiderForStudy = volumesConsiderForStudy;
        }

        public FinalGapAnalysisReportViewModel()
        {
        }
    }
}