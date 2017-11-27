using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Query.Expressions;

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
                PercentSplitfails = (SplitFails / cycles.Count())*100;
                AverageGreenOccupancyPercent = cycles.Average(c => c.GreenOccupancyPercent);
                AverageRedOccupancyPercent = cycles.Average(c => c.RedOccupancyPercent);
            }
        }

        public double AverageRedOccupancyPercent { get; private set; } = 0;

        public double AverageGreenOccupancyPercent { get; private set; } = 0;

        public double PercentSplitfails { get; private set; } = 0;

        public double SplitFails { get; private set; } = 0;

        public DateTime StartTime { get; private set; }
        public DateTime EndTime { get; private set; }
        
    }
}
