using System;

namespace MOE.Common.Business
{
    public class Volume
    {
        private readonly int binSizeMultiplier;

        //private int yAxis;

        public Volume(DateTime startTime, DateTime endTime, int binSize)
        {
            StartTime = startTime;
            EndTime = endTime;
            XAxis = startTime;
            if (binSize == 0)
                binSizeMultiplier = 0;
            else
                binSizeMultiplier = 60 / binSize;
        }

        public DateTime XAxis { get; set; }

        public DateTime StartTime { get; }

        public DateTime EndTime { get; }

        public int DetectorCount { get; set; }

        public int YAxis => DetectorCount * binSizeMultiplier;

        //public Volume(DateTime startTime, DateTime endTime, int binSize, int volume)
        //{
        //    this.startTime = startTime;
        //    this.endTime = endTime;
        //    this.xAxis = endTime;
        //    this.binSizeMultiplier = 60 / binSize;
        //    this.detectorCount = volume;
        //}

        //public void AddDetectorToVolume()
        //{
        //    DetectorCount++;
        //}
    }
}