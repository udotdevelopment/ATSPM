using ATSPM.IRepositories;
using ATSPM.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ATSPM.Infrastructure.Repositories.EntityFramework
{
    public class PhasePedAggregationRepository : IPhasePedAggregationRepository
    {
        private readonly MOEContext _db;


        public PhasePedAggregationRepository(MOEContext context)
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

        public bool Exists(string signalId, int phaseNumber, DateTime startDate, DateTime endDate)
        {
            var result =  _db.PhasePedAggregations.Any(p =>
                p.SignalId == signalId && p.PhaseNumber == phaseNumber && p.BinStartTime >= startDate &&
                p.BinStartTime < endDate);
            return result;
        }

        public List<int> GetAvailablePhaseNumbers(Signal signal, DateTime startDate, DateTime endDate)
        {
            return _db.PhasePedAggregations.Where(p =>
                    p.SignalId == signal.SignalId && p.BinStartTime >= startDate &&
                    p.BinStartTime < endDate)
                .Select(p => p.PhaseNumber).ToList();
        }


        public void Update(PhasePedAggregation approachSplitFailAggregation)
        {
            throw new NotImplementedException();
        }
    }
}