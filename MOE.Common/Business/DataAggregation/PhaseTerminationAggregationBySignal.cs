using System;
using System.Collections.Generic;
using System.Linq;
using MOE.Common.Business.WCFServiceLibrary;
using MOE.Common.Models;

namespace MOE.Common.Business.DataAggregation
{
    public class PhaseTerminationAggregationBySignal : AggregationBySignal
    {

        public List<PhaseTerminationAggregationByPhase> PhaseTerminations { get; }
        public PhaseTerminationAggregationBySignal(PhaseTerminationAggregationOptions options, Models.Signal signal) : base(
            options, signal)
        {
            PhaseTerminations = new List<PhaseTerminationAggregationByPhase>();
            GetPhaseTerminationAggregationContainersForAllPhases(options, signal);
            LoadBins(null, null);
        }

        public PhaseTerminationAggregationBySignal(PhaseTerminationAggregationOptions options, Models.Signal signal,
            int phaseNumber) : base(options, signal)
        {
            PhaseTerminations = new List<PhaseTerminationAggregationByPhase>();
            PhaseTerminations.Add(new PhaseTerminationAggregationByPhase(signal, phaseNumber, options, options.SelectedAggregatedDataType));
            LoadBins(null, null);
        }

        protected override void LoadBins(SignalAggregationMetricOptions options, Models.Signal signal)
        {
            for (var i = 0; i < BinsContainers.Count; i++)
            for (var binIndex = 0; binIndex < BinsContainers[i].Bins.Count; binIndex++)
            {
                var bin = BinsContainers[i].Bins[binIndex];
                foreach (var approachSplitFailAggregationContainer in PhaseTerminations)
                {
                    bin.Sum += approachSplitFailAggregationContainer.BinsContainers[i].Bins[binIndex].Sum;
                    bin.Average = PhaseTerminations.Count > 0 ? bin.Sum / PhaseTerminations.Count : 0;
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
                    foreach (var approachSplitFailAggregationContainer in PhaseTerminations)
                        bin.Sum += approachSplitFailAggregationContainer.BinsContainers[i].Bins[binIndex].Sum;
                    bin.Average = PhaseTerminations.Count > 0 ? bin.Sum / PhaseTerminations.Count : 0;
                }
            }
        }

        private void GetPhaseTerminationAggregationContainersForAllPhases(
            PhaseTerminationAggregationOptions options, Models.Signal signal)
        {
            List<int> availablePhases = GetAvailablePhasesForSignal(options, signal);
            foreach (var phaseNumber in availablePhases)
            {
                PhaseTerminations.Add(
                    new PhaseTerminationAggregationByPhase(signal, phaseNumber, options,
                        options.SelectedAggregatedDataType));
            }
        }

        private static List<int> GetAvailablePhasesForSignal(PhaseTerminationAggregationOptions options, Models.Signal signal)
        {
            var phaseTerminationAggregationRepository =
                Models.Repositories.PhaseTerminationAggregationRepositoryFactory.Create();
            var availablePhases =
                phaseTerminationAggregationRepository.GetAvailablePhaseNumbers(signal, options.StartDate,
                    options.EndDate);
            return availablePhases;
        }
        
    }
}