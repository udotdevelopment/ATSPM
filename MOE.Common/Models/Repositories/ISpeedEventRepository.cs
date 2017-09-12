using System;
using System.Collections.Generic;

namespace MOE.Common.Models.Repositories
{
    public interface ISpeedEventRepository
    {
        List<Speed_Events> GetSpeedEventsByDetector(DateTime startDate, DateTime endDate, Detector detector);
        List<Speed_Events> GetSpeedEventsBySiganl(DateTime startDate, DateTime endDate, Approach approach);
    }
}