using System.Collections.Generic;
using System.Linq;

namespace MOE.Common.Models.Repositories
{
    public interface IMetricTypesDefaultValuesRepository
    {
        List<MetricTypesDefaultValues> GetAll();
        IQueryable<string> GetListOfCharts();
        List<MetricTypesDefaultValues> GetChartDefaults(string chart);
        Dictionary<string, string> GetAllAsDictionary();
        Dictionary<string, string> GetChartDefaultsAsDictionary(string chart);
        void Update(MetricTypesDefaultValues option);
    }
}
