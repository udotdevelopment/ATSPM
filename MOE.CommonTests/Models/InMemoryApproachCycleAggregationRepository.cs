using System;
using System.Collections.Generic;
using System.Linq;
using MOE.Common.Models;
using MOE.Common.Models.Repositories;

namespace MOE.CommonTests.Models
{
    public partial class InMemoryApproachCycleAggregationRepository: IPhaseCycleAggregationRepository
    {
        private InMemoryMOEDatabase _db;

        public InMemoryApproachCycleAggregationRepository()
        {
            _db = new InMemoryMOEDatabase();
        }

        public InMemoryApproachCycleAggregationRepository(InMemoryMOEDatabase context )
        {
            _db = context;
        }

        public int GetApproachCycleCountAggregationByApproachIdAndDateRange(int approachId, DateTime start,
            DateTime end)
        {
            throw new NotImplementedException();
        }

        public PhaseCycleAggregation Add(PhaseCycleAggregation priorityAggregation)
        {
            throw new NotImplementedException();
        }

        public void Update(PhaseCycleAggregation priorityAggregation)
        {
            throw new NotImplementedException();
        }

        public void Remove(PhaseCycleAggregation priorityAggregation)
        {
            throw new NotImplementedException();
        }

        public List<PhaseCycleAggregation> GetApproachCyclesAggregationByApproachIdAndDateRange(int approachId, DateTime startDate, DateTime endDate, int phaseNumber)
        {
            return _db.ApproachCycleAggregations.Where(r => r.ApproachId == approachId
                                                                && r.BinStartTime >= startDate &&
                                                                r.BinStartTime < endDate && r.PhaseNumber == phaseNumber).ToList();
        }

        public List<PhaseCycleAggregation> GetApproachCyclesAggregationByApproachIdAndDateRange(int approachId, DateTime start, DateTime end)
        {
            return _db.ApproachCycleAggregations.Where(r => r.ApproachId == approachId
                                                                && r.BinStartTime >= start &&
                                                                r.BinStartTime < end).ToList();
        }

        public double GetAverageRedToRedCyclesBySignalIdPhase(string signalId, int phaseNumber, DateTime start, DateTime end)
        {
            throw new NotImplementedException();
        }

        public List<PhaseCycleAggregation> GetApproachCyclesAggregationBySignalIdPhaseAndDateRange(string signalId, int phase, DateTime start, DateTime end)
        {
            throw new NotImplementedException();
        }
    }
}