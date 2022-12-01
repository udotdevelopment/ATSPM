using System;
using System.Collections.Generic;
using System.Linq;

namespace MOE.Common.Models.Repositories
{
    public class PhaseTerminationAggregationRepository : IPhaseTerminationAggregationRepository
    {
        private readonly SPM _db;

        public PhaseTerminationAggregationRepository()
        {
            _db = new SPM();
        }

        public PhaseTerminationAggregationRepository(SPM context)
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
        

        public List<PhaseTerminationAggregation> GetPhaseTerminationsAggregationBySignalIdPhaseNumberAndDateRange(string signalId, int phaseNumber,
            DateTime startDate, DateTime endDate)
        {
            return _db.PhaseTerminationAggregations.Where(p =>
                p.SignalId == signalId && p.PhaseNumber == phaseNumber && p.BinStartTime >= startDate &&
                p.BinStartTime < endDate).ToList();
        }

        public List<int> GetAvailablePhaseNumbers(Signal signal, DateTime startDate, DateTime endDate)
        {
            return _db.PhaseTerminationAggregations.Where(p =>
                    p.SignalId == signal.SignalID && p.BinStartTime >= startDate &&
                    p.BinStartTime < endDate)
                .Select(p => p.PhaseNumber).Distinct().ToList();
        }


        public void Update(PhaseTerminationAggregation approachSplitFailAggregation)
        {
            throw new NotImplementedException();
        }

        public DateTime? GetLastAggregationDate()
        {
            return _db.PhaseTerminationAggregations.Max(s => (DateTime?)s.BinStartTime);
        }
    }
}