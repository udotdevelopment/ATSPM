using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOE.Common.Business.LeftTurnGapReport
{
    public static class LeftTurnSplitFailAnalysis
    {
        public static double GetSplitFailPercent(string signalId, int directionTypeId, DateTime start, DateTime end, TimeSpan startTime, TimeSpan endTime)
        {
            var detectors = LeftTurnReportPreCheck.GetLeftTurnDetectors(signalId, directionTypeId);
            var approach = LeftTurnReportPreCheck.GetLTPhaseNumberPhaseTypeByDirection(signalId, directionTypeId);
            var phase = LeftTurnReportPreCheck.GetOpposingPhase(approach);
            var repository = Models.Repositories.ApproachSplitFailAggregationRepositoryFactory.Create();
            List<Models.ApproachSplitFailAggregation> splitFailsAggregates = new List<Models.ApproachSplitFailAggregation>();
            for (var tempDate = start.Date; tempDate <= end; tempDate = tempDate.AddDays(1))
            {
                splitFailsAggregates.AddRange(repository.GetApproachSplitFailsAggregationByApproachIdAndDateRange(detectors.First().ApproachID, tempDate.Date.Add(startTime), tempDate.Date.Add(endTime),
                    true));
                splitFailsAggregates.AddRange(repository.GetApproachSplitFailsAggregationByApproachIdAndDateRange(detectors.First().ApproachID, tempDate.Date.Add(startTime), tempDate.Date.Add(endTime),
                    false));
            }
            int cycles = splitFailsAggregates.Sum(s => s.Cycles);
            int splitFails = splitFailsAggregates.Sum(s => s.SplitFailures);
            if (cycles == 0)
                throw new ArithmeticException("Cycles cannot be zero");
            return splitFails / cycles;
        }
    }
}
