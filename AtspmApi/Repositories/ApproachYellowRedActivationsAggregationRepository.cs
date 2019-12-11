using System;
using System.Collections.Generic;
using System.Linq;
using AtspmApi.Models;

namespace AtspmApi.Repositories
{
    public class ApproachYellowRedActivationsAggregationRepository : IApproachYellowRedActivationsAggregationRepository
    {
        private readonly Models.AtspmApi _db;

        public ApproachYellowRedActivationsAggregationRepository()
        {
            _db = new Models.AtspmApi();
        }

        public ApproachYellowRedActivationsAggregationRepository(Models.AtspmApi context)
        {
            _db = context;
        }


        public int GetApproachYellowRedActivationsCountAggByApproach(int approachId,
            DateTime start, DateTime end)
        {
            var yellowRedActivations = 0;
            if (_db.ApproachYellowRedActivationAggregations.Any(r => r.ApproachId == approachId
                                                                     && r.BinStartTime >= start &&
                                                                     r.BinStartTime < end))
                yellowRedActivations = _db.ApproachYellowRedActivationAggregations.Where(r => r.ApproachId == approachId
                                                                                              && r.BinStartTime >=
                                                                                              start &&
                                                                                              r.BinStartTime < end)
                    .Count();
            return yellowRedActivations;
        }



        public List<ApproachYellowRedActivationAggregation>
            GetApproachYellowRedActivationssAggregationByApproach(int approachId, DateTime startDate,
                DateTime endDate)
        {
            return _db.ApproachYellowRedActivationAggregations.Where(r => r.ApproachId == approachId
                                                                          && r.BinStartTime >= startDate &&
                                                                          r.BinStartTime < endDate).ToList();
        }

    }
}