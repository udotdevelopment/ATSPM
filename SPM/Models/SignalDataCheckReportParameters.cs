namespace SPM.Models
{
    public class SignalDataCheckReportParameters
    {
        public bool CheckLeftTurnVolume { get; private set; }
        public bool CheckGapOut { get; private set; }
        public bool CheckPedCycle { get; private set; }
    }
}