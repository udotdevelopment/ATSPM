using System;
using System.Collections.Generic;
using System.Linq;

namespace MOE.Common.Models.Repositories
{
    public class PhaseSplitMonitorAggregationRepository : IPhaseSplitMonitorAggregationRepository
    {
        private readonly SPM _db;

        public PhaseSplitMonitorAggregationRepository()
        {
            _db = new SPM();
        }

        public PhaseSplitMonitorAggregationRepository(SPM context)
        {
            _db = context;
        }
        

        public void Remove(PhaseSplitMonitorAggregation splitMonitorAggregation)
        {
            throw new NotImplementedException();
        }
        

        public List<PhaseSplitMonitorAggregation> GetSplitMonitorAggregationBySignalIdPhaseNumberAndDateRange(string signalId, int phaseNumber,
            DateTime startDate, DateTime endDate)
        {
            return _db.PhaseSplitMonitorAggregationsAggregations.Where(p =>
                p.SignalId == signalId && p.PhaseNumber == phaseNumber && p.BinStartTime >= startDate &&
                p.BinStartTime < endDate).ToList();
        }

        public List<int> GetAvailablePhaseNumbers(Signal signal, DateTime startDate, DateTime endDate)
        {
            return _db.PhaseSplitMonitorAggregationsAggregations.Where(p =>
                    p.SignalId == signal.SignalID && p.BinStartTime >= startDate &&
                    p.BinStartTime < endDate)
                .Select(p => p.PhaseNumber).Distinct().ToList();
        }

        public void Update(PhaseSplitMonitorAggregation splitMonitorAggregation)
        {
            throw new NotImplementedException();
        }

        public PhaseSplitMonitorAggregation Add(PhaseSplitMonitorAggregation splitMonitorAggregation)
        {
            throw new NotImplementedException();
        }
    }
}