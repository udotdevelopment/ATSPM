using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ATSPM.Models
{
    [Table("DetectionTypeDetectors")]
    public class DetectionTypeGraph_Detector
    {
        [Column(Order = 0)]
        [Key]
        public int DetectionType_DetectionTypeID { get; set; }

        [Column(Order = 1)]
        [Key]
        public string Detectors_DetectorID { get; set; }
    }
}