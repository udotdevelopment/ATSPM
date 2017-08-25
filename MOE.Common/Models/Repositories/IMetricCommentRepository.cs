using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOE.Common.Models.Repositories
{
    public interface IMetricCommentRepository
    {
        List<Models.MetricComment> GetAllMetricComments();
        Models.MetricComment GetMetricCommentByMetricCommentID(int metricCommentID);
        void AddOrUpdate(MOE.Common.Models.MetricComment metricComment);
        void Add(MOE.Common.Models.MetricComment metricComment);
        void Remove(MOE.Common.Models.MetricComment metricComment);
        MetricComment GetLatestCommentForReport(string signalID, int metricID);
        List<Models.MetricType> GetMetricTypesByMetricComment(Models.MetricComment metricComment);

    }
}
