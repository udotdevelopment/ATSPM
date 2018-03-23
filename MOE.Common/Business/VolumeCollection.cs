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
            for (DateTime start = startTime; start < endTime; start = start.AddMinutes(binSize))
            {
                var v = new Volume(start, start.AddMinutes(binSize), binSize);
                Items.Add(v);
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