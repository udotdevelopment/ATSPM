using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace MOE.Common.Business.ExecutiveReporting
{
    public class ArchiveMetricDetector
    {
        double hoursInPeriod;
        double daysInPeriod;

        List<ArchiveMetricDay> days = new List<ArchiveMetricDay>();
        public List<ArchiveMetricDay> Days
        {
            get
            {
                return days;
            }
        }

        List<int> routes = new List<int>();
        public List<int> Routes
        {
            get
            {
                return routes;
            }
        }

        DateTime start;
        DateTime end;
        string detectorId;

        private double greenTime = -1;
        public double GreenTime
        {
            get
            {
                if (greenTime == -1)
                {
                    greenTime = days.Sum(item => item.GreenTime);
                }
                return greenTime;

            }
        }

        public double PercentGreen
        {
            get
            {
                return GreenTime / (HoursCount * 3600);
            }
        }

        public double Aog
        {
            get { return TotalVolume - Aor; }
        }

        public double PercentAog
        {
            get { return Aog / TotalVolume; }
        }

        public double PlatoonRatio
        {
            get { return PercentAog / PercentGreen; }
        }

        private double totalVolume = -1;
        public double TotalVolume
        {
            get
            {
                if (totalVolume == -1)
                {
                    totalVolume = days.Sum(item => item.TotalVolume);
                }
                return totalVolume;

            }
        }

        public double HourlyVolume
        {
            get
            {
                return TotalVolume / hoursInPeriod;
            }
        }

        //public double ValidHours
        //{
        //    get
        //    {
        //        return days[0].ValidHours;
        //    }
        //} 

        public double AverageDailyVolume
        {
            get
            {
                return days.Average(d => d.TotalVolume);
            }
        }

        public double DailyVolume
        {
            get
            {
                return days.Sum(d => d.TotalVolume) / ValidDaysValue;
            }
        }

        public double ValidDaysValue
        {
            get
            {
                return days.Sum(d => d.ValidDayValue);
            }
        }

        private double aor = -1;
        public double Aor
        {
            get
            {
                if (aor == -1)
                {
                    aor = days.Sum(item => item.Aor);
                }
                return aor;

            }
        }
        
        public double AorPerDay
        {
            get
            {
                return Aor / DaysCount;
            }
        }

        private double totalDelay = -1;
        public double TotalDelay
        {
            get
            {
                if (totalDelay == -1)
                {
                    totalDelay = days.Sum(item => item.TotalDelay);
                }
                return totalDelay;

            }
        }

        public double AverageDelayPerDay
        {
            get { return TotalDelay / ValidDaysValue; }
        }

        public double HoursCount
        {
            get { return days.Sum(d => d.HoursCount); }
        }

        public double DaysCount
        {
            get { return days.Count; }
        }



        public ArchiveMetricDetector(DateTime start, DateTime end, string detectorId,

            List<MOE.Common.Models.Route_Detectors> routeTable, double hours,
            double days, List<MOE.Common.Models.Repositories.RegionArchiveMetric> regionTable)
        {
            this.start = start;
            this.end = end;
            this.detectorId = detectorId;
            this.hoursInPeriod = hours;
            this.daysInPeriod = days;
            GetStatistics(regionTable);
            SetRoutes(routeTable);
        }

        private void SetRoutes(List<MOE.Common.Models.Route_Detectors> routeTable)
        {
            routes = (from r in routeTable
                      where r.DetectorID == detectorId
                      select r.RouteID).ToList();
        }

        private void GetStatistics(List<MOE.Common.Models.Repositories.RegionArchiveMetric> detectorTable)
        {
            DateTime tempDate = start;
            while (tempDate < end)
            {
                var dataRows = (from d in detectorTable
                                where d.DetectorID == detectorId
                                && d.Year == tempDate.Year
                                && d.Month == tempDate.Month
                                && d.Day == tempDate.Day
                                select d).ToList();
                days.Add(new ArchiveMetricDay(dataRows, tempDate, hoursInPeriod));
                tempDate = tempDate.AddDays(1);


            }
        }
    }
}
