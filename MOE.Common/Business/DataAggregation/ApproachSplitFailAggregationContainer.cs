using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MOE.Common.Business.Bins;
using MOE.Common.Models;

namespace MOE.Common.Business.DataAggregation
{

    public class ApproachSplitFailAggregationContainer
    {
        public Approach Approach { get; }
        public List<BinsContainer> BinsContainers { get; set; } = new List<BinsContainer>();

        public ApproachSplitFailAggregationContainer(Approach approach, List<BinsContainer> binsContainers, DateTime startDate, DateTime endDate, bool getProtectedPhase)
        {
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
