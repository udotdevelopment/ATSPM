using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace MOE.Common.Business
{
    public class SpeedExportAvgSpeedCollection
    {
        public List<SpeedExportAvgSpeed> Items = new List<SpeedExportAvgSpeed>();

        public SpeedExportAvgSpeedCollection(DateTime startTime, DateTime endTime, int binSize,
            int minspeedfilter, List<PhaseCycleBase> cycles)
        {
            DateTime dt = startTime;

            while (dt.AddMinutes(binSize) < endTime)
            {
                DateTime endDate = dt.AddMinutes(binSize);
                SpeedExportAvgSpeed Avg = new SpeedExportAvgSpeed(dt, endDate, minspeedfilter, 
                    GetSpeedHits(dt, endDate, cycles));
                Items.Add(Avg);
                dt = dt.AddMinutes(binSize);
            }
            Items.Add(new SpeedExportAvgSpeed(dt, endTime, minspeedfilter,
                    GetSpeedHits(dt, endTime, cycles)));

        }

        private List<Models.Speed_Events> GetSpeedHits(DateTime startDate, DateTime endDate, 
            List<PhaseCycleBase> Cycles)
        {
            List <Models.Speed_Events > list = new List<Models.Speed_Events>();
           foreach(var scg in Cycles)
           {
               var listItems = from s in scg.SpeedsForCycle
                               where s.timestamp >= startDate && s.timestamp < endDate
                               select s;

               list.AddRange(listItems);
               //foreach (var sh in listItems)
               //{
               //    list.Add(sh);
               //}
           }
            return list;
        }

        
    }
    
}
