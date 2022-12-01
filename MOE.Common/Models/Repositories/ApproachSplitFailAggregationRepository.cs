using System;
using System.Collections.Generic;
using System.Linq;

namespace MOE.Common.Models.Repositories
{
    public class ApproachSplitFailAggregationRepository : IApproachSplitFailAggregationRepository
    {
        private readonly SPM _db;

        public ApproachSplitFailAggregationRepository()
        {
            _db = new SPM();
        }

        public ApproachSplitFailAggregationRepository(SPM context)
        {
            _db = context;
        }

        public ApproachSplitFailAggregation Add(ApproachSplitFailAggregation approachSplitFailAggregation)
        {
            throw new NotImplementedException();
        }

        public int GetApproachSplitFailCountAggregationByApproachIdAndDateRange(int approachId, DateTime start,
            DateTime end)
        {
            var splitFails = 0;
            if (_db.ApproachSplitFailAggregations.Any(r => r.ApproachId == approachId
                                                           && r.BinStartTime >= start && r.BinStartTime <= end))
                splitFails = _db.ApproachSplitFailAggregations.Where(r => r.ApproachId == approachId
                                                                          && r.BinStartTime >= start &&
                                                                          r.BinStartTime <= end)
                    .Sum(r => r.SplitFailures);
            return splitFails;
        }

        public void Remove(ApproachSplitFailAggregation approachSplitFailAggregation)
        {
            throw new NotImplementedException();
        }

        public List<ApproachSplitFailAggregation> GetApproachSplitFailsAggregationByApproachIdAndDateRange(
            int approachId, DateTime startDate, DateTime endDate, bool getProtectedPhase)
        {
            return _db.ApproachSplitFailAggregations.Where(r => r.ApproachId == approachId
                                                                && r.BinStartTime >= startDate &&
                                                                r.BinStartTime <= endDate 
                                                                && r.IsProtectedPhase == getProtectedPhase).ToList();
        }


        public void Update(ApproachSplitFailAggregation approachSplitFailAggregation)
        {
            throw new NotImplementedException();
        }

        public DateTime? GetLastAggregationDate()
        {
            return _db.ApproachSplitFailAggregations.Max(s => s.BinStartTime);
        }
    }
}