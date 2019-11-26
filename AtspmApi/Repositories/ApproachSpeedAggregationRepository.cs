using System;
using System.Collections.Generic;
using System.Linq;
using AtspmApi.Models;

namespace AtspmApi.Repositories
{
    public class ApproachSpeedAggregationRepository : IApproachSpeedAggregationRepository
    {
        private readonly Models.AtspmApi _db;


        public ApproachSpeedAggregationRepository()
        {
            _db = new Models.AtspmApi();
        }

        public ApproachSpeedAggregationRepository(Models.AtspmApi context)
        {
            _db = context;
        }
        public int GetApproachSpeedCountAggregationByApproachIdAndDateRange(int approachId, DateTime start,
            DateTime end)
        {
            var cycles = 0;
            if (_db.ApproachSpeedAggregations.Any(r => r.ApproachId == approachId
                                                       && r.BinStartTime >= start && r.BinStartTime < end))
                cycles = _db.ApproachSpeedAggregations.Where(r => r.ApproachId == approachId
                                                                  && r.BinStartTime >= start &&
                                                                  r.BinStartTime < end).Count();
            return cycles;
        }

        public List<ApproachSpeedAggregation> GetSpeedsByApproachIDandDateRange(int approachId, DateTime start,
            DateTime end)
        {
            var activationsList = (from r in _db.ApproachSpeedAggregations
                where r.ApproachId == approachId
                      && r.BinStartTime >= start && r.BinStartTime < end
                select r).ToList();

            return activationsList;
        }

        //public void Update(ApproachSpeedAggregation approachSpeedAggregation)
        //{
        //    throw new NotImplementedException();
        //}

        //public void Remove(ApproachSpeedAggregation approachSpeedAggregation)
        //{
        //    throw new NotImplementedException();
        //}

        //public void Remove(int id)
        //{
        //    throw new NotImplementedException();
        //}

        //public void Add(ApproachSpeedAggregation approachSpeedAggregation)
        //{
        //    throw new NotImplementedException();
        //}
    }
}