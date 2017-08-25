using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace MOE.Common.Models
{
    public class DetectorMetrics
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DetectorMetricsID { get; set; }
        [Required]
        public virtual DetectionType DetectionType { get; set; }
        [Required]
        public virtual MetricType MetricType { get; set; }

    }
}
