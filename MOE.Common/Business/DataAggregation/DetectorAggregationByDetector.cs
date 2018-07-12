using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using MOE.Common.Business.Bins;
using MOE.Common.Business.WCFServiceLibrary;
using MOE.Common.Models.Repositories;

namespace MOE.Common.Business.DataAggregation
{
    public class DetectorAggregationByDetector : AggregationByDetector
    {
        public DetectorAggregationByDetector(Models.Detector detector, DetectorVolumeAggregationOptions options) : base(
            detector, options)
        {
        }

        protected override void LoadBins(Models.Detector detector, DetectorAggregationMetricOptions options)
        {
            var detectorAggregationRepository =
                DetectorAggregationsRepositoryFactory.Create();
            var detectorAggregations =
                detectorAggregationRepository.GetDetectorAggregationByApproachIdAndDateRange(
                    detector.ID, options.StartDate, options.EndDate);
            if (detectorAggregations != null)
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
                        if (detectorAggregations.Any(s => s.BinStartTime >= bin.Start && s.BinStartTime < bin.End))
                        {
                            var volume = 0;
                            switch (options.SelectedAggregatedDataType.DataName)
                            {
                                case "DetectorActivationCount":
                                    volume =
                                        detectorAggregations.Where(s =>
                                                s.BinStartTime >= bin.Start && s.BinStartTime < bin.End)
                                            .Sum(s => s.Volume);
                                    break;
                                default:
                                    throw new Exception("Unknown Aggregate Data Type for Split Failure");
                            }

                            concurrentBins.Add(new Bin
                            {
                                Start = bin.Start,
                                End = bin.End,
                                Sum = volume,
                                Average = volume
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