using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOE.Common.Models.Repositories
{
    public class PriorityAggregationDatasRepositoryFactory
    {
        private static IPriorityAggregationDatasRepository _priorityAggregationDatasRepository;

        public static IPriorityAggregationDatasRepository Create()
        {
            if (_priorityAggregationDatasRepository != null)
            {
                return _priorityAggregationDatasRepository;
            }
            return new PriorityAggregationDatasRepository();
        }

        public static void SetArchivedMetricsRepository(IPriorityAggregationDatasRepository newRepository)
        {
            _priorityAggregationDatasRepository = newRepository;
        }
    }
}
