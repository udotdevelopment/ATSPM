using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ATSPM.Application.Reports.Business.LeftTurnGapReport
{
    public class GapDurationResult
    {
        public double GapDurationPercent { get; set; }
        public double Capacity { get; set; }    
        public double Demand { get; set; }
        public Dictionary<DateTime, Double> AcceptableGaps { get; set; }
        public String Direction { get; set; }
        public string OpposingDirection { get; internal set; }
    }
}
