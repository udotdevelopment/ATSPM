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
        public PcdAggregationByApproach(Approach approach, ApproachPcdAggregationOptions options, DateTime startDate,
            DateTime endDate,
            bool getProtectedPhase, AggregatedDataType dataType) : base(approach, options, startDate, endDate,
            getProtectedPhase, dataType)
        {
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
                    var cycleAggregationRepository = Models.Repositories.ApproachCycleAggregationRepositoryFactory.Create();
                    var cycleAggregtaions =
                        cycleAggregationRepository.GetApproachCyclesAggregationByApproachIdAndDateRange(
                            approach.ApproachID, options.StartDate, options.EndDate, getProtectedPhase);
                    //foreach (var bin in binsContainer.Bins)
                    Parallel.ForEach(binsContainer.Bins, bin =>
                    {
                        if (pcdAggregations.Any(s => s.BinStartTime >= bin.Start && s.BinStartTime < bin.End))
                        {
                            double pcdCount = 0;
                            switch (dataType.DataName)
                            {
                                case "ArrivalsOnGreen":
                                    pcdCount =
                                        pcdAggregations.Where(s => s.BinStartTime >= bin.Start && s.BinStartTime < bin.End)
                                            .Sum(s => s.ArrivalsOnGreen);
                                    break;
                                case "ArrivalsOnRed":
                                    pcdCount =
                                        pcdAggregations.Where(s => s.BinStartTime >= bin.Start && s.BinStartTime < bin.End)
                                            .Sum(s => s.ArrivalsOnRed);
                                    break;
                                case "ArrivalsOnYellow":
                                    pcdCount =
                                        pcdAggregations.Where(s => s.BinStartTime >= bin.Start && s.BinStartTime < bin.End)
                                            .Sum(s => s.ArrivalsOnYellow);
                                    break;
                                case "PercentArrivalsOnGreen":
                                    pcdCount = Convert.ToInt32(Math.Round(GetPercentArrivalOnGreen(bin, pcdAggregations)));
                                    break;
                                case "PlatoonRatio":
                                    double percentArrivalOnGreen = GetPercentArrivalOnGreen(bin, pcdAggregations);
                                    double greenTime = cycleAggregtaions.Where(s => s.BinStartTime >= bin.Start && s.BinStartTime < bin.End).Sum(s => s.GreenTime);
                                    if (greenTime > 0)
                                        pcdCount = percentArrivalOnGreen / greenTime;
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
            double arrivalsOnGreen = pcdAggregations.Where(s => s.BinStartTime >= bin.Start && s.BinStartTime < bin.End)
                .Sum(s => s.ArrivalsOnGreen);
            int totalArrivals = pcdAggregations.Where(s => s.BinStartTime >= bin.Start && s.BinStartTime < bin.End)
                                    .Sum(s => s.ArrivalsOnGreen) +
                                pcdAggregations.Where(s => s.BinStartTime >= bin.Start && s.BinStartTime < bin.End)
                                    .Sum(s => s.ArrivalsOnYellow) +
                                pcdAggregations.Where(s => s.BinStartTime >= bin.Start && s.BinStartTime < bin.End)
                                    .Sum(s => s.ArrivalsOnRed);
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