using System;
using System.Collections.Generic;

#nullable disable

namespace ATSPM.Infrastructure.Repositories.EntityFramework.Repositories
{
    public partial class MetricComment
    {
        public MetricComment()
        {
            MetricCommentMetricTypes = new HashSet<MetricCommentMetricType>();
        }

        public int CommentId { get; set; }
        public string SignalId { get; set; }
        public DateTime TimeStamp { get; set; }
        public string CommentText { get; set; }
        public int VersionId { get; set; }

        public virtual ICollection<MetricCommentMetricType> MetricCommentMetricTypes { get; set; }
    }
}
