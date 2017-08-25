using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MOE.Common.Business
{
    public class AvgSpeedBucketCollection
    {
        public List<AvgSpeedBucket> Items = new List<AvgSpeedBucket>();

        public AvgSpeedBucketCollection(DateTime startTime, DateTime endTime, List<Cycle> CycleCollection, int binSize, int minspeedfilter, int movementdelay)
        {
            DateTime dt = startTime;

            while (dt.AddMinutes(binSize) < endTime)
            {
                AvgSpeedBucket Avg = new AvgSpeedBucket(dt, dt.AddMinutes(binSize), CycleCollection, binSize, minspeedfilter, movementdelay);
                Items.Add(Avg);
                dt = dt.AddMinutes(binSize);


            }

        }


        public void AddItem(AvgSpeedBucket average)
        {
            Items.Add(average);
        }

        public int GetAverageSpeed(List<Models.Speed_Events> SpeedEvents)
        {
            int TotalSpeed = 0;
            int AvgSpeed = 0;
            foreach (Models.Speed_Events speed in SpeedEvents)
            {
                TotalSpeed = TotalSpeed + speed.MPH;
            }

            

            double RawAvgSpeed = TotalSpeed / Items.Count;
            AvgSpeed = Convert.ToInt32(Math.Round(RawAvgSpeed));
            return AvgSpeed;
        }
    }
}

