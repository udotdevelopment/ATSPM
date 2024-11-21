using System;
using System.Collections.Generic;
using System.Linq;
using MOE.Common.Models;
using MOE.Common.Models.Repositories;

namespace MOE.CommonTests.Models
{
    public class InMemoryApproachSpeedAggregationRepository: IApproachSpeedAggregationRepository
    {
        private InMemoryMOEDatabase _db;

        public InMemoryApproachSpeedAggregationRepository()
        {
            _db = new InMemoryMOEDatabase();
        }

        public InMemoryApproachSpeedAggregationRepository(InMemoryMOEDatabase context)
        {
            _db = context;
        }
        public List<ApproachSpeedAggregation> GetSpeedsByApproachIDandDateRange(int approachId, DateTime start, DateTime end)
        {

            var activationsList = (from r in this._db.ApproachSpeedAggregations
                where r.ApproachId == approachId
                      && r.BinStartTime >= start && r.BinStartTime <= end
                select r).ToList();

            return activationsList;

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

        public DateTime? GetLastAggregationDate()
        {
            throw new NotImplementedException();
        }
    }
}