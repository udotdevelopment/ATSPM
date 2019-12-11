using System;
using System.Collections.Generic;
using AtspmApi.Models;

namespace AtspmApi.Repositories
{
    public interface ISpeedEventRepository
    {
        List<Speed_Events> GetSpeedEventsByDetector(DateTime startDate, DateTime endDate, Detector detector,
            int minSpeedFilter);

        List<Speed_Events> GetSpeedEventsBySignal(DateTime startDate, DateTime endDate, Approach approach);
    }
}