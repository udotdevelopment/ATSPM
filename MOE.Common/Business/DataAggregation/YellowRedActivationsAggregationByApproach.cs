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
    public class YellowRedActivationsAggregationByApproach : AggregationByApproach
    {
        public YellowRedActivationsAggregationByApproach(Approach approach, ApproachYellowRedActivationsAggregationOptions options,
            DateTime startDate,
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
            var yellowRedActivationAggregationRepository =
                ApproachYellowRedActivationsAggregationRepositoryFactory.Create();
            var yellowRedActivations =
                yellowRedActivationAggregationRepository
                    .GetApproachYellowRedActivationssAggregationByApproachIdAndDateRange(
                        approach.ApproachID, options.StartDate, options.EndDate, getProtectedPhase);
            if (yellowRedActivations != null)
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
                        if (yellowRedActivations.Any(s => s.BinStartTime >= bin.Start && s.BinStartTime < bin.End))
                        {
                            var yellowRedActivationCount = 0;
                            switch (dataType.DataName)
                            {
                                case "SevereRedLightViolations":
                                    yellowRedActivationCount =
                                        yellowRedActivations.Where(s =>
                                                s.BinStartTime >= bin.Start && s.BinStartTime < bin.End)
                                            .Sum(s => s.SevereRedLightViolations);
                                    break;
                                case "TotalRedLightViolations":
                                    yellowRedActivationCount =
                                        yellowRedActivations.Where(s =>
                                                s.BinStartTime >= bin.Start && s.BinStartTime < bin.End)
                                            .Sum(s => s.TotalRedLightViolations);
                                    break;
                                case "YellowActivations":
                                    yellowRedActivationCount =
                                        yellowRedActivations.Where(s =>
                                                s.BinStartTime >= bin.Start && s.BinStartTime < bin.End)
                                            .Sum(s => s.YellowActivations);
                                    break;
                                case "ViolationTime":
                                    yellowRedActivationCount =
                                        yellowRedActivations.Where(s =>
                                                s.BinStartTime >= bin.Start && s.BinStartTime < bin.End)
                                            .Sum(s => s.ViolationTime);
                                    break;
                                case "Cycles":
                                    yellowRedActivationCount =
                                        yellowRedActivations.Where(s =>
                                                s.BinStartTime >= bin.Start && s.BinStartTime < bin.End)
                                            .Sum(s => s.Cycles);
                                    break;
                                default:

                                    throw new Exception("Unknown Aggregate Data Type for Yellow Red Activation");
                            }

                            concurrentBins.Add(new Bin
                            {
                                Start = bin.Start,
                                End = bin.End,
                                Sum = yellowRedActivationCount,
                                Average = yellowRedActivationCount
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