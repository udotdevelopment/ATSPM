using System;
using System.Collections.Generic;
using System.Linq;
using MOE.Common.Models;

namespace MOE.Common.Business.PEDDelay
{
    public class PedPlan
    {
        public PedPlan(int phaseNumber, DateTime startDate, DateTime endDate, int planNumber)
        {
            StartDate = startDate;
            EndDate = endDate;
            PlanNumber = planNumber;
            PhaseNumber = phaseNumber;
        }

        public DateTime StartDate { get; }
        public DateTime EndDate { get; }
        public int PlanNumber { get; }
        public int PhaseNumber { get; }
        public List<Controller_Event_Log> Events { get; set; }
        public List<PedCycle> Cycles { get; set; } = new List<PedCycle>();
        public int UniquePedDetections { get; set; }
        public double CyclesWithPedRequests => Cycles.Count;
        public double PedBeginWalkCount
        {
            get
            {
                return Events.Where(e => e.EventCode == 21 || e.EventCode == 67).Count();
            }
        }
        public double PedCallsRegisteredCount
        {
            get
            {
                return Events.Where(e => e.EventCode == 45).Count();
            }
        }
        public double MinDelay
        {
            get
            {
                if (CyclesWithPedRequests > 0)
                    return Cycles.Min(c => c.Delay);
                return 0;
            }
        }

        public double MaxDelay
        {
            get
            {
                if (CyclesWithPedRequests > 0)
                    return Cycles.Max(c => c.Delay);
                return 0;
            }
        }

        public double AvgDelay
        {
            get
            {
                if (CyclesWithPedRequests > 0)
                    return Cycles.Average(c => c.Delay);
                return 0;
            }
        }
    }
}