using System;
using System.Collections.Generic;
using AtspmApi.Models;

namespace AtspmApi.Repositories
{
    public interface IApproachSpeedAggregationRepository
    {
        int GetApproachSpeedCountAggregationByApproachIdAndDateRange(int approachId, DateTime start, DateTime end);
        List<ApproachSpeedAggregation> GetSpeedsByApproachIDandDateRange(int approachId, DateTime start, DateTime end);
        //void Add(ApproachSpeedAggregation approachSpeedAggregation);
        //void Remove(ApproachSpeedAggregation approachSpeedAggregation);
        //void Remove(int id);
        //void Update(ApproachSpeedAggregation approachSpeedAggregation);
    }
}