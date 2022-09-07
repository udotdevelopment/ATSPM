using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using MOE.Common.Business.Bins;
using MOE.Common.Business.WCFServiceLibrary;
using MOE.Common.Models.Repositories;

namespace MOE.Common.Business.DataAggregation
{
    public class PreemptionAggregationBySignal : AggregationBySignal
    {
        public PreemptionAggregationBySignal(PreemptionAggregationOptions options, Models.Signal signal) : base(
            options, signal)
        {
            LoadBins(options, signal);
        }

        protected override void LoadBins(SignalAggregationMetricOptions options, Models.Signal signal)
        {
            var preemptionAggregationRepository =
                PreemptAggregationDatasRepositoryFactory.Create();
            var selectionEndDate = BinsContainers.Max(b => b.End);
            //Add a day so that it gets all the data for the entire end day instead of stoping at 12:00AM
            if(options.TimeOptions.SelectedBinSize == BinFactoryOptions.BinSize.Day)
            {
                selectionEndDate = selectionEndDate.AddDays(1);
            }

            var preemptions =
                preemptionAggregationRepository.GetPreemptionsBySignalIdAndDateRange(
                    signal.SignalID, BinsContainers.Min(b => b.Start), selectionEndDate);
            if (preemptions != null)
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
                        if (preemptions.Any(s => s.BinStartTime >= bin.Start && s.BinStartTime < bin.End))
                        {
                            var preemptionSum = 0;

                            switch (options.SelectedAggregatedDataType.DataName)
                            {
                                case "PreemptNumber":
                                    preemptionSum = preemptions.Where(s =>
                                            s.BinStartTime >= bin.Start && s.BinStartTime < bin.End)
                                        .Sum(s => s.PreemptNumber);
                                    break;
                                case "PreemptRequests":
                                    preemptionSum = preemptions.Where(s =>
                                            s.BinStartTime >= bin.Start && s.BinStartTime < bin.End)
                                        .Sum(s => s.PreemptRequests);
                                    break;
                                case "PreemptServices":
                                    preemptionSum = preemptions.Where(s =>
                                            s.BinStartTime >= bin.Start && s.BinStartTime < bin.End)
                                        .Sum(s => s.PreemptServices);
                                    break;
                            }
                            Bin newBin = new Bin
                            {
                                Start = bin.Start,
                                End = bin.End,
                                Sum = preemptionSum,
                                Average = preemptionSum
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

        protected override void LoadBins(ApproachAggregationMetricOptions options, Models.Signal signal)
        {
            throw new System.NotImplementedException();
        }
    }
}