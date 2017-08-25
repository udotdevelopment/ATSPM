using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MOE.Common.Business
{
    public class Volume
    {
        private DateTime xAxis;
        public DateTime XAxis
        {
            get
            {
                return xAxis; 
            }
            set
            {
                xAxis = value;
            }
        }

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

        private int detectorCount;
        public int DetectorCount
        {
            get
            {
                return detectorCount;
            }

        }

        private int yAxis;
        public int YAxis
        {
            get
            {
                return detectorCount * binSizeMultiplier;
            }
            
        }

        private int binSizeMultiplier;

        public Volume(DateTime startTime, DateTime endTime, int binSize)
        {
            this.startTime = startTime;
            this.endTime = endTime;
            this.xAxis = endTime;
            if (binSize == 0)
            {
                this.binSizeMultiplier = 0;
            }
            else
            {
                this.binSizeMultiplier = 60 / binSize;
            }
        }

        public Volume(DateTime startTime, DateTime endTime, int binSize, int volume)
        {
            this.startTime = startTime;
            this.endTime = endTime;
            this.xAxis = endTime;
            this.binSizeMultiplier = 60 / binSize;
            this.detectorCount = volume;
        }

        public void AddDetectorToVolume()
        {
            detectorCount++;
        }
    }
}
