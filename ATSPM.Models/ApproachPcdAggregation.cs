using System;
using System.Collections.Generic;

#nullable disable

namespace ATSPM.Application.Models
{
    public partial class ApproachPcdAggregation
    {
        public DateTime BinStartTime { get; set; }
        public string SignalId { get; set; }
        public int ApproachId { get; set; }
        public int PhaseNumber { get; set; }
        public bool IsProtectedPhase { get; set; }
        public int ArrivalsOnGreen { get; set; }
        public int ArrivalsOnRed { get; set; }
        public int ArrivalsOnYellow { get; set; }
        public int Volume { get; set; }
        public int TotalDelay { get; set; }
    }
}
