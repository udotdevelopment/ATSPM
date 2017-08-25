using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOE.Common.Business.ExecutiveReporting
{
    public class ArchiveMetricIntersection
    {
        List<ArchiveMetricDetector> detectors = new List<ArchiveMetricDetector>();
        public List<ArchiveMetricDetector> Detectors
        {
            get { return detectors; }
        }

        string signalId;
        string primaryName;
        string secondaryName;
        DateTime start;
        DateTime end;
        double hoursInPeriod;
        double daysInPeriod;

        int region;
        public int Region
        {
            get { return region; }
        }

        public string Description
        {
            get
            {
                return primaryName + " " + secondaryName;
            }
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
                return TotalVolume / hoursInPeriod;
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

        public double AorPercent
        {
            get
            {
                return Aor / TotalVolume;
            }
        }

        public double DelayPerVehicle
        {
            get
            {
                return TotalDelay / TotalVolume;
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

        public double AorPerDay
        {
            get
            {
                return Aor /daysInPeriod;
            }
        }

        private double totalDelay = -1;
        public double TotalDelay
        {
            get
            {
                if (totalDelay == -1)
                {
                    totalDelay = detectors.Sum(item => item.TotalDelay) ;
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

        public ArchiveMetricIntersection(string signalId, string primaryName, string secondaryName,
            int region, DateTime start, DateTime end, List<MOE.Common.Models.Route_Detectors> routeTable, double hours,
            double days, //Data.ExecutiveReports.ArchiveMetricRegionAnalysisDataTable 
            List<MOE.Common.Models.Repositories.RegionArchiveMetric> 
            regionArchiveData,
            List<MOE.Common.Models.Detector> graphDetectors)
        {
            this.signalId = signalId;
            this.primaryName = primaryName;
            this.secondaryName = secondaryName;
            this.start = start;
            this.end = end;
            this.hoursInPeriod = hours;
            this.daysInPeriod = days;
            this.region = region;

            //Data.MOETableAdapters.GraphDetectorIDsTableAdapter adapter =
            //    new Data.MOETableAdapters.GraphDetectorIDsTableAdapter();

            //DataRow[] rows = graphDetectors "SignalId = " + signalId);

            //Pending the executive report rewrite
            //var rows = (from g in graphDetectors
            //           where g.SignalID == signalId
            //           select g).ToList();

            //Parallel.ForEach(rows, drow =>
            //    {
            //        detectors.Add(new ArchiveMetricDetector(start, end, drow.DetectorID,
            //            routeTable, hours, daysInPeriod, regionArchiveData));
            //    });
        }
    }
}
