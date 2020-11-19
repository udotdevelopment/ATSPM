using System;
using System.Collections.Generic;
using System.Linq;

namespace MOE.Common.Business
{
    public class TimingAndActuationCycle 
    {
        public TimingAndActuationCycle(DateTime startGreen, DateTime endMinGreen, DateTime startYellow, DateTime startRedClearance, DateTime startRed, DateTime endRed, DateTime overlapDark)
        {
            StartGreen = startGreen;
            EndMinGreen = endMinGreen;
            StartYellow = startYellow;
            StartRedClearance = startRedClearance;
            StartRed = startRed;
            EndRed = endRed;
            OverLapDark = overlapDark;
        }
        public DateTime StartGreen { get; set; }
        public DateTime EndMinGreen { get; set; }
        public DateTime StartYellow { get; set; }
        public DateTime StartRedClearance { get; set; }
        public DateTime StartRed { get; set; }
        public DateTime EndRed { get; set; }
        public DateTime OverLapDark { get; set; }
    }
}
