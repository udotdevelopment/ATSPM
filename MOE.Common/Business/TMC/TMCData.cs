using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

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
