using System;
using System.Collections.Generic;

namespace MOE.Common.Models.Repositories
{
    public interface IPhasePedAggregationRepository:IAggregationRepositoryBase
    {
        PhasePedAggregation Add(PhasePedAggregation pedAggregation);
        void Update(PhasePedAggregation pedAggregation);
        void Remove(PhasePedAggregation pedAggregation);
        List<int> GetAvailablePhaseNumbers(Signal signal, DateTime startDate, DateTime endDate);
        List<PhasePedAggregation> GetPhasePedsAggregationBySignalIdPhaseNumberAndDateRange(string signalId, int phaseNumber, DateTime startDate, DateTime endDate);
    }
}