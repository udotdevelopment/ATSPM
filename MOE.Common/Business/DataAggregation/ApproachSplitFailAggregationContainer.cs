using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MOE.Common.Business.Bins;
using MOE.Common.Business.WCFServiceLibrary;
using MOE.Common.Models;

namespace MOE.Common.Business.DataAggregation
{

    public class ApproachSplitFailAggregationContainer
    {
        public Approach Approach { get; }
        public List<BinsContainer> BinsContainers { get; set; } = new List<BinsContainer>();

        public ApproachSplitFailAggregationContainer(Approach approach, BinFactoryOptions TimeOptions, DateTime startDate, DateTime endDate, 
            bool getProtectedPhase, ApproachSplitFailAggregationOptions.AggregatedDataTypes dataType)
        {
            BinsContainers = BinFactory.GetBins(TimeOptions);
            Approach = approach;
            var splitFailAggregationRepository =
                Models.Repositories.ApproachSplitFailAggregationRepositoryFactory.Create();
            List<ApproachSplitFailAggregation> splitFails =
                splitFailAggregationRepository.GetApproachSplitFailsAggregationByApproachIdAndDateRange(
                    approach.ApproachID, startDate, endDate, getProtectedPhase);
            if (splitFails != null)
            {
                ConcurrentBag<BinsContainer> concurrentBinContainers = new ConcurrentBag<BinsContainer>();
                //foreach (var binsContainer in binsContainers)
                Parallel.ForEach(BinsContainers, binsContainer =>
                {
                    BinsContainer tempBinsContainer =
                        new BinsContainer(binsContainer.Start, binsContainer.End);
                    ConcurrentBag<Bin> concurrentBins = new ConcurrentBag<Bin>();
                    //foreach (var bin in binsContainer.Bins)
                    Parallel.ForEach(binsContainer.Bins, bin =>
                    {
                        if (splitFails.Any(s => s.BinStartTime >= bin.Start && s.BinStartTime < bin.End))
                        {
                            int splitFailCount = 0;
                            switch (dataType)
                            {
                                case ApproachSplitFailAggregationOptions.AggregatedDataTypes.SplitFails:
                                     splitFailCount =
                                        splitFails.Where(s => s.BinStartTime >= bin.Start && s.BinStartTime < bin.End)
                                            .Sum(s => s.SplitFailures);
                                    break;
                                case ApproachSplitFailAggregationOptions.AggregatedDataTypes.ForceOffs:
                                    splitFailCount =
                                        splitFails.Where(s => s.BinStartTime >= bin.Start && s.BinStartTime < bin.End)
                                            .Sum(s => s.ForceOffs);
                                    break;
                                case ApproachSplitFailAggregationOptions.AggregatedDataTypes.GapOuts:
                                    splitFailCount =
                                        splitFails.Where(s => s.BinStartTime >= bin.Start && s.BinStartTime < bin.End)
                                            .Sum(s => s.GapOuts);
                                    break;
                                case ApproachSplitFailAggregationOptions.AggregatedDataTypes.MaxOuts:
                                    splitFailCount =
                                        splitFails.Where(s => s.BinStartTime >= bin.Start && s.BinStartTime < bin.End)
                                            .Sum(s => s.MaxOuts);
                                    break;
                                    default:
                                    
                                        throw new Exception("Unknown Aggregate Data Type for Split Failure");
                            }

                            concurrentBins.Add(new Bin
                            {
                                Start = bin.Start,
                                End = bin.End,
                                Sum = splitFailCount,
                                Average = splitFailCount
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
                    concurrentBinContainers.Add(tempBinsContainer);
                });
                BinsContainers = concurrentBinContainers.OrderBy(b => b.Start).ToList();
            }
        }
    }
}
