using ATSPM.Application.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ATSPM.Models
{
    public partial class ActionLog
    {
        [NotMapped]
        public int[] CheckBoxListReturnActions { get; set; }

        [NotMapped]
        public List<Application.Models.Action> CheckBoxListAllActions { get; set; }

        [NotMapped]
        public int[] CheckBoxListReturnMetricTypes { get; set; }

        [NotMapped]
        public List<MetricType> CheckBoxListAllMetricTypes { get; set; }
    }
}