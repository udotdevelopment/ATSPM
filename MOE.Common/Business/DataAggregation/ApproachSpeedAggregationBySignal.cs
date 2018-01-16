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
                        .Sum(a => a.Value);

                    summedVolumes += apprSpeed.BinsContainer.Bins.Where(a => a.Start == bin.Start)
                        .Sum(a => a.Value);
                }
                bin.Value = summedSpeedTotals;
            }

        }


        public double Order { get; set; }

        public ApproachSpeedAggregationBySignal(AggregationMetricOptions options, Models.Signal signal, BinsContainer binsContainer)
        {
            Signal = signal;
            ApproachSpeeds = new List<ApproachSpeedAggregationContainer>();
            foreach (var approach in signal.Approaches)
            {
                ApproachSpeeds.Add(
                    new ApproachSpeedAggregationContainer(approach, binsContainer));
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
            int splitFails = Convert.ToInt32(Math.Round(ApproachSpeeds
                .Where(a => a.Approach.DirectionType.DirectionTypeID == direction.DirectionTypeID)
                .Average(a => a.BinsContainer.SumValue)));
            return splitFails;
        }
    }

    public class ApproachSpeedAggregationContainer
    {
        public Approach Approach { get; }
        public BinsContainer BinsContainer { get; set; } = new BinsContainer();

        public ApproachSpeedAggregationContainer(Approach approach, BinsContainer binsContainer)//, AggregationMetricOptions.XAxisTimeTypes aggregationType)
        {
            Approach = approach;
            var splitFailAggregationRepository =
                MOE.Common.Models.Repositories.ApproachSplitFailAggregationRepositoryFactory.Create();


            foreach (var bin in binsContainer.Bins)
            {

                var splitFails = splitFailAggregationRepository
                    .GetApproachSplitFailAggregationByApproachIdAndDateRange(
                        approach.ApproachID, bin.Start, bin.End);
                BinsContainer.Bins.Add(new Bin { Start = bin.Start, End = bin.End, Value = splitFails });

            }
        }




    }
}