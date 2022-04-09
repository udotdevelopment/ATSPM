using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOE.Common.Business.LeftTurnGapReport
{
    public static class LeftTurnPedestrianAnalysis
    {
        public static double GetPedestrianPercentage(string signalId, int directionTypeId, DateTime start, DateTime end, TimeSpan startTime, TimeSpan endTime)
        {
            var detectors = LeftTurnReportPreCheck.GetLeftTurnDetectors(signalId, directionTypeId);
            var approach = LeftTurnReportPreCheck.GetLTPhaseNumberPhaseTypeByDirection(signalId, directionTypeId);
            int opposingPhase = LeftTurnReportPreCheck.GetOpposingPhase(approach);
            double cycleAverage =  GetCycleAverage(signalId, start, end, startTime, endTime, opposingPhase);
            double pedCycleAverage = GetPedCycleAverage(signalId, start, end, startTime, endTime, opposingPhase);
            if(cycleAverage == 0)
                throw new ArithmeticException("Cycle average cannot be zero");
            return pedCycleAverage / cycleAverage;
        }

        private static double GetPedCycleAverage(string signalId, DateTime start, DateTime end, TimeSpan startTime, TimeSpan endTime, int phase)
        {
            var repository = Models.Repositories.PhasePedAggregationRepositoryFactory.Create();
            List<Models.PhasePedAggregation> cycleAggregations = new List<Models.PhasePedAggregation>();
            for (var tempDate = start.Date; tempDate <= end; tempDate = tempDate.AddDays(1))
            {
                cycleAggregations.AddRange(repository.GetPhasePedsAggregationBySignalIdPhaseNumberAndDateRange(signalId, phase, tempDate.Date.Add(startTime), tempDate.Date.Add(endTime)));
            }
            return cycleAggregations.Average(a => a.PedCycles);
        }

        private static double GetCycleAverage(string signalId, DateTime start, DateTime end, TimeSpan startTime, TimeSpan endTime, int phase)
        {
            var repository = Models.Repositories.PhaseCycleAggregationsRepositoryFactory.Create();
            List<Models.PhaseCycleAggregation> cycleAggregations = new List<Models.PhaseCycleAggregation>();
            for (var tempDate = start.Date; tempDate <= end; tempDate = tempDate.AddDays(1))
            {
                cycleAggregations.AddRange(repository.GetApproachCyclesAggregationBySignalIdPhaseAndDateRange(signalId, phase, tempDate.Date.Add(startTime), tempDate.Date.Add(endTime)));
            }
            return cycleAggregations.Average(a => a.TotalRedToRedCycles);
        }
    }
}
