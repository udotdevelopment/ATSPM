using System;
using System.Collections.Generic;
using System.Linq;
using MOE.Common.Models;
using MOE.Common.Models.Repositories;
using MOE.CommonTests.Models;


namespace MOE.CommonTests.Models
{
    public class InMemoryDetectorAggregationsRepository : IDetectorAggregationsRepository
    {
        private InMemoryMOEDatabase _db;


        public InMemoryDetectorAggregationsRepository()
        {
            _db = new InMemoryMOEDatabase();
        }

        public InMemoryDetectorAggregationsRepository(InMemoryMOEDatabase context)
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

        public List<DetectorAggregation> GetDetectorAggregationByApproachIdAndDateRange(int detectorId, DateTime startDate, DateTime endDate)
        {
            return _db.DetectorAggregations.Where(r => r.DetectorPrimaryId == detectorId
                                                                && r.BinStartTime >= startDate &&
                                                                r.BinStartTime <= endDate).ToList();
        }

        public void Update(DetectorAggregation DetectorAggregation)
        {
            throw new NotImplementedException();
        }
    }
}