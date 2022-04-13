using System.Collections.Generic;

namespace MOE.Common.Models.Repositories
{
    public interface IMetricTypesDefaultValuesRepository
    {
        List<MetricTypesDefaultValues> GetChartDefaults(string chart);
    }
}
