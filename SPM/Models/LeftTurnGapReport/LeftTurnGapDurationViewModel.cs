using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SPM.Models
{
    public class LeftTurnGapDurationViewModel
    {
        public double GapDurationPercent { get; set; }
        public double Capacity { get; set; }
        public double Demand { get; set; }
        public Dictionary<DateTime, Double> AcceptableGaps { get; set; }
        public bool? ConsiderForStudy { get; set; }
        public string Direction { get; set; }
        public string OpposingDirection { get; set; }
    }
}