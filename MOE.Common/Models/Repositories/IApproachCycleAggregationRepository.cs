using System;
using System.Collections.Generic;

namespace MOE.Common.Models.Repositories
{
    public interface IApproachCycleAggregationRepository
    {
        int GetApproachCycleCountAggregationByApproachIdAndDateRange(int versionId, DateTime start,
            DateTime end);

        PhaseCycleAggregation Add(PhaseCycleAggregation priorityAggregation);
        void Update(PhaseCycleAggregation priorityAggregation);
        void Remove(PhaseCycleAggregation priorityAggregation);

        List<PhaseCycleAggregation> GetApproachCyclesAggregationByApproachIdAndDateRange(int approachId,
            DateTime startDate, DateTime endDate, bool getProtectedPhase);
    }
}