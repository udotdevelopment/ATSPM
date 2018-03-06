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
    public class EventCountAggregationByApproach : AggregationByApproach
    {
        public EventCountAggregationByApproach(Approach approach, ApproachEventCountAggregationOptions options, DateTime startDate,
            DateTime endDate,
            bool getProtectedPhase, AggregatedDataType dataType) : base(approach, options, startDate, endDate,
            getProtectedPhase, dataType)
        {
        }

        protected override void LoadBins(Approach approach, ApproachAggregationMetricOptions options,
            bool getProtectedPhase,
            AggregatedDataType dataType)
        {
            var eventCountAggregationRepository =
                PhaseEventCountAggregationRepositoryFactory.Create();
            var eventCounts =
                eventCountAggregationRepository.GetPhaseEventCountAggregationByPhaseIdAndDateRange(
                    approach.ApproachID, options.StartDate, options.EndDate, getProtectedPhase);
            if (eventCounts != null)
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
                        if (eventCounts.Any(s => s.BinStartTime >= bin.Start && s.BinStartTime < bin.End))
                        {
                            var eventCountCount = 0;
                            switch (dataType.DataName)
                            {
                                case "EventCount":
                                    eventCountCount =
                                        eventCounts.Where(s => s.BinStartTime >= bin.Start && s.BinStartTime < bin.End)
                                            .Sum(s => s.EventCount);
                                    break;
                                default:

                                    throw new Exception("Unknown Aggregate Data Type for Split Failure");
                            }

                            concurrentBins.Add(new Bin
                            {
                                Start = bin.Start,
                                End = bin.End,
                                Sum = eventCountCount,
                                Average = eventCountCount
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