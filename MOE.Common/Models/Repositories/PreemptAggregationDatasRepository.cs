using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage;

namespace MOE.Common.Models.Repositories
{
    public class PreemptAggregationDatasRepository : IPreemptAggregationDatasRepository
    {
        SPM db = new SPM();

     
        private Models.SPM _db;


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

        public PreemptionAggregation Add(PreemptionAggregation preemptionAggregation)
        {
            throw new NotImplementedException();
        }



        public List<DetectorAggregation> GetActivationsByDetectorIDandDateRange(string detectorId, DateTime Start, DateTime End)
        {
            throw new NotImplementedException();
        }

        public List<PreemptionAggregation> GetPreemptAggregationByVersionIdAndDateRange(int versionId, DateTime start, DateTime end)
        {
            var records = (from r in this._db.PreemptionAggregations
                                                           where r.VersionId == versionId
                      && r.BinStartTime >= start && r.BinStartTime <= end
                select r).ToList();

            return records;
        }

        public void Remove(PreemptionAggregation preemptionAggregation)
        {
            throw new NotImplementedException();
        }

        public List<PreemptionAggregation> GetPreemptionsBySignalIdAndDateRange(string signalId, DateTime startDate, DateTime endDate)
        {
            return db.PreemptionAggregations
                .Where(p => p.SignalId == signalId && p.BinStartTime >= startDate && p.BinStartTime < endDate).ToList();
        }


        public void Update(PreemptionAggregation preemptionAggregation)
        {
            throw new NotImplementedException();
        }

        DetectorAggregation IPreemptAggregationDatasRepository.Add(PreemptionAggregation preemptionAggregation)
        {
            throw new NotImplementedException();
        }
    }
}
