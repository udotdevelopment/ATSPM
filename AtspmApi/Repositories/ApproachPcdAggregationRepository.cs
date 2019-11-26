using System;
using System.Collections.Generic;
using System.Linq;
using AtspmApi.Models;

namespace AtspmApi.Repositories
{
    public class ApproachPcdAggregationRepository : IApproachPcdAggregationRepository
    {
        private readonly Models.AtspmApi _db;

        public ApproachPcdAggregationRepository()
        {
            _db = new Models.AtspmApi();
        }

        public ApproachPcdAggregationRepository(Models.AtspmApi context)
        {
            _db = context;
        }

        //public ApproachPcdAggregation Add(ApproachPcdAggregation approachPcdAggregation)
        //{
        //    throw new NotImplementedException();
        //}

        public int GetApproachPcdAggCountByApproach(int approachId, DateTime start, DateTime end)
        {
            var pcd = 0;
            if (_db.ApproachPcdAggregations.Any(r => r.ApproachId == approachId
                                                     && r.BinStartTime >= start && r.BinStartTime < end))
                pcd = _db.ApproachPcdAggregations.Where(r => r.ApproachId == approachId
                                                             && r.BinStartTime >= start &&
                                                             r.BinStartTime < end).Count();
            return pcd;
        }

        public List<ApproachPcdAggregation> GetApproachPcdsAggByApproach(int approachId,
            DateTime startDate, DateTime endDate)
        {
            return _db.ApproachPcdAggregations.Where(r => r.ApproachId == approachId
                                                          && r.BinStartTime >= startDate &&
                                                          r.BinStartTime < endDate).ToList();
        }

        //public void Remove(ApproachPcdAggregation approachPcdAggregation)
        //{
        //    throw new NotImplementedException();
        //}

        //public List<ApproachPcdAggregation> GetApproachPcdsAggregationByApproachIdAndDateRange(int approachId,
        //    DateTime startDate, DateTime endDate, bool getProtectedPhase)
        //{
        //    return _db.ApproachPcdAggregations.Where(r => r.ApproachId == approachId
        //                                                  && r.BinStartTime >= startDate &&
        //                                                  r.BinStartTime <= endDate
        //                                                  && r.IsProtectedPhase == getProtectedPhase).ToList();
        //}


        //public void Update(ApproachPcdAggregation approachPcdAggregation)
        //{
        //    throw new NotImplementedException();
        //}
    }
}