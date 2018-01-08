using System;
using System.Collections.Generic;

namespace MOE.Common.Models.Repositories
{
    public interface IApproachSplitFailAggregationRepository
    {
        int GetApproachSplitFailAggregationByVersionIdAndDateRange(int versionId, DateTime start,
            DateTime end);
        ApproachSplitFailAggregation Add(ApproachSplitFailAggregation priorityAggregation);
        void Update(ApproachSplitFailAggregation priorityAggregation);
        void Remove(ApproachSplitFailAggregation priorityAggregation);
    }
}