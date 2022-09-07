using ATSPM.Application.Models;
using System;
using System.Collections.Generic;

namespace ATSPM.IRepositories
{
    public interface IPhaseLeftTurnGapAggregationRepository
    {
        PhaseLeftTurnGapAggregation Add(PhaseLeftTurnGapAggregation phaseLeftTurnGapAggregation);
        void Update(PhaseLeftTurnGapAggregation phaseLeftTurnGapAggregation);
        void Remove(PhaseLeftTurnGapAggregation phaseLeftTurnGapAggregation);
        List<int> GetAvailablePhaseNumbers(Signal signal, DateTime startDate, DateTime endDate);
        List<PhaseLeftTurnGapAggregation> GetPhaseLeftTurnGapAggregationBySignalIdPhaseNumberAndDateRange(string signalId, int phaseNumber, DateTime startDate, DateTime endDate);
        double GetSummedGapsBySignalIdPhaseNumberAndDateRange(string signalId, int phaseNumber, DateTime startDate, DateTime endDate, int gapCountColumn);
        bool Exists(string signalId, int phaseNumber, DateTime dateTime1, DateTime dateTime2);
    }
}