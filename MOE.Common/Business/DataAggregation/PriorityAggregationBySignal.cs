using System;
using System.Collections.Generic;
using System.Linq;
using MOE.Common.Business.Bins;
using MOE.Common.Business.WCFServiceLibrary;

namespace MOE.Common.Business.DataAggregation
{
    public class PriorityAggregationBySignal
    {
        public Models.Signal Signal { get; }
        public List<SignalPriorityAggregationContainer> PriorityTotals { get; }
        public int TotalPriorityRequests { get { return PriorityTotals.Sum(c => c.BinsContainer.SumValue); } }
        public int AveragePriorityRequests{ get { return Convert.ToInt32(Math.Round(PriorityTotals.Average(c => c.BinsContainer.SumValue))); } }

        public void GetPriorityByBin(BinsContainer binsContainer)
        {



            foreach (var bin in binsContainer.Bins)
            {
                int summedServicedPriority = 0;
                foreach (var preempt in PriorityTotals)
                {
                    summedServicedPriority += preempt.BinsContainer.Bins.Where(a => a.Start == bin.Start)
                        .Sum(a => a.Value);
                }
                bin.Value = summedServicedPriority;
            }

        }




        public double Order { get; set; }

        public PriorityAggregationBySignal(AggregationMetricOptions options, Models.Signal signal, BinsContainer binsContainer)
        {
            Signal = signal;
            PriorityTotals = new List<SignalPriorityAggregationContainer>();

            PriorityTotals.Add(
                new SignalPriorityAggregationContainer(Signal, binsContainer));


        }

        public void GetPriorityTotalsBySignalByPriorityNumber(BinsContainer binsContainer, int priorityNumber)
        {

            PriorityTotals.Clear();

            PriorityTotals.Add(
                new SignalPriorityAggregationContainer(Signal, binsContainer, priorityNumber));


        }



    }

    public class SignalPriorityAggregationContainer
    {
        public Models.Signal Signal { get; }
        public BinsContainer BinsContainer { get; set; } = new BinsContainer();

        public SignalPriorityAggregationContainer(Models.Signal signal, BinsContainer binsContainer) //, AggregationMetricOptions.XAxisTimeTypes aggregationType)
        {
            Signal = signal;



            var priorityAggregationRepository =
                MOE.Common.Models.Repositories.PriorityAggregationDatasRepositoryFactory.Create();


            foreach (var bin in binsContainer.Bins)
            {

                var records = priorityAggregationRepository
                    .GetPriorityAggregationByVersionIdAndDateRange(
                        Signal.VersionID, bin.Start, bin.End);

                var totalRequests = records.Sum(s => s.PriorityRequests);

                BinsContainer.Bins.Add(new Bin { Start = bin.Start, End = bin.End, Value = totalRequests });

            }
        }



        public SignalPriorityAggregationContainer(Models.Signal signal, BinsContainer binsContainer, int priorityNumber)
        {

            Signal = signal;

            var priorityAggregationRepository =
                MOE.Common.Models.Repositories.PriorityAggregationDatasRepositoryFactory.Create();


            foreach (var bin in binsContainer.Bins)
            {

                var records = priorityAggregationRepository
                    .GetPriorityAggregationByVersionIdAndDateRange(
                        Signal.VersionID, bin.Start, bin.End).Where(e => e.PriorityNumber == priorityNumber);

                var totalRequests = records.Sum(s => s.PriorityRequests);

                BinsContainer.Bins.Add(new Bin { Start = bin.Start, End = bin.End, Value = totalRequests });

            } 
        }




    }
}