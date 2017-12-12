using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOE.Common.Models.Repositories
{
    public class SpeedEventRepository : ISpeedEventRepository
    {
        Models.SPM db = new Models.SPM();

        public List<Speed_Events> GetSpeedEventsBySiganl(DateTime startDate, DateTime endDate, Models.Approach approach)
        {
            var speedEvents = new List<Speed_Events>();
            foreach (var detector in approach.Detectors)
            {
                speedEvents.AddRange(db.Speed_Events.Where(s => s.DetectorID == detector.DetectorID && s.timestamp >= startDate && s.timestamp < endDate).ToList());    
            }
            return speedEvents;
        }

        public List<Speed_Events> GetSpeedEventsByDetector(DateTime startDate, DateTime endDate, Models.Detector detector, int minSpeedFilter)
        {

            this.db.Database.CommandTimeout = 180;
            List<Models.Speed_Events> speedEvents = (from r in db.Speed_Events
                where r.timestamp > startDate
                      && r.timestamp < endDate
                      && r.DetectorID == detector.DetectorID 
                      && r.MPH > minSpeedFilter
                select r).ToList();
            return speedEvents;
        }
    }
}
