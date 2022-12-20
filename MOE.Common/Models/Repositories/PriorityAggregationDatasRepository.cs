using System;
using System.Collections.Generic;
using System.Linq;

namespace MOE.Common.Models.Repositories
{
    public class PriorityAggregationDatasRepository : IPriorityAggregationDatasRepository
    {
        private readonly SPM _db;
        private readonly SPM db = new SPM();


        public PriorityAggregationDatasRepository()
        {
            _db = new SPM();
        }

        public PriorityAggregationDatasRepository(SPM context)
        {
            _db = context;
        }

        public PriorityAggregation Add(PriorityAggregation priorityAggregation)
        {
            _db.PriorityAggregations.Add(priorityAggregation);
            return priorityAggregation;
        }


        public List<PriorityAggregation> GetPriorityAggregationByVersionIdAndDateRange(int versionId, DateTime start,
            DateTime end)
        {
            var records = (from r in _db.PriorityAggregations
                where r.BinStartTime >= start && r.BinStartTime <= end
                select r).ToList();

            return records;
        }

        public void Remove(PriorityAggregation priorityAggregation)
        {
            throw new NotImplementedException();
        }

        public List<PriorityAggregation> GetPriorityBySignalIdAndDateRange(string signalId, DateTime start,
            DateTime end)
        {
            return db.PriorityAggregations
                .Where(p => p.SignalId == signalId && p.BinStartTime >= start && p.BinStartTime < end).ToList();
        }


        public void Update(PriorityAggregation priorityAggregation)
        {
            throw new NotImplementedException();
        }

        public DateTime? GetLastAggregationDate()
        {
            return _db.PriorityAggregations.Max(s => (DateTime?)s.BinStartTime);
        }
    }
}