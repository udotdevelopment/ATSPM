using System;
using System.Collections.Generic;

#nullable disable

namespace ATSPM.Application.Models
{
    public partial class PhaseSplitMonitorAggregation
    {
        public DateTime BinStartTime { get; set; }
        public string SignalId { get; set; }
        public int PhaseNumber { get; set; }
        public int EightyFifthPercentileSplit { get; set; }
        public int SkippedCount { get; set; }
    }
}
