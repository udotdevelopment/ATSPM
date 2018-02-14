using System;
using System.Collections.Generic;
using System.Linq;
using MOE.Common.Business.Bins;
using MOE.Common.Business.WCFServiceLibrary;
using MOE.Common.Models;

namespace MOE.Common.Business.DataAggregation
{
    public class SplitFailAggregationBySignal
    {
        public Models.Signal Signal { get; }
        public List<ApproachSplitFailAggregationContainer> ApproachSplitFailures { get;}
        public int TotalSplitFailures { get { return BinsContainers.Sum(c => c.SumValue); } }

        public int AverageSplitFailures
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
                    return numberOfBins > 0 ? Convert.ToInt32(Math.Round(TotalSplitFailures / numberOfBins)) : 0;
                }
            }
        }

        public List<BinsContainer> BinsContainers{get; private set;}
        

        public SplitFailAggregationBySignal(ApproachSplitFailAggregationOptions options, Models.Signal signal)
        {
            BinsContainers = BinFactory.GetBins(options.TimeOptions); 
            Signal = signal;
            ApproachSplitFailures = new List<ApproachSplitFailAggregationContainer>();
            GetApproachSplitFailAggregationContainersForAllApporaches(options, signal);
            SetSumSplitFailuresByBin();
        }

        private void GetApproachSplitFailAggregationContainersForAllApporaches(ApproachSplitFailAggregationOptions options, Models.Signal signal)
        {
            foreach (var approach in signal.Approaches)
            {
                ApproachSplitFailures.Add(
                       new ApproachSplitFailAggregationContainer(approach, options.TimeOptions, options.StartDate, options.EndDate,
                           true, options.SelectedAggregatedDataType));
                if (approach.PermissivePhaseNumber != null)
                {
                    ApproachSplitFailures.Add(
                        new ApproachSplitFailAggregationContainer(approach, options.TimeOptions, options.StartDate, options.EndDate,
                            false, options.SelectedAggregatedDataType));
                }
            }
        }

        public SplitFailAggregationBySignal(ApproachSplitFailAggregationOptions options, Models.Signal signal, int phaseNumber)
        {
            BinsContainers =  BinFactory.GetBins(options.TimeOptions); 
            Signal = signal;
            ApproachSplitFailures = new List<ApproachSplitFailAggregationContainer>();
            foreach (var approach in signal.Approaches)
            {
                if (approach.ProtectedPhaseNumber == phaseNumber)
                {
                    ApproachSplitFailures.Add(
                        new ApproachSplitFailAggregationContainer(approach, options.TimeOptions, options.StartDate, options.EndDate,
                            true, options.SelectedAggregatedDataType));
                    if (approach.PermissivePhaseNumber != null && approach.PermissivePhaseNumber == phaseNumber)
                    {
                        ApproachSplitFailures.Add(
                            new ApproachSplitFailAggregationContainer(approach, options.TimeOptions, options.StartDate, options.EndDate,
                                false, options.SelectedAggregatedDataType));
                    }
                }
            }
            SetSumSplitFailuresByBin();
        }

        public SplitFailAggregationBySignal(ApproachSplitFailAggregationOptions options, Models.Signal signal, DirectionType direction)
        {
            BinsContainers = BinFactory.GetBins(options.TimeOptions); 
            Signal = signal;
            ApproachSplitFailures = new List<ApproachSplitFailAggregationContainer>();
            foreach (var approach in signal.Approaches)
            {
                if (approach.DirectionType.DirectionTypeID == direction.DirectionTypeID)
                {
                    ApproachSplitFailures.Add(
                        new ApproachSplitFailAggregationContainer(approach, options.TimeOptions, options.StartDate, options.EndDate,
                            true, options.SelectedAggregatedDataType));
                    if (approach.PermissivePhaseNumber != null)
                    {
                        ApproachSplitFailures.Add(
                            new ApproachSplitFailAggregationContainer(approach, options.TimeOptions, options.StartDate, options.EndDate,
                                false, options.SelectedAggregatedDataType));
                    }
                }
            }
            SetSumSplitFailuresByBin();
        }

        private void SetSumSplitFailuresByBin()
        {
            for (int i = 0; i < BinsContainers.Count; i++)
            {
                for (var binIndex = 0; binIndex < BinsContainers[i].Bins.Count; binIndex++)
                {
                    var bin = BinsContainers[i].Bins[binIndex];
                    foreach (var approachSplitFailAggregationContainer in ApproachSplitFailures)
                    {
                        bin.Sum += approachSplitFailAggregationContainer.BinsContainers[i].Bins[binIndex].Sum;
                    }
                    bin.Average = ApproachSplitFailures.Count > 0 ? (bin.Sum / ApproachSplitFailures.Count) :  0;
                }
            }
        }

        public int GetSplitFailsByDirection(DirectionType direction)
        {
            int splitFails = 0;
            if (ApproachSplitFailures != null)
            {
                splitFails = ApproachSplitFailures
                    .Where(a => a.Approach.DirectionType.DirectionTypeID == direction.DirectionTypeID)
                    .Sum(a => a.BinsContainers.FirstOrDefault().SumValue);
            }
            return splitFails;
        }

        public int GetAverageSplitFailsByDirection(DirectionType direction)
        {
            var approachSplitFailuresByDirection = ApproachSplitFailures
                .Where(a => a.Approach.DirectionType.DirectionTypeID == direction.DirectionTypeID);
            int splitFails = 0;
            if (approachSplitFailuresByDirection.Any())
            {
                splitFails = Convert.ToInt32(Math.Round(approachSplitFailuresByDirection
                    .Average(a => a.BinsContainers.FirstOrDefault().SumValue)));
            }
            return splitFails;
        }
    }
    
        
    
        
}