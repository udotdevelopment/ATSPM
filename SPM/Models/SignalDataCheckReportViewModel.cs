namespace SPM.Models
{
    public class SignalDataCheckReportViewModel
    {
        public bool? LeftTurnVolumeOk { get; set; }
        public bool? GapOutOk { get; set; }
        public bool? PedCycleOk { get; set; }

        public SignalDataCheckReportViewModel(bool? leftTurnVolumesOk, bool? gapOutOk, bool? pedCyclesOk)
        {
            LeftTurnVolumeOk = leftTurnVolumesOk;
            GapOutOk = gapOutOk;
            PedCycleOk = pedCyclesOk;
        }
    }
}