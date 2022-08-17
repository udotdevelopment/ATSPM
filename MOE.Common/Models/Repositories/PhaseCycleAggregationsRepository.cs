using System;
using System.Collections.Generic;
using System.Linq;

namespace MOE.Common.Models.Repositories
{
    public class PhaseCycleAggregationsRepository : IPhaseCycleAggregationRepository
    {
        private readonly SPM _db;

        public PhaseCycleAggregationsRepository()
        {
            _db = new SPM();
        }

        public PhaseCycleAggregationsRepository(SPM context)
        {
            _db = context;
        }

        public PhaseCycleAggregation Add(PhaseCycleAggregation phaseCycleAggregation)
        {
            throw new NotImplementedException();
        }

        public int GetApproachCycleCountAggregationByApproachIdAndDateRange(int approachId, DateTime start,
            DateTime end)
        {
            var cycles = 0;
            if (_db.PhaseCycleAggregations.Any(r => r.ApproachId == approachId
                                                       && r.BinStartTime >= start && r.BinStartTime <= end))
                cycles = _db.PhaseCycleAggregations.Where(r => r.ApproachId == approachId
                                                                  && r.BinStartTime >= start &&
                                                                  r.BinStartTime <= end)
                    .Sum(r => r.TotalRedToRedCycles);
            return cycles;
        }

        public void Remove(PhaseCycleAggregation phaseCycleAggregation)
        {
            throw new NotImplementedException();
        }

        public List<PhaseCycleAggregation> GetApproachCyclesAggregationByApproachIdAndDateRange(int approachId, DateTime start,
            DateTime end)
        {
            if (_db.PhaseCycleAggregations.Any(r => r.ApproachId == approachId
                                                       && r.BinStartTime >= start && r.BinStartTime <= end))
                return _db.PhaseCycleAggregations.Where(r => r.ApproachId == approachId
                                                                 && r.BinStartTime >= start &&
                                                                 r.BinStartTime <= end)
                    .ToList();
            else
                return new List<PhaseCycleAggregation>();
        }

        public List<PhaseCycleAggregation> GetApproachCyclesAggregationBySignalIdPhaseAndDateRange(string signalId, int phase,  DateTime start,
            DateTime end)
        {
            if (_db.PhaseCycleAggregations.Any(r => r.SignalId == signalId
            && r.PhaseNumber == phase && r.BinStartTime >= start && r.BinStartTime <= end))
                return _db.PhaseCycleAggregations.Where(r => r.SignalId == signalId
                                                                 && r.PhaseNumber == phase
                                                                 && r.BinStartTime >= start 
                                                                 && r.BinStartTime <= end)
                    .ToList();
            else
                return new List<PhaseCycleAggregation>();
        }

        public double GetAverageRedToRedCyclesBySignalIdPhase(string signalId, int phaseNumber, DateTime start,
            DateTime end)
        {
            if (_db.PhaseCycleAggregations.Any(r => r.SignalId == signalId && r.PhaseNumber == phaseNumber
                                                       && r.BinStartTime >= start && r.BinStartTime <= end))
                return _db.PhaseCycleAggregations.Where(r => r.SignalId == signalId
                                                            && r.PhaseNumber == phaseNumber
                                                            && r.BinStartTime >= start
                                                            && r.BinStartTime <= end)
                                                  .Average(r => r.TotalRedToRedCycles);

            else
                return 0;
        }

        //public List<ApproachCycleAggregation> GetApproachCyclesAggregationByApproachIdAndDateRange(int approachId,
        //    DateTime startDate, DateTime endDate, bool getProtectedPhase)
        //{
        //    return _db.ApproachCycleAggregations.Where(r => r.ApproachId == approachId
        //                                                    && r.BinStartTime >= startDate &&
        //                                                    r.BinStartTime <= endDate
        //                                                    && r.IsProtectedPhase == getProtectedPhase).ToList();
        //}


        public void Update(PhaseCycleAggregation phaseCycleAggregation)
        {
            throw new NotImplementedException();
        }
    }
}