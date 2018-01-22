using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MOE.Common.Business.Bins
{
    [DataContract]
    public class BinFactoryOptions
    {

            public enum BinSizes
            {
                FifteenMinutes, ThirtyMinutes, Hour, Day, Month, Year,
                Week
            }
            public enum TimeOptions { StartToEnd, TimePeriod }
        [DataMember]
            public DateTime Start { get; set; }
        [DataMember]
        public DateTime End { get; set; }

        [DataMember]
        public int? TimeOfDayStartHour { get; set; }

        [DataMember]
        public int? TimeOfDayStartMinute { get; set; }

        [DataMember]
        public int? TimeOfDayEndHour { get; set; }

        [DataMember]
        public int? TimeOfDayEndMinute { get; set; }

        [DataMember]
        public List<DayOfWeek> DaysOfWeek { get; set; }

        [DataMember]
        public TimeOptions TimeOption { get; set; }

        [DataMember]
        public BinSizes BinSize { get; set; }

        [DataMember]
        public List<DateTime> DateList { get; set; }

            public BinFactoryOptions(DateTime start, DateTime end, int? timeOfDayStartHour, int? timeOfDayStartMinute, int? timeOfDayEndHour, int? timeOfDayEndMinute, List<DayOfWeek> daysOfWeek, BinSizes binSize, TimeOptions timeOption)
            {
                Start = start;
                End = end;
                TimeOfDayStartHour = timeOfDayStartHour;
                TimeOfDayStartMinute = timeOfDayStartMinute;
                TimeOfDayEndHour = timeOfDayEndHour;
                TimeOfDayEndMinute = timeOfDayEndMinute;
                DaysOfWeek = daysOfWeek;
                BinSize = binSize;
                TimeOption = timeOption;
                if (daysOfWeek != null)
                {
                    DateList = GetDateList(Start, End, DaysOfWeek);
                }
                else
                {
                    DateList = new List<DateTime>();
                    for (DateTime counterDate = start; counterDate <= end; counterDate = counterDate.AddDays(1))
                    {
                        DateList.Add(counterDate.Date);
                    }
                }
            }

        private List<DateTime> GetDateList(DateTime startDate, DateTime endDate, List<DayOfWeek> daysOfWeek)
        {
            List<DateTime> dates = new List<DateTime>();

            for (DateTime counterDate = startDate; counterDate <= endDate; counterDate = counterDate.AddDays(1))
            {
                if (daysOfWeek.Contains(counterDate.DayOfWeek))
                {
                   
                    dates.Add(counterDate);
                }
            }

            return dates;
        }
    }
    }
