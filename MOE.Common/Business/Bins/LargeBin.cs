using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MOE.Common.Models;

namespace MOE.Common.Business.Bins
{
    public class LargeBin: Bin
    {
        public List<DayOfWeek>DaysOfWeek { get; set; }
        public  int TODStartHour { get; set; }
        public int TODStartMinute { get; set; }
        public int TODEndHour { get; set; }
        public int TODEndMinute { get; set; }
        public List<DateTime> Dates { get; set; }
        public List<Aggregation> Records { get; set; }
        public List<Aggregation> FilteredRecords { get; set; }



        public void FilterRecords()
        {
            foreach (var date in Dates)
            {
                DateTime start = date.Date.AddHours(TODStartHour).AddMinutes(TODStartMinute);
                DateTime end = date.Date.AddHours(TODEndHour).AddMinutes(TODEndMinute);

                var dateResults = from r in Records
                    where r.BinStartTime > start && r.BinStartTime < end
                    select r;

                FilteredRecords.AddRange(dateResults);
            }
        }

    }


}