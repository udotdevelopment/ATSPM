using ATSPM.Application.Models;
using System.Collections.Generic;

namespace ATSPM.IRepositories
{
    public interface IMetricTypeRepository
    {
        List<MetricType> GetAllMetrics();

        List<MetricType> GetAllToDisplayMetrics();

        List<MetricType> GetAllToAggregateMetrics();
        List<MetricType> GetBasicMetrics();

        List<MetricType> GetMetricsByIDs(List<int> metricIDs);

        MetricType GetMetricsByID(int metricID);

        List<MetricType> GetMetricTypesByMetricComment(MetricComment metricComment);
    }
}