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

    public class DetectorAggregationByDetector:AggregationByDetector
    {

        protected override void LoadBins(Models.Detector detector, DateTime startDate, DateTime endDate, 
            AggregatedDataType dataType)
        {
            var detectorAggregationRepository =
               Models.Repositories.DetectorAggregationsRepositoryFactory.Create();
            List<DetectorAggregation> detectorAggregations =
                detectorAggregationRepository.GetDetectorAggregationByApproachIdAndDateRange(
                    detector.ID, startDate, endDate);
            if (detectorAggregations != null)
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
                        if (detectorAggregations.Any(s => s.BinStartTime >= bin.Start && s.BinStartTime < bin.End))
                        {
                            int volume = 0;
                            switch (dataType.DataName)
                            {
                                case "Volume":
                                    volume =
                                       detectorAggregations.Where(s => s.BinStartTime >= bin.Start && s.BinStartTime < bin.End)
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

        public DetectorAggregationByDetector(Models.Detector detector, BinFactoryOptions timeOptions, DateTime startDate, DateTime endDate, 
             AggregatedDataType dataType) :base(detector, timeOptions,startDate, endDate, dataType)
        {
        }
    }
}
