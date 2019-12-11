using System;
using System.Collections.Generic;
using System.Linq;
using AtspmApi.Models;

namespace AtspmApi.Repositories
{
    public class ApproachCycleAggregationRepository : IApproachCycleAggregationRepository
    {
        private readonly Models.AtspmApi _db;

        public ApproachCycleAggregationRepository()
        {
            _db = new Models.AtspmApi();
        }

        public ApproachCycleAggregationRepository(Models.AtspmApi context)
        {
            _db = context;
        }

        public List<ApproachCycleAggregation> ApproachCycleAggByApproach(int approachId, DateTime start,
            DateTime end)
        {
            return _db.ApproachCycleAggregations.Where(r => r.ApproachId == approachId
                                                            && r.BinStartTime >= start &&
                                                            r.BinStartTime < end).ToList();
        }
        //public ApproachCycleAggregation Add(ApproachCycleAggregation approachCycleAggregation)
        //{
        //    throw new NotImplementedException();
        //}

        public int GetApproachCycleCountAggByApproachCount(int approachId, DateTime start,
            DateTime end)
        {
            var cycles = 0;
            if (_db.ApproachCycleAggregations.Any(r => r.ApproachId == approachId
                                                       && r.BinStartTime >= start && r.BinStartTime < end))
                cycles = _db.ApproachCycleAggregations.Where(r => r.ApproachId == approachId
                                                                  && r.BinStartTime >= start &&
                                                                  r.BinStartTime < end).Count();
            return cycles;
        }

        //public void Remove(ApproachCycleAggregation approachCycleAggregation)
        //{
        //    throw new NotImplementedException();
        //}

        //public List<ApproachCycleAggregation> GetApproachCyclesAggregationByApproachIdAndDateRange(int approachId,
        //    DateTime startDate, DateTime endDate, bool getProtectedPhase)
        //{
        //    return _db.ApproachCycleAggregations.Where(r => r.ApproachId == approachId
        //                                                    && r.BinStartTime >= startDate &&
        //                                                    r.BinStartTime <= endDate
        //                                                    && r.IsProtectedPhase == getProtectedPhase).ToList();
        //}


        //public void Update(ApproachCycleAggregation approachCycleAggregation)
        //{
        //    throw new NotImplementedException();
        //}
    }
}