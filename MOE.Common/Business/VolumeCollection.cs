using System;
using System.Collections.Generic;
using System.Linq;
using MOE.Common.Models;

namespace MOE.Common.Business
{
    public class VolumeCollection
    {
        public List<Volume> Items = new List<Volume>();

        public VolumeCollection(DateTime startTime, DateTime endTime, List<Controller_Event_Log> detectorEvents,
            int binSize)
        {
            var dt = startTime;

            while (dt.AddMinutes(binSize) <= endTime)
            {
                var v = new Volume(dt, dt.AddMinutes(binSize), binSize);
                Items.Add(v);
                dt = dt.AddMinutes(binSize);
            }

            foreach (var row in detectorEvents)
            {
                var query = from item in Items
                    where item.StartTime <= row.Timestamp && item.EndTime > row.Timestamp
                    select item;


                foreach (var volume in query)
                    volume.AddDetectorToVolume();
            }
        }


    }
}