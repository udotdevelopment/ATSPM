using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOE.Common.Models.Repositories
{
    public class SignalAggregationDataRepositoryRepositoryFactory
    {
        private static ISignalAggregationDataRepository signalAggregationDataRepository;

        public static ISignalAggregationDataRepository Create()
        {
            if (signalAggregationDataRepository != null)
            {
                return signalAggregationDataRepository;
            }
            return new SignalAggregationDataRepository();
        }

        public static void SetDetectorCommentRepository(ISignalAggregationDataRepository newRepository)
        {
            signalAggregationDataRepository = newRepository;
        }
    }
}
