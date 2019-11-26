using System;
using System.Collections.Generic;
using System.Linq;
using AtspmApi.Models;

namespace AtspmApi.Repositories
{
    public class PriorityAggregationDatasRepository : IPriorityAggregationDatasRepository
    {
        private readonly Models.AtspmApi _db;
        private readonly Models.AtspmApi db = new Models.AtspmApi();


        public PriorityAggregationDatasRepository()
        {
            _db = new Models.AtspmApi();
        }

        public PriorityAggregationDatasRepository(Models.AtspmApi context)
        {
            _db = context;
        }

        public PriorityAggregation Add(PriorityAggregation priorityAggregation)
        {
            _db.PriorityAggregations.Add(priorityAggregation);
            return priorityAggregation;
        }


        public int GetPriorityAggCountBySignal(string signalId, DateTime start, DateTime end)
        {
            var cycles = 0;
            if (_db.PriorityAggregations.Any(p => p.SignalID == signalId && p.BinStartTime >= start && p.BinStartTime < end))
                cycles = _db.PriorityAggregations.Where(p => p.SignalID == signalId && p.BinStartTime >= start && p.BinStartTime < end).Count();
            return cycles;

        }
        //public List<PriorityAggregation> GetPriorityAggregationByVersionIdAndDateRange(int versionId, DateTime start,
        //    DateTime end)
        //{
        //    var records = (from r in _db.PriorityAggregations
        //        where r.VersionId == versionId
        //              && r.BinStartTime >= start && r.BinStartTime < end
        //        select r).ToList();

        //    return records;
        //}

        public void Remove(PriorityAggregation priorityAggregation)
        {
            throw new NotImplementedException();
        }

        public List<PriorityAggregation> GetPriorityBySignalIdAndDateRange(string signalId, DateTime start,
            DateTime end)
        {
            return db.PriorityAggregations
                .Where(p => p.SignalID == signalId && p.BinStartTime >= start && p.BinStartTime < end).ToList();
        }


        public void Update(PriorityAggregation priorityAggregation)
        {
            throw new NotImplementedException();
        }
    }
}