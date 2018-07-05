using System;
using System.Collections.Generic;
using System.Linq;

namespace MOE.Common.Business.PEDDelay
{
    public class PedPlan : PlanBase
    {
        public PedPlan(string signalID, int phaseNumber, DateTime startDate, DateTime endDate, int planNumber)
        {
            StartDate = startDate;
            EndDate = endDate;
            PlanNumber = planNumber;
        }

        public DateTime StartDate { get; }

        public DateTime EndDate { get; }

        public int PlanNumber { get; }

        public int PhaseNumber { get; }

        public double PedActuations => Cycles.Count;

        public double MinDelay
        {
            get
            {
                if (PedActuations > 0)
                    return Cycles.Min(c => c.Delay);
                return 0;
            }
        }

        public double MaxDelay
        {
            get
            {
                if (PedActuations > 0)
                    return Cycles.Max(c => c.Delay);
                return 0;
            }
        }

        public double AvgDelay
        {
            get
            {
                if (PedActuations > 0)
                    return Cycles.Average(c => c.Delay);
                return 0;
            }
        }

        public List<PedCycle> Cycles { get; set; } = new List<PedCycle>();
    }
}