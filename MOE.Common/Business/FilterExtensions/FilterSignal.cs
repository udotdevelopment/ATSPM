using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel.Security.Tokens;
using System.Text;
using System.Threading.Tasks;

namespace MOE.Common.Business.FilterExtensions
{
    [DataContract]
    public class FilterSignal
    {
        [DataMember]
        public string SignalId { get; set; }
        [DataMember]
        public int VersionId { get; set; }
        public string Description { get; set; }
        [DataMember]
        public bool Exclude { get; set; } = false;
        [DataMember]
        public List<FilterApproach> FilterApproaches { get; set; } = new List<FilterApproach>();

    }
}
