using System;
using System.Collections.Generic;
using AtspmApi.Models;

namespace AtspmApi.Repositories
{
    public interface IPhaseTerminationAggregationRepository
    {

        PhaseTerminationAggregation Add(PhaseTerminationAggregation priorityAggregation);
        void Update(PhaseTerminationAggregation priorityAggregation);
        void Remove(PhaseTerminationAggregation priorityAggregation);
        //List<int> GetAvailablePhaseNumbers(Signal signal, DateTime startDate, DateTime endDate);
        List<PhaseTerminationAggregation> GetPhaseTerminationsAggregationBySignal(string signalId, DateTime startDate, DateTime endDate);
        int GetPhaseTermAggCountBySignal(string signalId, DateTime startDate, DateTime endDate);
    }
}