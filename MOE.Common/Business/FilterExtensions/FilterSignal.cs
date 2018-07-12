using System.Collections.Generic;
using System.Runtime.Serialization;

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
        public bool Exclude { get; set; }

        [DataMember]
        public List<FilterApproach> FilterApproaches { get; set; } = new List<FilterApproach>();
    }
}