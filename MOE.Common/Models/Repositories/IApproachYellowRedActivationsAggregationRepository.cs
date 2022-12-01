using System;
using System.Collections.Generic;
using MOE.Common.Business.DataAggregation;

namespace MOE.Common.Models.Repositories
{
    public interface IApproachYellowRedActivationsAggregationRepository: IAggregationRepositoryBase
    {
        int GetApproachYellowRedActivationsCountAggregationByApproachIdAndDateRange(int versionId, DateTime start,
            DateTime end);

        YellowRedActivationsAggregationByApproach Add(YellowRedActivationsAggregationByApproach priorityAggregation);
        void Update(YellowRedActivationsAggregationByApproach priorityAggregation);
        void Remove(YellowRedActivationsAggregationByApproach priorityAggregation);

        List<ApproachYellowRedActivationAggregation>
            GetApproachYellowRedActivationssAggregationByApproachIdAndDateRange(int approachId, DateTime startDate,
                DateTime endDate, bool getProtectedPhase);
    }
}