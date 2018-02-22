using System;
using System.Collections.Generic;

namespace MOE.Common.Business.PCD
{
    public class Phase
    {
        public Phase(int phaseNumber)
        {
            PhaseNumber = phaseNumber;
        }

        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public List<Plan> Plans { get; set; }

        public int PhaseNumber { get; set; }
    }
}