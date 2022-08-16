using System;
using System.Collections.Generic;

#nullable disable

namespace ATSPM.Application.Models
{
    public partial class MetricType
    {
        public MetricType()
        {
            ActionLogMetricTypes = new HashSet<ActionLogMetricType>();
            DetectionTypeMetricTypes = new HashSet<DetectionTypeMetricType>();
            MetricCommentMetricTypes = new HashSet<MetricCommentMetricType>();
        }

        public int MetricId { get; set; }
        public string ChartName { get; set; }
        public string Abbreviation { get; set; }
        public bool ShowOnWebsite { get; set; }
        public bool ShowOnAggregationSite { get; set; }
        public int DisplayOrder { get; set; }

        public virtual ICollection<ActionLogMetricType> ActionLogMetricTypes { get; set; }
        public virtual ICollection<DetectionTypeMetricType> DetectionTypeMetricTypes { get; set; }
        public virtual ICollection<MetricCommentMetricType> MetricCommentMetricTypes { get; set; }
    }
}
