using System;
using System.Collections.Generic;
using AtspmApi.Models;

namespace AtspmApi.Repositories
{
    public interface IApproachSplitFailAggregationRepository
    {
        int GetApproachSplitFailCountAggByApproach(int versionId, DateTime start,
            DateTime end);

        ApproachSplitFailAggregation Add(ApproachSplitFailAggregation priorityAggregation);
        void Update(ApproachSplitFailAggregation priorityAggregation);
        void Remove(ApproachSplitFailAggregation priorityAggregation);

        List<ApproachSplitFailAggregation> GetApproachSplitFailsAggregationByApproachId(int approachId,
            DateTime startDate, DateTime endDate);
    }
}