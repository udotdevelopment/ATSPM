using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOE.Common.Business
{
    public class PlanSplitFail:Plan
    {
        public int TotalCycles { get;  }
        public int FailsInPlan { get; set; }
        public PlanSplitFail(DateTime start, DateTime end, int planNumber, List<CycleSplitFail> cycles) : base(start, end, planNumber)
        {
            var cyclesForPlan = cycles.Where(c => c.StartTime >= start && c.StartTime < end).ToList();
            TotalCycles = cyclesForPlan.Count;
            FailsInPlan = cyclesForPlan.Sum(c => c.FailsInCycle);
        }
    }
}
