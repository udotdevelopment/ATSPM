using System;
using System.Collections.Generic;
using System.Linq;
using MOE.Common.Business.Bins;
using MOE.Common.Business.WCFServiceLibrary;
using MOE.Common.Models;

namespace MOE.Common.Business.DataAggregation
{
    public class SplitFailAggregationBySignal : AggregationBySignal
    {
        public SplitFailAggregationBySignal(ApproachSplitFailAggregationOptions options, Models.Signal signal) : base(
            options, signal)
        {
            ApproachSplitFailures = new List<SplitFailAggregationByApproach>();
            GetApproachSplitFailAggregationContainersForAllApporaches(options, signal);
            LoadBins(null, null);
        }

        public SplitFailAggregationBySignal(ApproachSplitFailAggregationOptions options, Models.Signal signal,
            int phaseNumber) : base(options, signal)
        {
            ApproachSplitFailures = new List<SplitFailAggregationByApproach>();
            foreach (var approach in signal.Approaches)
                if (approach.ProtectedPhaseNumber == phaseNumber)
                {
                    ApproachSplitFailures.Add(
                        new SplitFailAggregationByApproach(approach, options, options.StartDate,
                            options.EndDate,
                            true, options.SelectedAggregatedDataType));
                    if (approach.PermissivePhaseNumber != null && approach.PermissivePhaseNumber == phaseNumber)
                        ApproachSplitFailures.Add(
                            new SplitFailAggregationByApproach(approach, options, options.StartDate,
                                options.EndDate,
                                false, options.SelectedAggregatedDataType));
                }
            LoadBins(null, null);
        }

        public SplitFailAggregationBySignal(ApproachSplitFailAggregationOptions options, Models.Signal signal,
            DirectionType direction) : base(options, signal)
        {
            ApproachSplitFailures = new List<SplitFailAggregationByApproach>();
            foreach (var approach in signal.Approaches)
                if (approach.DirectionType.DirectionTypeID == direction.DirectionTypeID)
                {
                    ApproachSplitFailures.Add(
                        new SplitFailAggregationByApproach(approach, options, options.StartDate,
                            options.EndDate,
                            true, options.SelectedAggregatedDataType));
                    if (approach.PermissivePhaseNumber != null)
                        ApproachSplitFailures.Add(
                            new SplitFailAggregationByApproach(approach, options, options.StartDate,
                                options.EndDate,
                                false, options.SelectedAggregatedDataType));
                }
            LoadBins(null, null);
        }

        public List<SplitFailAggregationByApproach> ApproachSplitFailures { get; }

        protected override void LoadBins(SignalAggregationMetricOptions options, Models.Signal signal)
        {
            for (var i = 0; i < BinsContainers.Count; i++)
            for (var binIndex = 0; binIndex < BinsContainers[i].Bins.Count; binIndex++)
            {
                var bin = BinsContainers[i].Bins[binIndex];
                foreach (var approachSplitFailAggregationContainer in ApproachSplitFailures)
                {
                    bin.Sum += approachSplitFailAggregationContainer.BinsContainers[i].Bins[binIndex].Sum;
                    bin.Average = ApproachSplitFailures.Count > 0 ? bin.Sum / ApproachSplitFailures.Count : 0;
                    }
            }
        }

        protected override void LoadBins(ApproachAggregationMetricOptions options, Models.Signal signal)
        {
            for (var i = 0; i < BinsContainers.Count; i++)
            {
                for (var binIndex = 0; binIndex < BinsContainers[i].Bins.Count; binIndex++)
                {
                    var bin = BinsContainers[i].Bins[binIndex];
                    foreach (var approachSplitFailAggregationContainer in ApproachSplitFailures)
                        if(approachSplitFailAggregationContainer.BinsContainers.Count > 0 )
                            bin.Sum += approachSplitFailAggregationContainer.BinsContainers[i].Bins[binIndex].Sum;
                    bin.Average = ApproachSplitFailures.Count > 0 ? bin.Sum / ApproachSplitFailures.Count : 0;
                }
            }
        }

        private void GetApproachSplitFailAggregationContainersForAllApporaches(
            ApproachSplitFailAggregationOptions options, Models.Signal signal)
        {
            foreach (var approach in signal.Approaches)
            {
                ApproachSplitFailures.Add(
                    new SplitFailAggregationByApproach(approach, options, options.TimeOptions.Start,
                        options.TimeOptions.End,
                        true, options.SelectedAggregatedDataType));
                if (approach.PermissivePhaseNumber != null)
                    ApproachSplitFailures.Add(
                        new SplitFailAggregationByApproach(approach, options, options.StartDate,
                            options.EndDate,
                            false, options.SelectedAggregatedDataType));
            }
        }

        public double GetSplitFailsByDirection(DirectionType direction)
        {
            double splitFails = 0;
            if (ApproachSplitFailures != null)
                splitFails = ApproachSplitFailures
                    .Where(a => a.Approach.DirectionType.DirectionTypeID == direction.DirectionTypeID)
                    .Sum(a => a.BinsContainers.FirstOrDefault().SumValue);
            return splitFails;
        }

        public int GetAverageSplitFailsByDirection(DirectionType direction)
        {
            var approachSplitFailuresByDirection = ApproachSplitFailures
                .Where(a => a.Approach.DirectionType.DirectionTypeID == direction.DirectionTypeID);
            var splitFails = 0;
            if (approachSplitFailuresByDirection.Any())
                splitFails = Convert.ToInt32(Math.Round(approachSplitFailuresByDirection
                    .Average(a => a.BinsContainers.FirstOrDefault().SumValue)));
            return splitFails;
        }
    }
}