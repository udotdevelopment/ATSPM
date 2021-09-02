namespace SPM.Models
{
    public class FinalGapAnalysisReportViewModel
    {
        public bool? GapDurationConsiderForStudy { get; set; }
        public bool? SplitFailsConsiderForStudy { get; set; }
        public bool? PedActuationsConsiderForStudy { get; set; }
        public bool? VolumesConsiderForStudy { get; set; }

        public FinalGapAnalysisReportViewModel(bool? gapDurationConsiderForStudy, 
            bool? splitFailsConsiderForStudy, bool? pedActuationsConsiderForStudy,
            bool? volumesConsiderForStudy)
        {
            GapDurationConsiderForStudy = gapDurationConsiderForStudy;
            SplitFailsConsiderForStudy = splitFailsConsiderForStudy;
            PedActuationsConsiderForStudy = pedActuationsConsiderForStudy;
            VolumesConsiderForStudy = volumesConsiderForStudy;
        }
    }
}