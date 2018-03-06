using System;
using System.Collections.Generic;
using System.Linq;
using MOE.CommonTests.Models;

namespace MOE.Common.Models.Repositories
{
    public class InMemoryApproachEventCountAggregationRepository : IApproachEventCountAggregationRepository
    {
        private InMemoryMOEDatabase _db;


        public InMemoryApproachEventCountAggregationRepository()
        {
            _db = new InMemoryMOEDatabase();
        }

        public InMemoryApproachEventCountAggregationRepository(InMemoryMOEDatabase context)
        {
            _db = context;
        }

        public int GetPhaseEventCountSumAggregationByPhaseIdAndDateRange(int approachId, DateTime start,
            DateTime end, bool getProtectedPhase )
        {
            var cycles = 0;
            if (getProtectedPhase)
            {
                if (_db.PhaseEventCountAggregations.Any(r => r.ApproachId == approachId && r.IsProtectedPhase
                                                             && r.BinStartTime >= start && r.BinStartTime <= end))
                {
                    cycles = _db.PhaseEventCountAggregations.Where(r => r.ApproachId == approachId && r.IsProtectedPhase
                                                                        && r.BinStartTime >= start && r.BinStartTime <= end)
                        .Sum(r => r.EventCount);
                }
            }
            else
            {
                if (_db.PhaseEventCountAggregations.Any(r => r.ApproachId == approachId && !r.IsProtectedPhase
                                                             && r.BinStartTime >= start && r.BinStartTime <= end))
                {
                    cycles = _db.PhaseEventCountAggregations.Where(r => r.ApproachId == approachId && !r.IsProtectedPhase
                                                                        && r.BinStartTime >= start && r.BinStartTime <= end)
                        .Sum(r => r.EventCount);
                }
            }
            return cycles;
        }
        

        public List<ApproachEventCountAggregation> GetPhaseEventCountAggregationByPhaseIdAndDateRange(int approachId, DateTime start,
            DateTime end, bool getProtectedPhase)
        {
            if (getProtectedPhase)
            {
                if (_db.PhaseEventCountAggregations.Any(r => r.ApproachId == approachId && r.IsProtectedPhase
                                                             && r.BinStartTime >= start && r.BinStartTime <= end))
                {
                    return _db.PhaseEventCountAggregations.Where(r => r.ApproachId == approachId && r.IsProtectedPhase
                                                                      && r.BinStartTime >= start &&
                                                                      r.BinStartTime <= end).ToList();
                }
            }
            else
            {
                if (_db.PhaseEventCountAggregations.Any(r => r.ApproachId == approachId && !r.IsProtectedPhase
                                                             && r.BinStartTime >= start && r.BinStartTime <= end))
                {
                    return _db.PhaseEventCountAggregations.Where(r => r.ApproachId == approachId && !r.IsProtectedPhase
                                                                      && r.BinStartTime >= start &&
                                                                      r.BinStartTime <= end).ToList();
                }
            }
            return new List<ApproachEventCountAggregation>();
        }
        
    }

    
}