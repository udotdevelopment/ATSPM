using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOE.Common.Models.Repositories
{
    public class MetricTypeRepositoryFactory
    {
        private static IMetricTypeRepository metricTypeRepository;

        public static IMetricTypeRepository Create()
        {
            if (metricTypeRepository != null)
            {
                return metricTypeRepository;
            }
            return new MetricTypeRepository();
        }

        public static void SetMetricsRepository(IMetricTypeRepository newRepository)
        {
            metricTypeRepository = newRepository;
        }
    }
}
