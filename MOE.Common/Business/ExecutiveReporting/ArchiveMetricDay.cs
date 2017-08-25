using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace MOE.Common.Business.ExecutiveReporting
{
    public class ArchiveMetricDay
    {
        private DateTime day;
        double hoursInPeriod;
        List<ArchiveMetricHour> hours = new List<ArchiveMetricHour>();
        public List<ArchiveMetricHour> Hours
        {
            get 
            {
                return hours;
            }
        }

        private double totalVolume = -1;
        public double TotalVolume
        {
            get {
                if (totalVolume == -1)
                {
                    totalVolume = hours.Where(d => d.ValidVolume == true).Sum(item => item.Volume);
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

        public double ValidHours
        {
            get
            {
                return hours.Where(d => d.ValidVolume == true).Count();
            }
        }

        public double ValidDayValue
        {
            get
            {
                return ValidHours/24;
            }
        }

        //private bool validDay;
        //public bool ValidDay
        //{
        //    get
        //    {
        //        if (ValidHours == 24)
        //            {
        //                validDay = true;
        //            }
        //            else
        //            {
        //                validDay = false;
        //            }
                
        //        return validDay;
        //    }
        //}

        private double aor = -1;
        public double Aor
        {
            get
            {
                if (aor == -1)
                {
                    aor = hours.Where(d => d.ValidVolume == true).Sum(item => item.Aor);
                }
                return aor;

            }
        }

        private double totalDelay = -1;
        public double TotalDelay
        {
            get
            {
                if (totalDelay == -1)
                {
                    totalDelay = hours.Where(d => d.ValidVolume == true).Sum(item => item.Delay);
                }
                return totalDelay;

            }
        }

        private double greenTime = -1;
        public double GreenTime
        {
            get
            {
                if (greenTime == -1)
                {
                    greenTime = hours.Where(d => d.ValidVolume == true).Sum(item => item.GreenTime);
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

        public double HoursCount
        {
            get { return hours.Count; }
        }

        public ArchiveMetricDay(List<MOE.Common.Models.Repositories.RegionArchiveMetric> data, DateTime day, double hrs)
        {
            this.day = day;
            this.hoursInPeriod = hrs;
            foreach (MOE.Common.Models.Repositories.RegionArchiveMetric row in data)
            {
                hours.Add(new ArchiveMetricHour(row.Volume, row.Speed, row.Delay,
                    row.AoR, row.SpeedHits, row.BinGreenTime));
            }
            CheckData();
        }

        private void CheckData()
        {
            for (int i = 7; i < hours.Count - 1; i++)
            {
                if (hours[i - 1].Volume == 0 && hours[i].Volume == 0)
                {
                    hours[i - 1].ValidVolume = false;
                    hours[i].ValidVolume = false;
                }
                //if (hours[i - 1].SpeedHits == 0 && hours[i].SpeedHits == 0)
                //{
                //    hours[i - 1].ValidSpeed = false;
                //    hours[i].ValidSpeed = false;
                //}
            }
        }
    }
}
