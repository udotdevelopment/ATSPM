using System.Collections.Generic;
using System.Linq;

namespace MOE.Common.Models.Repositories
{
    public class MetricFilterTypesRepository : IMetricFilterTypesRepository
    {
        private readonly SPM db = new SPM();

        public List<MetricsFilterType> GetAllFilters()
        {
            var results = (from r in db.MetricsFilterTypes
                select r).ToList();
            return results;
        }
    }
}