using System;
using System.Collections.Generic;

#nullable disable

namespace ATSPM.Application.Models
{
    public partial class PhaseLeftTurnGapAggregation
    {
        public DateTime BinStartTime { get; set; }
        public string SignalId { get; set; }
        public int PhaseNumber { get; set; }
        public int ApproachId { get; set; }
        public int GapCount1 { get; set; }
        public int GapCount2 { get; set; }
        public int GapCount3 { get; set; }
        public int GapCount4 { get; set; }
        public int GapCount5 { get; set; }
        public int GapCount6 { get; set; }
        public int GapCount7 { get; set; }
        public int GapCount8 { get; set; }
        public int GapCount9 { get; set; }
        public int GapCount10 { get; set; }
        public int GapCount11 { get; set; }
        public double SumGapDuration1 { get; set; }
        public double SumGapDuration2 { get; set; }
        public double SumGapDuration3 { get; set; }
        public double SumGreenTime { get; set; }
    }
}
