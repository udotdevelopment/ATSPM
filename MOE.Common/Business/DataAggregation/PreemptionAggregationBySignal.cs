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
    public class PreemptionAggregationBySignal
    {
        public Models.Signal Signal { get; }
        public int TotalPreemptions { get { return BinsContainers.Sum(c => c.SumValue); } }
        public List<BinsContainer> BinsContainers { get; private set; }

        public int AveragePreemptions
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
                    return numberOfBins > 0 ? Convert.ToInt32(Math.Round(TotalPreemptions / numberOfBins)) : 0;
                }
            }
        }

        

        public PreemptionAggregationBySignal(SignalPreemptionAggregationOptions options, Models.Signal signal)
        {
            BinsContainers = BinFactory.GetBins(options.TimeOptions); 
            Signal = signal;
            var preemptionAggregationRepository =
                Models.Repositories.PreemptAggregationDatasRepositoryFactory.Create();
            List<PreemptionAggregation> preemptions =
                preemptionAggregationRepository.GetPreemptionsBySignalIdAndDateRange(
                    signal.SignalID, BinsContainers.Min(b => b.Start), BinsContainers.Max(b => b.End));
            if (preemptions != null)
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
                        if (preemptions.Any(s => s.BinStartTime >= bin.Start && s.BinStartTime < bin.End))
                        {
                            int preemptionSum = 0;
                                
                            switch (options.SelectedPreemptionData)
                            {
                                case SignalPreemptionAggregationOptions.PreemptionData.PreemptNumber:
                                    preemptionSum = preemptions.Where(s => s.BinStartTime >= bin.Start && s.BinStartTime < bin.End)
                                        .Sum(s => s.PreemptNumber);
                                    break;
                                case SignalPreemptionAggregationOptions.PreemptionData.PreemptRequests:
                                    preemptionSum = preemptions.Where(s => s.BinStartTime >= bin.Start && s.BinStartTime < bin.End)
                                        .Sum(s => s.PreemptRequests);
                                    break;
                                case SignalPreemptionAggregationOptions.PreemptionData.PreemptServices:
                                    preemptionSum = preemptions.Where(s => s.BinStartTime >= bin.Start && s.BinStartTime < bin.End)
                                        .Sum(s => s.PreemptServices);
                                    break;
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
        

        //private void SetSumSplitFailuresByBin()
        //{
        //    for (int i = 0; i < BinsContainers.Count; i++)
        //    {
        //        for (var binIndex = 0; binIndex < BinsContainers[i].Bins.Count; binIndex++)
        //        {
        //            var bin = BinsContainers[i].Bins[binIndex];
        //            bin.Average = ApproachSplitFailures.Count > 0 ? (bin.Sum / ApproachSplitFailures.Count) :  0;
        //        }
        //    }
        //}
        
    }
    
        
    
        
}