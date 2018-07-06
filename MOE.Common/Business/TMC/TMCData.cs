using System;
using System.Runtime.Serialization;

namespace MOE.Common.Business.TMC
{
    [DataContract]
    public class TMCData
    {
        [DataMember]
        public string Direction { get; set; }

        [DataMember]
        public string MovementType { get; set; }

        [DataMember]
        public string LaneType { get; set; }

        [DataMember]
        public DateTime Timestamp { get; set; }

        [DataMember]
        public int Count { get; set; }
    }
}