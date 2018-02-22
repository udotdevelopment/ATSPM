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

    public class CycleAggregationByApproach:AggregationByApproach
    {

        protected override void LoadBins(Approach approach, DateTime startDate, DateTime endDate, bool getProtectedPhase,
            AggregatedDataType dataType)
        {
            var approachCycleAggregationRepository =
               Models.Repositories.ApproachCycleAggregationRepositoryFactory.Create();
            List<Models.ApproachCycleAggregation> approachCycles =
                approachCycleAggregationRepository.GetApproachCyclesAggregationByApproachIdAndDateRange(
                    approach.ApproachID, startDate, endDate, getProtectedPhase);
            if (approachCycles != null)
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
                        if (approachCycles.Any(s => s.BinStartTime >= bin.Start && s.BinStartTime < bin.End))
                        {
                            int approachCycleCount = 0;
                            switch (dataType.DataName)
                            {
                                case "TotalCycles":
                                    approachCycleCount =
                                       approachCycles.Where(s => s.BinStartTime >= bin.Start && s.BinStartTime < bin.End)
                                           .Sum(s => s.TotalCycles);
                                    break;
                                case "RedTime":
                                    approachCycleCount =
                                        (int) approachCycles.Where(s => s.BinStartTime >= bin.Start && s.BinStartTime < bin.End)
                                            .Sum(s => s.RedTime);
                                    break;
                                case "YellowTime":
                                    approachCycleCount =
                                        (int) approachCycles.Where(s => s.BinStartTime >= bin.Start && s.BinStartTime < bin.End)
                                            .Sum(s => s.YellowTime);
                                    break;
                                case "GreenTime":
                                    approachCycleCount =
                                        (int) approachCycles.Where(s => s.BinStartTime >= bin.Start && s.BinStartTime < bin.End)
                                            .Sum(s => s.GreenTime);
                                    break;
                                case "PedActuations":
                                    approachCycleCount =
                                        (int)approachCycles.Where(s => s.BinStartTime >= bin.Start && s.BinStartTime < bin.End)
                                            .Sum(s => s.PedActuations);
                                    break;
                                default:

                                    throw new Exception("Unknown Aggregate Data Type for Approach Cycle");
                            }

                            concurrentBins.Add(new Bin
                            {
                                Start = bin.Start,
                                End = bin.End,
                                Sum = approachCycleCount,
                                Average = approachCycleCount
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

        public CycleAggregationByApproach(Approach approach, BinFactoryOptions timeOptions, DateTime startDate, DateTime endDate, 
            bool getProtectedPhase, AggregatedDataType dataType) :base(approach, timeOptions,startDate, endDate, getProtectedPhase, dataType)
        {
        }
    }
}
