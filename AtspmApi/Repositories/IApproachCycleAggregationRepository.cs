using System;
using System.Collections.Generic;
using AtspmApi.Models;

namespace AtspmApi.Repositories
{
    public interface IApproachCycleAggregationRepository
    {
        int GetApproachCycleCountAggByApproachCount(int approachId, DateTime start,
            DateTime end);

        List<ApproachCycleAggregation> ApproachCycleAggByApproach(int approachId, DateTime start,
            DateTime end);
        //ApproachCycleAggregation Add(ApproachCycleAggregation priorityAggregation);
        //void Update(ApproachCycleAggregation priorityAggregation);
        //void Remove(ApproachCycleAggregation priorityAggregation);

        //List<ApproachCycleAggregation> GetApproachCyclesAggregationByApproachIdAndDateRange(int approachId,
        //    DateTime startDate, DateTime endDate, bool getProtectedPhase);
    }
}