using System;
using System.Collections.Generic;
using AtspmApi.Models;

namespace AtspmApi.Repositories
{
    public interface IApproachYellowRedActivationsAggregationRepository
    {
        int GetApproachYellowRedActivationsCountAggByApproach(int versionId, DateTime start,
            DateTime end);

        List<ApproachYellowRedActivationAggregation>
            GetApproachYellowRedActivationssAggregationByApproach(int approachId, DateTime startDate,
                DateTime endDate);
    }
}