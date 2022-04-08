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
    public class PhaseLeftTurnGapAggregationByApproach : AggregationByApproach
    {
        public PhaseLeftTurnGapAggregationByApproach(Approach approach, ApproachAggregationMetricOptions options, DateTime startDate,
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
            var approachLeftTurnGapAggregationRepository =
                PhaseLeftTurnGapAggregationRepositoryFactory.Create();
            var approachLeftTurnGaps =
                approachLeftTurnGapAggregationRepository.GetAggregationByApproachIdAndDateRange(
                    approach.ApproachID, options.StartDate, options.EndDate);
            if (approachLeftTurnGaps != null)
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
                        if (approachLeftTurnGaps.Any(s => s.BinStartTime >= bin.Start && s.BinStartTime < bin.End))
                        {
                            var approachCycleCount = 0.0;
                            switch (dataType.DataName)
                            {
                                case PhaseLeftTurnGapAggregationOptions.GAP_COUNT_1:
                                    approachCycleCount =
                                        approachLeftTurnGaps.Where(s =>
                                                s.BinStartTime >= bin.Start && s.BinStartTime < bin.End)
                                            .Sum(s => s.GapCount1);
                                    break;
                                case PhaseLeftTurnGapAggregationOptions.GAP_COUNT_2:
                                    approachCycleCount =
                                        approachLeftTurnGaps.Where(s =>
                                                s.BinStartTime >= bin.Start && s.BinStartTime < bin.End)
                                            .Sum(s => s.GapCount2);
                                    break;
                                case PhaseLeftTurnGapAggregationOptions.GAP_COUNT_3:
                                    approachCycleCount =
                                        (int) approachLeftTurnGaps.Where(s =>
                                                s.BinStartTime >= bin.Start && s.BinStartTime < bin.End)
                                            .Sum(s => s.GapCount3);
                                    break;
                                case PhaseLeftTurnGapAggregationOptions.GAP_COUNT_4:
                                    approachCycleCount =
                                        (int) approachLeftTurnGaps.Where(s =>
                                                s.BinStartTime >= bin.Start && s.BinStartTime < bin.End)
                                            .Sum(s => s.GapCount4);
                                    break;
                                case PhaseLeftTurnGapAggregationOptions.GAP_COUNT_5:
                                    approachCycleCount =
                                        (int) approachLeftTurnGaps.Where(s =>
                                                s.BinStartTime >= bin.Start && s.BinStartTime < bin.End)
                                            .Sum(s => s.GapCount5);
                                    break;
                                case PhaseLeftTurnGapAggregationOptions.GAP_COUNT_6:
                                    approachCycleCount =
                                        approachLeftTurnGaps.Where(s =>
                                                s.BinStartTime >= bin.Start && s.BinStartTime < bin.End)
                                            .Sum(s => s.GapCount6);
                                    break;
                                case PhaseLeftTurnGapAggregationOptions.GAP_COUNT_7:
                                    approachCycleCount =
                                        approachLeftTurnGaps.Where(s =>
                                                s.BinStartTime >= bin.Start && s.BinStartTime < bin.End)
                                            .Sum(s => s.GapCount7);
                                    break;
                                case PhaseLeftTurnGapAggregationOptions.GAP_COUNT_8:
                                    approachCycleCount =
                                        (int)approachLeftTurnGaps.Where(s =>
                                               s.BinStartTime >= bin.Start && s.BinStartTime < bin.End)
                                            .Sum(s => s.GapCount8);
                                    break;
                                case PhaseLeftTurnGapAggregationOptions.GAP_COUNT_9:
                                    approachCycleCount =
                                        (int)approachLeftTurnGaps.Where(s =>
                                               s.BinStartTime >= bin.Start && s.BinStartTime < bin.End)
                                            .Sum(s => s.GapCount9);
                                    break;
                                case PhaseLeftTurnGapAggregationOptions.GAP_COUNT_10:
                                    approachCycleCount =
                                        (int)approachLeftTurnGaps.Where(s =>
                                               s.BinStartTime >= bin.Start && s.BinStartTime < bin.End)
                                            .Sum(s => s.GapCount10);
                                    break;
                                case PhaseLeftTurnGapAggregationOptions.GAP_COUNT_11:
                                    approachCycleCount =
                                        approachLeftTurnGaps.Where(s =>
                                                s.BinStartTime >= bin.Start && s.BinStartTime < bin.End)
                                            .Sum(s => s.GapCount11);
                                    break;
                                case PhaseLeftTurnGapAggregationOptions.SUM_GAP_DURATION_1:
                                    approachCycleCount =
                                        approachLeftTurnGaps.Where(s =>
                                                s.BinStartTime >= bin.Start && s.BinStartTime < bin.End)
                                            .Sum(s => s.SumGapDuration1);
                                    break;
                                case PhaseLeftTurnGapAggregationOptions.SUM_GAP_DURATION_2:
                                    approachCycleCount =
                                        (int)approachLeftTurnGaps.Where(s =>
                                               s.BinStartTime >= bin.Start && s.BinStartTime < bin.End)
                                            .Sum(s => s.SumGapDuration2);
                                    break;
                                case PhaseLeftTurnGapAggregationOptions.SUM_GAP_DURATION_3:
                                    approachCycleCount =
                                        (int)approachLeftTurnGaps.Where(s =>
                                               s.BinStartTime >= bin.Start && s.BinStartTime < bin.End)
                                            .Sum(s => s.SumGapDuration3);
                                    break;
                                case PhaseLeftTurnGapAggregationOptions.SUM_GREEN_TIME:
                                    approachCycleCount =
                                        (int)approachLeftTurnGaps.Where(s =>
                                               s.BinStartTime >= bin.Start && s.BinStartTime < bin.End)
                                            .Sum(s => s.SumGreenTime);
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