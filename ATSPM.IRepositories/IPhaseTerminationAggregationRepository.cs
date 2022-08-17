using ATSPM.Application.Models;
using System;
using System.Collections.Generic;

namespace ATSPM.IRepositories
{
    public interface IPhaseTerminationAggregationRepository
    {

        PhaseTerminationAggregation Add(PhaseTerminationAggregation priorityAggregation);
        void Update(PhaseTerminationAggregation priorityAggregation);
        void Remove(PhaseTerminationAggregation priorityAggregation);
        List<int> GetAvailablePhaseNumbers(Signal signal, DateTime startDate, DateTime endDate);
        List<PhaseTerminationAggregation> GetPhaseTerminationsAggregationBySignalIdPhaseNumberAndDateRange(string signalId, int phaseNumber, DateTime startDate, DateTime endDate);
        bool Exists(string signalId, int phaseNumber, DateTime startDate, DateTime endDate);
    }
}