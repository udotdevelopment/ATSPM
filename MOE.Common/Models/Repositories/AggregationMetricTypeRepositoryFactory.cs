using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOE.Common.Models.Repositories
{
    public class AggregationMetricTypeRepositoryFactory
    {
        private static IAggregationMetricTypeRepository aggregationMetricTypeRepository;

        public static IAggregationMetricTypeRepository Create()
        {
            if (aggregationMetricTypeRepository != null)
            {
                return aggregationMetricTypeRepository;
            }
            return new AggregationMetricTypeRepository();
        }

        public static void SetMetricsRepository(IAggregationMetricTypeRepository newRepository)
        {
            aggregationMetricTypeRepository = newRepository;
        }
    }
}
