using System;
using System.Collections.Generic;

namespace MOE.Common.Models.Repositories
{
    public interface IPhaseCycleAggregationRepository
    {
        int GetApproachCycleCountAggregationByApproachIdAndDateRange(int versionId, DateTime start,
            DateTime end);

        PhaseCycleAggregation Add(PhaseCycleAggregation priorityAggregation);
        void Update(PhaseCycleAggregation priorityAggregation);
        void Remove(PhaseCycleAggregation priorityAggregation);
        List<PhaseCycleAggregation> GetApproachCyclesAggregationByApproachIdAndDateRange(int approachId, DateTime start,
            DateTime end);
        double GetAverageRedToRedCyclesBySignalIdPhase(string signalId, int phaseNumber, DateTime start,
            DateTime end);
        List<PhaseCycleAggregation> GetApproachCyclesAggregationBySignalIdPhaseAndDateRange(string signalId, int phase, DateTime start,
            DateTime end);
    }
}