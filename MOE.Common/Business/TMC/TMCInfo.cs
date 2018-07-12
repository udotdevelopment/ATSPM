using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MOE.Common.Business.TMC
{
    public class TMCInfo
    {
        [DataMember] public List<string> ImageLocations;

        [DataMember] public List<TMCData> tmcData;

        public TMCInfo()
        {
            ImageLocations = new List<string>();
            tmcData = new List<TMCData>();
        }
    }
}