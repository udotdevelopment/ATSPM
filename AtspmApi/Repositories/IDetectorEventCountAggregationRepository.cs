using System;
using System.Collections.Generic;
using AtspmApi.Models;

namespace AtspmApi.Repositories
{
    public interface IDetectorEventCountAggregationRepository
    {
         int GetDetectorEventCountSumAggregationByDetectorIdAndDateRange(int detectorId, DateTime start,
            DateTime end);


        List<DetectorEventCountAggregation> GetDetectorEventCountAggregationByDetectorIdAndDateRange(
            int detectorId, DateTime start,
            DateTime end);
    }
}