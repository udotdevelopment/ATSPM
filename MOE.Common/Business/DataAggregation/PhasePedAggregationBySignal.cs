using System;
using System.Collections.Generic;
using System.Linq;
using MOE.Common.Business.WCFServiceLibrary;
using MOE.Common.Models;

namespace MOE.Common.Business.DataAggregation
{
    public class PhasePedAggregationBySignal : AggregationBySignal
    {

        public List<PhasePedAggregationByPhase> PedAggregations { get; }
        public PhasePedAggregationBySignal(PhasePedAggregationOptions options, Models.Signal signal) : base(
            options, signal)
        {
            PedAggregations = new List<PhasePedAggregationByPhase>();
            GetPhasePedAggregationContainersForAllPhases(options, signal);
            LoadBins(null, null);
        }

        public PhasePedAggregationBySignal(PhasePedAggregationOptions options, Models.Signal signal,
            int phaseNumber) : base(options, signal)
        {
            PedAggregations = new List<PhasePedAggregationByPhase>();
            PedAggregations.Add(new PhasePedAggregationByPhase(signal, phaseNumber, options, options.SelectedAggregatedDataType));
            LoadBins(null, null);
        }

        protected override void LoadBins(SignalAggregationMetricOptions options, Models.Signal signal)
        {
            for (var i = 0; i < BinsContainers.Count; i++)
            for (var binIndex = 0; binIndex < BinsContainers[i].Bins.Count; binIndex++)
            {
                var bin = BinsContainers[i].Bins[binIndex];
                foreach (var approachSplitFailAggregationContainer in PedAggregations)
                {
                    bin.Sum += approachSplitFailAggregationContainer.BinsContainers[i].Bins[binIndex].Sum;
                    bin.Average = PedAggregations.Count > 0 ? bin.Sum / PedAggregations.Count : 0;
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
                    foreach (var approachSplitFailAggregationContainer in PedAggregations)
                        bin.Sum += approachSplitFailAggregationContainer.BinsContainers[i].Bins[binIndex].Sum;
                    bin.Average = PedAggregations.Count > 0 ? bin.Sum / PedAggregations.Count : 0;
                }
            }
        }

        private void GetPhasePedAggregationContainersForAllPhases(
            PhasePedAggregationOptions options, Models.Signal signal)
        {
            List<int> availablePhases = GetAvailablePhasesForSignal(options, signal);
            foreach (var phaseNumber in availablePhases)
            {
                PedAggregations.Add(
                    new PhasePedAggregationByPhase(signal, phaseNumber, options, options.SelectedAggregatedDataType));
            }
        }

        private static List<int> GetAvailablePhasesForSignal(PhasePedAggregationOptions options, Models.Signal signal)
        {
            var phaseTerminationAggregationRepository =
                Models.Repositories.PhasePedAggregationRepositoryFactory.Create();
            var availablePhases =
                phaseTerminationAggregationRepository.GetAvailablePhaseNumbers(signal, options.StartDate,
                    options.EndDate);
            return availablePhases;
        }
        
    }
}