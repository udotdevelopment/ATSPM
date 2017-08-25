using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOE.Common.Business.ExecutiveReporting
{

    public class ArchiveMetricRegion
    {
        private List<ArchiveMetricIntersection> intersections = new List<ArchiveMetricIntersection>();
        public List<ArchiveMetricIntersection> Intersections
        {
            get
            {
                return intersections;
            }
        }

        DateTime start;
        DateTime end;
        int region;
        double hoursInPeriod;
        double daysInPeriod;

        public int Region
        {
            get
            {
                return region;
            }
        }

        private double totalVolume = -1;
        public double TotalVolume
        {
            get
            {
                if (totalVolume == -1)
                {
                    totalVolume = intersections.Sum(item => item.TotalVolume);
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
        //        return intersections.Sum(d => d.ValidHours);
        //    }
        //}

        public double AverageDailyVolumePerApproach
        {
            get
            {
                return intersections.Sum(d => d.TotalVolume) / ValidDaysValue;
            }
        }

        public double DailyVolume
        {
            get
            {
                return intersections.Sum(d => d.TotalVolume) / daysInPeriod;
            }
        }

        public double ValidDaysValue
        {
            get
            {
                return intersections.Sum(d => d.ValidDaysValue);
            }
        }

        private double aor = -1;
        public double Aor
        {
            get
            {
                if (aor == -1)
                {
                    aor = intersections.Sum(item => item.Aor);
                }
                return aor;

            }
        }

        public double AorPerDay
        {
            get
            {
                return Aor / daysInPeriod;
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


        private double totalDelay = -1;
        public double TotalDelay
        {
            get
            {
                if (totalDelay == -1)
                {
                    totalDelay = intersections.Sum(item => item.TotalDelay) ;
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
                    greenTime = intersections.Sum(item => item.GreenTime);
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
            get { return intersections.Sum(d => d.HoursCount); }
        }

        public double DaysCount
        {
            get { return intersections.Sum(d => d.DaysCount); }
        }

        public double NumberOfApproaches
        {
            get { return intersections.Sum(d => d.NumberOfApproaches); }
        }

        public double NumberOfIntersections
        {
            get { return intersections.Count; }
        }

        public double DelayPerApproach
        {
            get { return TotalDelay / NumberOfApproaches / ValidDaysValue; }
        }

        public double DelayPerApproachHrs
        {
            get { return DelayPerApproach / 3600; }
        }

        public ArchiveMetricRegion(DateTime start, DateTime end, int startHour, int endHour, int region, List<DayOfWeek> dayTypes, double hours, double days)
        {
            //Pending the executive report rewrite
            //this.start = start;
            //this.end = end;
            //this.region = region;
            //this.hoursInPeriod = hours;
            //this.daysInPeriod = days;

            ////Get all signals with data for this period
            //Models.Repositories.IArchivedMetricsRepository archivedMetricsRepository = Models.Repositories.ArchivedMetricsRepositoryFactory.CreateArchivedMetricRepository();
            //Models.Repositories.RouteRepository routeRepository = new Models.Repositories.RouteRepository();

            ////List<Models.Route> routes = routeRepository.SelectAllRoutes();
            //List<Models.Signal> signals = archivedMetricsRepository.GetIntersections(start, end, region);
            //List<Models.Detectors> detectors = archivedMetricsRepository.GetDetectors(start, end, region);

            ////Get the Routes to associate them with the detectors
            //Models.Repositories.IRouteDetectorsRepository routeDetectorRepository =
            //    Models.Repositories.RouteDetectorRepositoryFactory.CreateRouteDetectorRepository();
            //List<Models.Route_Detectors> routeDetectors = routeDetectorRepository.SelectByRegion(region);

            ////Get all data for this region            
            //List<Models.Repositories.RegionArchiveMetric> archiveRegionData = 
            //    archivedMetricsRepository.GetRegionArchiveMetrics(start, end, startHour, endHour, dayTypes, region);
            //Parallel.ForEach(signals, drow =>
            //    {
                    
            //        intersections.Add(new ArchiveMetricIntersection(drow.SignalID,
            //            drow.PrimaryName, drow.Secondary_Name, region, start, end, routeDetectors,
            //            hoursInPeriod, daysInPeriod, archiveRegionData, detectors
            //            ));
            //    });
        }
    }
}
