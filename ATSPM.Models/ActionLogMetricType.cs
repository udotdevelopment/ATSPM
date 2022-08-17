using System;
using System.Collections.Generic;

#nullable disable

namespace ATSPM.Application.Models
{
    public partial class ActionLogMetricType
    {
        public int ActionLogActionLogId { get; set; }
        public int MetricTypeMetricId { get; set; }

        public virtual ActionLog ActionLogActionLog { get; set; }
        public virtual MetricType MetricTypeMetric { get; set; }
    }
}
