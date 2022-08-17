using System;
using System.Collections.Generic;

#nullable disable

namespace ATSPM.Application.Models
{
    public partial class PhaseCycleAggregation
    {
        public DateTime BinStartTime { get; set; }
        public string SignalId { get; set; }
        public int ApproachId { get; set; }
        public int PhaseNumber { get; set; }
        public int RedTime { get; set; }
        public int YellowTime { get; set; }
        public int GreenTime { get; set; }
        public int TotalRedToRedCycles { get; set; }
        public int TotalGreenToGreenCycles { get; set; }
    }
}
