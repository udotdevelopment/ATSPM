using System;
using System.Collections.Generic;

namespace MOE.Common.Models.Repositories
{
    public interface IPhaseTerminationAggregationRepository:IAggregationRepositoryBase
    {

        PhaseTerminationAggregation Add(PhaseTerminationAggregation priorityAggregation);
        void Update(PhaseTerminationAggregation priorityAggregation);
        void Remove(PhaseTerminationAggregation priorityAggregation);
        List<int> GetAvailablePhaseNumbers(Signal signal, DateTime startDate, DateTime endDate);
        List<PhaseTerminationAggregation> GetPhaseTerminationsAggregationBySignalIdPhaseNumberAndDateRange(string signalId, int phaseNumber, DateTime startDate, DateTime endDate);
    }
}