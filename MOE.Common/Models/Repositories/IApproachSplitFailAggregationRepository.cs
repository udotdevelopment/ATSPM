using System;
using System.Collections.Generic;

namespace MOE.Common.Models.Repositories
{
    public interface IApproachSplitFailAggregationRepository:IAggregationRepositoryBase
    {
        int GetApproachSplitFailCountAggregationByApproachIdAndDateRange(int versionId, DateTime start,
            DateTime end);

        ApproachSplitFailAggregation Add(ApproachSplitFailAggregation priorityAggregation);
        void Update(ApproachSplitFailAggregation priorityAggregation);
        void Remove(ApproachSplitFailAggregation priorityAggregation);

        List<ApproachSplitFailAggregation> GetApproachSplitFailsAggregationByApproachIdAndDateRange(int approachId,
            DateTime startDate, DateTime endDate, bool getProtectedPhase);
    }
}