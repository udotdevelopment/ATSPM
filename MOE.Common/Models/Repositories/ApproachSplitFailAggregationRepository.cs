using System;
using System.Collections.Generic;
using System.Linq;
using Remotion.Linq.Parsing.Structure.IntermediateModel;

namespace MOE.Common.Models.Repositories
{
    public class ApproachSplitFailAggregationRepository: IApproachSplitFailAggregationRepository
    {
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

        public int GetApproachSplitFailAggregationByApproachIdAndDateRange(int approachId, DateTime start, DateTime end)
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
