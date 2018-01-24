using System;
using System.Collections.Concurrent;
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
        public int TotalSplitFailures { get { return ApproachSplitFailures.Sum(c => c.BinsContainers.FirstOrDefault().SumValue); } }
        public int AverageSplitFailures { get { return Convert.ToInt32(Math.Round(ApproachSplitFailures.Average(c => c.BinsContainers.FirstOrDefault().SumValue))); } }

        public void GetSplitFailuresByBin(List<BinsContainer> binsContainers)
        {
            var container = binsContainers.FirstOrDefault();
            if (container != null)
            {
                foreach (var bin in container.Bins)
                {
                    int summedSplitFailures = 0;
                    foreach (var approachSplitFail in ApproachSplitFailures)
                    {
                        summedSplitFailures += approachSplitFail.BinsContainers.FirstOrDefault().Bins.Where(a => a.Start == bin.Start)
                            .Sum(a => a.Sum);
                    }
                    bin.Sum = summedSplitFailures;
                }
            }
        }


        public double Order { get; set; }

        public SpliFailAggregationBySignal(AggregationMetricOptions options, Models.Signal signal, List<BinsContainer> binsContainers)
        {
            Signal = signal;
            ApproachSplitFailures = new List<ApproachSplitFailAggregationContainer>();
            foreach (var approach in signal.Approaches)
            {
                 ApproachSplitFailures.Add(
                        new ApproachSplitFailAggregationContainer(approach, binsContainers, options.StartDate,
                            options.EndDate));
            }

        }


        public int GetSplitFailsByDirection(DirectionType direction)
        {
            int splitFails = ApproachSplitFailures
                .Where(a => a.Approach.DirectionType.DirectionTypeID == direction.DirectionTypeID)
                .Sum(a => a.BinsContainers.FirstOrDefault().SumValue);
            return splitFails;
        }

        public int GetAverageSplitFailsByDirection(DirectionType direction)
        {
            int splitFails = Convert.ToInt32(Math.Round(ApproachSplitFailures
                .Where(a => a.Approach.DirectionType.DirectionTypeID == direction.DirectionTypeID)
                .Average(a => a.BinsContainers.FirstOrDefault().SumValue)));
            return splitFails;
        }
    }
    
    public class ApproachSplitFailAggregationContainer
    {
        public Approach Approach { get; }
        public List<BinsContainer> BinsContainers { get; set; } = new List<BinsContainer>();

        public ApproachSplitFailAggregationContainer(Approach approach, List<BinsContainer> binsContainers, DateTime startDate, DateTime endDate)
        {
            Approach = approach;
            var splitFailAggregationRepository =
                Models.Repositories.ApproachSplitFailAggregationRepositoryFactory.Create();
            List<ApproachSplitFailAggregation> splitFails =
                splitFailAggregationRepository.GetApproachSplitFailsAggregationByApproachIdAndDateRange(
                    approach.ApproachID, startDate, endDate);
            if (splitFails != null)
            {
                //foreach (var binsContainer in binsContainers)
                Parallel.ForEach(binsContainers, binsContainer =>
                {

                    BinsContainer tempBinsContainer =
                        new BinsContainer(binsContainer.Start, binsContainer.End);
                    ConcurrentBag<Bin> concurrentBins = new ConcurrentBag<Bin>();
                    //foreach (var bin in binsContainer.Bins)
                    Parallel.ForEach(binsContainer.Bins, bin =>
                    {
                        if (splitFails.Any(s => s.BinStartTime >= bin.Start && s.BinStartTime < bin.End))
                        {
                            int splitFailCount =
                                splitFails.Where(s => s.BinStartTime >= bin.Start && s.BinStartTime < bin.End)
                                    .Sum(s => s.SplitFailures);
                            double splitFailAverage =
                                splitFails.Where(s => s.BinStartTime >= bin.Start && s.BinStartTime < bin.End)
                                    .Average(s => s.SplitFailures);
                            concurrentBins.Add(new Bin
                            {
                                Start = bin.Start,
                                End = bin.End,
                                Sum = splitFailCount,
                                Average = splitFailAverage
                            });
                        }
                        else
                        {
                            concurrentBins.Add(new Bin
                            {
                                Start = bin.Start,
                                End = bin.End,
                                Sum = 0,
                                Average = 0
                            });
                        }

                    });
                    tempBinsContainer.Bins = concurrentBins.OrderBy(c => c.Start).ToList();
                    BinsContainers.Add(tempBinsContainer);
                });
                BinsContainers = BinsContainers.OrderBy(b => b.Start).ToList();
            }
        }
    }
        
    
        
}