using System;
using System.Collections.Generic;
using System.Linq;
using MOE.Common.Business.Bins;
using MOE.Common.Business.WCFServiceLibrary;
using MOE.Common.Models;

namespace MOE.Common.Business.DataAggregation
{
    public class ApproachSpeedAggregationBySignal
    {
        public Models.Signal Signal { get; }
        public List<ApproachSpeedAggregationContainer> ApproachSpeeds { get; }
       
        public int AverageSpeed { get { return 200; } }

        public void GetSpeedAverageByBin(BinsContainer binsContainer)
        {



            foreach (var bin in binsContainer.Bins)
            {
                int summedSpeedTotals = 0;
                int summedVolumes = 0;
                foreach (var apprSpeed in ApproachSpeeds)
                {
                    summedSpeedTotals += apprSpeed.BinsContainer.Bins.Where(a => a.Start == bin.Start)
                        .Sum(a => a.Sum);

                    summedVolumes += apprSpeed.BinsContainer.Bins.Where(a => a.Start == bin.Start)
                        .Sum(a => a.Sum);
                }
                bin.Sum = summedSpeedTotals;
            }

        }


        public double Order { get; set; }

        public ApproachSpeedAggregationBySignal(ApproachAggregationMetricOptions options, Models.Signal signal, List<BinsContainer> binsContainers)
        {
            Signal = signal;
            ApproachSpeeds = new List<ApproachSpeedAggregationContainer>();
            foreach (var approach in signal.Approaches)
            {
                ApproachSpeeds.Add(
                    new ApproachSpeedAggregationContainer(approach, binsContainers));
            }

        }


        public int GetApproachSpeedsByDirection(DirectionType direction)
        {
            int speed = ApproachSpeeds
                .Where(a => a.Approach.DirectionType.DirectionTypeID == direction.DirectionTypeID)
                .Sum(a => a.BinsContainer.SumValue);
            return speed;
        }

        public int GetAverageSpeedByDirection(DirectionType direction)
        {
            int speed = Convert.ToInt32(Math.Round(ApproachSpeeds
                .Where(a => a.Approach.DirectionType.DirectionTypeID == direction.DirectionTypeID)
                .Average(a => a.BinsContainer.SumValue)));
            return speed;
        }
    }

    public class ApproachSpeedAggregationContainer
    {
        public Approach Approach { get; }
        public BinsContainer BinsContainer { get; set; } 

        public ApproachSpeedAggregationContainer(Approach approach, List<BinsContainer> binsContainer)//, AggregationMetricOptions.XAxisTimeTypes aggregationType)
        {
            Approach = approach;
            var splitFailAggregationRepository = Models.Repositories.ApproachSplitFailAggregationRepositoryFactory.Create();
            var container = binsContainer.FirstOrDefault();
            if (container != null)
            {
                foreach (var bin in container.Bins)
                {
                    var splitFails = splitFailAggregationRepository
                        .GetApproachSplitFailCountAggregationByApproachIdAndDateRange(
                            approach.ApproachID, bin.Start, bin.End);
                    BinsContainer.Bins.Add(new Bin {Start = bin.Start, End = bin.End, Sum = splitFails});
                }
            }
        }




    }
}