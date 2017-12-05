using System.Collections.Generic;

namespace MOE.Common.Models.Repositories
{
    public interface IAggregationMetricTypeRepository
    {
        List<Models.AggregationMetricType> GetAllMetrics();

        List<AggregationMetricType> GetAllToDisplayMetrics();
        List<AggregationMetricType> GetMetricsByIDs(List<int> metricIDs);
        AggregationMetricType GetMetricsByID(int metricID);


    }
}