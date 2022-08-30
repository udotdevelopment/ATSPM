using ATSPM.Application.Models;
using System;
using System.Collections.Generic;

namespace ATSPM.IRepositories
{
    public interface ISpeedEventRepository
    {
        List<SpeedEvent> GetSpeedEventsByDetector(DateTime startDate, DateTime endDate, Detector detector,
            int minSpeedFilter);

        List<SpeedEvent> GetSpeedEventsBySignal(DateTime startDate, DateTime endDate, Approach approach);
    }
}