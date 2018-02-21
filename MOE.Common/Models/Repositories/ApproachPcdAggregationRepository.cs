using System;
using System.Collections.Generic;
using System.Linq;
using Remotion.Linq.Parsing.Structure.IntermediateModel;

namespace MOE.Common.Models.Repositories
{
    public class ApproachPcdAggregationRepository: IApproachPcdAggregationRepository
    {
        private Models.SPM _db;
        public ApproachPcdAggregationRepository()
        {
            _db = new SPM();
        }
        public ApproachPcdAggregationRepository(SPM context)
        {
            _db = context;
        }

        public ApproachPcdAggregation Add(ApproachPcdAggregation approachPcdAggregation)
        {
            throw new NotImplementedException();
        }

        public int GetApproachPcdCountAggregationByApproachIdAndDateRange(int approachId, DateTime start, DateTime end)
        {
            int pcd = 0;
            if (_db.ApproachPcdAggregations.Any(r => r.ApproachId == approachId
                                                           && r.BinStartTime >= start && r.BinStartTime <= end))
            {
                pcd = _db.ApproachPcdAggregations.Where(r => r.ApproachId == approachId
                                                                              && r.BinStartTime >= start &&
                                                                              r.BinStartTime <= end)
                    .Sum(r => r.ArrivalsOnGreen);
            }
            return pcd;
        }

        public void Remove(ApproachPcdAggregation approachPcdAggregation)
        {
            throw new NotImplementedException();
        }

        public List<ApproachPcdAggregation> GetApproachPcdsAggregationByApproachIdAndDateRange(int approachId, DateTime startDate, DateTime endDate, bool getProtectedPhase)
        {
            return _db.ApproachPcdAggregations.Where(r => r.ApproachId == approachId
                                                                          && r.BinStartTime >= startDate &&
                                                                          r.BinStartTime <= endDate).ToList();
        }


        public void Update(ApproachPcdAggregation approachPcdAggregation)
        {
            throw new NotImplementedException();
        }


    }
}
