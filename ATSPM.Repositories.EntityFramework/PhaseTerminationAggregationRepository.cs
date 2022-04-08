using ATSPM.IRepositories;
using ATSPM.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ATSPM.Infrastructure.Repositories.EntityFramework
{
    public class PhaseTerminationAggregationRepository : IPhaseTerminationAggregationRepository
    {
        private readonly MOEContext _db;


        public PhaseTerminationAggregationRepository(MOEContext context)
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
                    p.SignalId == signal.SignalId && p.BinStartTime >= startDate &&
                    p.BinStartTime < endDate)
                .Select(p => p.PhaseNumber).Distinct().ToList();
        }


        public void Update(PhaseTerminationAggregation approachSplitFailAggregation)
        {
            throw new NotImplementedException();
        }

        public bool Exists(string signalId, int phaseNumber, DateTime startDate, DateTime endDate)
        {
            return _db.PhaseTerminationAggregations.Any(p =>
                p.SignalId == signalId && p.PhaseNumber == phaseNumber && p.BinStartTime >= startDate &&
                p.BinStartTime < endDate);
        }
    }
}