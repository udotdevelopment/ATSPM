using System;
using System.Collections.Generic;
using System.Linq;
using MOE.Common.Business.Bins;
using MOE.Common.Business.WCFServiceLibrary;
using MOE.Common.Models;

namespace MOE.Common.Business.DataAggregation
{
    public class PreemptAggregationBySignal
    {
        public Models.Signal Signal { get; }
        public List<SignalPreemptAggregationContainer> PreemptionTotals { get; }
        public int TotalPreemptsServiced { get { return PreemptionTotals.Sum(c => c.BinsContainer.SumValue); } }
        public int AveragePreemptsServiced { get { return Convert.ToInt32(Math.Round(PreemptionTotals.Average(c => c.BinsContainer.SumValue))); } }

        public void GetSplitFailuresByBin(BinsContainer binsContainer)
        {



            foreach (var bin in binsContainer.Bins)
            {
                int summedSplitFailures = 0;
                foreach (var preempt in PreemptionTotals)
                {
                    summedSplitFailures += preempt.BinsContainer.Bins.Where(a => a.Start == bin.Start)
                        .Sum(a => a.Value);
                }
                bin.Value = summedSplitFailures;
            }

        }


        public double Order { get; set; }

        public PreemptAggregationBySignal(AggregationMetricOptions options, Models.Signal signal, BinsContainer binsContainer)
        {
            Signal = signal;
            PreemptionTotals = new List<SignalPreemptAggregationContainer>();

                PreemptionTotals.Add(
                    new SignalPreemptAggregationContainer(Signal, binsContainer));
            

        }



    }

    public class SignalPreemptAggregationContainer
    {
        public Models.Signal Signal { get; }
        public BinsContainer BinsContainer { get; set; } = new BinsContainer();

        public SignalPreemptAggregationContainer(Models.Signal signal, BinsContainer binsContainer)//, AggregationMetricOptions.XAxisTimeTypes aggregationType)
        {
            Signal = signal;
            var preemptAggregationRepository =
                MOE.Common.Models.Repositories.PreemptAggregationDatasRepositoryFactory.Create();


            foreach (var bin in binsContainer.Bins)
            {

                var servicedTotal = preemptAggregationRepository
                    .GetPreemptAggregationTotalByVersionIdAndDateRange(
                        signal.VersionID, bin.Start, bin.End);
                BinsContainer.Bins.Add(new Bin { Start = bin.Start, End = bin.End, Value = servicedTotal });

            }
        }




    }
}