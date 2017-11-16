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
            int minspeedfilter, List<RedToRedCycle> Cycles)
        {
            DateTime dt = startTime;

            while (dt.AddMinutes(binSize) < endTime)
            {
                DateTime endDate = dt.AddMinutes(binSize);
                SpeedExportAvgSpeed Avg = new SpeedExportAvgSpeed(dt, endDate, minspeedfilter, 
                    GetSpeedHits(dt, endDate, Cycles));
                Items.Add(Avg);
                dt = dt.AddMinutes(binSize);
            }
            Items.Add(new SpeedExportAvgSpeed(dt, endTime, minspeedfilter,
                    GetSpeedHits(dt, endTime, Cycles)));

        }

        private List<Models.Speed_Events> GetSpeedHits(DateTime startDate, DateTime endDate, 
            List<MOE.Common.Business.RedToRedCycle> Cycles)
        {
            List <Models.Speed_Events > list = new List<Models.Speed_Events>();
           foreach(RedToRedCycle scg in Cycles)
           {
                //TODO:Fix for speed report
               //var listItems = from s in scg.SpeedsForCycle
               //                where s.timestamp >= startDate && s.timestamp < endDate
               //                select s;

               //list.AddRange(listItems);
               //foreach (var sh in listItems)
               //{
               //    list.Add(sh);
               //}
           }
            return list;
        }

        
    }
    
}
