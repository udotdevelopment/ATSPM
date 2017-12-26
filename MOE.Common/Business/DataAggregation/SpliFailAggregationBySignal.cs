using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MOE.Common.Business.SplitFail;
using MOE.Common.Business.WCFServiceLibrary;
using MOE.Common.Models;

namespace MOE.Common.Business.DataAggregation
{
    public class SpliFailAggregationBySignal
    {
        public List<ApproachSplitFailAggregationContainer> Containers { get;}
        public SpliFailAggregationBySignal(AggregationMetricOptions options)
        {
            var signalRepository = MOE.Common.Models.Repositories.SignalsRepositoryFactory.Create();
            Models.Signal signal = signalRepository.GetVersionOfSignalByDate(options.SignalID, options.StartDate);
            Containers = new List<ApproachSplitFailAggregationContainer>();
            foreach (var approach in signal.Approaches)
            {
                Containers.Add(new ApproachSplitFailAggregationContainer(approach, options.StartDate, options.EndDate));
            }

        }
    }


    public class ApproachSplitFailAggregationContainer
    {
        public Approach Approach { get; set; }
        public List<ApproachSplitFailAggregation> SplitFails { get; }

        public ApproachSplitFailAggregationContainer(Approach approach, DateTime start, DateTime end)
        {
            Approach = approach;
            var splitFailAggregationRepository =
                MOE.Common.Models.Repositories.ApproachSplitFailAggregationRepositoryFactory.Create();
            SplitFails = splitFailAggregationRepository.GetApproachSplitFailAggregationByVersionIdAndDateRange(
                approach.ApproachID, start, end);
        }
    }
}