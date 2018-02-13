using System.Runtime.Serialization;

namespace MOE.Common.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [DataContract]
    public partial class MetricComment:Comment
    {
        public MetricComment()
        {
          
        }


        [Required]
        [DataMember]
        public int VersionID { get; set; }
        [DataMember]
        public virtual Models.Signal Signal { get; set; }

        [DataMember]
        public string SignalID { get; set; }


        [DataMember]
        public List<int> MetricTypeIDs { get; set; }
        [DataMember]
        public virtual ICollection<MetricType> MetricTypes { get; set; }

    }
}
