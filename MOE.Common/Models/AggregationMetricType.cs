using System.ComponentModel.DataAnnotations;

namespace MOE.Common.Models
{
    public class AggregationMetricType
    {
        [Key]
        public int MetricID{ get; set; }

        [Required]
        public string ChartName { get; set; }

        [Required]
        public string Abbreviation { get; set; }

        [Required]
        public bool ShowOnWebsite { get; set; }
    }
}