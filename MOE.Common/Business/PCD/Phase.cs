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
        //public List<MOE.Common.Models.Detector> LaneGroups { get; set; } //TODO: model change.
        public List<PCD.Plan> Plans { get; set; }
        public List<PCD.Cycle> Cycles { get; set; }
        public int PhaseNumber { get; set; }

        public Phase(int phaseNumber)
        {
            PhaseNumber = phaseNumber;
        }
    }
}
