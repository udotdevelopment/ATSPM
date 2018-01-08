using System;
using System.Collections.Generic;
using System.Linq;
using MOE.Common.Models;
using MOE.Common.Models.Repositories;

namespace MOE.CommonTests.Models
{
    public class InMemoryApproachSplitFailAggregationRepository: IApproachSplitFailAggregationRepository
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

        public int GetApproachSplitFailAggregationByVersionIdAndDateRange(int approachId, DateTime start, DateTime end)
        {
         
                var records = (from r in this._db.ApproachSplitFailAggregations
                    where r.ApproachId == approachId
                          && r.BinStartTime >= start && r.BinStartTime <= end
                    select r).Sum(r => r.SplitFailures);

                return records;
            
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
    }
}