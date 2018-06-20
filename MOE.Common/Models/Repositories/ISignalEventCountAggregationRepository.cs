using System;
using System.Collections.Generic;

namespace MOE.Common.Models.Repositories
{
    public interface ISignalEventCountAggregationRepository
    {
        int GetSignalEventCountSumAggregationBySignalIdAndDateRange(string signalId, DateTime start, DateTime end);

        List<SignalEventCountAggregation> GetSignalEventCountAggregationBySignalIdAndDateRange(string signalId, DateTime start,
            DateTime end );
    }
}