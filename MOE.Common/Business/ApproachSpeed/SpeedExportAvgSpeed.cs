using System;
using System.Collections.Generic;
using System.Linq;
using MOE.Common.Models;

namespace MOE.Common.Business
{
    public class SpeedExportAvgSpeed
    {
        public List<Speed_Events> Speeds = new List<Speed_Events>();


        public SpeedExportAvgSpeed(DateTime startTime, DateTime endTime,
            int minspeedfilter, List<Speed_Events> Speeds)
        {
            StartTime = startTime;
            EndTime = endTime;
            MinSpeedFilter = minspeedfilter;
            this.Speeds = Speeds;
        }

        public DateTime StartTime { get; }

        public DateTime EndTime { get; }

        public int AvgSpeed
        {
            get
            {
                if (Speeds.Count > 1)
                    return Convert.ToInt32(
                        Math.Round(Speeds.Where(d => d.MPH >= MinSpeedFilter).Average(d => d.MPH), 0));
                return 0;
            }
        }

        public int MinSpeedFilter { get; }
    }
}