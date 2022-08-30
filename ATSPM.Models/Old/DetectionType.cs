using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace ATSPM.Models
{
    [DataContract]
    public class DetectionType
    {
        public DetectionType()
        {
            Detectors = new HashSet<Detector>();
            MetricTypes = new HashSet<MetricType>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [DataMember]
        public int DetectionTypeID { get; set; }

        [Required]
        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public virtual ICollection<Detector> Detectors { get; set; }

        [DataMember]
        public virtual ICollection<MetricType> MetricTypes { get; set; }

        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<GraphDetectorDetection> GraphDetectors { get; set; }
    }
}