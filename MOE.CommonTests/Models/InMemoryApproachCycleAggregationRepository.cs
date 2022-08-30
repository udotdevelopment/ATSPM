using System;
using System.Collections.Generic;
using System.Linq;
using MOE.Common.Models;
using MOE.Common.Models.Repositories;

namespace MOE.CommonTests.Models
{
    public partial class InMemoryApproachCycleAggregationRepository: IApproachCycleAggregationRepository
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

        public ApproachCycleAggregation Add(ApproachCycleAggregation priorityAggregation)
        {
            throw new NotImplementedException();
        }

        public void Update(ApproachCycleAggregation priorityAggregation)
        {
            throw new NotImplementedException();
        }

        public void Remove(ApproachCycleAggregation priorityAggregation)
        {
            throw new NotImplementedException();
        }

        public List<ApproachCycleAggregation> GetApproachCyclesAggregationByApproachIdAndDateRange(int approachId, DateTime startDate, DateTime endDate, bool getProtectedPhase)
        {
            return _db.ApproachCycleAggregations.Where(r => r.ApproachId == approachId
                                                                && r.BinStartTime >= startDate &&
                                                                r.BinStartTime < endDate && r.IsProtectedPhase == getProtectedPhase).ToList();
        }
    }
}