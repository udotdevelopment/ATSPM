using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MOE.Common.Business
{
    public class AvgSpeedBucketCollection
    {
        public List<AvgSpeedBucket> AvgSpeedBuckets = new List<AvgSpeedBucket>();

        public AvgSpeedBucketCollection(DateTime startTime, DateTime endTime, int binSize, int movementdelay, List<CycleSpeed> cycles)
        {
            DateTime dt = startTime;
            while (dt.AddMinutes(binSize) <= endTime)
            {
                AvgSpeedBucket avg = new AvgSpeedBucket(dt, dt.AddMinutes(binSize), binSize, movementdelay, cycles.Where(c => c.StartTime >= dt && c.EndTime < endTime).ToList());
                AvgSpeedBuckets.Add(avg);
                dt = dt.AddMinutes(binSize);
            }
        }
        

        public int GetAverageSpeed(List<Models.Speed_Events> speedEvents)
        {
            int TotalSpeed = 0;
            int AvgSpeed = 0;
            foreach (Models.Speed_Events speed in speedEvents)
            {
                TotalSpeed = TotalSpeed + speed.MPH;
            }
            double RawAvgSpeed = TotalSpeed / speedEvents.Count;
            AvgSpeed = Convert.ToInt32(Math.Round(RawAvgSpeed));
            return AvgSpeed;
        }
    }
}

