using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MOE.Common.Business.Bins;
using MOE.Common.Business.WCFServiceLibrary;
using MOE.Common.Models;

namespace MOE.Common.Business.DataAggregation
{
    public class PriorityAggregationBySignal
    {
        public Models.Signal Signal { get; }
        public int TotalPriorities { get { return BinsContainers.Sum(c => c.SumValue); } }
        public List<BinsContainer> BinsContainers { get; private set; }



        public int AveragePriorities
        {
            get
            {
                if (BinsContainers.Count > 1)
                {
                    return Convert.ToInt32(Math.Round(BinsContainers.Average(b => b.SumValue)));
                }
                else
                {
                    double numberOfBins = 0;
                    foreach (var binsContainer in BinsContainers)
                    {
                        numberOfBins += binsContainer.Bins.Count;
                    }
                    return numberOfBins > 0 ? Convert.ToInt32(Math.Round(TotalPriorities / numberOfBins)) : 0;
                }
            }
        }

        

        public PriorityAggregationBySignal(SignalPriorityAggregationOptions options, Models.Signal signal)
        {
            BinsContainers = BinFactory.GetBins(options.TimeOptions); 
            Signal = signal;
            var priorityAggregationRepository =
                Models.Repositories.PriorityAggregationDatasRepositoryFactory.Create();
            List<PriorityAggregation> priorityAggregations =
                priorityAggregationRepository.GetPriorityBySignalIdAndDateRange(
                    signal.SignalID, BinsContainers.Min(b => b.Start), BinsContainers.Max(b => b.End));
            if (priorityAggregations != null)
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
                        if (priorityAggregations.Any(s => s.BinStartTime >= bin.Start && s.BinStartTime < bin.End))
                        {
                            int preemptionSum = 0;
                                
                            switch (options.SelectedAggregatedDataType)
                            {
                                case SignalPriorityAggregationOptions.AggregatedDataTypes.PriorityNumber:
                                    preemptionSum = priorityAggregations.Where(s => s.BinStartTime >= bin.Start && s.BinStartTime < bin.End)
                                        .Sum(s => s.PriorityNumber);
                                    break;
                                case SignalPriorityAggregationOptions.AggregatedDataTypes.PriorityRequests:
                                    preemptionSum = priorityAggregations.Where(s => s.BinStartTime >= bin.Start && s.BinStartTime < bin.End)
                                        .Sum(s => s.PriorityRequests);
                                    break;
                                case SignalPriorityAggregationOptions.AggregatedDataTypes.PriorityServiceEarlyGreen:
                                    preemptionSum = priorityAggregations.Where(s => s.BinStartTime >= bin.Start && s.BinStartTime < bin.End)
                                        .Sum(s => s.PriorityServiceEarlyGreen);
                                    break;
                                case SignalPriorityAggregationOptions.AggregatedDataTypes.PriorityServiceExtendedGreen:
                                    preemptionSum = priorityAggregations.Where(s => s.BinStartTime >= bin.Start && s.BinStartTime < bin.End)
                                        .Sum(s => s.PriorityServiceEarlyGreen);
                                    break;
                                default:
                                        throw new Exception("Invalid Aggregate Data Type");
                            }
                            concurrentBins.Add(new Bin
                            {
                                Start = bin.Start,
                                End = bin.End,
                                Sum = preemptionSum,
                                Average = preemptionSum
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