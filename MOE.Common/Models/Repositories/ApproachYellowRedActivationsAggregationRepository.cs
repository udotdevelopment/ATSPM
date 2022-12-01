using System;
using System.Collections.Generic;
using System.Linq;
using MOE.Common.Business.DataAggregation;

namespace MOE.Common.Models.Repositories
{
    public class ApproachYellowRedActivationsAggregationRepository : IApproachYellowRedActivationsAggregationRepository
    {
        private readonly SPM _db;

        public ApproachYellowRedActivationsAggregationRepository()
        {
            _db = new SPM();
        }

        public ApproachYellowRedActivationsAggregationRepository(SPM context)
        {
            _db = context;
        }

        public YellowRedActivationsAggregationByApproach Add(
            YellowRedActivationsAggregationByApproach approachYellowRedActivationsAggregation)
        {
            throw new NotImplementedException();
        }

        public int GetApproachYellowRedActivationsCountAggregationByApproachIdAndDateRange(int approachId,
            DateTime start, DateTime end)
        {
            var yellowRedActivations = 0;
            if (_db.ApproachYellowRedActivationAggregations.Any(r => r.ApproachId == approachId
                                                                     && r.BinStartTime >= start &&
                                                                     r.BinStartTime <= end))
                yellowRedActivations = _db.ApproachYellowRedActivationAggregations.Where(r => r.ApproachId == approachId
                                                                                              && r.BinStartTime >=
                                                                                              start &&
                                                                                              r.BinStartTime <= end)
                    .Sum(r => r.TotalRedLightViolations);
            return yellowRedActivations;
        }

        public void Remove(YellowRedActivationsAggregationByApproach approachYellowRedActivationsAggregation)
        {
            throw new NotImplementedException();
        }

        public List<ApproachYellowRedActivationAggregation>
            GetApproachYellowRedActivationssAggregationByApproachIdAndDateRange(int approachId, DateTime startDate,
                DateTime endDate, bool getProtectedPhase)
        {
            return _db.ApproachYellowRedActivationAggregations.Where(r => r.ApproachId == approachId
                                                                          && r.BinStartTime >= startDate &&
                                                                          r.BinStartTime <= endDate
                                                                          && r.IsProtectedPhase == getProtectedPhase).ToList();
        }


        public void Update(YellowRedActivationsAggregationByApproach approachYellowRedActivationsAggregation)
        {
            throw new NotImplementedException();
        }

        public  DateTime? GetLastAggregationDate()
        {
            return _db.ApproachYellowRedActivationAggregations.Max(s => (DateTime?)s.BinStartTime);
        }
    }
}