using System;
using System.Collections.Generic;
using System.Linq;
using MOE.Common.Models;

namespace MOE.Common.Business
{
    public class VolumeCollection
    {
        public List<Volume> Items = new List<Volume>();

        public VolumeCollection(VolumeCollection primaryDirectionVolume, VolumeCollection opposingDirectionVolume, int binSize)
        {
            if (primaryDirectionVolume != null && opposingDirectionVolume != null)
            {
                for (int i = 0; i < primaryDirectionVolume.Items.Count; i++)
                {
                    Volume primaryBin = primaryDirectionVolume.Items[i];
                    Volume opposingBin = opposingDirectionVolume.Items[i];
                    Volume totalBin = new Volume(primaryBin.StartTime, primaryBin.EndTime, binSize);
                    totalBin.DetectorCount = primaryBin.DetectorCount + opposingBin.DetectorCount;
                    Items.Add(totalBin);
                }
            }
        }

        public VolumeCollection(DateTime startTime, DateTime endTime, List<Controller_Event_Log> detectorEvents,
            int binSize)
        {
            for (DateTime start = startTime; start < endTime; start = start.AddMinutes(binSize))
            {
                var v = new Volume(start, start.AddMinutes(binSize), binSize);
                v.DetectorCount = detectorEvents.Count(d => d.Timestamp >= v.StartTime && d.Timestamp < v.EndTime);
                Items.Add(v);
            }
        }


    }
}