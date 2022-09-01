using ATSPM.IRepositories;
using ATSPM.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ATSPM.Infrastructure.Repositories.EntityFramework
{
    public class SpeedEventRepository : ISpeedEventRepository
    {
        private readonly MOEContext db;

        public SpeedEventRepository(MOEContext db)
        {
            this.db = db;
        }

        public List<SpeedEvent> GetSpeedEventsBySignal(DateTime startDate, DateTime endDate, Approach approach)
        {
            var speedEvents = new List<SpeedEvent>();
            foreach (var detector in approach.Detectors)
                speedEvents.AddRange(db.SpeedEvents.Where(s =>
                    s.DetectorId == detector.DetectorId && s.Timestamp >= startDate && s.Timestamp < endDate).ToList());
            return speedEvents;
        }

        public List<SpeedEvent> GetSpeedEventsByDetector(DateTime startDate, DateTime endDate, Detector detector,
            int minSpeedFilter)
        {
            //db.Database.CommandTimeout = 180;
            var speedEvents = (from r in db.SpeedEvents
                               where r.Timestamp > startDate
                                     && r.Timestamp < endDate
                                     && r.DetectorId == detector.DetectorId
                                     && r.Mph > minSpeedFilter
                               select r).ToList();
            return speedEvents;
        }
    }
}