using System;
using System.Collections.Generic;
using System.Linq;

namespace MOE.Common.Models.Repositories
{
    public class ApproachSpeedAggregationRepository : IApproachSpeedAggregationRepository
    {
        private readonly SPM _db;


        public ApproachSpeedAggregationRepository()
        {
            _db = new SPM();
        }

        public ApproachSpeedAggregationRepository(SPM context)
        {
            _db = context;
        }

        public List<ApproachSpeedAggregation> GetSpeedsByApproachIDandDateRange(int approachId, DateTime start,
            DateTime end)
        {
            var activationsList = (from r in _db.ApproachSpeedAggregations
                where r.ApproachId == approachId
                      && r.BinStartTime >= start && r.BinStartTime <= end
                select r).ToList();

            return activationsList;
        }

        public void Update(ApproachSpeedAggregation approachSpeedAggregation)
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

        public void Add(ApproachSpeedAggregation approachSpeedAggregation)
        {
            throw new NotImplementedException();
        }

        public DateTime? GetLastAggregationDate()
        {
            return _db.ApproachSpeedAggregations.Max(s => (DateTime?)s.BinStartTime);
        }
    }
}