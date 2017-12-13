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

        public List<DetectorAggregation> GetActivationsByDetectorIDandDateRange(string detectorId, DateTime Start, DateTime End)
        {
            throw new NotImplementedException();
        }

        public DetectorAggregation Add(DetectorAggregation DetectorAggregation)
        {
            throw new NotImplementedException();
        }

        public List<DetectorAggregation> GetActivationsByDetectorIDandDateRange(int detectorPrimaryId, DateTime start, DateTime end)
        {
            List<DetectorAggregation> activationsList = (from r in this._db.DetectorAggregations
                where r.DetectorPrimaryId == detectorPrimaryId
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