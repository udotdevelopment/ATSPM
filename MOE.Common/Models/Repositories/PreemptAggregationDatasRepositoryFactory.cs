using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOE.Common.Models.Repositories
{
    public class PreemptAggregationDatasRepositoryFactory
    {
        private static IPreemptAggregationDatasRepository _preemptAggregationDatasRepository;

        public static IPreemptAggregationDatasRepository Create()
        {
            if (_preemptAggregationDatasRepository != null)
            {
                return _preemptAggregationDatasRepository;
            }
            return new PreemptAggregationDatasRepository();
        }

        public static void SetArchivedMetricsRepository(IPreemptAggregationDatasRepository newRepository)
        {
            _preemptAggregationDatasRepository = newRepository;
        }
    }
}
