using System;
using System.Collections.Generic;

namespace MOE.Common.Models.Repositories
{
    public interface IApproachEventCountAggregationRepository
    {
        int GetPhaseEventCountSumAggregationByPhaseIdAndDateRange(int approachId, DateTime start,
            DateTime end, bool getProtectedPhase);

        List<ApproachEventCountAggregation> GetPhaseEventCountAggregationByPhaseIdAndDateRange(int approachId, DateTime start,
            DateTime end, bool getProtectedPhase);
    }
}