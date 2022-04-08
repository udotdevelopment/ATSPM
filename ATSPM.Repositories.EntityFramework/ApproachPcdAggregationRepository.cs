using ATSPM.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using ATSPM.Application.Models;

namespace ATSPM.Infrastructure.Repositories.EntityFramework
{
    public class ApproachPcdAggregationRepository : IApproachPcdAggregationRepository
    {
        private readonly MOEContext _db;


        public ApproachPcdAggregationRepository(MOEContext context)
        {
            _db = context;
        }

        public ApproachPcdAggregation Add(ApproachPcdAggregation approachPcdAggregation)
        {
            throw new NotImplementedException();
        }

        public int GetApproachPcdCountAggregationByApproachIdAndDateRange(int approachId, DateTime start, DateTime end)
        {
            var pcd = 0;
            if (_db.ApproachPcdAggregations.Any(r => r.ApproachId == approachId
                                                     && r.BinStartTime >= start && r.BinStartTime <= end))
                pcd = _db.ApproachPcdAggregations.Where(r => r.ApproachId == approachId
                                                             && r.BinStartTime >= start &&
                                                             r.BinStartTime <= end)
                    .Sum(r => r.ArrivalsOnGreen);
            return pcd;
        }

        public void Remove(ApproachPcdAggregation approachPcdAggregation)
        {
            throw new NotImplementedException();
        }

        public List<ApproachPcdAggregation> GetApproachPcdsAggregationByApproachIdAndDateRange(int approachId,
            DateTime startDate, DateTime endDate, bool getProtectedPhase)
        {
            return _db.ApproachPcdAggregations.Where(r => r.ApproachId == approachId
                                                          && r.BinStartTime >= startDate &&
                                                          r.BinStartTime <= endDate
                                                          && r.IsProtectedPhase == getProtectedPhase).ToList();
        }


        public void Update(ApproachPcdAggregation approachPcdAggregation)
        {
            throw new NotImplementedException();
        }
    }
}