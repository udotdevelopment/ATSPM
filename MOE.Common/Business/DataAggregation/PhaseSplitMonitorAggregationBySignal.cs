using System;
using System.Collections.Generic;
using System.Linq;
using MOE.Common.Business.WCFServiceLibrary;
using MOE.Common.Models;

namespace MOE.Common.Business.DataAggregation
{
    public class PhaseSplitMonitorAggregationBySignal : AggregationBySignal
    {

        public List<PhaseSplitMonitorAggregationByPhase> SplitMonitorAggregations { get; }
        public PhaseSplitMonitorAggregationBySignal(PhaseSplitMonitorAggregationOptions options, Models.Signal signal) : base(
            options, signal)
        {
            SplitMonitorAggregations = new List<PhaseSplitMonitorAggregationByPhase>();
            GetSplitMonitorAggregationContainersForAllPhases(options, signal);
            LoadBins(null, null);
        }

        public PhaseSplitMonitorAggregationBySignal(PhaseSplitMonitorAggregationOptions options, Models.Signal signal,
            int phaseNumber) : base(options, signal)
        {
            SplitMonitorAggregations = new List<PhaseSplitMonitorAggregationByPhase>();
            SplitMonitorAggregations.Add(new PhaseSplitMonitorAggregationByPhase(signal, phaseNumber, options, options.SelectedAggregatedDataType));
            LoadBins(null, null);
        }

        protected override void LoadBins(SignalAggregationMetricOptions options, Models.Signal signal)
        {
            for (var i = 0; i < BinsContainers.Count; i++)
            for (var binIndex = 0; binIndex < BinsContainers[i].Bins.Count; binIndex++)
            {
                var bin = BinsContainers[i].Bins[binIndex];
                foreach (var approachSplitFailAggregationContainer in SplitMonitorAggregations)
                {
                    bin.Sum += approachSplitFailAggregationContainer.BinsContainers[i].Bins[binIndex].Sum;
                    bin.Average = SplitMonitorAggregations.Count > 0 ? bin.Sum / SplitMonitorAggregations.Count : 0;
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
                    foreach (var approachSplitFailAggregationContainer in SplitMonitorAggregations)
                        bin.Sum += approachSplitFailAggregationContainer.BinsContainers[i].Bins[binIndex].Sum;
                    bin.Average = SplitMonitorAggregations.Count > 0 ? bin.Sum / SplitMonitorAggregations.Count : 0;
                }
            }
        }

        private void GetSplitMonitorAggregationContainersForAllPhases(
            PhaseSplitMonitorAggregationOptions options, Models.Signal signal)
        {
            List<int> availablePhases = GetAvailablePhasesForSignal(options, signal);
            foreach (var phaseNumber in availablePhases)
            {
                SplitMonitorAggregations.Add(
                    new PhaseSplitMonitorAggregationByPhase(signal, phaseNumber, options,
                        options.SelectedAggregatedDataType));
            }
        }

        private static List<int> GetAvailablePhasesForSignal(PhaseSplitMonitorAggregationOptions options, Models.Signal signal)
        {
            var phaseTerminationAggregationRepository =
                Models.Repositories.PhaseSplitMonitorAggregationRepositoryFactory.Create();
            var availablePhases =
                phaseTerminationAggregationRepository.GetAvailablePhaseNumbers(signal, options.StartDate,
                    options.EndDate);
            return availablePhases;
        }
        
    }
}