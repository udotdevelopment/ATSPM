using System;
using System.Collections.Generic;
using System.Linq;
using MOE.Common.Business.DataAggregation;
using MOE.Common.Models;
using MOE.Common.Models.Repositories;

namespace MOE.CommonTests.Models
{
    public partial class InMemoryApproachPcdAggregationRepository: IApproachPcdAggregationRepository
    {
        private InMemoryMOEDatabase _db;

        public InMemoryApproachPcdAggregationRepository()
        {
            _db = new InMemoryMOEDatabase();
        }

        public InMemoryApproachPcdAggregationRepository(InMemoryMOEDatabase context )
        {
            _db = context;
        }

        public int GetApproachPcdCountAggregationByApproachIdAndDateRange(int versionId, DateTime start,
            DateTime end)
        {
            throw new NotImplementedException();
        }

        public ApproachPcdAggregation Add(ApproachPcdAggregation priorityAggregation)
        {
            throw new NotImplementedException();
        }

        public void Update(ApproachPcdAggregation priorityAggregation)
        {
            throw new NotImplementedException();
        }

        public void Remove(ApproachPcdAggregation priorityAggregation)
        {
            throw new NotImplementedException();
        }

        public List<ApproachPcdAggregation> GetApproachPcdsAggregationByApproachIdAndDateRange(int approachId, DateTime startDate, DateTime endDate, bool getProtectedPhase)
        {
             var list = _db.ApproachPcdAggregations.Where(r => r.ApproachId == approachId
                                                                && r.BinStartTime >= startDate &&
                                                                r.BinStartTime < endDate && r.IsProtectedPhase == getProtectedPhase).ToList();
            return list;
        }

        public DateTime? GetLastAggregationDate()
        {
            throw new NotImplementedException();
        }
    }
}