using System;
using System.Collections.Generic;
using System.Linq;
using MOE.Common.Models;
using MOE.Common.Models.Repositories;

namespace MOE.CommonTests.Models
{
    public class InMemoryPriorityAggregationDatasRepository: IPriorityAggregationDatasRepository
    {
       


        private InMemoryMOEDatabase _db;


        public InMemoryPriorityAggregationDatasRepository()
        {
            _db = new InMemoryMOEDatabase();
        }

        public InMemoryPriorityAggregationDatasRepository(InMemoryMOEDatabase context)
        {
            _db = context;
        }
        public PriorityAggregation Add(PriorityAggregation priorityAggregation)
        {
            throw new NotImplementedException();
        }

        public List<PriorityAggregation> GetPriorityAggregationByVersionIdAndDateRange(string signalId, DateTime start, DateTime end)
        {
            var records = (from r in this._db.PriorityAggregations
                where r.SignalId == signalId
                      && r.BinStartTime >= start && r.BinStartTime <= end
                select r).ToList();

            return records;
        }

        public void Remove(PriorityAggregation priorityAggregation)
        {
            throw new NotImplementedException();
        }

        public List<PriorityAggregation> GetPriorityBySignalIdAndDateRange(string signalId, DateTime start, DateTime end)
        {
            return _db.PriorityAggregations
                .Where(p => p.SignalId == signalId && p.BinStartTime >= start && p.BinStartTime < end).ToList();
        }

        public void Update(PriorityAggregation priorityAggregation)
        {
            throw new NotImplementedException();
        }

        public List<PriorityAggregation> GetPriorityAggregationByVersionIdAndDateRange(int versionId, DateTime start, DateTime end)
        {
            throw new NotImplementedException();
        }
    }
}