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
            var endTimeAllowPartialBinSize = endTime.AddMinutes(binSize);
            //this allows ending time period that is shorter than the BinSize to be counted
            //e.g. 14 min (11:45 pm - 11:49 pm) vs 15 min BinSize
            //however it's not inclusive (not <=) so for whole time periods 
            //(e.g. 12:00 am - 12:00 am) we will not count 1 more period

            while (dt.AddMinutes(binSize) < endTimeAllowPartialBinSize)
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