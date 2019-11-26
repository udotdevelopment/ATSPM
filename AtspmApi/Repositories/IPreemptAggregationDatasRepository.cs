using System;
using System.Collections.Generic;
using AtspmApi.Models;

namespace AtspmApi.Repositories
{
    public interface IPreemptAggregationDatasRepository
    {
        //List<PreemptionAggregation> GetPreemptAggregationByVersionIdAndDateRange(int versionId, DateTime start,
        //    DateTime end);

        int GetPreemptAggCountBySignal(string signalId, DateTime start, DateTime end);


        //int GetPreemptAggregationTotalByVersionIdPreemptNumberAndDateRange(int versionId, DateTime start,
        //    DateTime end, int preemptNumber);


        PreemptionAggregation Add(PreemptionAggregation preemptionAggregation);
        void Update(PreemptionAggregation preemptionAggregation);
        void Remove(PreemptionAggregation preemptionAggregation);

        List<PreemptionAggregation> GetPreemptionsBySignalIdAndDateRange(string signalId, DateTime startDate,
            DateTime endDate);
    }
}