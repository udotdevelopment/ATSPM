using System;
using System.Collections.Generic;
using AtspmApi.Models;

namespace AtspmApi.Repositories
{
    public interface IPhasePedAggregationRepository
    {
        PhasePedAggregation Add(PhasePedAggregation pedAggregation);
        void Update(PhasePedAggregation pedAggregation);
        void Remove(PhasePedAggregation pedAggregation);
        //List<int> GetAvailablePhaseNumbers(Signal signal, DateTime startDate, DateTime endDate);
        int GetPhasePedsAggCountBySignal(string signalId, DateTime start,  DateTime end);

        List<PhasePedAggregation> GetPhasePedsAggBySignal(string signalId, DateTime startDate, DateTime endDate);
    }
}