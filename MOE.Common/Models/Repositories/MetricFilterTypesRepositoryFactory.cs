using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOE.Common.Models.Repositories
{
    public class MetricFilterTypesRepositoryFactory
    {
        private static IMetricFilterTypesRepository metricFilterTypeRepository;

        public static IMetricFilterTypesRepository Create()
        {
            if (metricFilterTypeRepository != null)
            {
                return metricFilterTypeRepository;
            }
            return new MetricFilterTypesRepository();
        }

        public static void SetArchivedMetricsRepository(IMetricFilterTypesRepository newRepository)
        {
            metricFilterTypeRepository = newRepository;
        }
    }
}
