namespace MOE.Common.Business
{
    public class SPMChartOptions
    {
        public bool ShowTotalVolume { get; set; }
        public bool ShowSBEBVolume { get; set; }
        public bool ShowNBWBVolume { get; set; }
        public bool ShowDirectionalSplits { get; set; }
        public int VolumeBinSize { get; set; }
        public double yAxisMaximum { get; set; }
        public bool ShowTMCDetection { get; set; }
        public bool ShowAdvanceDetection { get; set; }
    }
}