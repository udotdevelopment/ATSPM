using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOE.Common.Models.Inrix.Repositories
{
    public interface IDataRepository
    {
        List<Models.Inrix.Datum> GetTravelTimes(string TMCCode, int confidence, List<DayOfWeek> validdays, DateTime start, DateTime end);
    }
}
