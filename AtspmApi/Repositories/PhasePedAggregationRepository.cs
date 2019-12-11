using System;
using System.Collections.Generic;
using System.Linq;
using AtspmApi.Models;

namespace AtspmApi.Repositories
{
    public class PhasePedAggregationRepository : IPhasePedAggregationRepository
    {
        private readonly Models.AtspmApi _db;

        public PhasePedAggregationRepository()
        {
            _db = new Models.AtspmApi();
        }

        public PhasePedAggregationRepository(Models.AtspmApi context)
        {
            _db = context;
        }

        public PhasePedAggregation Add(PhasePedAggregation approachSplitFailAggregation)
        {
            throw new NotImplementedException();
        }
        

        public void Remove(PhasePedAggregation approachSplitFailAggregation)
        {
            throw new NotImplementedException();
        }

        public int GetPhasePedsAggCountBySignal(string signalId, DateTime startDate, DateTime endDate)

        {
            var cycles = 0;
            if (_db.PhasePedAggregations.Any(p => p.SignalId == signalId && p.BinStartTime >= startDate &&
                                                  p.BinStartTime < endDate))
                cycles = _db.PhasePedAggregations.Where(p =>
                    p.SignalId == signalId && p.BinStartTime >= startDate &&
                    p.BinStartTime < endDate).Count();
            return cycles;
        }
    
        public List<PhasePedAggregation> GetPhasePedsAggBySignal(string signalId, 
            DateTime startDate, DateTime endDate)
        {
            return _db.PhasePedAggregations.Where(p =>
                p.SignalId == signalId && p.BinStartTime >= startDate &&
                p.BinStartTime < endDate).ToList();
        }

        public List<int> GetAvailablePhaseNumbers(Signal signal, DateTime startDate, DateTime endDate)
        {
            return _db.PhasePedAggregations.Where(p =>
                    p.SignalId == signal.SignalID && p.BinStartTime >= startDate &&
                    p.BinStartTime < endDate)
                .Select(p => p.PhaseNumber).ToList();
        }


        public void Update(PhasePedAggregation approachSplitFailAggregation)
        {
            throw new NotImplementedException();
        }
    }
}