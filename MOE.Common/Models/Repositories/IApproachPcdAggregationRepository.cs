using System;
using System.Collections.Generic;

namespace MOE.Common.Models.Repositories
{
    public interface IApproachPcdAggregationRepository: IAggregationRepositoryBase
    {
        int GetApproachPcdCountAggregationByApproachIdAndDateRange(int versionId, DateTime start,
            DateTime end);

        ApproachPcdAggregation Add(ApproachPcdAggregation priorityAggregation);
        void Update(ApproachPcdAggregation priorityAggregation);
        void Remove(ApproachPcdAggregation priorityAggregation);

        List<ApproachPcdAggregation> GetApproachPcdsAggregationByApproachIdAndDateRange(int approachId,
            DateTime startDate, DateTime endDate, bool getProtectedPhase);
    }
}