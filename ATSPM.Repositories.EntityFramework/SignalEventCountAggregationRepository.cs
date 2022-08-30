using ATSPM.IRepositories;
using ATSPM.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ATSPM.Infrastructure.Repositories.EntityFramework
{
    public class SignalEventCountAggregationRepository : ISignalEventCountAggregationRepository
    {
        private readonly MOEContext _db;


        public SignalEventCountAggregationRepository(MOEContext context)
        {
            _db = context;
        }




        public int GetSignalEventCountSumAggregationBySignalIdAndDateRange(string signalId, DateTime start, DateTime end)
        {
            var cycles = 0;
            if (_db.SignalEventCountAggregations.Any(r => r.SignalId == signalId
                                                          && r.BinStartTime >= start && r.BinStartTime <= end))
            {
                cycles = _db.SignalEventCountAggregations.Where(r => r.SignalId == signalId
                                                                     && r.BinStartTime >= start &&
                                                                     r.BinStartTime <= end)
                    .Sum(r => r.EventCount);
            }
            return cycles;
        }

        public List<SignalEventCountAggregation> GetSignalEventCountAggregationBySignalIdAndDateRange(string signalId,
            DateTime startDate, DateTime endDate)
        {
            return _db.SignalEventCountAggregations.Where(r => r.SignalId == signalId
                                                            && r.BinStartTime >= startDate &&
                                                            r.BinStartTime <= endDate).ToList();
        }
    }


}