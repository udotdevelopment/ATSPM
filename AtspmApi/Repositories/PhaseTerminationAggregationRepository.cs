using System;
using System.Collections.Generic;
using System.Linq;
using AtspmApi.Models;

namespace AtspmApi.Repositories
{
    public class PhaseTerminationAggregationRepository : IPhaseTerminationAggregationRepository
    {
        private readonly Models.AtspmApi _db;

        public PhaseTerminationAggregationRepository()
        {
            _db = new Models.AtspmApi();
        }

        public PhaseTerminationAggregationRepository(Models.AtspmApi context)
        {
            _db = context;
        }

        public PhaseTerminationAggregation Add(PhaseTerminationAggregation approachSplitFailAggregation)
        {
            throw new NotImplementedException();
        }
        

        public void Remove(PhaseTerminationAggregation approachSplitFailAggregation)
        {
            throw new NotImplementedException();
        }
        

        public List<PhaseTerminationAggregation> GetPhaseTerminationsAggregationBySignal(string signalId, 
            DateTime startDate, DateTime endDate)
        {
            return _db.PhaseTerminationAggregations.Where(p =>
                p.SignalId == signalId && p.BinStartTime >= startDate &&
                p.BinStartTime < endDate).ToList();
        }

        public List<int> GetAvailablePhaseNumbers(Signal signal, DateTime startDate, DateTime endDate)
        {
            return _db.PhaseTerminationAggregations.Where(p =>
                    p.SignalId == signal.SignalID && p.BinStartTime >= startDate &&
                    p.BinStartTime < endDate)
                .Select(p => p.PhaseNumber).Distinct().ToList();
        }

        public int GetPhaseTermAggCountBySignal(string signalId, DateTime startDate, DateTime endDate)
        {
            var cycles = 0;
            if (_db.PhaseTerminationAggregations.Any(p =>
                p.SignalId == signalId && p.BinStartTime >= startDate &&
                p.BinStartTime < endDate))
                cycles = _db.PhaseTerminationAggregations.Where(p =>
                    p.SignalId == signalId && p.BinStartTime >= startDate &&
                    p.BinStartTime < endDate).Count();
            return cycles;
        }

        public void Update(PhaseTerminationAggregation approachSplitFailAggregation)
        {
            throw new NotImplementedException();
        }
    }
}