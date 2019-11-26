using System;
using System.Collections.Generic;
using AtspmApi.Models;

namespace AtspmApi.Repositories
{
    public interface ISignalEventCountAggregationRepository
    {
        int GetSignalEventCountAggCountBySignal(string signalId, DateTime start, DateTime end);

        List<SignalEventCountAggregation> GetSignalEventCountAggregationBySignal(string signalId, DateTime start,
            DateTime end );
    }
}