using System;
using System.Collections.Generic;
using System.Linq;

namespace MOE.Common.Business
{
    public class PlanSplitFail : Plan
    {
        public PlanSplitFail(DateTime start, DateTime end, int planNumber, List<CycleSplitFail> cycles) : base(start,
            end, planNumber)
        {
            var cyclesForPlan = cycles.Where(c => c.StartTime >= start && c.StartTime < end).ToList();
            if (cyclesForPlan.Count > 0)
            {
                TotalCycles = cyclesForPlan.Count;
                FailsInPlan = cyclesForPlan.Count(c => c.IsSplitFail);
                PercentFails = FailsInPlan / TotalCycles * 100;
            }
        }

        public double TotalCycles { get; }
        public double FailsInPlan { get; }
        public double PercentFails { get; }
    }
}