using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ATSPM.Models
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