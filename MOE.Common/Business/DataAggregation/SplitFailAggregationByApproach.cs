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

    public class SplitFailAggregationByApproach:AggregationByApproach
    {

        protected override void LoadBins(Approach approach, DateTime startDate, DateTime endDate, bool getProtectedPhase,
            AggregatedDataType dataType)
        {
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
                            switch (dataType.DataName)
                            {
                                case "SplitFails":
                                    splitFailCount =
                                       splitFails.Where(s => s.BinStartTime >= bin.Start && s.BinStartTime < bin.End)
                                           .Sum(s => s.SplitFailures);
                                    break;
                                case "ForceOffs":
                                    splitFailCount =
                                        splitFails.Where(s => s.BinStartTime >= bin.Start && s.BinStartTime < bin.End)
                                            .Sum(s => s.ForceOffs);
                                    break;
                                case "GapOuts":
                                    splitFailCount =
                                        splitFails.Where(s => s.BinStartTime >= bin.Start && s.BinStartTime < bin.End)
                                            .Sum(s => s.GapOuts);
                                    break;
                                case "MaxOuts":
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

        public SplitFailAggregationByApproach(Approach approach, BinFactoryOptions timeOptions, DateTime startDate, DateTime endDate, 
            bool getProtectedPhase, AggregatedDataType dataType) :base(approach, timeOptions,startDate, endDate, getProtectedPhase, dataType)
        {
        }
    }
}
