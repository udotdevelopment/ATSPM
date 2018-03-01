using System;
using System.Collections.Generic;

namespace MOE.Common.Models.Repositories
{
    public interface IPhaseEventCountAggregationRepository
    {
        int GetPhaseEventCountSumAggregationByPhaseIdAndDateRange(int approachId, DateTime start,
            DateTime end, bool getProtectedPhase);

        List<PhaseEventCountAggregation> GetPhaseEventCountAggregationByPhaseIdAndDateRange(int approachId, DateTime start,
            DateTime end, bool getProtectedPhase);
    }
}