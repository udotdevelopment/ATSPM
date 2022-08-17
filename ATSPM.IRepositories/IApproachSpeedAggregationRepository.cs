using ATSPM.Application.Models;
using System;
using System.Collections.Generic;

namespace ATSPM.IRepositories
{
    public interface IApproachSpeedAggregationRepository
    {
        List<ApproachSpeedAggregation> GetSpeedsByApproachIDandDateRange(int approachId, DateTime start, DateTime end);
        void Add(ApproachSpeedAggregation approachSpeedAggregation);
        void Remove(ApproachSpeedAggregation approachSpeedAggregation);
        void Remove(int id);
        void Update(ApproachSpeedAggregation approachSpeedAggregation);
    }
}