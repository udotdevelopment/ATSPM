using System;
using System.Collections.Generic;
using System.Linq;
using MOE.Common.Business.WCFServiceLibrary;
using MOE.Common.Models;

namespace MOE.Common.Business.DataAggregation
{
    public class EventCountAggregationBySignal : AggregationBySignal
    {
        public EventCountAggregationBySignal(ApproachEventCountAggregationOptions options, Models.Signal signal) : base(
            options, signal)
        {
            ApproachEventCountures = new List<EventCountAggregationByApproach>();
            GetApproachEventCountAggregationContainersForAllApporaches(options, signal);
            LoadBins(null, null);
        }

        public EventCountAggregationBySignal(ApproachEventCountAggregationOptions options, Models.Signal signal,
            int phaseNumber) : base(options, signal)
        {
            ApproachEventCountures = new List<EventCountAggregationByApproach>();
            foreach (var approach in signal.Approaches)
                if (approach.ProtectedPhaseNumber == phaseNumber)
                {
                    ApproachEventCountures.Add(
                        new EventCountAggregationByApproach(approach, options, options.StartDate,
                            options.EndDate,
                            true, options.SelectedAggregatedDataType));
                    if (approach.PermissivePhaseNumber != null && approach.PermissivePhaseNumber == phaseNumber)
                        ApproachEventCountures.Add(
                            new EventCountAggregationByApproach(approach, options, options.StartDate,
                                options.EndDate,
                                false, options.SelectedAggregatedDataType));
                }
            LoadBins(null, null);
        }

        public EventCountAggregationBySignal(ApproachEventCountAggregationOptions options, Models.Signal signal,
            DirectionType direction) : base(options, signal)
        {
            ApproachEventCountures = new List<EventCountAggregationByApproach>();
            foreach (var approach in signal.Approaches)
                if (approach.DirectionType.DirectionTypeID == direction.DirectionTypeID)
                {
                    ApproachEventCountures.Add(
                        new EventCountAggregationByApproach(approach, options, options.StartDate,
                            options.EndDate,
                            true, options.SelectedAggregatedDataType));
                    if (approach.PermissivePhaseNumber != null)
                        ApproachEventCountures.Add(
                            new EventCountAggregationByApproach(approach, options, options.StartDate,
                                options.EndDate,
                                false, options.SelectedAggregatedDataType));
                }
            LoadBins(null, null);
        }

        public List<EventCountAggregationByApproach> ApproachEventCountures { get; }

        protected override void LoadBins(SignalAggregationMetricOptions options, Models.Signal signal)
        {
            for (var i = 0; i < BinsContainers.Count; i++)
            for (var binIndex = 0; binIndex < BinsContainers[i].Bins.Count; binIndex++)
            {
                var bin = BinsContainers[i].Bins[binIndex];
                foreach (var approachEventCountAggregationContainer in ApproachEventCountures)
                {
                    bin.Sum += approachEventCountAggregationContainer.BinsContainers[i].Bins[binIndex].Sum;
                    bin.Average = ApproachEventCountures.Count > 0 ? bin.Sum / ApproachEventCountures.Count : 0;
                    }
            }
        }

        protected override void LoadBins(ApproachAggregationMetricOptions options, Models.Signal signal)
        {
            for (var i = 0; i < BinsContainers.Count; i++)
            for (var binIndex = 0; binIndex < BinsContainers[i].Bins.Count; binIndex++)
            {
                var bin = BinsContainers[i].Bins[binIndex];
                foreach (var approachEventCountAggregationContainer in ApproachEventCountures)
                {
                    bin.Sum += approachEventCountAggregationContainer.BinsContainers[i].Bins[binIndex].Sum;
                    bin.Average = ApproachEventCountures.Count > 0 ? bin.Sum / ApproachEventCountures.Count : 0;
                }
            }
        }

        private void GetApproachEventCountAggregationContainersForAllApporaches(
            ApproachEventCountAggregationOptions options, Models.Signal signal)
        {
            foreach (var approach in signal.Approaches)
            {
                ApproachEventCountures.Add(
                    new EventCountAggregationByApproach(approach, options, options.StartDate,
                        options.EndDate,
                        true, options.SelectedAggregatedDataType));
                if (approach.PermissivePhaseNumber != null)
                    ApproachEventCountures.Add(
                        new EventCountAggregationByApproach(approach, options, options.StartDate,
                            options.EndDate,
                            false, options.SelectedAggregatedDataType));
            }
        }


        public int GetEventCountsByDirection(DirectionType direction)
        {
            var splitFails = 0;
            if (ApproachEventCountures != null)
                splitFails = ApproachEventCountures
                    .Where(a => a.Approach.DirectionType.DirectionTypeID == direction.DirectionTypeID)
                    .Sum(a => a.BinsContainers.FirstOrDefault().SumValue);
            return splitFails;
        }

        public int GetAverageEventCountsByDirection(DirectionType direction)
        {
            var approachEventCounturesByDirection = ApproachEventCountures
                .Where(a => a.Approach.DirectionType.DirectionTypeID == direction.DirectionTypeID);
            var splitFails = 0;
            if (approachEventCounturesByDirection.Any())
                splitFails = Convert.ToInt32(Math.Round(approachEventCounturesByDirection
                    .Average(a => a.BinsContainers.FirstOrDefault().SumValue)));
            return splitFails;
        }
    }
}