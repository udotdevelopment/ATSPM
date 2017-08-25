using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOE.Common.Models.Repositories
{
    public class MetricFilterTypesRepository : IMetricFilterTypesRepository
    {
        Models.SPM db = new SPM();

        public List<Models.MetricsFilterType> GetAllFilters()
        {
            List<Models.MetricsFilterType> results = (from r in db.MetricsFilterTypes
                                           select r).ToList();
            return results;
        }
    }
}
