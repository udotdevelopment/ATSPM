using System;
using System.Collections.Generic;

#nullable disable

namespace ATSPM.Application.Models
{
    public partial class ApproachSplitFailAggregation
    {
        public DateTime BinStartTime { get; set; }
        public string SignalId { get; set; }
        public int ApproachId { get; set; }
        public int PhaseNumber { get; set; }
        public bool IsProtectedPhase { get; set; }
        public int SplitFailures { get; set; }
        public int GreenOccupancySum { get; set; }
        public int RedOccupancySum { get; set; }
        public int GreenTimeSum { get; set; }
        public int RedTimeSum { get; set; }
        public int Cycles { get; set; }
    }
}
