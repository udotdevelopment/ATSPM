using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SPM.Models
{
    public class SplitFailResultViewModel
    {
        public double SplitFailPercent { get; set; }
        public int CyclesWithSplitFails { get; set; }
        public bool ConsiderForStudy { get; set; }
        public Dictionary<DateTime, double> PercentCyclesWithSplitFailList { get; set; }
        public string Direction { get; set; }
    }
}