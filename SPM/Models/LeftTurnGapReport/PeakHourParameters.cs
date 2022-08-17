using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SPM.Models.LeftTurnGapReport
{
    public class PeakHourParameters
    {
        public string SignalId { get; set; }
        public int ApproachId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int[] DaysOfWeek { get; set; }
    }
}
