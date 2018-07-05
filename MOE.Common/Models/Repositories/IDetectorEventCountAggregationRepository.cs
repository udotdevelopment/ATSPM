using System;
using System.Collections.Generic;

namespace MOE.Common.Models.Repositories
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