using System;
using System.Collections.Generic;

namespace MOE.Common.Models.Repositories
{
    public interface IDetectorAggregationsRepository
    {
        List<DetectorAggregation> GetActivationsByDetectorIDandDateRange(int detectorId, DateTime Start, DateTime End);
        DetectorAggregation Add(DetectorAggregation DetectorAggregation);
        void Update(DetectorAggregation DetectorAggregation);
        void Remove(DetectorAggregation DetectorAggregation);

        List<DetectorAggregation> GetDetectorAggregationByApproachIdAndDateRange(int detectorId, DateTime startDate,
            DateTime endDate);
    }
}