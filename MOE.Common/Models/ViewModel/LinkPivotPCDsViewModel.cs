namespace MOE.Common.Models.ViewModel
{
    public class LinkPivotPCDsViewModel
    {
        public int ExistingAog { get; set; }
        public double ExistingAogPercent { get; set; }
        public string ExistingChart { get; set; }
        public string ExistingDownChart { get; set; }
        public int PredictedAog { get; set; }
        public double PredictedAogPercent { get; set; }
        public string PredictedChart { get; set; }
        public string PredictedDownChart { get; set; }
        public string DownstreamBeforeTitle { get; set; }
        public string UpstreamBeforeTitle { get; set; }
        public string DownstreamAfterTitle { get; set; }
        public string UpstreamAfterTitle { get; set; }
    }
}