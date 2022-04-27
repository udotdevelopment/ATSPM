using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using MOE.Common.Business.Bins;
using MOE.Common.Business.WCFServiceLibrary;
using MOE.Common.Models;
using MOE.Common.Models.Repositories;

namespace MOE.Common.Business.DataAggregation
{
    public class SplitFailAggregationByApproach : AggregationByApproach
    {
        public SplitFailAggregationByApproach(Approach approach, ApproachSplitFailAggregationOptions options, DateTime startDate,
            DateTime endDate,
            bool getProtectedPhase, AggregatedDataType dataType) : base(approach, options, startDate, endDate,
            getProtectedPhase, dataType)
        {
            LoadBins(approach, options, getProtectedPhase, dataType);
        }

        protected override void LoadBins(Approach approach, ApproachAggregationMetricOptions options,
            bool getProtectedPhase,
            AggregatedDataType dataType)
        {
            var splitFailAggregationRepository =
                ApproachSplitFailAggregationRepositoryFactory.Create();
            var selectionEndDate = BinsContainers.Max(b => b.End);
            //Add a day so that it gets all the data for the entire end day instead of stoping at 12:00AM
            if (options.TimeOptions.SelectedBinSize == BinFactoryOptions.BinSize.Day)
            {
                selectionEndDate = selectionEndDate.AddDays(1);
            }
            var splitFails =
                splitFailAggregationRepository.GetApproachSplitFailsAggregationByApproachIdAndDateRange(
                    approach.ApproachID, options.TimeOptions.Start, selectionEndDate, getProtectedPhase);
            if (splitFails != null)
            {
                var concurrentBinContainers = new ConcurrentBag<BinsContainer>();
                foreach (var binsContainer in BinsContainers)
                //Parallel.ForEach(BinsContainers, binsContainer =>
                {
                    var tempBinsContainer =
                        new BinsContainer(binsContainer.Start, binsContainer.End);
                    var concurrentBins = new ConcurrentBag<Bin>();
                    foreach (var bin in binsContainer.Bins)
                    //Parallel.ForEach(binsContainer.Bins, bin =>
                    {
                        if (splitFails.Any(s => s.BinStartTime >= bin.Start && s.BinStartTime < bin.End))
                        {
                            var splitFailCount = 0;
                            switch (dataType.DataName)
                            {
                                case "SplitFailures":
                                    splitFailCount =
                                        splitFails.Where(s => s.BinStartTime >= bin.Start && s.BinStartTime < bin.End)
                                            .Sum(s => s.SplitFailures);
                                    break;
                                case "GreenOccupancySum":
                                    splitFailCount =
                                        splitFails.Where(s => s.BinStartTime >= bin.Start && s.BinStartTime < bin.End)
                                            .Sum(s => s.GreenOccupancySum);
                                    break;
                                case "RedOccupancySum":
                                    splitFailCount =
                                        splitFails.Where(s => s.BinStartTime >= bin.Start && s.BinStartTime < bin.End)
                                            .Sum(s => s.RedOccupancySum);
                                    break;
                                case "GreenTimeSum":
                                    splitFailCount =
                                        splitFails.Where(s => s.BinStartTime >= bin.Start && s.BinStartTime < bin.End)
                                            .Sum(s => s.GreenTimeSum);
                                    break;
                                case "RedTimeSum":
                                    splitFailCount =
                                        splitFails.Where(s => s.BinStartTime >= bin.Start && s.BinStartTime < bin.End)
                                            .Sum(s => s.RedTimeSum);
                                    break;
                                case "Cycles":
                                    splitFailCount =
                                        splitFails.Where(s => s.BinStartTime >= bin.Start && s.BinStartTime < bin.End)
                                            .Sum(s => s.Cycles);
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
                    }//);
                    tempBinsContainer.Bins = concurrentBins.OrderBy(c => c.Start).ToList();
                    concurrentBinContainers.Add(tempBinsContainer);
                }//);
                BinsContainers = concurrentBinContainers.OrderBy(b => b.Start).ToList();
            }
        }
        
    }
}