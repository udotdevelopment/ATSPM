using System;
using System.Collections.Generic;
using System.Linq;

namespace MOE.Common.Models.Repositories
{
    public class PhaseLeftTurnGapAggregationRepository : IPhaseLeftTurnGapAggregationRepository
    {
        private readonly SPM _db;

        public PhaseLeftTurnGapAggregationRepository()
        {
            _db = new SPM();
        }

        public PhaseLeftTurnGapAggregationRepository(SPM context)
        {
            _db = context;
        }

        public PhaseLeftTurnGapAggregation Add(PhaseLeftTurnGapAggregation approachSplitFailAggregation)
        {
            throw new NotImplementedException();
        }
        

        public void Remove(PhaseLeftTurnGapAggregation approachSplitFailAggregation)
        {
            throw new NotImplementedException();
        }
        

        public List<PhaseLeftTurnGapAggregation> GetPhaseLeftTurnGapAggregationBySignalIdPhaseNumberAndDateRange(string signalId, int phaseNumber,
            DateTime startDate, DateTime endDate)
        {
            return _db.PhaseLeftTurnGapAggregations.Where(p =>
                p.SignalId == signalId && p.PhaseNumber == phaseNumber && p.BinStartTime >= startDate &&
                p.BinStartTime < endDate).ToList();
        }

        public int GetSummedGapsBySignalIdPhaseNumberAndDateRange(string signalId, int phaseNumber,
            DateTime startDate, DateTime endDate, int gapCountColumn)
        {
             var query = _db.PhaseLeftTurnGapAggregations.Where(p =>
                p.SignalId == signalId && p.PhaseNumber == phaseNumber && p.BinStartTime >= startDate &&
                p.BinStartTime < endDate);
            switch (gapCountColumn)
            {
                case 1: return query.Sum(p => p.GapCount1);
                case 2: return query.Sum(p => p.GapCount2);
                case 3: return query.Sum(p => p.GapCount3);
                case 4: return query.Sum(p => p.GapCount4);
                case 5: return query.Sum(p => p.GapCount5);
                case 6: return query.Sum(p => p.GapCount6);
                case 7: return query.Sum(p => p.GapCount7);
                case 8: return query.Sum(p => p.GapCount8);
                case 9: return query.Sum(p => p.GapCount9);
                case 10: return query.Sum(p => p.GapCount10);
                case 11: return query.Sum(p => p.GapCount11);
                default: throw new Exception("Gap Column not found");
            }
        }

        public List<int> GetAvailablePhaseNumbers(Signal signal, DateTime startDate, DateTime endDate)
        {
            return _db.PhaseLeftTurnGapAggregations.Where(p =>
                    p.SignalId == signal.SignalID && p.BinStartTime >= startDate &&
                    p.BinStartTime < endDate)
                .Select(p => p.PhaseNumber).Distinct().ToList();
        }

        public void Update(PhaseLeftTurnGapAggregation phaseLeftTurnGapAggregation)
        {
            throw new NotImplementedException();
        }

        public List<PhaseLeftTurnGapAggregation> GetAggregationByApproachIdAndDateRange(int approachId, DateTime start, DateTime end)
        {
            if (_db.PhaseLeftTurnGapAggregations.Any(r => r.ApproachId == approachId
                                                      && r.BinStartTime >= start && r.BinStartTime <= end))
                return _db.PhaseLeftTurnGapAggregations.Where(r => r.ApproachId == approachId
                                                                 && r.BinStartTime >= start &&
                                                                 r.BinStartTime <= end)
                    .ToList();
            else
                return new List<PhaseLeftTurnGapAggregation>();
        }
    }
}