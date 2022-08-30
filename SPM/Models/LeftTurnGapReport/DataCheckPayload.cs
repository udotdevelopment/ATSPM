using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SPM.Models.LeftTurnGapReport
{
    public class DataCheckPayload
    {
        public string SignalId { get; set; }
        public int VolumePerHourThreshold { get; set; }
        public double GapOutThreshold { get; set; }

        public double PedestrianThreshold { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int ApproachId { get; set; }
        public int[] DaysOfWeek { get; set; }
    }       
}   