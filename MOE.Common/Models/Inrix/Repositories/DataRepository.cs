using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOE.Common.Models.Inrix.Repositories
{
    public class DataRepository : IDataRepository
    {

        MOE.Common.Models.Inrix.Inrix db = new Inrix();

        public List<Models.Inrix.Datum> GetTravelTimes(string TMCCode, int confidence, List<DayOfWeek> validdays, DateTime start, DateTime end)
        {
            List<Models.Inrix.Datum> TravelTimes = (from r in db.Data
                                                   where r.tmc_code == TMCCode &&
                                                   validdays.Contains(r.measurement_tstamp.Value.DayOfWeek) &&
                                                   r.measurement_tstamp >= start && r.measurement_tstamp <= end
                                                    select r).ToList();



            return TravelTimes;
        }
    }
}
