using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace MOE.Common.Models
{
    public partial class MetricComment
    {
        [NotMapped]
        public List<MetricType> AllMetricTypes { get; set; }

        [NotMapped]
        public int[] MetricIDs { get; set; }
    }
}