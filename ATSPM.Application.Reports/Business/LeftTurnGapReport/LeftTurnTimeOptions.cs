using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ATSPM.Application.Reports.Business.LeftTurnGapReport
{
    public enum TimeOptions { PeakHour, PeakPeriod, FullDay, Custom}
    public class LeftTurnTimeOptions
    {
        public TimeOptions SelectedTimeOption { get; set; }
        public int? StartHour { get; set; }
        public int? StartMinute { get; set; }
        public int? EndHour { get; set; }
        public int? EndMinute { get; set; }

    }
}
