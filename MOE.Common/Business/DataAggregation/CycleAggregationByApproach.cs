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
    public class CycleAggregationByApproach : AggregationByApproach
    {
        public CycleAggregationByApproach(Approach approach, ApproachAggregationMetricOptions options, DateTime startDate,
            DateTime endDate,
            bool getProtectedPhase, AggregatedDataType dataType) : base(approach, options, startDate, endDate,
            getProtectedPhase, dataType)
        {
        }

        protected override void LoadBins(Approach approach, ApproachAggregationMetricOptions options,
            bool getProtectedPhase,
            AggregatedDataType dataType)
        {
            var approachCycleAggregationRepository =
                ApproachCycleAggregationRepositoryFactory.Create();
            var approachCycles =
                approachCycleAggregationRepository.GetApproachCyclesAggregationByApproachIdAndDateRange(
                    approach.ApproachID, options.StartDate, options.EndDate, getProtectedPhase);
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
                                case "TotalCycles":
                                    approachCycleCount =
                                        approachCycles.Where(s =>
                                                s.BinStartTime >= bin.Start && s.BinStartTime < bin.End)
                                            .Sum(s => s.TotalCycles);
                                    break;
                                case "RedTime":
                                    approachCycleCount =
                                        (int) approachCycles.Where(s =>
                                                s.BinStartTime >= bin.Start && s.BinStartTime < bin.End)
                                            .Sum(s => s.RedTime);
                                    break;
                                case "YellowTime":
                                    approachCycleCount =
                                        (int) approachCycles.Where(s =>
                                                s.BinStartTime >= bin.Start && s.BinStartTime < bin.End)
                                            .Sum(s => s.YellowTime);
                                    break;
                                case "GreenTime":
                                    approachCycleCount =
                                        (int) approachCycles.Where(s =>
                                                s.BinStartTime >= bin.Start && s.BinStartTime < bin.End)
                                            .Sum(s => s.GreenTime);
                                    break;
                                case "PedActuations":
                                    approachCycleCount =
                                        approachCycles.Where(s =>
                                                s.BinStartTime >= bin.Start && s.BinStartTime < bin.End)
                                            .Sum(s => s.PedActuations);
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