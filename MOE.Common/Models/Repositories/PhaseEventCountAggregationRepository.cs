using System;
using System.Collections.Generic;
using System.Linq;

namespace MOE.Common.Models.Repositories
{
    public class ApproachEventCountAggregationRepository : IApproachEventCountAggregationRepository
    {
        private readonly SPM _db;

        public ApproachEventCountAggregationRepository()
        {
            _db = new SPM();
        }

        public ApproachEventCountAggregationRepository(SPM context)
        {
            _db = context;
        }

        public int GetPhaseEventCountSumAggregationByPhaseIdAndDateRange(int approachId, DateTime start,
            DateTime end, bool getProtectedPhase )
        {
            var cycles = 0;
            if (getProtectedPhase)
            {
                if (_db.ApproachEventCountAggregations.Any(r => r.ApproachId == approachId && r.IsProtectedPhase
                                                             && r.BinStartTime >= start && r.BinStartTime <= end))
                {
                    cycles = _db.ApproachEventCountAggregations.Where(r => r.ApproachId == approachId && r.IsProtectedPhase
                                                                        && r.BinStartTime >= start && r.BinStartTime <= end)
                        .Sum(r => r.EventCount);
                }
            }
            else
            {
                if (_db.ApproachEventCountAggregations.Any(r => r.ApproachId == approachId && !r.IsProtectedPhase
                                                             && r.BinStartTime >= start && r.BinStartTime <= end))
                {
                    cycles = _db.ApproachEventCountAggregations.Where(r => r.ApproachId == approachId && !r.IsProtectedPhase
                                                                        && r.BinStartTime >= start && r.BinStartTime <= end)
                        .Sum(r => r.EventCount);
                }
            }
            return cycles;
        }
        

        public List<ApproachEventCountAggregation> GetApproachEventCountAggregationByPhaseIdAndDateRange(int approachId, DateTime start,
            DateTime end, bool getProtectedPhase)
        {
            if (getProtectedPhase)
            {
                if (_db.ApproachEventCountAggregations.Any(r => r.ApproachId == approachId && r.IsProtectedPhase
                                                             && r.BinStartTime >= start && r.BinStartTime <= end))
                {
                    return _db.ApproachEventCountAggregations.Where(r => r.ApproachId == approachId && r.IsProtectedPhase
                                                                      && r.BinStartTime >= start &&
                                                                      r.BinStartTime <= end).ToList();
                }
            }
            else
            {
                if (_db.ApproachEventCountAggregations.Any(r => r.ApproachId == approachId && !r.IsProtectedPhase
                                                             && r.BinStartTime >= start && r.BinStartTime <= end))
                {
                    return _db.ApproachEventCountAggregations.Where(r => r.ApproachId == approachId && !r.IsProtectedPhase
                                                                      && r.BinStartTime >= start &&
                                                                      r.BinStartTime <= end).ToList();
                }
            }
            return new List<ApproachEventCountAggregation>();
        }
    }

    
}