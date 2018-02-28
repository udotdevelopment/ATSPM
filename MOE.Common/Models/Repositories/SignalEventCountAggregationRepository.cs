using System;
using System.Collections.Generic;
using System.Linq;

namespace MOE.Common.Models.Repositories
{
    public class SignalEventCountAggregationRepository : ISignalEventCountAggregationRepository
    {
        private readonly SPM _db;

        public SignalEventCountAggregationRepository()
        {
            _db = new SPM();
        }

        public SignalEventCountAggregationRepository(SPM context)
        {
            _db = context;
        }

        public int GetSignalEventCountSumAggregationBySignalIdAndDateRange(string SignalId, DateTime start,
            DateTime end)
        {
            var cycles = 0;
            if (_db.SignalEventCountAggregations.Any(r => r.SignalId == SignalId
                                                       && r.BinStartTime >= start && r.BinStartTime <= end))
                cycles = _db.SignalEventCountAggregations.Where(r => r.SignalId == SignalId
                                                                  && r.BinStartTime >= start &&
                                                                  r.BinStartTime <= end)
                    .Sum(r => r.EventCount);
            return cycles;
        }
        

        public List<SignalEventCountAggregation> GetSignalEventCountAggregationBySignalIdAndDateRange(string SignalId,
            DateTime startDate, DateTime endDate)
        {
            return _db.SignalEventCountAggregations.Where(r => r.SignalId == SignalId
                                                            && r.BinStartTime >= startDate &&
                                                            r.BinStartTime <= endDate).ToList();
        }
    }

    
}