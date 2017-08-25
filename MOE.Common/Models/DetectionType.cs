using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace MOE.Common.Models
{
    public class DetectionType
    {

        public DetectionType()
        {
            Detectors = new HashSet<Detector>();
            MetricTypes = new HashSet<MetricType>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int DetectionTypeID { get; set; }
        [Required]
        public string Description { get; set; }

        public virtual ICollection<Detector> Detectors { get; set; }
        public virtual ICollection<MetricType> MetricTypes { get; set; }

        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<GraphDetectorDetection> GraphDetectors { get; set; }

    }
}
