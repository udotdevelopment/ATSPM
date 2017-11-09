using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOE.Common.Business.CustomReport
{
    public class Plan
    {
        public DateTime StartDate { get; }

        public DateTime EndDate { get; }

        public int PlanNumber { get; }


        public Plan(DateTime startDate, DateTime endDate, int planNumber)
        {
            StartDate = startDate;
            EndDate = endDate;
            PlanNumber = planNumber;
        }

        

    }
}
