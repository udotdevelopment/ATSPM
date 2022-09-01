using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using MOE.Common.Business.Bins;
using MOE.Common.Business.WCFServiceLibrary;
using MOE.Common.Models.Repositories;

namespace MOE.Common.Business.DataAggregation
{
    public class SignalEventCountAggregationBySignal : AggregationBySignal
    {
        public SignalEventCountAggregationBySignal(SignalEventCountAggregationOptions options, Models.Signal signal) : base(
            options, signal)
        {
            LoadBins(options, signal);
        }

        protected override void LoadBins(SignalAggregationMetricOptions options, Models.Signal signal)
        {
            var signalEventCountAggregationRepository = SignalEventCountAggregationRepositoryFactory
                .Create();
            var selectionEndDate = BinsContainers.Max(b => b.End);
            //Add a day so that it gets all the data for the entire end day instead of stoping at 12:00AM
            if (options.TimeOptions.SelectedBinSize == BinFactoryOptions.BinSize.Day)
            {
                selectionEndDate = selectionEndDate.AddDays(1);
            }
            var signalEventCounts =
                signalEventCountAggregationRepository.GetSignalEventCountAggregationBySignalIdAndDateRange(
                    signal.SignalID, BinsContainers.Min(b => b.Start), selectionEndDate);
            if (signalEventCounts != null)
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
                        if (signalEventCounts.Any(s => s.BinStartTime >= bin.Start && s.BinStartTime < bin.End))
                        {
                            var signalEventCountSum = 0;

                            switch (options.SelectedAggregatedDataType.DataName)
                            {
                                case "EventCount":
                                    signalEventCountSum = signalEventCounts.Where(s =>
                                            s.BinStartTime >= bin.Start && s.BinStartTime < bin.End)
                                        .Sum(s => s.EventCount);
                                    break;
                            }
                            Bin newBin = new Bin
                            {
                                Start = bin.Start,
                                End = bin.End,
                                Sum = signalEventCountSum,
                                Average = signalEventCountSum
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