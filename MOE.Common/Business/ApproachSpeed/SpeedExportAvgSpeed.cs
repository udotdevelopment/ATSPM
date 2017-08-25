using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MOE.Common.Business
{
    public class SpeedExportAvgSpeed
    {

        public List<Models.Speed_Events> Speeds = new List<Models.Speed_Events>();


        private DateTime startTime;
        public DateTime StartTime
        {
            get
            {
                return startTime;
            }
            
        }

        private DateTime endTime;
        public DateTime EndTime
        {
            get
            {
                return endTime;
            }
            
        }

        private int avgSpeed;
        public int AvgSpeed
        {
            get
            {
                if (Speeds.Count > 1)
                {
                    return Convert.ToInt32(Math.Round(Speeds.Where(d => d.MPH >= minSpeedFilter).Average(d => d.MPH), 0));
                }
                else
                {
                    return 0;
                }
            }
        }

        

        private int minSpeedFilter;
        public int MinSpeedFilter
        {
            get
            {
                return minSpeedFilter;
            }
        }

        
        private int binSizeMultiplier;

        public SpeedExportAvgSpeed(DateTime startTime, DateTime endTime,
            int minspeedfilter, List<Models.Speed_Events> Speeds)
        {
            this.startTime = startTime;
            this.endTime = endTime;
            this.minSpeedFilter = minspeedfilter;
            this.Speeds = Speeds;
        }

        
        

    }
}
