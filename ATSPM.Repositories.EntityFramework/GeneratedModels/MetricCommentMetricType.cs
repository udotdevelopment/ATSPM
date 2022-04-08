using System;
using System.Collections.Generic;

#nullable disable

namespace ATSPM.Infrastructure.Repositories.EntityFramework.Repositories
{
    public partial class MetricCommentMetricType
    {
        public int MetricCommentCommentId { get; set; }
        public int MetricTypeMetricId { get; set; }

        public virtual MetricComment MetricCommentComment { get; set; }
        public virtual MetricType MetricTypeMetric { get; set; }
    }
}
