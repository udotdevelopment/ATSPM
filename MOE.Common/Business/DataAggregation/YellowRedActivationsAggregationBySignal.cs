using System;
using System.Collections.Generic;
using System.Linq;
using MOE.Common.Business.WCFServiceLibrary;
using MOE.Common.Models;

namespace MOE.Common.Business.DataAggregation
{
    public class YellowRedActivationsAggregationBySignal : AggregationBySignal
    {
        public YellowRedActivationsAggregationBySignal(ApproachYellowRedActivationsAggregationOptions options,
            Models.Signal signal) : base(
            options, signal)
        {
            ApproachYellowRedActivationsures = new List<YellowRedActivationsAggregationByApproach>();
            GetApproachYellowRedActivationsAggregationContainersForAllApporaches(options, signal);
            LoadBins(null, null);
        }

        public YellowRedActivationsAggregationBySignal(ApproachYellowRedActivationsAggregationOptions options,
            Models.Signal signal,
            int phaseNumber) : base(options, signal)
        {
            ApproachYellowRedActivationsures = new List<YellowRedActivationsAggregationByApproach>();
            foreach (var approach in signal.Approaches)
                if (approach.ProtectedPhaseNumber == phaseNumber)
                {
                    ApproachYellowRedActivationsures.Add(
                        new YellowRedActivationsAggregationByApproach(approach, options, options.StartDate,
                            options.EndDate,
                            true, options.SelectedAggregatedDataType));
                    if (approach.PermissivePhaseNumber != null && approach.PermissivePhaseNumber == phaseNumber)
                        ApproachYellowRedActivationsures.Add(
                            new YellowRedActivationsAggregationByApproach(approach, options,
                                options.StartDate,
                                options.EndDate,
                                false, options.SelectedAggregatedDataType));
                }
            LoadBins(null, null);
        }

        public YellowRedActivationsAggregationBySignal(ApproachYellowRedActivationsAggregationOptions options,
            Models.Signal signal,
            DirectionType direction) : base(options, signal)
        {
            ApproachYellowRedActivationsures = new List<YellowRedActivationsAggregationByApproach>();
            foreach (var approach in signal.Approaches)
                if (approach.DirectionType.DirectionTypeID == direction.DirectionTypeID)
                {
                    ApproachYellowRedActivationsures.Add(
                        new YellowRedActivationsAggregationByApproach(approach, options, options.StartDate,
                            options.EndDate,
                            true, options.SelectedAggregatedDataType));
                    if (approach.PermissivePhaseNumber != null)
                        ApproachYellowRedActivationsures.Add(
                            new YellowRedActivationsAggregationByApproach(approach, options,
                                options.StartDate,
                                options.EndDate,
                                false, options.SelectedAggregatedDataType));
                }
            LoadBins(null, null);
        }

        public List<YellowRedActivationsAggregationByApproach> ApproachYellowRedActivationsures { get; }

        protected override void LoadBins(SignalAggregationMetricOptions options, Models.Signal signal)
        {
            for (var i = 0; i < BinsContainers.Count; i++)
            for (var binIndex = 0; binIndex < BinsContainers[i].Bins.Count; binIndex++)
            {
                var bin = BinsContainers[i].Bins[binIndex];
                foreach (var approachYellowRedActivationsAggregationContainer in ApproachYellowRedActivationsures)
                    bin.Sum += approachYellowRedActivationsAggregationContainer.BinsContainers[i].Bins[binIndex].Sum;
                bin.Average = ApproachYellowRedActivationsures.Count > 0
                    ? bin.Sum / ApproachYellowRedActivationsures.Count
                    : 0;
            }
        }

        protected override void LoadBins(ApproachAggregationMetricOptions options, Models.Signal signal)
        {
            for (var i = 0; i < BinsContainers.Count; i++)
            for (var binIndex = 0; binIndex < BinsContainers[i].Bins.Count; binIndex++)
            {
                var bin = BinsContainers[i].Bins[binIndex];
                foreach (var approachYellowRedActivationsAggregationContainer in ApproachYellowRedActivationsures)
                    bin.Sum += approachYellowRedActivationsAggregationContainer.BinsContainers[i].Bins[binIndex].Sum;
                bin.Average = ApproachYellowRedActivationsures.Count > 0
                    ? bin.Sum / ApproachYellowRedActivationsures.Count
                    : 0;
            }
        }

        private void GetApproachYellowRedActivationsAggregationContainersForAllApporaches(
            ApproachYellowRedActivationsAggregationOptions options, Models.Signal signal)
        {
            foreach (var approach in signal.Approaches)
            {
                ApproachYellowRedActivationsures.Add(
                    new YellowRedActivationsAggregationByApproach(approach, options, options.StartDate,
                        options.EndDate,
                        true, options.SelectedAggregatedDataType));
                if (approach.PermissivePhaseNumber != null)
                    ApproachYellowRedActivationsures.Add(
                        new YellowRedActivationsAggregationByApproach(approach, options, options.StartDate,
                            options.EndDate,
                            false, options.SelectedAggregatedDataType));
            }
        }


        public double GetYellowRedActivationssByDirection(DirectionType direction)
        {
            double splitFails = 0;
            if (ApproachYellowRedActivationsures != null)
                splitFails = ApproachYellowRedActivationsures
                    .Where(a => a.Approach.DirectionType.DirectionTypeID == direction.DirectionTypeID)
                    .Sum(a => a.BinsContainers.FirstOrDefault().SumValue);
            return splitFails;
        }

        public int GetAverageYellowRedActivationssByDirection(DirectionType direction)
        {
            var approachYellowRedActivationsuresByDirection = ApproachYellowRedActivationsures
                .Where(a => a.Approach.DirectionType.DirectionTypeID == direction.DirectionTypeID);
            var splitFails = 0;
            if (approachYellowRedActivationsuresByDirection.Any())
                splitFails = Convert.ToInt32(Math.Round(approachYellowRedActivationsuresByDirection
                    .Average(a => a.BinsContainers.FirstOrDefault().SumValue)));
            return splitFails;
        }
    }
}