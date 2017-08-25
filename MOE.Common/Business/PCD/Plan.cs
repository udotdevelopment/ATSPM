using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MOE.Common.Business.PCD
{
    public class Plan
    {
        public DateTime PlanStart;
        public DateTime PlanEnd;
        public int PlanNumber;

        public Plan(DateTime planStart, DateTime planEnd, int planNumber)
        {
            this.PlanStart = planStart;
            this.PlanEnd = planEnd;
            this.PlanNumber = planNumber;
        }
    }
}
