using System;
using System.Collections.Generic;
using System.Linq;

namespace MOE.Common.Models.Inrix.Repositories
{
    public class DataRepository : IDataRepository
    {
        private readonly Inrix db = new Inrix();

        public List<Datum> GetTravelTimes(string TMCCode, int confidence, List<DayOfWeek> validdays, DateTime start,
            DateTime end)
        {
            var TravelTimes = (from r in db.Data
                where r.tmc_code == TMCCode &&
                      validdays.Contains(r.measurement_tstamp.Value.DayOfWeek) &&
                      r.measurement_tstamp >= start && r.measurement_tstamp <= end
                select r).ToList();


            return TravelTimes;
        }
    }
}