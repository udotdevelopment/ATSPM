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
    public class SpeedAggregationByApproach : AggregationByApproach
    {
        public SpeedAggregationByApproach(Approach approach, BinFactoryOptions timeOptions, DateTime startDate,
            DateTime endDate,
            bool getProtectedPhase, AggregatedDataType dataType) : base(approach, timeOptions, startDate, endDate,
            getProtectedPhase, dataType)
        {
        }

        protected override void LoadBins(Approach approach, DateTime startDate, DateTime endDate,
            bool getProtectedPhase,
            AggregatedDataType dataType)
        {
            var approachSpeedAggregationRepository =
                ApproachSpeedAggregationRepositoryFactory.Create();
            var speedAggregations =
                approachSpeedAggregationRepository.GetSpeedsByApproachIDandDateRange(approach.ApproachID, startDate, endDate);
            if (speedAggregations != null)
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
                        if (speedAggregations.Any(s => s.BinStartTime >= bin.Start && s.BinStartTime < bin.End))
                        {
                            double speedAggregationCount = 0;
                            switch (dataType.DataName)
                            {
                                case "SummedSpeed":
                                    speedAggregationCount =
                                        speedAggregations.Where(s => s.BinStartTime >= bin.Start && s.BinStartTime < bin.End)
                                            .Sum(s => s.SummedSpeed);
                                    break;
                                case "SpeedVolume":
                                    speedAggregationCount =
                                        speedAggregations.Where(s => s.BinStartTime >= bin.Start && s.BinStartTime < bin.End)
                                            .Sum(s => s.SpeedVolume);
                                    break;
                                case "Speed85Th":
                                    speedAggregationCount =
                                        speedAggregations.Where(s => s.BinStartTime >= bin.Start && s.BinStartTime < bin.End)
                                            .Sum(s => s.Speed85Th);
                                    break;
                                case "Speed15Th":
                                    speedAggregationCount =
                                        speedAggregations.Where(s => s.BinStartTime >= bin.Start && s.BinStartTime < bin.End)
                                            .Sum(s => s.Speed15Th);
                                    break;
                                default:

                                    throw new Exception("Unknown Aggregate Data Type for Split Failure");
                            }

                            concurrentBins.Add(new Bin
                            {
                                Start = bin.Start,
                                End = bin.End,
                                Sum = Convert.ToInt32(Math.Round(speedAggregationCount)),
                                Average = speedAggregationCount
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