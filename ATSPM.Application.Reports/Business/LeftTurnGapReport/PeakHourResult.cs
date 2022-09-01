using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ATSPM.Application.Reports.Business.LeftTurnGapReport
{
    public class PeakHourResult
    {
        public int AmStartHour { get; set; }
        public int AmEndHour { get; set; }
        public int AmStartMinute { get; set; }
        public int AmEndMinute { get; set; }
        public int PmStartHour { get; set; }
        public int PmEndHour { get; set; }
        public int PmStartMinute { get; set; }
        public int PmEndMinute { get; set; }
    }
}
