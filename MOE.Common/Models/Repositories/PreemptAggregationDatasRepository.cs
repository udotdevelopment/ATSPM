using System;
using System.Collections.Generic;
using System.Linq;

namespace MOE.Common.Models.Repositories
{
    public class PreemptAggregationDatasRepository : IPreemptAggregationDatasRepository
    {
        private readonly SPM _db;
        private readonly SPM db = new SPM();


        public PreemptAggregationDatasRepository()
        {
            _db = new SPM();
        }

        public PreemptAggregationDatasRepository(SPM context)
        {
            _db = context;
        }

        public int GetPreemptAggregationTotalByVersionIdAndDateRange(int versionId, DateTime start, DateTime end)
        {
            var serviced = (from r in _db.PreemptionAggregations
                where r.VersionId == versionId
                      && r.BinStartTime >= start && r.BinStartTime <= end
                select r.PreemptServices).Sum();

            return serviced;
        }

        public int GetPreemptAggregationTotalByVersionIdPreemptNumberAndDateRange(int versionId, DateTime start,
            DateTime end,
            int preemptNumber)
        {
            var serviced = (from r in _db.PreemptionAggregations
                where r.VersionId == versionId && r.PreemptNumber == preemptNumber
                      && r.BinStartTime >= start && r.BinStartTime <= end
                select r.PreemptServices).Sum();

            return serviced;
        }

        public List<PreemptionAggregation> GetPreemptAggregationByVersionIdAndDateRange(int versionId, DateTime start,
            DateTime end)
        {
            var records = (from r in _db.PreemptionAggregations
                where r.VersionId == versionId
                      && r.BinStartTime >= start && r.BinStartTime <= end
                select r).ToList();

            return records;
        }

        public void Remove(PreemptionAggregation preemptionAggregation)
        {
            throw new NotImplementedException();
        }

        public List<PreemptionAggregation> GetPreemptionsBySignalIdAndDateRange(string signalId, DateTime startDate,
            DateTime endDate)
        {
            return db.PreemptionAggregations
                .Where(p => p.SignalId == signalId && p.BinStartTime >= startDate && p.BinStartTime < endDate).ToList();
        }


        public void Update(PreemptionAggregation preemptionAggregation)
        {
            throw new NotImplementedException();
        }

        PreemptionAggregation IPreemptAggregationDatasRepository.Add(PreemptionAggregation preemptionAggregation)
        {
            db.PreemptionAggregations.Add(preemptionAggregation);
            db.SaveChanges();
            return preemptionAggregation;
        }

        public PreemptionAggregation Add(PreemptionAggregation preemptionAggregation)
        {
            throw new NotImplementedException();
        }


        public List<DetectorAggregation> GetActivationsByDetectorIDandDateRange(string detectorId, DateTime Start,
            DateTime End)
        {
            throw new NotImplementedException();
        }
    }
}