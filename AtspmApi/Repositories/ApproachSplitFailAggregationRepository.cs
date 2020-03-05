using System;
using System.Collections.Generic;
using System.Linq;
using AtspmApi.Models;

namespace AtspmApi.Repositories
{
    public class ApproachSplitFailAggregationRepository : IApproachSplitFailAggregationRepository
    {
        private readonly Models.AtspmApi _db;

        public ApproachSplitFailAggregationRepository()
        {
            _db = new Models.AtspmApi();
        }

        public ApproachSplitFailAggregationRepository(Models.AtspmApi context)
        {
            _db = context;
        }

        public ApproachSplitFailAggregation Add(ApproachSplitFailAggregation approachSplitFailAggregation)
        {
            throw new NotImplementedException();
        }

        public int GetApproachSplitFailCountAggByApproach(int approachId, DateTime start,
            DateTime end)
        {
            var splitFails = 0;
            if (_db.ApproachSplitFailAggregations.Any(r => r.ApproachId == approachId
                                                           && r.BinStartTime >= start && r.BinStartTime < end))
                splitFails = _db.ApproachSplitFailAggregations.Where(r => r.ApproachId == approachId
                                                                          && r.BinStartTime >= start &&
                                                                          r.BinStartTime < end).Count();
            return splitFails;
        }

        public void Remove(ApproachSplitFailAggregation approachSplitFailAggregation)
        {
            throw new NotImplementedException();
        }

        public List<ApproachSplitFailAggregation> GetApproachSplitFailsAggregationByApproachId(
            int approachId, DateTime startDate, DateTime endDate)
        {
            return _db.ApproachSplitFailAggregations.Where(r => r.ApproachId == approachId
                                                                && r.BinStartTime >= startDate &&
                                                                r.BinStartTime < endDate).ToList();
        }


        public void Update(ApproachSplitFailAggregation approachSplitFailAggregation)
        {
            throw new NotImplementedException();
        }
    }
}