using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOE.Common.Models.Repositories
{
    public class ArchivedMetricsRepository : IArchivedMetricsRepository
    {
        MOE.Common.Models.SPM db = new SPM();

        public DateTime GetLastArchiveRunDate()
        {
            var lastRunDate = (from d in db.Archived_Metrics
                               select d.Timestamp).Max();
            return lastRunDate;
        }

        public List<Models.Signal> GetIntersections(DateTime startDate, DateTime endDate, int region)
        {
             //MOE.Common.Business.ModelObjectHelpers.SignalModelHelper smh = new Business.ModelObjectHelpers.SignalModelHelper()
            var intersections = (from a in db.Archived_Metrics
                                join gd in db.Detectors on a.DetectorID equals gd.DetectorID
                                join appr in db.Approaches on gd.ApproachID equals appr.ApproachID
                                join s in db.Signals on appr.SignalID equals s.SignalID
                                where a.Timestamp < endDate
                                && s.RegionID == region
                                 && gd.DetectionTypeIDs.Contains(2)
                                select s).ToList();
            return intersections;
        }

        public List<Models.Detector> GetDetectors(DateTime startDate, DateTime endDate, int region)
        {
            
            var detectors = (from a in db.Archived_Metrics
                                join gd in db.Detectors on a.DetectorID equals gd.DetectorID
                                join appr in db.Approaches on gd.ApproachID equals appr.ApproachID
                                join s in db.Signals on appr.SignalID equals s.SignalID
                                where a.Timestamp < endDate
                                && s.RegionID == region
                               && gd.DetectionTypeIDs.Contains(2)
                                 select gd).ToList();
            return detectors;
        }

        public List<RegionArchiveMetric> GetRegionArchiveMetrics(DateTime start, DateTime end, int startHour, int endHour, List<DayOfWeek> dayTypes, int region)
        {
            
            var results = (from a in db.Archived_Metrics
                           join gd in db.Detectors on a.DetectorID equals gd.DetectorID
                           join s in db.Signals on gd.Approach.SignalID equals s.SignalID
                           where a.Timestamp >= start && a.Timestamp <= end
                           && gd.Approach.Signal.RegionID == region
                           && gd.DetectorSupportsThisMetric(6) == true
                           && dayTypes.Contains(a.Timestamp.DayOfWeek)
                           group new { a.Volume, a.speed, a.delay, a.AoR, a.SpeedHits, a.BinGreenTime } by
                           new { s.SignalID, gd.DetectorID, a.Timestamp.Year, a.Timestamp.Month, a.Timestamp.Day, a.Timestamp.Hour } into temp
                           orderby temp.Key.SignalID, temp.Key.DetectorID, temp.Key.Year, temp.Key.Month, temp.Key.Day, temp.Key.Hour
                           select new RegionArchiveMetric()
                           {
                               SignalID = temp.Key.SignalID,
                               DetectorID = temp.Key.DetectorID,
                               Year = temp.Key.Year,
                               Month = temp.Key.Month,
                               Day = temp.Key.Day,
                               Hour = temp.Key.Hour,
                               Volume = temp.Sum(x => x.Volume).Value,
                               Speed = temp.Sum(x => x.speed).Value,
                               Delay = temp.Sum(x => x.delay).Value,
                               AoR = temp.Sum(x => x.AoR).Value,
                               SpeedHits = temp.Sum(x => x.SpeedHits).Value,
                               BinGreenTime = temp.Sum(x => x.BinGreenTime).Value
                           }
                           ).ToList();


            return results;


        }


    }

    public class RegionArchiveMetric
    {
    public string SignalID { get; set; }
    public string DetectorID { get; set; }

    public int Year { get; set; }
    public int Month { get; set; }
    public int Day { get; set; }
    public int Hour { get; set; }
    public int Volume { get; set; }
    public int Speed { get; set; }
    public int Delay { get; set; }
    public int AoR { get; set; }
    public int SpeedHits { get; set; }
    public int BinGreenTime { get; set; }
    }

    
}
