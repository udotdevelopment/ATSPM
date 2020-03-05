using System.Collections.Generic;
using AtspmApi.Models;

namespace AtspmApi.Repositories
{
    public interface IMetricTypeRepository
    {
        List<MetricType> GetAllMetrics();

        List<MetricType> GetAllToDisplayMetrics();

        List<MetricType> GetAllToAggregateMetrics();
        List<MetricType> GetBasicMetrics();

        List<MetricType> GetMetricsByIDs(List<int> metricIDs);

        MetricType GetMetricsByID(int metricID);

        //List<MetricType> GetMetricTypesByMetricComment(MetricComment metricComment);
    }
}