using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MOE.Common.Business.Bins;
using MOE.Common.Business.WCFServiceLibrary;
using MOE.Common.Models;
using MOE.Common.Models.Repositories;

namespace MOE.Common.Business.DataAggregation
{
    public class PcdAggregationByApproach : AggregationByApproach
    {
        public const string VOLUME = "Volume";
        public const string TOTAL_DELAY = "TotalDelay";
        public const string ARRIVALS_ON_YELLOW = "ArrivalsOnYellow";
        public const string ARRIVALS_ON_RED = "ArrivalsOnRed";
        public const string ARRIVALS_ON_GREEN = "ArrivalsOnGreen";

        public PcdAggregationByApproach(Approach approach, ApproachPcdAggregationOptions options, DateTime startDate,
            DateTime endDate,
            bool getProtectedPhase, AggregatedDataType dataType) : base(approach, options, startDate, endDate,
            getProtectedPhase, dataType)
        {
            LoadBins(approach, options, getProtectedPhase, dataType);
        }

        protected override void LoadBins(Approach approach, ApproachAggregationMetricOptions options, 
            bool getProtectedPhase,
            AggregatedDataType dataType)
        {
            var approachPcdAggregationRepository =
                ApproachPcdAggregationRepositoryFactory.Create();
            var pcdAggregations =
                approachPcdAggregationRepository.GetApproachPcdsAggregationByApproachIdAndDateRange(
                    approach.ApproachID, options.StartDate, options.EndDate, getProtectedPhase);
            if (pcdAggregations != null)
            {
                var concurrentBinContainers = new ConcurrentBag<BinsContainer>();
                //foreach (var binsContainer in binsContainers)
                Parallel.ForEach(BinsContainers, binsContainer =>
                {
                    var tempBinsContainer =
                        new BinsContainer(binsContainer.Start, binsContainer.End);
                    var concurrentBins = new ConcurrentBag<Bin>();
                    var cycleAggregationRepository = Models.Repositories.PhaseCycleAggregationsRepositoryFactory.Create();
                    var cycleAggregtaions =
                        cycleAggregationRepository.GetApproachCyclesAggregationByApproachIdAndDateRange(
                            approach.ApproachID, options.StartDate, options.EndDate);
                    //foreach (var bin in binsContainer.Bins)
                    Parallel.ForEach(binsContainer.Bins, bin =>
                    {
                        if (pcdAggregations.Any(s => s.BinStartTime >= bin.Start && s.BinStartTime < bin.End))
                        {
                            double pcdCount = 0;
                            switch (dataType.DataName)
                            {
                                case ARRIVALS_ON_GREEN:
                                    pcdCount =
                                        pcdAggregations.Where(s => s.BinStartTime >= bin.Start && s.BinStartTime < bin.End)
                                            .Sum(s => s.ArrivalsOnGreen);
                                    break;
                                case ARRIVALS_ON_RED:
                                    pcdCount =
                                        pcdAggregations.Where(s => s.BinStartTime >= bin.Start && s.BinStartTime < bin.End)
                                            .Sum(s => s.ArrivalsOnRed);
                                    break;
                                case ARRIVALS_ON_YELLOW:
                                    pcdCount =
                                        pcdAggregations.Where(s => s.BinStartTime >= bin.Start && s.BinStartTime < bin.End)
                                            .Sum(s => s.ArrivalsOnYellow);
                                    break;
                                case TOTAL_DELAY:
                                    pcdCount = pcdAggregations.Where(s => s.BinStartTime >= bin.Start && s.BinStartTime < bin.End).Sum(s => s.TotalDelay);
                                    break;
                                case VOLUME:
                                    pcdCount = pcdAggregations.Where(s => s.BinStartTime >= bin.Start && s.BinStartTime < bin.End).Sum(s => s.Volume);
                                    break;
                                default:
                                    throw new Exception("Unknown Aggregate Data Type for Split Failure");
                            }

                            concurrentBins.Add(new Bin
                            {
                                Start = bin.Start,
                                End = bin.End,
                                Sum = pcdCount,
                                Average = pcdCount
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

        protected double GetPercentArrivalOnGreen(Bin bin, List<ApproachPcdAggregation> pcdAggregations)
        {
            double percentArrivalOnGreen = 0;
            double arrivalsOnGreen = pcdAggregations.Where(s => s.BinStartTime >= bin.Start && s.BinStartTime < bin.End).Sum(s => s.ArrivalsOnGreen);
            int totalArrivals = pcdAggregations.Where(s => s.BinStartTime >= bin.Start && s.BinStartTime < bin.End).Sum(s => s.ArrivalsOnGreen) +
                                pcdAggregations.Where(s => s.BinStartTime >= bin.Start && s.BinStartTime < bin.End).Sum(s => s.ArrivalsOnYellow) +
                                pcdAggregations.Where(s => s.BinStartTime >= bin.Start && s.BinStartTime < bin.End).Sum(s => s.ArrivalsOnRed);
            if (totalArrivals > 0)
            {
                percentArrivalOnGreen = arrivalsOnGreen / totalArrivals;
            }
            else
            {
                percentArrivalOnGreen = 0;
            }
            return percentArrivalOnGreen;
        }

    }
}