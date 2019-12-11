using System;
using System.Collections.Generic;
using AtspmApi.Models;

namespace AtspmApi.Repositories
{
    public interface IApproachEventCountAggregationRepository
    {
        int GetPhaseEventCountAggByApproach(int approachId, DateTime start, DateTime end);

        List<ApproachEventCountAggregation> GetApproachEventCountAggregationByApproach(int approachId, DateTime start,
            DateTime end);
    }
}