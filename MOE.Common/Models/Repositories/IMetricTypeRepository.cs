using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOE.Common.Models.Repositories
{
    public interface IMetricTypeRepository
    {
        List<Models.MetricType> GetAllMetrics();

        List<MetricType> GetAllToDisplayMetrics();

        List<MetricType> GetBasicMetrics();

        List<MetricType> GetMetricsByIDs(List<int> metricIDs);

        MetricType GetMetricsByID(int metricID);

        List<Models.MetricType> GetMetricTypesByMetricComment(Models.MetricComment metricComment);

    }
}
