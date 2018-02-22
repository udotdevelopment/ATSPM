using System;
using System.Collections.Generic;
using System.Linq;
using MOE.Common.Business.WCFServiceLibrary;
using MOE.Common.Models;

namespace MOE.Common.Business.DataAggregation
{
    public class CycleAggregationBySignal : AggregationBySignal
    {
        public CycleAggregationBySignal(ApproachCycleAggregationOptions options, Models.Signal signal) : base(
            options, signal)
        {
            ApproachCycleures = new List<CycleAggregationByApproach>();
            GetApproachCycleAggregationContainersForAllApporaches(options, signal);
            LoadBins(null, null);
        }

        public CycleAggregationBySignal(ApproachCycleAggregationOptions options, Models.Signal signal,
            int phaseNumber) : base(options, signal)
        {
            ApproachCycleures = new List<CycleAggregationByApproach>();
            foreach (var approach in signal.Approaches)
                if (approach.ProtectedPhaseNumber == phaseNumber)
                {
                    ApproachCycleures.Add(
                        new CycleAggregationByApproach(approach, options.TimeOptions, options.StartDate,
                            options.EndDate,
                            true, options.SelectedAggregatedDataType));
                    if (approach.PermissivePhaseNumber != null && approach.PermissivePhaseNumber == phaseNumber)
                        ApproachCycleures.Add(
                            new CycleAggregationByApproach(approach, options.TimeOptions, options.StartDate,
                                options.EndDate,
                                false, options.SelectedAggregatedDataType));
                }
            LoadBins(null, null);
        }

        public CycleAggregationBySignal(ApproachCycleAggregationOptions options, Models.Signal signal,
            DirectionType direction) : base(options, signal)
        {
            ApproachCycleures = new List<CycleAggregationByApproach>();
            foreach (var approach in signal.Approaches)
                if (approach.DirectionType.DirectionTypeID == direction.DirectionTypeID)
                {
                    ApproachCycleures.Add(
                        new CycleAggregationByApproach(approach, options.TimeOptions, options.StartDate,
                            options.EndDate,
                            true, options.SelectedAggregatedDataType));
                    if (approach.PermissivePhaseNumber != null)
                        ApproachCycleures.Add(
                            new CycleAggregationByApproach(approach, options.TimeOptions, options.StartDate,
                                options.EndDate,
                                false, options.SelectedAggregatedDataType));
                }
            LoadBins(null, null);
        }

        public List<CycleAggregationByApproach> ApproachCycleures { get; }

        protected override void LoadBins(SignalAggregationMetricOptions options, Models.Signal signal)
        {
            for (var i = 0; i < BinsContainers.Count; i++)
            for (var binIndex = 0; binIndex < BinsContainers[i].Bins.Count; binIndex++)
            {
                var bin = BinsContainers[i].Bins[binIndex];
                foreach (var approachCycleAggregationContainer in ApproachCycleures)
                    bin.Sum += approachCycleAggregationContainer.BinsContainers[i].Bins[binIndex].Sum;
                bin.Average = ApproachCycleures.Count > 0 ? bin.Sum / ApproachCycleures.Count : 0;
            }
        }

        private void GetApproachCycleAggregationContainersForAllApporaches(
            ApproachCycleAggregationOptions options, Models.Signal signal)
        {
            foreach (var approach in signal.Approaches)
            {
                ApproachCycleures.Add(
                    new CycleAggregationByApproach(approach, options.TimeOptions, options.StartDate,
                        options.EndDate,
                        true, options.SelectedAggregatedDataType));
                if (approach.PermissivePhaseNumber != null)
                    ApproachCycleures.Add(
                        new CycleAggregationByApproach(approach, options.TimeOptions, options.StartDate,
                            options.EndDate,
                            false, options.SelectedAggregatedDataType));
            }
        }


        public int GetCyclesByDirection(DirectionType direction)
        {
            var splitFails = 0;
            if (ApproachCycleures != null)
                splitFails = ApproachCycleures
                    .Where(a => a.Approach.DirectionType.DirectionTypeID == direction.DirectionTypeID)
                    .Sum(a => a.BinsContainers.FirstOrDefault().SumValue);
            return splitFails;
        }

        public int GetAverageCyclesByDirection(DirectionType direction)
        {
            var approachCycleuresByDirection = ApproachCycleures
                .Where(a => a.Approach.DirectionType.DirectionTypeID == direction.DirectionTypeID);
            var splitFails = 0;
            if (approachCycleuresByDirection.Any())
                splitFails = Convert.ToInt32(Math.Round(approachCycleuresByDirection
                    .Average(a => a.BinsContainers.FirstOrDefault().SumValue)));
            return splitFails;
        }
    }
}