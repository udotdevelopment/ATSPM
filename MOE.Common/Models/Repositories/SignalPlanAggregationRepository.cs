using System;
using System.Collections.Generic;
using System.Linq;

namespace MOE.Common.Models.Repositories
{
    public class SignalPlanAggregationRepository : ISignalPlanAggregationRepository
    {
        private readonly SPM _db;

        public SignalPlanAggregationRepository()
        {
            _db = new SPM();
        }

        public SignalPlanAggregationRepository(SPM context)
        {
            _db = context;
        }       


        

        public DateTime? GetLastAggregationDate()
        {
            return _db.SignalPlanAggregations.Max(s => (DateTime?)s.End);
        }
    }

    
}