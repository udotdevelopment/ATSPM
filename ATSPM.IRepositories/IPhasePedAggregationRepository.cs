using ATSPM.Application.Models;
using System;
using System.Collections.Generic;

namespace ATSPM.IRepositories
{
    public interface IPhasePedAggregationRepository
    {
        PhasePedAggregation Add(PhasePedAggregation pedAggregation);
        void Update(PhasePedAggregation pedAggregation);
        void Remove(PhasePedAggregation pedAggregation);
        List<int> GetAvailablePhaseNumbers(Signal signal, DateTime startDate, DateTime endDate);
        List<PhasePedAggregation> GetPhasePedsAggregationBySignalIdPhaseNumberAndDateRange(string signalId, int phaseNumber, DateTime startDate, DateTime endDate);
        bool Exists(string signalId, int phaseNumber, DateTime startDate, DateTime endDate);
    }
}