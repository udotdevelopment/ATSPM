using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOE.Common.Business.LeftTurnGapReport
{
    public static class LeftTurnGapOutAnalysis
    {
        public static double GetPercentOfGapDuration(string signalId, int directionTypeId, DateTime start, DateTime end, TimeSpan startTime, TimeSpan endTime)
        {
            var approach = LeftTurnReportPreCheck.GetLTPhaseNumberPhaseTypeByDirection(signalId, directionTypeId);
            int opposingPhase = LeftTurnReportPreCheck.GetOpposingPhase(approach);
            int numberOfOposingLanes = GetNumberOfOpposingLanes(signalId, opposingPhase);
            double criticalGap = GetCriticalGap(numberOfOposingLanes);

            int gapCountTotal = GetGapSummedTotal(signalId, opposingPhase, start, end, startTime, endTime, criticalGap);
            //int gapForAllGapsGreaterThanCriticalGapTotal = GetGapForAllGapsGreaterThanCriticalGapTotal(signalId, opposingPhase, start, end, startTime, endTime, criticalGap);
            double gapDemand = GetGapDemand(signalId, directionTypeId, start, end, startTime, endTime, criticalGap);
            if (gapCountTotal == 0)
                throw new ArithmeticException("Gap Count cannot be zero");
            return (gapDemand / gapCountTotal);
        }

        private static double GetGapDemand(string signalId, int directionTypeId, DateTime start, DateTime end, TimeSpan startTime, TimeSpan endTime, double criticalGap)
        {
            var repository = Models.Repositories.DetectorEventCountAggregationRepositoryFactory.Create();
            var detectors = LeftTurnReportPreCheck.GetLeftTurnDetectors(signalId, directionTypeId);
            int totalActivations = 0;
            for (var tempDate = start.Date; tempDate <= end; tempDate = tempDate.AddDays(1))
            {
                foreach (var detector in detectors)
                {
                    totalActivations += repository.GetDetectorEventCountSumAggregationByDetectorIdAndDateRange(detector.ID, tempDate.Date.Add(startTime), tempDate.Date.Add(endTime));
                }
            }
            return totalActivations * criticalGap;
        }

        private static int GetGapSummedTotal(string signalId, int phaseNumber, DateTime start, DateTime end, TimeSpan startTime, TimeSpan endTime, double criticalGap)
        {
            var repository = Models.Repositories.PhaseLeftTurnGapAggregationRepositoryFactory.Create();
            List<Models.PhaseLeftTurnGapAggregation> amAggregations = new List<Models.PhaseLeftTurnGapAggregation>();
            int gapColumn = 1;
            if (criticalGap == 4.1)
                gapColumn = 6;
            else if (criticalGap == 5.3)
                gapColumn = 7;
            int gapTotal = 0;
            for (var tempDate = start.Date; tempDate <= end; tempDate = tempDate.AddDays(1))
            {
               gapTotal += repository.GetSummedGapsBySignalIdPhaseNumberAndDateRange(
                    signalId, phaseNumber, tempDate.Date.Add(startTime), tempDate.Date.Add(endTime), gapColumn);
            }
            return gapTotal;
        }

        private static double GetCriticalGap(int numberOfOposingLanes)
        {
            if(numberOfOposingLanes <= 2)
            {
                return 4.1;
            }
            else
            {
                return 5.3;          
            }
        }

        public static int GetNumberOfOpposingLanes(string signalId, int opposingPhase)
        {
            var repository = Models.Repositories.DetectorRepositoryFactory.Create();
            return repository.GetDetectorsBySignalID(signalId).Count(d => d.Approach.ProtectedPhaseNumber == opposingPhase);
        }
    }
}
