using MOE.Common.Business.Bins;
using MOE.Common.Business.WCFServiceLibrary;
using MOE.Common.Models;
using MOE.Common.Models.Repositories;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MOE.Common.Business.DataAggregation
{
    public class DetectorAggregationByDetector : AggregationByDetector
    {
        public DetectorAggregationByDetector(Models.Detector detector, DetectorVolumeAggregationOptions options) : base(
            detector, options)
        {
            LoadBins(detector, options);
        }

        public override void LoadBins(Models.Detector detector, DetectorAggregationMetricOptions options)
        {
            var detectorAggregationRepository = DetectorEventCountAggregationRepositoryFactory.Create();
            var detectorAggregations = detectorAggregationRepository.GetDetectorEventCountAggregationByDetectorIdAndDateRange(
                    detector.ID, options.StartDate, options.EndDate);
            BinsContainers = GetBinsContainers(options, detectorAggregations, BinsContainers);
        }

        public static List<BinsContainer> GetBinsContainers(
            DetectorAggregationMetricOptions options,
            List<DetectorEventCountAggregation> detectorAggregations,
            List<BinsContainer> binsContainers
            )
        {
            var concurrentBinContainers = new ConcurrentBag<BinsContainer>();
            if (detectorAggregations != null)
            {
                //foreach (var binsContainer in binsContainers)
                Parallel.ForEach(binsContainers, binsContainer =>
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
                                            .Sum(s => s.EventCount);
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
            }
            return concurrentBinContainers.OrderBy(b => b.Start).ToList();
        }
    }
}