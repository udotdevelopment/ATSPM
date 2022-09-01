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
    public class PhaseTerminationAggregationByPhase : AggregationByPhase
    {
        public PhaseTerminationAggregationByPhase(Models.Signal signal, int phaseNumber, PhaseTerminationAggregationOptions options, AggregatedDataType dataType) 
            : base(signal, phaseNumber, options, dataType)
        {
            LoadBins(signal, phaseNumber, options, dataType);
        }

        protected override void LoadBins(Models.Signal signal, int phaseNumber, PhaseAggregationMetricOptions options,
            AggregatedDataType dataType)
        {
            var phaseTerminationAggregationRepository = PhaseTerminationAggregationRepositoryFactory.Create();
            var splitFails = phaseTerminationAggregationRepository.GetPhaseTerminationsAggregationBySignalIdPhaseNumberAndDateRange(
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
                                case "GapOuts":
                                    terminationCount =
                                        splitFails.Where(s => s.BinStartTime >= bin.Start && s.BinStartTime < bin.End)
                                            .Sum(s => s.GapOuts);
                                    break;
                                case "ForceOffs":
                                    terminationCount =
                                        splitFails.Where(s => s.BinStartTime >= bin.Start && s.BinStartTime < bin.End)
                                            .Sum(s => s.ForceOffs);
                                    break;
                                case "MaxOuts":
                                    terminationCount =
                                        splitFails.Where(s => s.BinStartTime >= bin.Start && s.BinStartTime < bin.End)
                                            .Sum(s => s.MaxOuts);
                                    break;
                                case "Unknown":
                                    terminationCount =
                                        splitFails.Where(s => s.BinStartTime >= bin.Start && s.BinStartTime < bin.End)
                                            .Sum(s => s.UnknownTerminationTypes);
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