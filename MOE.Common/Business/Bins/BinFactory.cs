using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;

namespace MOE.Common.Business.Bins
{

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
                    return GetYearBinsForRange(timeOptions);
                default:
                    return GetBinsForRange(timeOptions, 15);
            }
        }

        private static BinsContainer GetYearBinsForRange(BinFactoryOptions timeOptions)
        {

            BinsContainer binsContainer = new BinsContainer();



            List<Bin> bins = new List<Bin>();
            for (DateTime startTime = new DateTime(timeOptions.Start.Year, timeOptions.Start.Month, 1); startTime.Year <= timeOptions.End.Year && startTime.Month <= timeOptions.End.Month; startTime = startTime.AddYears(1))
            {
                if (timeOptions.TimeOption == BinFactoryOptions.TimeOptions.StartToEnd)
                {
                    bins.Add(new Bin { Start = startTime, End = startTime.AddYears(1) });
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

        private static BinsContainer GetMonthBinsForRange(BinFactoryOptions timeOptions)
        {
            //BinsContainer containers = new BinsContainer();

            //for (DateTime startTime = new DateTime(timeOptions.Start.Year, timeOptions.Start.Month, 1); startTime.Year <= timeOptions.End.Year && startTime.Month <= timeOptions.End.Month; startTime = startTime.AddMonths(1))
            //{

            //    List<Bin> bins = new List<Bin>();
            //    var filteredDates = from d in timeOptions.DateList
            //        where d >= startTime.Date && d < startTime.Date.AddMonths(1)
            //        select d;

            //    foreach (var date in filteredDates)
            //    {
            //        if (timeOptions.TimeOfDayStartHour != null)
            //            bins.Add(new Bin { Start = date.Date, End = date.Date.AddHours(23).AddMinutes(59).AddSeconds(59) });
            //    }


            //    BinsContainer binsContainer = new BinsContainer();
            //    binsContainer.Start = startTime;
            //    binsContainer.End = startTime.AddYears(1);

            //    binsContainer.Bins = bins;
            //    containers.BinsContainers.Add(binsContainer);
            //}

            //return containers;

            BinsContainer binsContainer = new BinsContainer();



            List<Bin> bins = new List<Bin>();
            for (DateTime startTime = new DateTime(timeOptions.Start.Year, timeOptions.Start.Month, 1); startTime.Year <= timeOptions.End.Year && startTime.Month <= timeOptions.End.Month; startTime = startTime.AddMonths(1))
            {
                if (timeOptions.TimeOption == BinFactoryOptions.TimeOptions.StartToEnd)
                {
                    bins.Add(new Bin { Start = startTime, End = startTime.AddMonths(1) });
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
            //BinsContainer containers = new BinsContainer();

            //for (DateTime startTime = timeOptions.Start; startTime < timeOptions.End; startTime = startTime.AddMinutes(minutes)) 
            //{

            //    List<Bin> bins = new List<Bin>();
            //    var filteredDates = from d in timeOptions.DateList
            //        where d >= startTime.Date && d < startTime.Date.AddMonths(1)
            //        select d;

            //    foreach (var date in filteredDates)
            //    {
            //        if (timeOptions.TimeOfDayStartHour != null)
            //            bins.Add(new Bin { Start = date.Date, End = date.Date.AddHours(23).AddMinutes(59).AddSeconds(59) });
            //    }


            //    BinsContainer binsContainer = new BinsContainer();
            //    binsContainer.Start = startTime;
            //    binsContainer.End = startTime.AddYears(1);

            //    binsContainer.Bins = bins;
            //    containers.BinsContainers.Add(binsContainer);
            //}

            //return containers;

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
                    bins.Add(new Bin { Start = startTime, End = startTime.AddMinutes(minutes) });
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