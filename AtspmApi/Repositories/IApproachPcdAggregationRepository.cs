using System;
using System.Collections.Generic;
using AtspmApi.Models;

namespace AtspmApi.Repositories
{
    public interface IApproachPcdAggregationRepository
    {
        int GetApproachPcdAggCountByApproach(int approachId, DateTime start,
            DateTime end);
        List<ApproachPcdAggregation> GetApproachPcdsAggByApproach(int approachId,
            DateTime startDate, DateTime endDate);

        //ApproachPcdAggregation Add(ApproachPcdAggregation priorityAggregation);
        //void Update(ApproachPcdAggregation priorityAggregation);
        //void Remove(ApproachPcdAggregation priorityAggregation);

        //List<ApproachPcdAggregation> GetApproachPcdsAggregationByApproachIdAndDateRange(int approachId,
        //    DateTime startDate, DateTime endDate, bool getProtectedPhase);
    }
}