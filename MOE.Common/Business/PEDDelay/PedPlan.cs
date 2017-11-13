using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOE.Common.Business.PEDDelay
{
    public class PedPlan : PlanBase
    {


        public int PhaseNumber { get; set; }

        
        public double PedActuations
        {
            get { return Cycles.Count; }
        }

        public double MinDelay
        {
            get
            {
                if (PedActuations > 0) 
                { 
                    return Cycles.Min(c => c.Delay); 
                }
                return 0;
            }
            
        }

        public double MaxDelay
        {
            get
            {
                if (PedActuations > 0)
                {
                    return Cycles.Max(c => c.Delay);
                }
                return 0;
            }
        }

        public double AvgDelay
        {
            get
            {
                if (PedActuations > 0)
                {
                    return Cycles.Average(c => c.Delay);
                }
                return 0;
            }
        }


        public List<PedCycle> Cycles = new List<PedCycle>();


        
        
        
        
        public PedPlan(int phaseNumber, DateTime startDate, DateTime endDate, int planNumber)
        {
            PlanStart = startDate;
            PlanEnd = endDate;
            PlanNumber = planNumber;
        }

        

    }
}
