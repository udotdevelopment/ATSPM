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

        public void GetPreemptsByBin(List<BinsContainer> binsContainers)
        {
            var container = binsContainers.FirstOrDefault();
            if (container != null)
            {
                foreach (var bin in container.Bins)
                {
                    int summedServicedPreempts = 0;
                    foreach (var preempt in PreemptionTotals)
                    {
                        summedServicedPreempts += preempt.BinsContainer.Bins.Where(a => a.Start == bin.Start)
                            .Sum(a => a.Sum);
                    }
                    bin.Sum = summedServicedPreempts;
                }
            }

        }

     


        public double Order { get; set; }

        public PreemptAggregationBySignal(AggregationMetricOptions options, Models.Signal signal, List<BinsContainer> binsContainers)
        {
            Signal = signal;
            PreemptionTotals = new List<SignalPreemptAggregationContainer>();

                PreemptionTotals.Add(
                    new SignalPreemptAggregationContainer(Signal, binsContainers));
            

        }

        public void GetPreemptTotalsBySignalByPreemptNumber(List<BinsContainer> binsContainers, int preemptNumber)
        {

            PreemptionTotals.Clear();

            PreemptionTotals.Add(
                new SignalPreemptAggregationContainer(Signal,  binsContainers, preemptNumber));


        }



    }

    public class SignalPreemptAggregationContainer
    {
        public Models.Signal Signal { get; }
        public BinsContainer BinsContainer { get; set; }

        public SignalPreemptAggregationContainer(Models.Signal signal, List<BinsContainer> binsContainers) //, AggregationMetricOptions.XAxisTimeTypes aggregationType)
        {
            Signal = signal;
            var preemptAggregationRepository =
                MOE.Common.Models.Repositories.PreemptAggregationDatasRepositoryFactory.Create();
            var container = binsContainers.FirstOrDefault();
            if (container != null)
            {
                foreach (var bin in container.Bins)
                {
                    var servicedTotal = preemptAggregationRepository
                        .GetPreemptAggregationTotalByVersionIdAndDateRange(
                            Signal.VersionID, bin.Start, bin.End);
                    BinsContainer.Bins.Add(new Bin {Start = bin.Start, End = bin.End, Sum = servicedTotal});
                }
            }
        }

        public SignalPreemptAggregationContainer(Models.Signal signal, List<BinsContainer> binsContainers, int preemptNumber)
        {
            Signal = signal;
            var preemptAggregationRepository = Models.Repositories.PreemptAggregationDatasRepositoryFactory.Create();
            var container = binsContainers.FirstOrDefault();
            if (container != null)
            {
                foreach (var bin in container.Bins)
                {
                    var servicedTotal = preemptAggregationRepository
                        .GetPreemptAggregationTotalByVersionIdPreemptNumberAndDateRange(
                            Signal.VersionID, bin.Start, bin.End, preemptNumber);

                    if (servicedTotal != null)
                    {
                        BinsContainer.Bins.Add(new Bin {Start = bin.Start, End = bin.End, Sum = servicedTotal});
                    }
                    else
                    {
                        BinsContainer.Bins.Add(new Bin {Start = bin.Start, End = bin.End, Sum = 0});
                    }
                }
            }
        }




    }
}