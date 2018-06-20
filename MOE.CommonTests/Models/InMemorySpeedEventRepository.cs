using MOE.Common.Models;
using MOE.Common.Models.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOE.CommonTests.Models
{
    public class InMemorySpeedEventRepository : ISpeedEventRepository
    {
        private InMemoryMOEDatabase _db;

        public InMemorySpeedEventRepository(InMemoryMOEDatabase db)
        {
            this._db = db;
        }

        public InMemorySpeedEventRepository()
        {
            _db = new InMemoryMOEDatabase();
        }


        public List<Speed_Events> GetSpeedEventsByDetector(DateTime startDate, DateTime endDate, Detector detector, int minSpeedFilter)
        {
            var speedEvents = (from r in _db.Speed_Events
                               where r.timestamp > startDate
                                     && r.timestamp < endDate
                                     && r.DetectorID == detector.DetectorID
                                     && r.MPH > minSpeedFilter
                               select r).ToList();
            return speedEvents;
        }

        public List<Speed_Events> GetSpeedEventsBySignal(DateTime startDate, DateTime endDate, Approach approach)
        {

            var speedEvents = new List<Speed_Events>();
            foreach (var detector in approach.Detectors)
                speedEvents.AddRange(_db.Speed_Events.Where(s =>
                    s.DetectorID == detector.DetectorID && s.timestamp >= startDate && s.timestamp < endDate).ToList());
            return speedEvents;


        }
    }
}
