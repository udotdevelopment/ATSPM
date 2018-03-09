using System;
using System.Collections.Generic;
using System.Linq;
using MOE.Common.Models;

namespace MOE.Common.Business
{
    public class AvgSpeedBucketCollection
    {
        public List<AvgSpeedBucket> AvgSpeedBuckets = new List<AvgSpeedBucket>();

        public AvgSpeedBucketCollection(DateTime startTime, DateTime endTime, int binSize, int movementdelay,
            List<CycleSpeed> cycles)
        {
            var dt = startTime;
            while (dt.AddMinutes(binSize) <= endTime)
            {
                var avg = new AvgSpeedBucket(dt, dt.AddMinutes(binSize), binSize, movementdelay,
                    cycles);
                AvgSpeedBuckets.Add(avg);
                dt = dt.AddMinutes(binSize);
            }
        }


        public int GetAverageSpeed(List<Speed_Events> speedEvents)
        {
            var TotalSpeed = 0;
            var AvgSpeed = 0;
            foreach (var speed in speedEvents)
                TotalSpeed = TotalSpeed + speed.MPH;
            double RawAvgSpeed = TotalSpeed / speedEvents.Count;
            AvgSpeed = Convert.ToInt32(Math.Round(RawAvgSpeed));
            return AvgSpeed;
        }
    }
}