using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MOE.Common.Business.PCD
{
    public class Phase
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
   
        public List<PCD.Plan> Plans { get; set; }
   
        public int PhaseNumber { get; set; }

        public Phase(int phaseNumber)
        {
            PhaseNumber = phaseNumber;
        }
    }
}
