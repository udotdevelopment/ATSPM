using ATSPM.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using ATSPM.Application.Models;

namespace ATSPM.Infrastructure.Repositories.EntityFramework
{
    public class ApproachCycleAggregationRepository : IApproachCycleAggregationRepository
    {
        private readonly MOEContext _db;

        public ApproachCycleAggregationRepository(MOEContext context)
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
                                                       && r.BinStartTime >= start && r.BinStartTime < end))
                cycles = _db.PhaseCycleAggregations.Where(r => r.ApproachId == approachId
                                                                  && r.BinStartTime >= start &&
                                                                  r.BinStartTime < end)
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
                                                       && r.BinStartTime >= start && r.BinStartTime < end))
                return _db.PhaseCycleAggregations.Where(r => r.ApproachId == approachId
                                                                 && r.BinStartTime >= start &&
                                                                 r.BinStartTime < end)
                    .ToList();
            else
                return new List<PhaseCycleAggregation>();
        }

        public bool Exists(string signalId, int phaseNumber, DateTime start,
            DateTime end)
        {
            return _db.PhaseCycleAggregations.Any(r => r.SignalId == signalId && r.PhaseNumber == phaseNumber
                                                             && r.BinStartTime >= start &&
                                                             r.BinStartTime < end);
        }

        public List<PhaseCycleAggregation> GetApproachCyclesAggregationBySignalIdPhaseAndDateRange(string signalId, int phase, DateTime start,
            DateTime end)
        {
            if (_db.PhaseCycleAggregations.Any(r => r.SignalId == signalId
            && r.PhaseNumber == phase && r.BinStartTime >= start && r.BinStartTime < end))
                return _db.PhaseCycleAggregations.Where(r => r.SignalId == signalId
                                                                 && r.PhaseNumber == phase
                                                                 && r.BinStartTime >= start
                                                                 && r.BinStartTime < end)
                    .ToList();
            else
                return new List<PhaseCycleAggregation>();
        }

        public double GetAverageRedToRedCyclesBySignalIdPhase(string signalId, int phaseNumber, DateTime start,
            DateTime end)
        {
            if (_db.PhaseCycleAggregations.Any(r => r.SignalId == signalId && r.PhaseNumber == phaseNumber
                                                       && r.BinStartTime >= start && r.BinStartTime < end))
                return _db.PhaseCycleAggregations.Where(r => r.SignalId == signalId
                                                            && r.PhaseNumber == phaseNumber
                                                            && r.BinStartTime >= start
                                                            && r.BinStartTime < end)
                                                  .Average(r => r.TotalRedToRedCycles);

            else
                return 0;
        }

        //public List<PhaseCycleAggregation> GetApproachCyclesAggregationByApproachIdAndDateRange(int approachId,
        //    DateTime startDate, DateTime endDate, bool getProtectedPhase)
        //{
        //    return _db.PhaseCycleAggregations.Where(r => r.ApproachId == approachId
        //                                                    && r.BinStartTime >= startDate &&
        //                                                    r.BinStartTime < endDate
        //                                                    && r.IsProtectedPhase == getProtectedPhase).ToList();
        //}


        public void Update(PhaseCycleAggregation phaseCycleAggregation)
        {
            throw new NotImplementedException();
        }

        int IApproachCycleAggregationRepository.GetCycleCountBySignalIdAndDateRange(string signalId, int phaseNumber, DateTime dateTime1, DateTime dateTime2)
        {
            return _db.PhaseCycleAggregations.Where(r => r.SignalId == signalId
                                                            && r.PhaseNumber == phaseNumber
                                                            && r.BinStartTime >= dateTime1 &&
                                                            r.BinStartTime < dateTime2).ToList().Sum(p => p.TotalGreenToGreenCycles);
        }
    }
}