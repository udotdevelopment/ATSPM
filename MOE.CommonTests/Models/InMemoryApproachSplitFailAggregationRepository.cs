using System;
using System.Collections.Generic;
using System.Linq;
using MOE.Common.Models;
using MOE.Common.Models.Repositories;

namespace MOE.CommonTests.Models
{
    public partial class InMemoryApproachSplitFailAggregationRepository: IApproachSplitFailAggregationRepository
    {
        private InMemoryMOEDatabase _db;

        public InMemoryApproachSplitFailAggregationRepository()
        {
            _db = new InMemoryMOEDatabase();
        }

        public InMemoryApproachSplitFailAggregationRepository(InMemoryMOEDatabase context )
        {
            _db = context;
        }

        public int GetApproachSplitFailCountAggregationByApproachIdAndDateRange(int approachId, DateTime start,
            DateTime end)
        {

            int splitFails = 0;
            if (_db.ApproachSplitFailAggregations.Any(r => r.ApproachId == approachId
                                                           && r.BinStartTime >= start && r.BinStartTime <= end))
            {
                splitFails = _db.ApproachSplitFailAggregations.Where(r => r.ApproachId == approachId
                                                                          && r.BinStartTime >= start &&
                                                                          r.BinStartTime <= end)
                    .Sum(r => r.SplitFailures);
            }
            return splitFails;



        }

        public ApproachSplitFailAggregation Add(ApproachSplitFailAggregation priorityAggregation)
        {
            throw new NotImplementedException();
        }

        public void Update(ApproachSplitFailAggregation priorityAggregation)
        {
            throw new NotImplementedException();
        }

        public void Remove(ApproachSplitFailAggregation priorityAggregation)
        {
            throw new NotImplementedException();
        }

        public List<ApproachSplitFailAggregation> GetApproachSplitFailsAggregationByApproachIdAndDateRange(int approachId, DateTime startDate, DateTime endDate, bool getProtectedPhase)
        {
            return _db.ApproachSplitFailAggregations.Where(r => r.ApproachId == approachId
                                                                && r.BinStartTime >= startDate &&
                                                                r.BinStartTime < endDate && r.IsProtectedPhase == getProtectedPhase).ToList();
        }

        public DateTime? GetLastAggregationDate()
        {
            throw new NotImplementedException();
        }
    }
}