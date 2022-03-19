using Xunit;
using ATSPM.Application.Reports.Business.LeftTurnGapReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATSPM.Application.Reports.Business.LeftTurnGapReport.Tests
{
    public class LeftTurnVolumeAnalysisTests
    {
        [Fact()]
        public void GetDemandListTest()
        {
            DateTime start = DateTime.MinValue.AddDays(1);
            DateTime end = DateTime.MinValue.AddDays(5);
            TimeSpan startTime = new TimeSpan(6, 0, 0);
            TimeSpan endTime = new TimeSpan(9, 0, 0);
            int[] daysOfWeek = new int[5] { 1, 2, 3, 4, 5 };
            List<Models.DetectorEventCountAggregation> detectorCountAggregations =
                new List<Models.DetectorEventCountAggregation>();
            for (DateTime dt = DateTime.MinValue; dt < DateTime.MinValue.AddDays(7); dt = dt.AddMinutes(15))
            {
                detectorCountAggregations.Add(new Models.DetectorEventCountAggregation { BinStartTime = dt, EventCount = 5 });
            }
            var result = LeftTurnVolumeAnalysis.GetDemandList(start, end, startTime, endTime, daysOfWeek, detectorCountAggregations);
            foreach (var p in result)
            {
                Assert.True(p.Key.TimeOfDay >= startTime && p.Key.TimeOfDay < endTime);
                Assert.Contains((int)p.Key.DayOfWeek, daysOfWeek);
                Assert.Equal(10, p.Value);
            }
        }
    }
}