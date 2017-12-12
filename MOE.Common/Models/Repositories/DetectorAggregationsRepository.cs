using System;
using System.Collections.Generic;
using System.Linq;


namespace MOE.Common.Models.Repositories
{
    public class DetectorAggregationsRepository : IDetectorAggregationsRepository
    {
        private Models.SPM _db;


        public DetectorAggregationsRepository()
        {
            _db = new SPM();
        }

        public DetectorAggregationsRepository(SPM context)
        {
            _db = context;
        }
        public DetectorAggregation Add(DetectorAggregation DetectorAggregation)
        {
            throw new NotImplementedException();
        }

        public List<DetectorAggregation> GetActivationsByDetectorIDandDateRange(string detectorId, DateTime start, DateTime end)
        {
            List<DetectorAggregation> activationsList = (from r in this._db.DetectorAggregations
                where r.DetectorId == detectorId
                      && r.BinStartTime >= start && r.BinStartTime <= end
                                                         select r).ToList();

            return activationsList;
        }

        public void Remove(DetectorAggregation DetectorAggregation)
        {
            throw new NotImplementedException();
        }

        public void Update(DetectorAggregation DetectorAggregation)
        {
            throw new NotImplementedException();
        }
    }
}