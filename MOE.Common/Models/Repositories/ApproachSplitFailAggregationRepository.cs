using System;
using System.Collections.Generic;
using System.Linq;

namespace MOE.Common.Models.Repositories
{
    public class ApproachSplitFailAggregationRepository: IApproachSplitFailAggregationRepository
    {
        SPM db = new SPM();


        private Models.SPM _db;


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




        public List<ApproachSplitFailAggregation> GetApproachSplitFailAggregationByVersionIdAndDateRange(int approachId, DateTime start, DateTime end)
        {
            var records = (from r in this._db.ApproachSplitFailAggregations
                           where r.ApproachId == approachId
                      && r.BinStartTime >= start && r.BinStartTime <= end
                select r).ToList();

            return records;
        }

        public void Remove(ApproachSplitFailAggregation approachSplitFailAggregation)
        {
            throw new NotImplementedException();
        }


        public void Update(ApproachSplitFailAggregation approachSplitFailAggregation)
        {
            throw new NotImplementedException();
        }


    }
}
