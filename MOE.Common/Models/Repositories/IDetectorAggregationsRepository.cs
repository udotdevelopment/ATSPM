using System;
using System.Collections.Generic;

namespace MOE.Common.Models.Repositories
{
    public interface IDetectorAggregationsRepository
    {
        List<DetectorAggregation> GetActivationsByDetectorIDandDateRange(int detectorId, DateTime Start, DateTime End);
        DetectorAggregation Add(Models.DetectorAggregation DetectorAggregation);
        void Update(Models.DetectorAggregation DetectorAggregation);
        void Remove(Models.DetectorAggregation DetectorAggregation);
    }
}