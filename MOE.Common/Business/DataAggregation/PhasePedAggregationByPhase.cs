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
    public class PhasePedAggregationByPhase : AggregationByPhase
    {
        public const string PED_ACTUATIONS = "PedActuations";
        public const string MAX_PED_DELAY = "MaxPedDelay";
        public const string MIN_PED_DELAY = "MinPedDelay";
        public const string PED_DELAY_SUM = "PedDelaySum";
        public const string PED_CYCLES = "PedCycles";

        public PhasePedAggregationByPhase(Models.Signal signal, int phaseNumber, PhasePedAggregationOptions options, AggregatedDataType dataType) 
            : base(signal, phaseNumber, options, dataType)
        {
            LoadBins(signal, phaseNumber, options, dataType);
        }

        protected override void LoadBins(Models.Signal signal, int phaseNumber, PhaseAggregationMetricOptions options,
            AggregatedDataType dataType)
        {
            var phasePedAggregationRepository =  PhasePedAggregationRepositoryFactory.Create();
            var pedAggs = phasePedAggregationRepository.GetPhasePedsAggregationBySignalIdPhaseNumberAndDateRange(
                signal.SignalID, phaseNumber, options.StartDate, options.EndDate);
            if (pedAggs != null)
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
                        if (pedAggs.Any(s => s.BinStartTime >= bin.Start && s.BinStartTime < bin.End))
                        {
                            var pedAggCount = 0;
                            switch (dataType.DataName)
                            {
                                case PED_CYCLES:
                                    pedAggCount =
                                        pedAggs.Where(s => s.BinStartTime >= bin.Start && s.BinStartTime < bin.End)
                                            .Sum(s => s.PedCycles);
                                    break;
                                case PED_DELAY_SUM:
                                    pedAggCount =
                                        pedAggs.Where(s => s.BinStartTime >= bin.Start && s.BinStartTime < bin.End)
                                            .Sum(s => s.PedDelaySum);
                                    break;
                                case MIN_PED_DELAY:
                                    pedAggCount =
                                        pedAggs.Where(s => s.BinStartTime >= bin.Start && s.BinStartTime < bin.End)
                                            .Sum(s => s.MinPedDelay);
                                    break;
                                case MAX_PED_DELAY:
                                    pedAggCount =
                                        pedAggs.Where(s => s.BinStartTime >= bin.Start && s.BinStartTime < bin.End)
                                            .Sum(s => s.MaxPedDelay);
                                    break;
                                case PED_ACTUATIONS:
                                    pedAggCount =
                                        pedAggs.Where(s => s.BinStartTime >= bin.Start && s.BinStartTime < bin.End)
                                            .Sum(s => s.PedActuations);
                                    break;
                                default:

                                    throw new Exception("Unknown Aggregate Data Type for Split Failure");
                            }

                            concurrentBins.Add(new Bin
                            {
                                Start = bin.Start,
                                End = bin.End,
                                Sum = pedAggCount,
                                Average = pedAggCount
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