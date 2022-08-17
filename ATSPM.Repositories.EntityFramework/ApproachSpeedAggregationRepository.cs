using ATSPM.IRepositories;
using ATSPM.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ATSPM.Infrastructure.Repositories.EntityFramework
{
    public class ApproachSpeedAggregationRepository : IApproachSpeedAggregationRepository
    {
        private readonly MOEContext _db;


        public ApproachSpeedAggregationRepository(MOEContext context)
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
    }
}