using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace MOE.Common.Models
{
    public partial class ActionLog
    {
        [NotMapped]
        public int[] CheckBoxListReturnActions { get; set; }

        [NotMapped]
        public List<Action> CheckBoxListAllActions { get; set; }

        [NotMapped]
        public int[] CheckBoxListReturnMetricTypes { get; set; }

        [NotMapped]
        public List<MetricType> CheckBoxListAllMetricTypes { get; set; }
    }
}