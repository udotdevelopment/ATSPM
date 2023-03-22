using System;
using System.Collections.Generic;
using System.Linq;
using MOE.CommonTests.Models;

namespace MOE.Common.Models.Repositories
{
    public class InMemorySignalEventCountAggregationRepository : ISignalEventCountAggregationRepository
    {
        public InMemoryMOEDatabase _db;

        public InMemorySignalEventCountAggregationRepository()
        {
            _db = new InMemoryMOEDatabase();
        }

        public InMemorySignalEventCountAggregationRepository(InMemoryMOEDatabase context)
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

        public DateTime? GetLastAggregationDate()
        {
            throw new NotImplementedException();
        }
    }

    
}