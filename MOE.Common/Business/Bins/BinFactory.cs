using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Metadata.W3cXsd2001;

namespace MOE.Common.Business.Bins
{
    public class BinFactory
    {
        BinSizes BinSize { get; set; }

        //public static List<KeyValuePair<string, int>> BinSizes = new List<KeyValuePair<string, int>>();

        public List<Bin>Bins = new List<Bin>();

        public enum BinSizes {Fifteen, Hour,  Day, Week, Month, Year};

        public int TODStartHour { get; set; }
        public int TODStartMinute { get; set; }
        public int TODEndHour { get; set; }
        public int TODEndMinute { get; set; }
        public BinFactory( BinSizes binSize, DateTime start, DateTime end, int todStartHour, int todStartMinute, int todEndHour, int todEndMinute, List<DayOfWeek> daysOfWeek)
        {
            TODStartHour = todStartHour;
            TODStartMinute = todStartMinute;
            TODEndHour = todEndHour;
            TODEndMinute = todEndMinute;
            //PopulateBinSizeList();

            BinSize = binSize;

            BinEngine(start, end, daysOfWeek);
        }

        private void BinEngine(DateTime startDate, DateTime endDate, List<DayOfWeek> daysOfWeek)
        {


            switch (BinSize)
            {

                case BinSizes.Fifteen:
                {
                    CreateFifteenMinuteBins(startDate, endDate, daysOfWeek);
                }

                 break;

                case BinSizes.Hour:
                {
                    CreateHourBins(startDate, endDate, daysOfWeek);
                }
                    break;

                case BinSizes.Day:
                {
                    CreateDayBins(startDate, endDate, daysOfWeek);
                }
                    break;

                case BinSizes.Week:
                {
                    CreateWeekBins(startDate, endDate, daysOfWeek);
                }
                    break;

                case BinSizes.Month:
                {
                    CreateMonthBins(startDate, endDate, daysOfWeek);
                }
                    break;

                case BinSizes.Year:
                {

                    CreateYearBins( startDate,  endDate,  daysOfWeek);
                }
                break;
        }
            ;


            //Get the dates that match the daytype for the given period



        }

        private void CreateFifteenMinuteBins(DateTime startDate, DateTime endDate, List<DayOfWeek> daysOfWeek)
        {
            DateTime tempDate = startDate;
            List<DateTime>dates = GetDateList(daysOfWeek, tempDate, endDate);


            while (tempDate <= endDate)
            {
                if(dates.Contains(tempDate))
                {
                    DateTime segStart = tempDate.Date.AddHours(TODStartHour).AddMinutes(TODStartMinute);
                    DateTime segEnd = tempDate.Date.AddHours(TODEndHour).AddMinutes(TODEndMinute);

                    while (segStart <= segEnd)
                    {
                        Bin bin = new Bin
                        {
                            Start = segStart,
                            End = segStart.AddMinutes(15)


                        };
                        segStart = segStart.AddMinutes(15);
                        Bins.Add(bin);
                    }
                }

                tempDate = tempDate.AddDays(1);

            }
        }
        private void CreateHourBins(DateTime startDate, DateTime endDate, List<DayOfWeek> daysOfWeek)
        {
            DateTime tempDate = startDate;
            List<DateTime> dates = GetDateList(daysOfWeek, tempDate, endDate);


            while (tempDate <= endDate)
            {
                if (dates.Contains(tempDate))
                {
                    DateTime segStart = tempDate.Date.AddHours(TODStartHour).AddMinutes(TODStartMinute);
                    DateTime segEnd = tempDate.Date.AddHours(TODEndHour).AddMinutes(TODEndMinute);

                    while (segStart <= segEnd)
                    {
                        Bin bin = new Bin
                        {
                            Start = segStart,
                            End = segStart.AddHours(1)


                        };
                        segStart = segStart.AddHours(1);
                        Bins.Add(bin);
                    }
                }

                tempDate = tempDate.AddDays(1);

            }
        }
        private void CreateDayBins(DateTime startDate, DateTime endDate, List<DayOfWeek> daysOfWeek)
        {
            DateTime tempDate = startDate.Date;
            List<DateTime> dates = GetDateList(daysOfWeek, tempDate, endDate);

            while (tempDate <= endDate)
            {
                if (dates.Contains(tempDate))
                {
                    LargeBin bin = new LargeBin
                    {
                        Start = tempDate,
                        End = tempDate.AddDays(1),
                        DaysOfWeek = daysOfWeek,
                        TODStartHour = TODStartHour,
                        TODStartMinute = TODStartMinute,
                        TODEndHour = TODEndHour,
                        TODEndMinute = TODEndMinute,
                        Dates = new List<DateTime>()
                    };

                    Bins.Add(bin);
                }

                tempDate = tempDate.AddDays(1);

            }
        }



        private void CreateWeekBins(DateTime startDate, DateTime endDate, List<DayOfWeek> daysOfWeek)
        {
            DateTime tempDate = startDate;

            while (tempDate <= endDate)
            {
                LargeBin bin = new LargeBin
                {
                    Start = tempDate,
                    End = tempDate.AddDays(7),
                    DaysOfWeek = daysOfWeek,
                    TODStartHour = TODStartHour,
                    TODStartMinute = TODStartMinute,
                    TODEndHour = TODEndHour,
                    TODEndMinute = TODEndMinute,
                    Dates = GetDateList(daysOfWeek, tempDate, tempDate.AddDays(7))
                };

                Bins.Add(bin);

                tempDate = tempDate.AddDays(7);

            }
        }

        private void CreateYearBins(DateTime startDate, DateTime endDate, List<DayOfWeek> daysOfWeek)
        {
            DateTime tempDate = startDate;

            while (tempDate <= endDate)
            {
                LargeBin bin = new LargeBin
                {
                    Start = tempDate,
                    End = tempDate.AddYears(1),
                    DaysOfWeek = daysOfWeek,
                    TODStartHour = TODStartHour,
                    TODStartMinute = TODStartMinute,
                    TODEndHour = TODEndHour,
                    TODEndMinute = TODEndMinute,
                    Dates = GetDateList(daysOfWeek, tempDate, tempDate.AddYears(1))
                };

                Bins.Add(bin);

                tempDate = tempDate.AddYears(1);

            }
             
        }

        private void CreateMonthBins(DateTime startDate, DateTime endDate, List<DayOfWeek> daysOfWeek)
        {
            DateTime tempDate = startDate;

            while (tempDate <= endDate)
            {
                LargeBin bin = new LargeBin
                {
                    Start = tempDate,
                    End = tempDate.AddMonths(1),
                    DaysOfWeek = daysOfWeek,
                    TODStartHour = TODStartHour,
                    TODStartMinute = TODStartMinute,
                    TODEndHour = TODEndHour,
                    TODEndMinute = TODEndMinute,
                    Dates = GetDateList(daysOfWeek, tempDate, tempDate.AddMonths(1))
                };

                Bins.Add(bin);

                tempDate = tempDate.AddMonths(1);

            }

        }




        private List<DateTime>  GetDateList(List<DayOfWeek> daysOfWeek, DateTime startDay, DateTime endDay )
        {
            List<DateTime> dtList = new List<DateTime>();
            DateTime tempDate = startDay;

            while (tempDate <= endDay)
            {
                if (daysOfWeek.Contains(tempDate.DayOfWeek))
                {
                    dtList.Add(tempDate);
                }
                tempDate = tempDate.AddDays(1);
            }

            return dtList;
        }



    }


}