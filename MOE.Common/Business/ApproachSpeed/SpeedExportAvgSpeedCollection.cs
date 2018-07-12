using System;
using System.Collections.Generic;
using MOE.Common.Models;

namespace MOE.Common.Business
{
    public class SpeedExportAvgSpeedCollection
    {
        public List<SpeedExportAvgSpeed> Items = new List<SpeedExportAvgSpeed>();

        public SpeedExportAvgSpeedCollection(DateTime startTime, DateTime endTime, int binSize,
            int minspeedfilter, List<RedToRedCycle> Cycles)
        {
            var dt = startTime;

            while (dt.AddMinutes(binSize) < endTime)
            {
                var endDate = dt.AddMinutes(binSize);
                var Avg = new SpeedExportAvgSpeed(dt, endDate, minspeedfilter,
                    GetSpeedHits(dt, endDate, Cycles));
                Items.Add(Avg);
                dt = dt.AddMinutes(binSize);
            }
            Items.Add(new SpeedExportAvgSpeed(dt, endTime, minspeedfilter,
                GetSpeedHits(dt, endTime, Cycles)));
        }

        private List<Speed_Events> GetSpeedHits(DateTime startDate, DateTime endDate,
            List<RedToRedCycle> Cycles)
        {
            var list = new List<Speed_Events>();
            foreach (var scg in Cycles)
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