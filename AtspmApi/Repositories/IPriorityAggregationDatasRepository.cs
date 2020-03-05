using System;
using System.Collections.Generic;
using AtspmApi.Models;

namespace AtspmApi.Repositories
{
    public interface IPriorityAggregationDatasRepository
    {
        //List<PriorityAggregation> GetPriorityAggregationByVersionIdAndDateRange(int versionId, DateTime start,
        //    DateTime end);

        PriorityAggregation Add(PriorityAggregation priorityAggregation);
        void Update(PriorityAggregation priorityAggregation);
        void Remove(PriorityAggregation priorityAggregation);
        int GetPriorityAggCountBySignal(string signalId, DateTime start, DateTime end);
        List<PriorityAggregation> GetPriorityBySignalIdAndDateRange(string signalId, DateTime start, DateTime end);
    }
}