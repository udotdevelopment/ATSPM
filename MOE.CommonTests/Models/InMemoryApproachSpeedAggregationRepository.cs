using System;
using System.Collections.Generic;
using MOE.Common.Models;
using MOE.Common.Models.Repositories;

namespace MOE.CommonTests.Models
{
    public class InMemoryApproachSpeedAggregationRepository: IApproachSpeedAggregationRepository
    {
        public List<ApproachSpeedAggregation> GetSpeedsByApproachIDandDateRange(int approachId, DateTime start, DateTime end)
        {
            throw new NotImplementedException();
        }

        public void Add(ApproachSpeedAggregation approachSpeedAggregation)
        {
            throw new NotImplementedException();
        }

        public void Remove(ApproachSpeedAggregation approachSpeedAggregation)
        {
            throw new NotImplementedException();
        }

        public void Remove(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(ApproachSpeedAggregation approachSpeedAggregation)
        {
            throw new NotImplementedException();
        }
    }
}