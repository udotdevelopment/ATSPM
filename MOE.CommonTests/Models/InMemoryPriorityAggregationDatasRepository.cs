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

        public List<PriorityAggregation> GetPriorityAggregationByVersionIdAndDateRange(int versionId, DateTime start, DateTime end)
        {
            var records = (from r in this._db.PriorityAggregations
                where r.VersionId == versionId
                      && r.BinStartTime >= start && r.BinStartTime <= end
                select r).ToList();

            return records;
        }

        public void Remove(PriorityAggregation priorityAggregation)
        {
            throw new NotImplementedException();
        }

        public void Update(PriorityAggregation priorityAggregation)
        {
            throw new NotImplementedException();
        }
    }
}