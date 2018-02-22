using System;
using System.Collections.Generic;
using System.Linq;

namespace MOE.Common.Business.SplitFail
{
    public class SplitFailBin
    {
        public SplitFailBin(DateTime startTime, DateTime endTime, List<CycleSplitFail> cycles)
        {
            StartTime = startTime;
            EndTime = endTime;
            if (cycles.Count > 0)
            {
                SplitFails = cycles.Count(c => c.IsSplitFail);
                PercentSplitfails = SplitFails / cycles.Count() * 100;
                AverageGreenOccupancyPercent = cycles.Average(c => c.GreenOccupancyPercent);
                AverageRedOccupancyPercent = cycles.Average(c => c.RedOccupancyPercent);
            }
        }

        public double AverageRedOccupancyPercent { get; }

        public double AverageGreenOccupancyPercent { get; }

        public double PercentSplitfails { get; }

        public double SplitFails { get; }

        public DateTime StartTime { get; }
        public DateTime EndTime { get; }
    }
}