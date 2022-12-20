using System;
using System.Collections.Generic;
using System.Linq;
using MOE.Common.Business.DataAggregation;
using MOE.Common.Models;
using MOE.Common.Models.Repositories;

namespace MOE.CommonTests.Models
{
    public partial class InMemoryYellowRedActivationsAggregationByApproachRepository: IApproachYellowRedActivationsAggregationRepository
    {
        private InMemoryMOEDatabase _db;

        public InMemoryYellowRedActivationsAggregationByApproachRepository()
        {
            _db = new InMemoryMOEDatabase();
        }

        public InMemoryYellowRedActivationsAggregationByApproachRepository(InMemoryMOEDatabase context )
        {
            _db = context;
        }

        public int GetApproachYellowRedActivationsCountAggregationByApproachIdAndDateRange(int versionId, DateTime start,
            DateTime end)
        {
            throw new NotImplementedException();
        }

        public YellowRedActivationsAggregationByApproach Add(YellowRedActivationsAggregationByApproach priorityAggregation)
        {
            throw new NotImplementedException();
        }

        public void Update(YellowRedActivationsAggregationByApproach priorityAggregation)
        {
            throw new NotImplementedException();
        }

        public void Remove(YellowRedActivationsAggregationByApproach priorityAggregation)
        {
            throw new NotImplementedException();
        }

        public List<ApproachYellowRedActivationAggregation> GetApproachYellowRedActivationssAggregationByApproachIdAndDateRange(int approachId, DateTime startDate, DateTime endDate, bool getProtectedPhase)
        {
             var list = _db.ApproachYellowRedActivationAggregations.Where(r => r.ApproachId == approachId
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