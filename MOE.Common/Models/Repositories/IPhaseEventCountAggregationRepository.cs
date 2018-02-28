using System;
using System.Collections.Generic;

namespace MOE.Common.Models.Repositories
{
    public interface ISignalEventCountAggregationRepository
    {
        int GetSignalEventCountSumAggregationBySignalIdAndDateRange(string SignalId, DateTime start,
            DateTime end);

        List<SignalEventCountAggregation> GetSignalEventCountAggregationBySignalIdAndDateRange(string SignalId,
            DateTime startDate, DateTime endDate);
    }
}