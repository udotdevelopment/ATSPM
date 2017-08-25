using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace MOE.Common.Business.TMC
{
    public class TMCInfo
    {
        [DataMember]
        public List<string> ImageLocations;
        [DataMember]
        public List<TMCData> tmcData;

        public TMCInfo()
        {
        ImageLocations = new List<string>();
        tmcData = new List<TMCData>();
        }

    }
}
