using System;
using System.Collections.Generic;
using System.Linq;

namespace MOE.Common.Models.Repositories
{
    public class SpeedEventRepository : ISpeedEventRepository
    {
        private readonly SPM db = new SPM();

        public List<Speed_Events> GetSpeedEventsBySignal(DateTime startDate, DateTime endDate, Approach approach)
        {
            var speedEvents = new List<Speed_Events>();
            foreach (var detector in approach.Detectors)
                speedEvents.AddRange(db.Speed_Events.Where(s =>
                    s.DetectorID == detector.DetectorID && s.timestamp >= startDate && s.timestamp < endDate).ToList());
            return speedEvents;
        }

        public List<Speed_Events> GetSpeedEventsByDetector(DateTime startDate, DateTime endDate, Detector detector,
            int minSpeedFilter)
        {
            db.Database.CommandTimeout = 180;
            var speedEvents = (from r in db.Speed_Events
                where r.timestamp > startDate
                      && r.timestamp < endDate
                      && r.DetectorID == detector.DetectorID
                      && r.MPH > minSpeedFilter
                select r).ToList();
            return speedEvents;
        }
    }
}