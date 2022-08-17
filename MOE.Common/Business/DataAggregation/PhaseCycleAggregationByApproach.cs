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
    public class PhaseCycleAggregationByApproach : AggregationByApproach
    {
        public PhaseCycleAggregationByApproach(Approach approach, ApproachAggregationMetricOptions options, DateTime startDate,
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
            var approachCycleAggregationRepository =
                PhaseCycleAggregationsRepositoryFactory.Create();
            var approachCycles =
                approachCycleAggregationRepository.GetApproachCyclesAggregationByApproachIdAndDateRange(
                    approach.ApproachID, options.StartDate, options.EndDate);
            if (approachCycles != null)
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
                        if (approachCycles.Any(s => s.BinStartTime >= bin.Start && s.BinStartTime < bin.End))
                        {
                            var approachCycleCount = 0;
                            switch (dataType.DataName)
                            {
                                case PhaseCycleAggregationOptions.TOTAL_RED_TO_RED_CYCLES:
                                    approachCycleCount =
                                        approachCycles.Where(s =>
                                                s.BinStartTime >= bin.Start && s.BinStartTime < bin.End)
                                            .Sum(s => s.TotalRedToRedCycles);
                                    break;
                                case PhaseCycleAggregationOptions.TOTAL_GREEN_TO_GREEN_CYCLES:
                                    approachCycleCount =
                                        approachCycles.Where(s =>
                                                s.BinStartTime >= bin.Start && s.BinStartTime < bin.End)
                                            .Sum(s => s.TotalGreenToGreenCycles);
                                    break;
                                case PhaseCycleAggregationOptions.RED_TIME:
                                    approachCycleCount =
                                        (int) approachCycles.Where(s =>
                                                s.BinStartTime >= bin.Start && s.BinStartTime < bin.End)
                                            .Sum(s => s.RedTime);
                                    break;
                                case PhaseCycleAggregationOptions.YELLOW_TIME:
                                    approachCycleCount =
                                        (int) approachCycles.Where(s =>
                                                s.BinStartTime >= bin.Start && s.BinStartTime < bin.End)
                                            .Sum(s => s.YellowTime);
                                    break;
                                case PhaseCycleAggregationOptions.GREEN_TIME:
                                    approachCycleCount =
                                        (int) approachCycles.Where(s =>
                                                s.BinStartTime >= bin.Start && s.BinStartTime < bin.End)
                                            .Sum(s => s.GreenTime);
                                    break;
                                default:

                                    throw new Exception("Unknown Aggregate Data Type for Approach Cycle");
                            }
                            Bin newBin = new Bin
                            {
                                Start = bin.Start,
                                End = bin.End,
                                Sum = approachCycleCount,
                                Average = approachCycleCount
                            };
                            concurrentBins.Add(newBin);
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