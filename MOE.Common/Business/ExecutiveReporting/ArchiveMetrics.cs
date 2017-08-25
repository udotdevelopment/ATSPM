using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOE.Common.Business.ExecutiveReporting
{
    public class ArchiveMetrics
    {
        List<ArchiveMetricRegion> regions = new List<ArchiveMetricRegion>();
        List<ArchiveMetricCorridor> corridors = new List<ArchiveMetricCorridor>();

        private DateTime startDate;
        private DateTime endDate;
        double hoursInPeriod;
        double daysInPeriod;

        private double totalVolume = -1;
        public double TotalVolume
        {
            get
            {
                if (totalVolume == -1)
                {
                    totalVolume = regions.Sum(item => item.TotalVolume);
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
        //        return regions.Sum(d => d.ValidHours);
        //    }
        //}

        public double AverageDailyVolumePerApproach
        {
            get
            {
                return regions.Sum(d => d.TotalVolume) / ValidDaysValue;
            }
        }

        public double DailyVolume
        {
            get
            {
                return regions.Sum(d => d.TotalVolume) / daysInPeriod;
            }
        }

        public double ValidDaysValue
        {
            get
            {
                return regions.Sum(d => d.ValidDaysValue);
            }
        }

        private double aor = -1;
        public double Aor
        {
            get
            {
                if (aor == -1)
                {
                    aor = regions.Sum(item => item.Aor);
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

        private double totalDelay = -1;
        public double TotalDelay
        {
            get
            {
                if (totalDelay == -1)
                {
                    totalDelay = regions.Sum(item => item.TotalDelay);
                }
                return totalDelay;

            }
        }

        public double TotalDelayHours
        {
            get { return TotalDelay / 3600; }
        }

        public double DelayPerVehicle
        {
            get
            {
                return TotalDelay / TotalVolume ;
            }
        }

        private double greenTime = -1;
        public double GreenTime
        {
            get
            {
                if (greenTime == -1)
                {
                    greenTime = regions.Sum(item => item.GreenTime);
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
            get { return regions.Sum(d => d.HoursCount); }
        }

        public double DaysCount
        {
            get { return regions.Sum(d=> d.DaysCount); }
        }

        public double NumberOfApproaches
        {
            get { return regions.Sum(d => d.NumberOfApproaches); }
        }

        public double NumberOfIntersections
        {
            get { return regions.Sum(d => d.NumberOfIntersections); }
        }

        public double DelayPerApproach
        {
            get { return TotalDelay / NumberOfApproaches / ValidDaysValue; }
        }

        public double DelayPerApproachHrs
        {
            get { return DelayPerApproach /3600; }
        }
        
        /// <summary>
        /// Use this constructor for getting statistics on all detectors
        /// </summary>
        public ArchiveMetrics(DateTime StartDate, DateTime EndDate, Int32 StartHour, Int32 EndHour, List<DayOfWeek> DayTypes)
        {
            this.startDate = StartDate;
            this.endDate = EndDate;
            
            this.daysInPeriod = (EndDate - StartDate).TotalDays;
            this.hoursInPeriod = (EndHour - StartHour) * daysInPeriod;

            MOE.Common.Models.Repositories.IRegionsRepository regionRepository= 
                MOE.Common.Models.Repositories.RegionsRepositoryFactory.Create();
            List<MOE.Common.Models.Region> allRegions = regionRepository.GetAllRegions();
            

            foreach(MOE.Common.Models.Region region in allRegions)
            {
                    regions.Add(new ArchiveMetricRegion(StartDate, EndDate, StartHour, EndHour, region.ID, DayTypes, hoursInPeriod, daysInPeriod));
            }

            CreateCorridors();
        }

        public Data.ExecutiveReports.ExecutiveAnalysisDataTable GetAllData()
        {
            Data.ExecutiveReports.ExecutiveAnalysisDataTable table =
                new Data.ExecutiveReports.ExecutiveAnalysisDataTable();
            Data.ExecutiveReports.ExecutiveAnalysisRow stateRow =
                table.NewExecutiveAnalysisRow();
            stateRow.VolumeTotal = TotalVolume;
            stateRow.VolumePerDay = AverageDailyVolumePerApproach;
            stateRow.VolumePerHour = HourlyVolume;
            stateRow.AorTotal = Aor;
            stateRow.AorPercent = AorPercent;
            stateRow.AorPlatoonRatio = PlatoonRatio;
            stateRow.AorPerDay = AorPerDay;
            stateRow.DelayTotal = TotalDelay;
            stateRow.DelayPerVeh = DelayPerVehicle;
            stateRow.NumberOfApproaches = NumberOfApproaches;
            stateRow.NumberOfIntersections = NumberOfIntersections;
            stateRow.Type = "State";
            stateRow.DelayPerApproach = DelayPerApproachHrs;
            table.AddExecutiveAnalysisRow(stateRow);

            foreach (ArchiveMetricRegion region in regions)
            {
                Data.ExecutiveReports.ExecutiveAnalysisRow regionRow =
                table.NewExecutiveAnalysisRow();
                regionRow.VolumeTotal = region.TotalVolume;
                regionRow.VolumePerDay = region.AverageDailyVolumePerApproach;
                regionRow.VolumePerHour = region.HourlyVolume;
                regionRow.AorTotal = region.Aor;
                regionRow.AorPercent = region.AorPercent;
                regionRow.AorPlatoonRatio = region.PlatoonRatio;
                regionRow.AorPerDay = region.AorPerDay;
                regionRow.DelayTotal = region.TotalDelay;
                regionRow.DelayPerVeh = region.DelayPerVehicle;
                regionRow.DelayPerApproach = region.DelayPerApproachHrs;
                regionRow.Name = region.Region.ToString();
                regionRow.NumberOfApproaches = region.NumberOfApproaches;
                regionRow.NumberOfIntersections = region.NumberOfIntersections;
                regionRow.Type = "Region";
                regionRow.Region = region.Region.ToString();
                table.AddExecutiveAnalysisRow(regionRow);
            }

            foreach (ArchiveMetricRegion region in regions)
            {
                foreach (ArchiveMetricIntersection intersection in region.Intersections)
                {
                    Data.ExecutiveReports.ExecutiveAnalysisRow IntersectionRow =
                table.NewExecutiveAnalysisRow();
                    IntersectionRow.VolumeTotal = intersection.TotalVolume;
                    IntersectionRow.VolumePerDay = intersection.AverageDailyVolumePerApproach;
                    IntersectionRow.VolumePerHour = intersection.HourlyVolume;
                    IntersectionRow.AorTotal = intersection.Aor;
                    IntersectionRow.AorPercent = intersection.AorPercent;
                    IntersectionRow.AorPlatoonRatio = intersection.PlatoonRatio;
                    IntersectionRow.AorPerDay = intersection.AorPerDay;
                    IntersectionRow.DelayTotal = intersection.TotalDelay;
                    IntersectionRow.DelayPerVeh = intersection.DelayPerVehicle;
                    IntersectionRow.DelayPerApproach = intersection.DelayPerApproachHrs;
                    IntersectionRow.Name = intersection.Description;
                    IntersectionRow.NumberOfApproaches = intersection.NumberOfApproaches;
                    IntersectionRow.Type = "Intersection";
                    IntersectionRow.Region = intersection.Region.ToString();
                    table.AddExecutiveAnalysisRow(IntersectionRow);
                }
            }

            foreach (ArchiveMetricCorridor corridor in corridors)
            {
                Data.ExecutiveReports.ExecutiveAnalysisRow corridorRow =
                table.NewExecutiveAnalysisRow();
                corridorRow.VolumeTotal = corridor.TotalVolume;
                corridorRow.VolumePerDay = corridor.AverageDailyVolumePerApproach;
                corridorRow.VolumePerHour = corridor.HourlyVolume;
                corridorRow.AorTotal = corridor.Aor;
                corridorRow.AorPercent = corridor.AorPercent;
                corridorRow.AorPlatoonRatio = corridor.PlatoonRatio;
                corridorRow.AorPerDay = corridor.AorPerDay;
                corridorRow.DelayTotal = corridor.TotalDelay;
                corridorRow.DelayPerVeh = corridor.DelayPerVehicle;
                corridorRow.DelayPerApproach = corridor.DelayPerApproachHrs;
                corridorRow.Name = corridor.Description;
                corridorRow.NumberOfApproaches = corridor.NumberOfApproaches;
                corridorRow.Type = "Corridor";
                corridorRow.Region = corridor.Region.ToString();
                table.AddExecutiveAnalysisRow(corridorRow);
            }

            return table;
        }

        public Data.ExecutiveReports.StateAnalysisDataTable GetStateData()
        {
            Data.ExecutiveReports.StateAnalysisDataTable table = new Data.ExecutiveReports.StateAnalysisDataTable();
            Data.ExecutiveReports.StateAnalysisRow row = table.NewStateAnalysisRow();
            row.VolumeTotal = TotalVolume;
            row.VolumePerDay = AverageDailyVolumePerApproach;
            row.VolumePerHour = HourlyVolume;
            row.AorTotal = Aor;
            row.AorPercent = AorPercent;
            row.AorPlatoonRatio = PlatoonRatio;
            row.AorPerDay = AorPerDay;
            row.DelayTotal = DelayPerApproach;
            row.DelayPerVeh = DelayPerVehicle;
            row.NumberOfApproaches = NumberOfApproaches;
            row.NumberOfIntersections = NumberOfIntersections;
            table.AddStateAnalysisRow(row);
           
            return table;
        }


        public Data.ExecutiveReports.RegionAnalysisDataTable GetRegionData()
        {
            Data.ExecutiveReports.RegionAnalysisDataTable table = new Data.ExecutiveReports.RegionAnalysisDataTable();
            foreach (ArchiveMetricRegion region in regions)
            {
                Data.ExecutiveReports.RegionAnalysisRow row = table.NewRegionAnalysisRow();
                row.VolumeTotal = region.TotalVolume;
                row.VolumePerDay = region.AverageDailyVolumePerApproach;
                row.VolumePerHour = region.HourlyVolume;
                row.AorTotal = region.Aor;
                row.AorPercent = region.AorPercent;
                row.AorPlatoonRatio = region.PlatoonRatio;
                row.AorPerDay = region.AorPerDay;
                row.DelayTotal = region.DelayPerApproach;
                row.DelayPerVeh = region.DelayPerVehicle;
                row.Region = region.Region;
                row.NumberOfApproaches = region.NumberOfApproaches;
                row.NumberOfIntersections = region.NumberOfIntersections;
                table.AddRegionAnalysisRow(row);
            }
            return table;
        }

        public Data.ExecutiveReports.IntersectionAnalysisDataTable GetIntersectionData()
        {
            Data.ExecutiveReports.IntersectionAnalysisDataTable table =
                new Data.ExecutiveReports.IntersectionAnalysisDataTable();
            foreach(ArchiveMetricRegion region in regions)
            {
                foreach (ArchiveMetricIntersection intersection in region.Intersections)
                {
                    Data.ExecutiveReports.IntersectionAnalysisRow row = table.NewIntersectionAnalysisRow();
                    row.AorPercent = intersection.AorPercent;
                    row.AorPlatoonRatio = intersection.PlatoonRatio;
                    row.AorTotal = intersection.Aor;
                    row.AorPerDay = intersection.AorPerDay;
                    row.DelayPerVeh = intersection.DelayPerVehicle;
                    row.DelayTotal = intersection.DelayPerApproach;
                    row.Intersection = intersection.Description;
                    row.VolumePerDay = intersection.AverageDailyVolumePerApproach;
                    row.VolumePerHour = intersection.HourlyVolume;
                    row.VolumeTotal = intersection.TotalVolume;
                    row.NumberOfApproaches = intersection.NumberOfApproaches;
                    row.Region = intersection.Region;
                    table.AddIntersectionAnalysisRow(row);
                }
            }
            return table;
        }

        public Data.ExecutiveReports.CorridorAnalysisDataTable GetCorridorData()
        {
            Data.ExecutiveReports.CorridorAnalysisDataTable table = new Data.ExecutiveReports.CorridorAnalysisDataTable();
            foreach (ArchiveMetricCorridor corridor in corridors)
            {
                Data.ExecutiveReports.CorridorAnalysisRow row = table.NewCorridorAnalysisRow();
                row.VolumeTotal = corridor.TotalVolume;
                row.VolumePerDay = corridor.AverageDailyVolumePerApproach;
                row.VolumePerHour = corridor.HourlyVolume;
                row.AorTotal = corridor.Aor;
                row.AorPercent = corridor.AorPercent;
                row.AorPerDay = corridor.AorPerDay;
                row.AorPlatoonRatio = corridor.PlatoonRatio;
                row.DelayTotal = corridor.DelayPerApproach;
                row.DelayPerVeh = corridor.DelayPerVehicle;
                row.Corridor = corridor.Description;
                row.NumberOfApproaches = corridor.NumberOfApproaches;
                row.Region = corridor.Region;
                table.AddCorridorAnalysisRow(row);
            }
            return table;
        }

        private void CreateCorridors()
        {
            Models.Repositories.IRouteRepository routeRepository =
                Models.Repositories.RouteRepositoryFactory.Create();
            List<Models.Route> routes = routeRepository.SelectAllRoutes();
            foreach (Models.Route row in routes)
            {
                corridors.Add(new ArchiveMetricCorridor(row.RouteID, row.Description, startDate, endDate, 
                    hoursInPeriod, daysInPeriod, row.Region));
            }
            foreach (ArchiveMetricRegion region in regions)
            {
                foreach (ArchiveMetricIntersection intersection in region.Intersections)
                {
                    if (intersection != null)
                    {
                        foreach (ArchiveMetricDetector detector in intersection.Detectors)
                        {
                            if (detector != null && detector.Routes.Count > 0)
                            {
                                foreach (int route in detector.Routes)
                                {
                                    foreach (ArchiveMetricCorridor c in corridors)
                                    {
                                        if (c.CorridorId == route)
                                        {
                                            c.AddDetector(detector);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        
    }
}
