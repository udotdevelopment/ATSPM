using ATSPM.Application.Models;
using System;
using System.Collections.Generic;

namespace ATSPM.IRepositories
{
    public interface IDetectorEventCountAggregationRepository
    {
        int GetDetectorEventCountSumAggregationByDetectorIdAndDateRange(int detectorId, DateTime start,
           DateTime end);


        List<DetectorEventCountAggregation> GetDetectorEventCountAggregationByDetectorIdAndDateRange(
            int detectorId, DateTime start,
            DateTime end);

        bool DetectorEventCountAggregationExists(int detectorId, DateTime start,
            DateTime end);
    }
}