using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MOE.Common.Business.WCFServiceLibrary;
using MOE.Common.Models;

namespace MOE.Common.Business.DataAggregation
{
    class SplitFailAggregationByRoute
    {
        public SplitFailAggregationByRoute(ApproachAggregationMetricOptions options)
        {
            Container = new List<SplitFailAggregationBySignal>();

            //foreach (var sig in options.Signals)
            //{
            //    options.SignalID = sig.SignalID;
            //    var signalRepository = MOE.Common.Models.Repositories.SignalsRepositoryFactory.Create();
            //    Models.Signal signal = signalRepository.GetVersionOfSignalByDate(sig.SignalID, options.StartDate);

            //    SpliFailAggregationBySignal signalAggregation = new SpliFailAggregationBySignal(options);

            //    Container.Add(signalAggregation);
            //}

        }
        
        public List<SplitFailAggregationBySignal> Container { get; }


    }


    public class RouteSplitFailAggregationContainer
    {
        public Route Route { get; set; }
        public List<ApproachSplitFailAggregation> SplitFails { get; }

        public RouteSplitFailAggregationContainer(ApproachAggregationMetricOptions options)
        {
            //Approach = approach;
            //var splitFailAggregationRepository =
            //    MOE.Common.Models.Repositories.ApproachSplitFailAggregationRepositoryFactory.Create();
            //SplitFails = splitFailAggregationRepository.GetApproachSplitFailAggregationByApproachIdAndDateRange(
            //    approach.ApproachID, start, end);
        }
    }
}
