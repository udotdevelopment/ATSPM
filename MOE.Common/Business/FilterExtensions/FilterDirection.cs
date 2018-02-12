using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

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
