using ATSPM.IRepositories;
using ATSPM.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ATSPM.Infrastructure.Repositories.EntityFramework
{
    public class ApproachSplitFailAggregationRepository : IApproachSplitFailAggregationRepository
    {
        private readonly MOEContext _db;

        //public ApproachSplitFailAggregationRepository()
        //{
        //    _db = new MOEContext();
        //}

        public ApproachSplitFailAggregationRepository(MOEContext context)
        {
            _db = context;
        }

        public ApproachSplitFailAggregation Add(ApproachSplitFailAggregation approachSplitFailAggregation)
        {
            throw new NotImplementedException();
        }

        public int GetApproachSplitFailCountAggregationByApproachIdAndDateRange(int approachId, DateTime start,
            DateTime end)
        {
            var splitFails = 0;
            if (_db.ApproachSplitFailAggregations.Any(r => r.ApproachId == approachId
                                                           && r.BinStartTime >= start && r.BinStartTime < end))
                splitFails = _db.ApproachSplitFailAggregations.Where(r => r.ApproachId == approachId
                                                                          && r.BinStartTime >= start &&
                                                                          r.BinStartTime < end)
                    .Sum(r => r.SplitFailures);
            return splitFails;
        }

        public void Remove(ApproachSplitFailAggregation approachSplitFailAggregation)
        {
            throw new NotImplementedException();
        }

        public List<ApproachSplitFailAggregation> GetApproachSplitFailsAggregationBySignalIdPhaseDateRange(
            string signalId, int approachID, int phase, DateTime startDate, DateTime endDate)
        {
            return _db.ApproachSplitFailAggregations.Where(r => r.SignalId == signalId
                                                                && r.ApproachId == approachID
                                                                && r.PhaseNumber == phase
                                                                && r.BinStartTime >= startDate &&
                                                                r.BinStartTime < endDate).ToList();
        }

        public bool Exists(string signalId, int phaseNumber, DateTime startDate, DateTime endDate)
        {
            return _db.ApproachSplitFailAggregations.Any(p =>
                p.SignalId == signalId && p.PhaseNumber == phaseNumber && p.BinStartTime >= startDate &&
                p.BinStartTime < endDate);
        }


        public void Update(ApproachSplitFailAggregation approachSplitFailAggregation)
        {
            throw new NotImplementedException();
        }
    }
}