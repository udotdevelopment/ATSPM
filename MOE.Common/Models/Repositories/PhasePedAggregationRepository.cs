using System;
using System.Collections.Generic;
using System.Linq;

namespace MOE.Common.Models.Repositories
{
    public class PhasePedAggregationRepository : IPhasePedAggregationRepository
    {
        private readonly SPM _db;

        public PhasePedAggregationRepository()
        {
            _db = new SPM();
        }

        public PhasePedAggregationRepository(SPM context)
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
        

        public List<PhasePedAggregation> GetPhasePedsAggregationBySignalIdPhaseNumberAndDateRange(string signalId, int phaseNumber,
            DateTime startDate, DateTime endDate)
        {
            return _db.PhasePedAggregations.Where(p =>
                p.SignalId == signalId && p.PhaseNumber == phaseNumber && p.BinStartTime >= startDate &&
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

        public DateTime? GetLastAggregationDate()
        {
            return _db.PhasePedAggregations.Max(s => (DateTime?)s.BinStartTime);
        }
    }
}