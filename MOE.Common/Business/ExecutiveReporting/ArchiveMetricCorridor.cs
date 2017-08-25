using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MOE.Common.Business.ExecutiveReporting
{
    public class ArchiveMetricCorridor
    {
        double hoursInPeriod;
        double daysInPeriod;

        List<ArchiveMetricDetector> detectors = new List<ArchiveMetricDetector>();
        public List<ArchiveMetricDetector> Detectors
        {
            get { return detectors; }
        }

        private double totalVolume = -1;
        public double TotalVolume
        {
            get
            {
                if (totalVolume == -1)
                {
                    totalVolume = detectors.Sum(item => item.TotalVolume);
                }
                return totalVolume;

            }
        }

        public double HourlyVolume
        {
            get
            {
                return TotalVolume / HoursCount;
            }
        }

        //public double ValidHours
        //{
        //    get
        //    {
        //        return detectors.Sum(d => d.ValidHours);
        //    }
        //}

        public double AverageDailyVolumePerApproach
        {
            get
            {
                return detectors.Sum(d => d.TotalVolume) / ValidDaysValue;
            }
        }

        public double DailyVolume
        {
            get
            {
                return detectors.Sum(d => d.TotalVolume) / daysInPeriod;
            }
        }

        public double ValidDaysValue
        {
            get
            {
                return detectors.Sum(d => d.ValidDaysValue);
            }
        }

        private double aor = -1;
        public double Aor
        {
            get
            {
                if (aor == -1)
                {
                    aor = detectors.Sum(item => item.Aor);
                }
                return aor;

            }
        }

        public double AorPercent
        {
            get
            {
                return Aor / TotalVolume;
            }
        }

        public double AorPerDay
        {
            get
            {
                return Aor / daysInPeriod;
            }
        }

        public double DelayPerVehicle
        {
            get
            {
                return TotalDelay / TotalVolume;
            }
        }

        private double totalDelay = -1;
        public double TotalDelay
        {
            get
            {
                if (totalDelay == -1)
                {
                    totalDelay = detectors.Sum(item => item.TotalDelay);
                }
                return totalDelay;

            }
        }

        public double TotalDelayHours
        {
            get { return TotalDelay / 3600; }
        }

        private double greenTime = -1;
        public double GreenTime
        {
            get
            {
                if (greenTime == -1)
                {
                    greenTime = detectors.Sum(item => item.GreenTime);
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
            get { return detectors.Sum(d => d.HoursCount); }
        }

        public double DaysCount
        {
            get { return detectors.Sum(d=> d.DaysCount); }
        }

        private DateTime start;
        private DateTime end;

        private int region;
        public int Region
        {
            get { return region; }
        } 

        private int corridorId;
        public int CorridorId
        {
            get { return corridorId; }
        }

        private string description;
        public string Description
        {
            get { return description; }
        }

        public double NumberOfApproaches
        {
            get { return detectors.Count; }
        }        

        public double DelayPerApproach
        {
            get { return TotalDelay / NumberOfApproaches / ValidDaysValue; }
        }

        public double DelayPerApproachHrs
        {
            get { return DelayPerApproach / 3600; }
        }

        public ArchiveMetricCorridor(int corridorId, string description, DateTime start, DateTime end,
            double hours, double days, int region)
        {
            this.corridorId = corridorId;
            this.description = description;
            this.hoursInPeriod = hours;
            this.daysInPeriod = days;
            this.region = region;
        }

        public void AddDetector(ArchiveMetricDetector d)
        {
            detectors.Add(d);
        }
    }
}
