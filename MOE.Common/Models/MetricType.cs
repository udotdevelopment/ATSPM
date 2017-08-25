using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace MOE.Common.Models
{
    public class MetricType
    {
        [Key]
        public int MetricID { get; set; }

        [Required]
        public string ChartName { get; set; }

        [Required]
        public string Abbreviation { get; set; }

        [Required]
        public bool ShowOnWebsite { get; set; }

        //[Required]
        //[Column("DetectionType_DetectionTypeID")]
        //public int DetectionTypeID { get; set; }


        public virtual ICollection<DetectionType> DetectionTypes { get; set; }
        public virtual ICollection<MetricComment> Comments { get; set; }
        public virtual ICollection<ActionLog> ActionLogs { get; set; }
    }
}
