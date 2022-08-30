using Xunit;
using ATSPM.Application.Reports.Business.LeftTurnGapReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATSPM.Application.Reports.Business.LeftTurnGapReport.Tests
{
    public class LeftTurnPedestrianAnalysisTests
    {
        [Fact()]
        public void GetAverageCyclesTest()
        {
            DateTime start = DateTime.MinValue;
            DateTime end = DateTime.MinValue.AddDays(5);
            TimeSpan startTime = new TimeSpan(6, 0, 0);
            TimeSpan endTime = new TimeSpan(9, 0, 0);
            int[] daysOfWeek = new int[5] { 1, 2, 3, 4, 5 };
            List<Models.PhaseCycleAggregation> cyclesAggregate =
                new List<Models.PhaseCycleAggregation>();
            for (DateTime dt = DateTime.MinValue; dt < DateTime.MinValue.AddDays(5); dt = dt.AddMinutes(15))
            {
                cyclesAggregate.Add(new Models.PhaseCycleAggregation { BinStartTime = dt, TotalRedToRedCycles = 5 });
            }
            var result = LeftTurnPedestrianAnalysis.GetAverageCycles(start, end, startTime, endTime, daysOfWeek, cyclesAggregate);
            foreach (var p in result)
            {
                Assert.True(p.Key.TimeOfDay >= startTime && p.Key.TimeOfDay < endTime);
                Assert.Contains((int)p.Key.DayOfWeek, daysOfWeek);
                Assert.Equal(5, p.Value);
            }
        }
    }
}