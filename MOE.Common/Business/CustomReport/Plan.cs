using System;

namespace MOE.Common.Business.CustomReport
{
    public class Plan
    {
        public Plan(DateTime startDate, DateTime endDate, int planNumber)
        {
            StartDate = startDate;
            EndDate = endDate;
            PlanNumber = planNumber;
        }

        public DateTime StartDate { get; }

        public DateTime EndDate { get; }

        public int PlanNumber { get; }
    }
}