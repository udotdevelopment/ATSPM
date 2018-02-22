using System;
using System.Collections.Generic;

namespace MOE.Common.Models.Repositories
{
    public interface IApproachCycleAggregationRepository
    {
        int GetApproachCycleCountAggregationByApproachIdAndDateRange(int versionId, DateTime start,
            DateTime end);
        ApproachCycleAggregation Add(ApproachCycleAggregation priorityAggregation);
        void Update(ApproachCycleAggregation priorityAggregation);
        void Remove(ApproachCycleAggregation priorityAggregation);
        List<ApproachCycleAggregation> GetApproachCyclesAggregationByApproachIdAndDateRange(int approachId, DateTime startDate, DateTime endDate, bool getProtectedPhase);
    }
}