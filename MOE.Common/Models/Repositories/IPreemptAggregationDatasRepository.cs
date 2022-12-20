using System;
using System.Collections.Generic;

namespace MOE.Common.Models.Repositories
{
    public interface IPreemptAggregationDatasRepository:IAggregationRepositoryBase
    {
        List<PreemptionAggregation> GetPreemptAggregationByVersionIdAndDateRange(DateTime start,
            DateTime end);

        int GetPreemptAggregationTotalByVersionIdAndDateRange(DateTime start,
            DateTime end);


        int GetPreemptAggregationTotalByVersionIdPreemptNumberAndDateRange(DateTime start,
            DateTime end, int preemptNumber);


        PreemptionAggregation Add(PreemptionAggregation preemptionAggregation);
        void Update(PreemptionAggregation preemptionAggregation);
        void Remove(PreemptionAggregation preemptionAggregation);

        List<PreemptionAggregation> GetPreemptionsBySignalIdAndDateRange(string signalId, DateTime startDate,
            DateTime endDate);
    }
}