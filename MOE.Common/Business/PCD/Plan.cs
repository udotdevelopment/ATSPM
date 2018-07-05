using System;

namespace MOE.Common.Business.PCD
{
    public class Plan
    {
        public DateTime PlanEnd;
        public int PlanNumber;
        public DateTime PlanStart;

        public Plan(DateTime planStart, DateTime planEnd, int planNumber)
        {
            PlanStart = planStart;
            PlanEnd = planEnd;
            PlanNumber = planNumber;
        }
    }
}