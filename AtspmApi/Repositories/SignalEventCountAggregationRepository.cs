using System;
using System.Collections.Generic;
using System.Linq;
using AtspmApi.Models;

namespace AtspmApi.Repositories
{
    public class SignalEventCountAggregationRepository : ISignalEventCountAggregationRepository
    {
        private readonly Models.AtspmApi _db;

        public SignalEventCountAggregationRepository()
        {
            _db = new Models.AtspmApi();
        }

        public SignalEventCountAggregationRepository(Models.AtspmApi context)
        {
            _db = context;
        }

       


        public int GetSignalEventCountAggCountBySignal(string signalId, DateTime start, DateTime end)
        {
            var cycles = 0;
            if (_db.SignalEventCountAggregations.Any(r => r.SignalId == signalId
                                                          && r.BinStartTime >= start && r.BinStartTime < end))
            {
                cycles = _db.SignalEventCountAggregations.Where(r => r.SignalId == signalId
                                                                     && r.BinStartTime >= start &&
                                                                     r.BinStartTime < end).Count();
            }
            return cycles;
        }

        public List<SignalEventCountAggregation> GetSignalEventCountAggregationBySignal(string signalId,
            DateTime startDate, DateTime endDate)
        {
            return _db.SignalEventCountAggregations.Where(r => r.SignalId == signalId
                                                            && r.BinStartTime >= startDate &&
                                                            r.BinStartTime < endDate).ToList();
        }
    }

    
}