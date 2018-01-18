using System;
using System.Collections.Generic;
using System.Linq;
using MOE.Common.Models;
using MOE.Common.Models.Repositories;
using MOE.CommonTests.Models;

namespace MOE.CommonTests.Models
{
    public class InMemoryPreemptAggregationDatasRepository : IPreemptAggregationDatasRepository
    {
        private InMemoryMOEDatabase _db;

        public List<PreemptionAggregation> GetPreemptAggregationByVersionIdAndDateRange(int versionId, DateTime start, DateTime end)
        {
            var records = (from r in this._db.PreemptionAggregations
                where r.VersionId == versionId
                      && r.BinStartTime >= start && r.BinStartTime <= end
                select r).ToList();

            return records;
        }

        public int GetPreemptAggregationTotalByVersionIdAndDateRange(int versionId, DateTime start, DateTime end)
        {
            int serviced = (from r in this._db.PreemptionAggregations
                where r.VersionId == versionId
                      && r.BinStartTime >= start && r.BinStartTime <= end
                select r.PreemptServices).Sum();

            return serviced;
        }

        public int GetPreemptAggregationTotalByVersionIdPreemptNumberAndDateRange(int versionId, DateTime start, DateTime end,
            int preemptNumber)
        {
            int serviced = (from r in this._db.PreemptionAggregations
                where r.VersionId == versionId && r.PreemptNumber == preemptNumber
                      && r.BinStartTime >= start && r.BinStartTime <= end
                select r.PreemptServices).Sum();

            return serviced;
        }

        public InMemoryPreemptAggregationDatasRepository(InMemoryMOEDatabase db)
        {
            this._db = db;
        }

        public DetectorAggregation Add(PreemptionAggregation preemptionAggregation)
        {
            throw new NotImplementedException();
        }



        public InMemoryPreemptAggregationDatasRepository()
        {
            _db = new InMemoryMOEDatabase();
        }

        public void Remove(PreemptionAggregation preemptionAggregation)
        {
            throw new NotImplementedException();
        }

        public void Update(PreemptionAggregation preemptionAggregation)
        {
            throw new NotImplementedException();
        }
    }
}