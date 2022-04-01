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
    public class PhaseSplitMonitorAggregationByPhase : AggregationByPhase
    {
        public const string EIGHTY_FIFTH_PERCENTILE_SPLIT = "EightyFifthPercentileSplit";
        public const string SKIPPED_COUNT = "SkippedCount";

        public PhaseSplitMonitorAggregationByPhase(Models.Signal signal, int phaseNumber, PhaseSplitMonitorAggregationOptions options, AggregatedDataType dataType) 
            : base(signal, phaseNumber, options, dataType)
        {
            LoadBins(signal, phaseNumber, options, dataType);
        }

        protected override void LoadBins(Models.Signal signal, int phaseNumber, PhaseAggregationMetricOptions options,
            AggregatedDataType dataType)
        {
            var phaseSplitMonitorAggregationRepository = PhaseSplitMonitorAggregationRepositoryFactory.Create();
            var splitFails = phaseSplitMonitorAggregationRepository.GetSplitMonitorAggregationBySignalIdPhaseNumberAndDateRange(
                signal.SignalID, phaseNumber, options.StartDate, options.EndDate);
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
                            var terminationCount = 0;
                            switch (dataType.DataName)
                            {
                                case EIGHTY_FIFTH_PERCENTILE_SPLIT:
                                    terminationCount =
                                        splitFails.Where(s => s.BinStartTime >= bin.Start && s.BinStartTime < bin.End)
                                            .Sum(s => s.EightyFifthPercentileSplit);
                                    break;
                                case SKIPPED_COUNT:
                                    terminationCount =
                                        splitFails.Where(s => s.BinStartTime >= bin.Start && s.BinStartTime < bin.End)
                                            .Sum(s => s.SkippedCount);
                                    break;                                
                                default:

                                    throw new Exception("Unknown Aggregate Data Type for Split Failure");
                            }

                            concurrentBins.Add(new Bin
                            {
                                Start = bin.Start,
                                End = bin.End,
                                Sum = terminationCount,
                                Average = terminationCount
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