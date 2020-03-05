using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace MOE.Common.Models
{
    [DataContract]
    public partial class MetricComment : Comment
    {
        [Required]
        [DataMember]
        public int VersionID { get; set; }

        [DataMember]
        public virtual Signal Signal { get; set; }

        [DataMember]
        [StringLength(10)]
        public string SignalID { get; set; }


        [DataMember]
        public List<int> MetricTypeIDs { get; set; }

        [DataMember]
        public virtual ICollection<MetricType> MetricTypes { get; set; }
    }
}