using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace MOE.Common.Business
{
    public class Plan
    {
        public DateTime StartTime { get; }
        public DateTime EndTime { get; }
        public int PlanNumber { get; }

        public Plan(DateTime start, DateTime end, int planNumber)
        {
            StartTime = start;
            EndTime = end;
            PlanNumber = planNumber;
        }
        
    }
}
