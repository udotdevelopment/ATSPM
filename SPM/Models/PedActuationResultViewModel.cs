using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SPM.Models
{
    public class PedActuationResultViewModel
    {
        public double PedActuationPercent { get; set; }
        public int CyclesWithPedCalls { get; set; }
        public bool ConsiderForStudy { get; set; }
        public Dictionary<DateTime, double> PercentCyclesWithPedsList { get; set; }
        public string Direction { get; set; }
        public string OpposingDirection { get; set; }
    }
}