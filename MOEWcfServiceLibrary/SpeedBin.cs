using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace MOEWcfServiceLibrary
{
    [DataContract]
    public class SpeedBin
    {
        [DataMember]
        public DateTime StartDate;
        [DataMember]
        public DateTime EndDate;
        [DataMember]
        public string Direction1;
        [DataMember]
        public int? Direction1AverageSpeed;
        [DataMember]
        public string Direction2;
        [DataMember]
        public int? Direction2AverageSpeed;
        [DataMember]
        public string Direction3;
        [DataMember]
        public int? Direction3AverageSpeed;
        [DataMember]
        public string Direction4;
        [DataMember]
        public int? Direction4AverageSpeed;

    }
}
