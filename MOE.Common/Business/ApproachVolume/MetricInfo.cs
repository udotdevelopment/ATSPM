using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MOE.Common.Business.ApproachVolume
{
    [DataContract]
    public class MetricInfo
    {
        [DataMember]
        public string ImageLocation { get; set; }
        [DataMember]
        public string TotalVolume { get; set; }
        [DataMember]
        public string PeakHour { get; set; }
        [DataMember]
        public string PeakHourVolume { get; set; }
        [DataMember]
        public string PHF { get; set; }
        [DataMember]
        public string PeakHourKFactor { get; set; }
        [DataMember]
        public string D1TotalVolume { get; set; }
        [DataMember]
        public string D1PeakHour { get; set; }
        [DataMember]
        public string D1PHF { get; set; }
        [DataMember]
        public string D1PeakHourVolume { get; set; }
        [DataMember]
        public string D1PeakHourKValue { get; set; }
        [DataMember]
        public string D1PeakHourDValue { get; set; }
        [DataMember]
        public string D2TotalVolume { get; set; }
        [DataMember]
        public string D2PeakHour { get; set; }
        [DataMember]
        public string D2PHF { get; set; }
        [DataMember]
        public string D2PeakHourVolume { get; set; }
        [DataMember]
        public string D2PeakHourKValue { get; set; }
        [DataMember]
        public string D2PeakHourDValue { get; set; }
        [DataMember]
        public string Direction1 { get; set; }
        [DataMember]
        public string Direction2 { get; set; }



    }
}
