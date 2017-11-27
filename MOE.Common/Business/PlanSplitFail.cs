using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOE.Common.Business
{
    public class PlanSplitFail:Plan
    {
        public double TotalCycles { get; private set; } = 0;
        public double FailsInPlan { get; private set; } = 0;
        public double PercentFails { get; private set; } = 0;
        public PlanSplitFail(DateTime start, DateTime end, int planNumber, List<CycleSplitFail> cycles) : base(start, end, planNumber)
        {
            var cyclesForPlan = cycles.Where(c => c.StartTime >= start && c.StartTime < end).ToList();
            if (cyclesForPlan.Count > 0)
            {
                TotalCycles = cyclesForPlan.Count;
                FailsInPlan = cyclesForPlan.Count(c => c.IsSplitFail);
                PercentFails = (FailsInPlan / TotalCycles)*100;
            }
        }
    }
}
