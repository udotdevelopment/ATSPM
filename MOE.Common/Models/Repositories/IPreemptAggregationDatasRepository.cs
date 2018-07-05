using System;
using System.Collections.Generic;

namespace MOE.Common.Models.Repositories
{
    public interface IPreemptAggregationDatasRepository
    {
        List<PreemptionAggregation> GetPreemptAggregationByVersionIdAndDateRange(int versionId, DateTime start,
            DateTime end);

        int GetPreemptAggregationTotalByVersionIdAndDateRange(int versionId, DateTime start,
            DateTime end);


        int GetPreemptAggregationTotalByVersionIdPreemptNumberAndDateRange(int versionId, DateTime start,
            DateTime end, int preemptNumber);


        PreemptionAggregation Add(PreemptionAggregation preemptionAggregation);
        void Update(PreemptionAggregation preemptionAggregation);
        void Remove(PreemptionAggregation preemptionAggregation);

        List<PreemptionAggregation> GetPreemptionsBySignalIdAndDateRange(string signalId, DateTime startDate,
            DateTime endDate);
    }
}