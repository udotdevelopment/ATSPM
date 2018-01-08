using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Metadata.W3cXsd2001;

namespace MOE.Common.Business.Bins
{
    public class BinFactoryOptions
    {
        public enum BinSizes{FifteenMinutes, ThirtyMinutes, Hour, Day, Month, Year,
            Week
        }
        public enum TimeOptions{StartToEnd, TimePeriod}

        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public int? TimeOfDayStartHour { get; set; }
        public int? TimeOfDayStartMinute { get; set; }
        public int? TimeOfDayEndHour { get; set; }
        public int? TimeOfDayEndMinute { get; set; }
        public List<DayOfWeek> DaysOfWeek { get; set; }
        public TimeOptions TimeOption { get; set; }
        public BinSizes BinSize { get; set; }

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
        }
    }
    public static class BinFactory
    {
        public static BinsContainer GetBins(BinFactoryOptions timeOptions)
        {
            switch (timeOptions.BinSize)
            {
                case BinFactoryOptions.BinSizes.FifteenMinutes:
                    return GetBinsForRange(timeOptions, 15);
                case BinFactoryOptions.BinSizes.ThirtyMinutes:
                    return GetBinsForRange(timeOptions, 30);
                case BinFactoryOptions.BinSizes.Hour:
                    return GetBinsForRange(timeOptions, 60);
                case BinFactoryOptions.BinSizes.Day:
                    return GetBinsForRange(timeOptions, 60*24);
                case BinFactoryOptions.BinSizes.Week:
                    return GetBinsForRange(timeOptions, 60*24*7);
                case BinFactoryOptions.BinSizes.Month:
                    return GetMonthBinsForRange(timeOptions);
                case BinFactoryOptions.BinSizes.Year:
                    return CreateYearBinsForRange(timeOptions);
                default:
                    return GetBinsForRange(timeOptions, 15);
            }
        }

        private static BinsContainer CreateYearBinsForRange(BinFactoryOptions timeOptions)
        {
            BinsContainer binsContainer = new BinsContainer();
            List < Bin > bins = new List<Bin>();
            for (DateTime startTime = new DateTime(timeOptions.Start.Year,1,1); startTime.Year <= timeOptions.End.Year; startTime = startTime.AddYears(1))
            {
                if (timeOptions.TimeOption == BinFactoryOptions.TimeOptions.StartToEnd)
                {
                    bins.Add(new Bin {Start = startTime, End = startTime.AddYears(1)});
                }
                else
                {
                    if (timeOptions.DaysOfWeek.Contains(startTime.DayOfWeek) &&
                        startTime.TimeOfDay >= timeOptions.Start.TimeOfDay &&
                        startTime.TimeOfDay <= timeOptions.End.TimeOfDay)
                    {
                        bins.Add(new Bin { Start = startTime, End = startTime.AddYears(1) });
                    }
                }
            }
            binsContainer.Bins = bins;
            return binsContainer;
        }

        private static BinsContainer GetMonthBinsForRange(BinFactoryOptions timeOptions)
        {
            BinsContainer binsContainer = new BinsContainer();
            List < Bin > bins = new List<Bin>();
            for (DateTime startTime = new DateTime(timeOptions.Start.Year, timeOptions.Start.Month,1); startTime.Year <= timeOptions.End.Year && startTime.Month <= timeOptions.End.Month; startTime = startTime.AddMonths(1))
            {
                if (timeOptions.TimeOption == BinFactoryOptions.TimeOptions.StartToEnd)
                {
                    bins.Add(new Bin {Start = startTime, End = startTime.AddMonths(1)});
                }
                else
                {
                    if (timeOptions.DaysOfWeek.Contains(startTime.DayOfWeek) &&
                        startTime.TimeOfDay >= timeOptions.Start.TimeOfDay &&
                        startTime.TimeOfDay <= timeOptions.End.TimeOfDay)
                    {
                        bins.Add(new Bin { Start = startTime, End = startTime.AddMonths(1) });
                    }
                }
            }
            binsContainer.Bins = bins;
            return binsContainer;
        }

        private static BinsContainer GetBinsForRange(BinFactoryOptions timeOptions, int minutes)
        {
            BinsContainer binsContainer = new BinsContainer();
            TimeSpan startTimeSpan = new TimeSpan();
            TimeSpan endTimeSpan = new TimeSpan();
            if (timeOptions.TimeOption == BinFactoryOptions.TimeOptions.TimePeriod &&
                timeOptions.TimeOfDayStartHour != null &&
                timeOptions.TimeOfDayStartMinute != null &&
                timeOptions.TimeOfDayEndHour != null &&
                timeOptions.TimeOfDayEndMinute != null)
            {
                startTimeSpan = new TimeSpan(0, timeOptions.TimeOfDayStartHour.Value,
                    timeOptions.TimeOfDayStartMinute.Value, 0);
                endTimeSpan = new TimeSpan(0, timeOptions.TimeOfDayEndHour.Value,
                    timeOptions.TimeOfDayEndMinute.Value, 0);
            }
            
            List<Bin> bins = new List<Bin>();
            for (DateTime startTime = timeOptions.Start; startTime < timeOptions.End; startTime = startTime.AddMinutes(minutes))
            {
                if (timeOptions.TimeOption == BinFactoryOptions.TimeOptions.StartToEnd)
                {
                    bins.Add(new Bin {Start = startTime, End = startTime.AddMinutes(minutes)});
                }
                else
                {
                    
                    TimeSpan periodStartTimeSpan = new TimeSpan(0, startTime.Hour,
                        startTime.Minute, 0);
                    if (timeOptions.DaysOfWeek.Contains(startTime.DayOfWeek) &&
                        periodStartTimeSpan >= startTimeSpan &&
                        periodStartTimeSpan < endTimeSpan)
                    {
                        bins.Add(new Bin { Start = startTime, End = startTime.AddMinutes(minutes) });
                    }
                }
            }
            binsContainer.Bins = bins;
            return binsContainer;
        }
    }
}