using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MOE.Common.Business.Bins;
using MOE.Common.Business.SplitFail;
using MOE.Common.Business.WCFServiceLibrary;
using MOE.Common.Models;

namespace MOE.Common.Business.DataAggregation
{
    public class SpliFailAggregationBySignal
    {
        public Models.Signal Signal { get; }
        public List<ApproachSplitFailAggregationContainer> ApproachSplitFailures { get;}
        public int TotalSplitFailures { get { return ApproachSplitFailures.Sum(c => c.BinsContainer.SumValue); } }
        public int AverageSplitFailures { get { return Convert.ToInt32(Math.Round(ApproachSplitFailures.Average(c => c.BinsContainer.SumValue))); } }

        public void GetSplitFailuresByBin(BinsContainer binsContainer)
        {



                foreach (var bin in binsContainer.Bins)
                {
                    int summedSplitFailures = 0;
                    foreach (var approachSplitFail in ApproachSplitFailures)
                    {
                        summedSplitFailures += approachSplitFail.BinsContainer.Bins.Where(a => a.Start == bin.Start)
                            .Sum(a => a.Value);
                    }
                    bin.Value = summedSplitFailures;
                }
            
        }


        public double Order { get; set; }

        public SpliFailAggregationBySignal(AggregationMetricOptions options, Models.Signal signal, BinsContainer binsContainer)
        {
            Signal = signal;
            ApproachSplitFailures = new List<ApproachSplitFailAggregationContainer>();
            foreach (var approach in signal.Approaches)
            {
                ApproachSplitFailures.Add(
                    new ApproachSplitFailAggregationContainer(approach, binsContainer));
            }

        }


        public int GetSplitFailsByDirection(DirectionType direction)
        {
            int splitFails = ApproachSplitFailures
                .Where(a => a.Approach.DirectionType.DirectionTypeID == direction.DirectionTypeID)
                .Sum(a => a.BinsContainer.SumValue);
            return splitFails;
        }

        public int GetAverageSplitFailsByDirection(DirectionType direction)
        {
            int splitFails = Convert.ToInt32(Math.Round(ApproachSplitFailures
                .Where(a => a.Approach.DirectionType.DirectionTypeID == direction.DirectionTypeID)
                .Average(a => a.BinsContainer.SumValue)));
            return splitFails;
        }
    }
    
    public class ApproachSplitFailAggregationContainer
    {
        public Approach Approach { get; }
        public BinsContainer BinsContainer { get; set; } = new BinsContainer();

        public ApproachSplitFailAggregationContainer(Approach approach, BinsContainer binsContainer)//, AggregationMetricOptions.XAxisTimeTypes aggregationType)
        {
            Approach = approach;
            var splitFailAggregationRepository =
                MOE.Common.Models.Repositories.ApproachSplitFailAggregationRepositoryFactory.Create();


                foreach (var bin in BinsContainer.Bins)
                {

                    var splitFails = splitFailAggregationRepository
                        .GetApproachSplitFailAggregationByApproachIdAndDateRange(
                            approach.ApproachID, bin.Start, bin.End);
                    BinsContainer.Bins.Add(new Bin {Start = bin.Start, End = bin.End, Value = splitFails});

                }
        }
        

        
        
    }
}