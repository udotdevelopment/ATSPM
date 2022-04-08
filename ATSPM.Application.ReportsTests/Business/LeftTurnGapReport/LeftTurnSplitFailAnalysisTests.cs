using Xunit;
using ATSPM.Application.Reports.Business.LeftTurnGapReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATSPM.Application.Reports.Business.LeftTurnGapReport.Tests
{
    public class LeftTurnSplitFailAnalysisTests
    {
        [Fact()]
        public void GetPercentCyclesWithSplitFailsTest()
        {
            DateTime start = DateTime.MinValue;
            DateTime end = DateTime.MinValue.AddDays(5);
            TimeSpan startTime = new TimeSpan(6, 0, 0);
            TimeSpan endTime = new TimeSpan(9, 0, 0);
            int[] daysOfWeek = new int[5] { 1, 2, 3, 4, 5 };
            List<Models.ApproachSplitFailAggregation> splitFailsAggregates = 
                new List<Models.ApproachSplitFailAggregation>();
            for (DateTime dt = DateTime.MinValue; dt < DateTime.MinValue.AddDays(5); dt = dt.AddMinutes(15))
            {
                splitFailsAggregates.Add(new Models.ApproachSplitFailAggregation { BinStartTime = dt, Cycles = 5, SplitFailures = 5 });
            }
            var result = LeftTurnSplitFailAnalysis.GetPercentCyclesWithSplitFails(start, end, startTime, endTime, daysOfWeek, splitFailsAggregates);
            foreach(var p in result)
            {
                Assert.True(p.Key.TimeOfDay >= startTime && p.Key.TimeOfDay < endTime);
                Assert.Contains((int)p.Key.DayOfWeek, daysOfWeek);
                Assert.Equal(1, p.Value);
            }

        }
    }
}