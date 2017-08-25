using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MOE.Common.Business
{
    public class VolumeCollection
    {
        public List<Volume> Items = new List<Volume>();

        public VolumeCollection(DateTime startTime, DateTime endTime, List<Models.Controller_Event_Log> detectorEvents,
            int binSize)
        {
            DateTime dt = startTime;
            
            while(dt.AddMinutes(binSize) <= endTime)
            {
                Volume v = new Volume(dt, dt.AddMinutes(binSize), binSize);
                Items.Add(v);
                dt = dt.AddMinutes(binSize);
            }

            foreach (MOE.Common.Models.Controller_Event_Log row in detectorEvents)
            {
                var query = from item in Items
                            where item.StartTime < row.Timestamp && item.EndTime > row.Timestamp
                            select item;


                foreach (var volume in query)
                {
                    volume.AddDetectorToVolume();
                }

            }
        }

        public void AddItem(Volume volume)
        {
            Items.Add(volume);
        }
    }
}
