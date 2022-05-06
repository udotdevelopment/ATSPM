using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ATSPM.Application.Reports.Business.LeftTurnGapReport
{
    public class SplitFailResult
    {
        public double SplitFailPercent { get; set; }
        public int CyclesWithSplitFails { get; set; }
        public Dictionary<DateTime, double> PercentCyclesWithSplitFailList { get; internal set; }
        public string Direction { get; set; }
    }
}
