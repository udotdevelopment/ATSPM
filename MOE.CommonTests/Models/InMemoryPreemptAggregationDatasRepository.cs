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

        public List<PreemptionAggregation> GetPreemptAggregationByVersionIdAndDateRange(string signalId, DateTime start, DateTime end)
        {
            var records = (from r in this._db.PreemptionAggregations
                where r.SignalId == signalId
                      && r.BinStartTime >= start && r.BinStartTime <= end
                select r).ToList();

            return records;
        }

        public int GetPreemptAggregationTotalByVersionIdAndDateRange(string signalId, DateTime start, DateTime end)
        {
            int serviced = (from r in this._db.PreemptionAggregations
                where r.SignalId == signalId
                      && r.BinStartTime >= start && r.BinStartTime <= end
                select r.PreemptServices).Sum();

            return serviced;
        }

        public int GetPreemptAggregationTotalByVersionIdPreemptNumberAndDateRange(string signalId, DateTime start, DateTime end,
            int preemptNumber)
        {
            int serviced = (from r in this._db.PreemptionAggregations
                where r.SignalId == signalId && r.PreemptNumber == preemptNumber
                      && r.BinStartTime >= start && r.BinStartTime <= end
                select r.PreemptServices).Sum();

            return serviced;
        }

        public InMemoryPreemptAggregationDatasRepository(InMemoryMOEDatabase db)
        {
            this._db = db;
        }

        public PreemptionAggregation Add(PreemptionAggregation preemptionAggregation)
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

        public List<PreemptionAggregation> GetPreemptionsBySignalIdAndDateRange(string signalId, DateTime startDate, DateTime endDate)
        {
            return _db.PreemptionAggregations
                .Where(p => p.SignalId == signalId && p.BinStartTime >= startDate && p.BinStartTime < endDate).ToList();
        }

        public void Update(PreemptionAggregation preemptionAggregation)
        {
            throw new NotImplementedException();
        }

        public List<PreemptionAggregation> GetPreemptAggregationByVersionIdAndDateRange(DateTime start, DateTime end)
        {
            throw new NotImplementedException();
        }

        public int GetPreemptAggregationTotalByVersionIdAndDateRange(DateTime start, DateTime end)
        {
            throw new NotImplementedException();
        }

        public int GetPreemptAggregationTotalByVersionIdPreemptNumberAndDateRange(DateTime start, DateTime end, int preemptNumber)
        {
            throw new NotImplementedException();
        }
    }
}