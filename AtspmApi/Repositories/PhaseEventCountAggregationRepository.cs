using System;
using System.Collections.Generic;
using System.Linq;
using AtspmApi.Models;

namespace AtspmApi.Repositories
{
    public class ApproachEventCountAggregationRepository : IApproachEventCountAggregationRepository
    {
        private readonly Models.AtspmApi _db;

        public ApproachEventCountAggregationRepository()
        {
            _db = new Models.AtspmApi();
        }

        public ApproachEventCountAggregationRepository(Models.AtspmApi context)
        {
            _db = context;
        }

        public int GetPhaseEventCountAggByApproach(int approachId, DateTime start, DateTime end)
        {
            var cycles = 0;

                if (_db.ApproachEventCountAggregations.Any(r => r.ApproachId == approachId 
                                                             && r.BinStartTime >= start && r.BinStartTime < end))
                {
                    cycles = _db.ApproachEventCountAggregations.Where(r =>
                        r.ApproachId == approachId && r.IsProtectedPhase
                                                   && r.BinStartTime >= start && r.BinStartTime < end).Count();
                }

            return cycles;
        }

        public List<ApproachEventCountAggregation> GetApproachEventCountAggregationByApproach(int approachId, DateTime start, DateTime end)
        {

                if (_db.ApproachEventCountAggregations.Any(r => r.ApproachId == approachId 
                                                                && r.BinStartTime >= start && r.BinStartTime < end))
                {
                    return _db.ApproachEventCountAggregations.Where(r => r.ApproachId == approachId 
                                                                         && r.BinStartTime >= start &&
                                                                         r.BinStartTime < end).ToList();
                }

            return new List<ApproachEventCountAggregation>();
        }

        
    }

    
}