namespace MOE.Common.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class MetricComment:Comment
    {
        public MetricComment()
        {
          
        }


        [Required]
       public int VersionID { get; set; }
        public virtual Models.Signal Signal { get; set; }

        public string SignalID { get; set; }


        public List<int> MetricTypeIDs { get; set; }
        public virtual ICollection<MetricType> MetricTypes { get; set; }

    }
}
