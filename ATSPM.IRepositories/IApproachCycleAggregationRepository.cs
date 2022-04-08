using ATSPM.Application.Models;
using System;
using System.Collections.Generic;

namespace ATSPM.IRepositories
{
    public interface IApproachCycleAggregationRepository
    {
        int GetApproachCycleCountAggregationByApproachIdAndDateRange(int versionId, DateTime start,
            DateTime end);

        PhaseCycleAggregation Add(PhaseCycleAggregation priorityAggregation);
        void Update(PhaseCycleAggregation priorityAggregation);
        void Remove(PhaseCycleAggregation priorityAggregation);
        List<PhaseCycleAggregation> GetApproachCyclesAggregationByApproachIdAndDateRange(int approachId, DateTime start,
            DateTime end);
        double GetAverageRedToRedCyclesBySignalIdPhase(string signalId, int phaseNumber, DateTime start,
            DateTime end);
        List<PhaseCycleAggregation> GetApproachCyclesAggregationBySignalIdPhaseAndDateRange(string signalId, int phase, DateTime start,
            DateTime end);
        bool Exists(string signalId, int phaseNumber, DateTime start, DateTime end);
        int GetCycleCountBySignalIdAndDateRange(string signalId, DateTime dateTime1, DateTime dateTime2);
    }
}