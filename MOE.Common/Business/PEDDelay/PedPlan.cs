using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOE.Common.Business.PEDDelay
{
    public class PedPlan
    {
        private DateTime _StartDate;

        public DateTime StartDate
        {
            get { return _StartDate; }
        }

        private DateTime _EndDate;

        public DateTime EndDate
        {
            get { return _EndDate; }
        }

        private int _PlanNumber;

        public int PlanNumber
        {
            get { return _PlanNumber; }
        }

        private int _PhaseNumber;

        public int PhaseNumber
        {
            get { return _PhaseNumber; }
        }
        
        public double PedActuations
        {
            get { return _Cycles.Count; }
        }

        public double MinDelay
        {
            get {
                if (PedActuations > 0) 
                { 
                    return _Cycles.Min(c => c.Delay); 
                } 
                else { return 0;}
                }
            
        }

        public double MaxDelay
        {
            get
            {
                if (PedActuations > 0)
                {
                    return _Cycles.Max(c => c.Delay);
                }
                else { return 0; }
            }
        }

        public double AvgDelay
        {
            get
            {
                if (PedActuations > 0)
                {
                    return _Cycles.Average(c => c.Delay);
                }
                else { return 0; }
            }
        }


        private List<PedCycle> _Cycles = new List<PedCycle>();

        public List<PedCycle> Cycles
        {
            get { return _Cycles; }
            set { _Cycles = value; }
        }
        
        
        
        
        public PedPlan(string signalID, int phaseNumber, DateTime startDate, DateTime endDate, int planNumber)
        {
            _StartDate = startDate;
            _EndDate = endDate;
            _PlanNumber = planNumber;
        }

        

    }
}
