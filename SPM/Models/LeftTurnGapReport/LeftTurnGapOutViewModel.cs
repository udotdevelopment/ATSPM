using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SPM.Models
{
    public class LeftTurnGapOutViewModel
    {
        public double GapOutPercent { get; set; }
        public double Capacity { get; set; }
        public double Demand { get; set; }
        public Dictionary<DateTime, Double> AcceptableGaps { get; set; }
        public bool? ConsiderForStudy { get; set; }
    }
}