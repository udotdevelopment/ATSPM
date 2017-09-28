using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOE.Common.Models.Repositories
{
    public class ApproachSpeedAggregationRepositoryFactory
    {
        private static IApproachSpeedAggregationRepository _approachSpeedAggregationRepository;

        public static IApproachSpeedAggregationRepository Create()
        {
            if (_approachSpeedAggregationRepository != null)
            {
                return _approachSpeedAggregationRepository;
            }
            return new ApproachSpeedAggregationRepository();
        }

        public static void SetApplicationEventRepository(IApproachSpeedAggregationRepository newRepository)
        {
            _approachSpeedAggregationRepository = newRepository;
        }
    }
}