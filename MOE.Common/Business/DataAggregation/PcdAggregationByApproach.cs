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
    public class PcdAggregationByApproach : AggregationByApproach
    {
        public PcdAggregationByApproach(Approach approach, BinFactoryOptions timeOptions, DateTime startDate,
            DateTime endDate,
            bool getProtectedPhase, AggregatedDataType dataType) : base(approach, timeOptions, startDate, endDate,
            getProtectedPhase, dataType)
        {
        }

        protected override void LoadBins(Approach approach, DateTime startDate, DateTime endDate,
            bool getProtectedPhase,
            AggregatedDataType dataType)
        {
            var splitFailAggregationRepository =
                ApproachPcdAggregationRepositoryFactory.Create();
            var splitFails =
                splitFailAggregationRepository.GetApproachPcdsAggregationByApproachIdAndDateRange(
                    approach.ApproachID, startDate, endDate, getProtectedPhase);
            if (splitFails != null)
            {
                var concurrentBinContainers = new ConcurrentBag<BinsContainer>();
                //foreach (var binsContainer in binsContainers)
                Parallel.ForEach(BinsContainers, binsContainer =>
                {
                    var tempBinsContainer =
                        new BinsContainer(binsContainer.Start, binsContainer.End);
                    var concurrentBins = new ConcurrentBag<Bin>();
                    //foreach (var bin in binsContainer.Bins)
                    Parallel.ForEach(binsContainer.Bins, bin =>
                    {
                        if (splitFails.Any(s => s.BinStartTime >= bin.Start && s.BinStartTime < bin.End))
                        {
                            var splitFailCount = 0;
                            switch (dataType.DataName)
                            {
                                case "ArrivalsOnGreen":
                                    splitFailCount =
                                        splitFails.Where(s => s.BinStartTime >= bin.Start && s.BinStartTime < bin.End)
                                            .Sum(s => s.ArrivalsOnGreen);
                                    break;
                                case "ArrivalsOnRed":
                                    splitFailCount =
                                        splitFails.Where(s => s.BinStartTime >= bin.Start && s.BinStartTime < bin.End)
                                            .Sum(s => s.ArrivalsOnRed);
                                    break;
                                case "ArrivalsOnYellow":
                                    splitFailCount =
                                        splitFails.Where(s => s.BinStartTime >= bin.Start && s.BinStartTime < bin.End)
                                            .Sum(s => s.ArrivalsOnYellow);
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