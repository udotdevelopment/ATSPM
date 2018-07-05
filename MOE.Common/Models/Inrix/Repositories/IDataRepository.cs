using System;
using System.Collections.Generic;

namespace MOE.Common.Models.Inrix.Repositories
{
    public interface IDataRepository
    {
        List<Datum> GetTravelTimes(string TMCCode, int confidence, List<DayOfWeek> validdays, DateTime start,
            DateTime end);
    }
}