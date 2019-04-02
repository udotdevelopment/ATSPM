
using System;

namespace MOE.Common.Business
{
    public class Plan
    {
        public Plan(DateTime start, DateTime end, int planNumber)
        {
            StartTime = start;
            EndTime = end;
            PlanNumber = planNumber;
        }

        public DateTime StartTime { get; }
        public DateTime EndTime { get; }
        public int PlanNumber { get; }
    }
}