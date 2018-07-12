using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MOE.Common.Business.FilterExtensions
{
    [DataContract]
    public class FilterApproach
    {
        [DataMember]
        public int ApproachId { get; set; }

        public string Description { get; set; }

        [DataMember]
        public bool Exclude { get; set; }

        [DataMember]
        public List<FilterDetector> FilterDetectors { get; set; } = new List<FilterDetector>();
    }
}