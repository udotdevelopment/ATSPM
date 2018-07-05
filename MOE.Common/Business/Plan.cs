using System;

namespace MOE.Common.Business
{
    public class Plan: PlanBase
    {
        public Plan(DateTime start, DateTime end, int planNumber)
        {
            PlanStart = start;
            PlanEnd = end;
            PlanNumber = planNumber;
        }

        public DateTime StartTime { get; }
        public DateTime EndTime { get; }
        public int PlanNumber { get; }
    }
}