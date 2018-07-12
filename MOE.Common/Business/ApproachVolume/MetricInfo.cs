using System.Runtime.Serialization;

namespace MOE.Common.Business.ApproachVolume
{
    [DataContract]
    public class MetricInfo
    {
        [DataMember]
        public string Direction2PeakHourString { get; set; }
        [DataMember]
        public double Direction2PeakHourDFactor { get; set; }
        [DataMember]
        public double Direction2PeakHourKFactor { get; set; }
        [DataMember]
        public double Direction2PeakHourFactor { get; set; }
        [DataMember]
        public int Direction2PeakHourMaxValue { get; set; }
        [DataMember]
        public int Direction2PeakHourVolume { get; set; }
        [DataMember]
        public string Direction1PeakHourString { get; set; }
        [DataMember]
        public double Direction1PeakHourDFactor { get; set; }
        [DataMember]
        public double Direction1PeakHourKFactor { get; set; }
        [DataMember]
        public double Direction1PeakHourFactor { get; set; }
        [DataMember]
        public int Direction1PeakHourMaxValue { get; set; }
        [DataMember]
        public int Direction1PeakHourVolume { get; set; }
        [DataMember]
        public int CombinedVolume { get; set; }
        [DataMember]
        public string CombinedPeakHourString { get; set; }
        [DataMember]
        public double CombinedPeakHourKFactor { get; set; }
        [DataMember]
        public double CombinedPeakHourFactor { get; set; }
        [DataMember]
        public int CombinedPeakHourValue { get; set; }
        [DataMember]
        public string ImageLocation { get; set; }
        [DataMember]
        public string Direction1 { get; set; }
        [DataMember]
        public string Direction2 { get; set; }
        [DataMember]
        public int Direction2Volume { get; set; }
        [DataMember]
        public int Direction1Volume { get; set; }
        [DataMember]    
        public int CombinedPeakHourVolume { get; set; }
    }
}