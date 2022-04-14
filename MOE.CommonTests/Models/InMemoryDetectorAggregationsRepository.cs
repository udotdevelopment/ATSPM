using System;
using System.Collections.Generic;
using System.Linq;
using MOE.Common.Models;
using MOE.Common.Models.Repositories;
using MOE.CommonTests.Models;


namespace MOE.CommonTests.Models
{
    public class InMemoryDetectorAggregationsRepository : IDetectorEventCountAggregationRepository
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

        public List<DetectorEventCountAggregation> GetActivationsByDetectorIDandDateRange(string detectorId, DateTime Start, DateTime End)
        {
            throw new NotImplementedException();
        }

        public DetectorEventCountAggregation Add(DetectorEventCountAggregation DetectorAggregation)
        {
            throw new NotImplementedException();
        }

        public List<DetectorEventCountAggregation> GetActivationsByDetectorIDandDateRange(int detectorPrimaryId, DateTime start, DateTime end)
        {
            List<DetectorEventCountAggregation> activationsList = (from r in this._db.DetectorAggregations
                where r.DetectorPrimaryId == detectorPrimaryId
                      && r.BinStartTime >= start && r.BinStartTime <= end
                                                         select r).ToList();

            return activationsList;
        }

        public void Remove(DetectorEventCountAggregation DetectorAggregation)
        {
            throw new NotImplementedException();
        }

        public List<DetectorEventCountAggregation> GetDetectorAggregationByApproachIdAndDateRange(int detectorId, DateTime startDate, DateTime endDate)
        {
            return _db.DetectorAggregations.Where(r => r.DetectorPrimaryId == detectorId
                                                                && r.BinStartTime >= startDate &&
                                                                r.BinStartTime <= endDate).ToList();
        }

        public void Update(DetectorEventCountAggregation DetectorAggregation)
        {
            throw new NotImplementedException();
        }

        public int GetDetectorEventCountSumAggregationByDetectorIdAndDateRange(int detectorId, DateTime start, DateTime end)
        {
            throw new NotImplementedException();
        }

        public List<DetectorEventCountAggregation> GetDetectorEventCountAggregationByDetectorIdAndDateRange(int detectorId, DateTime start, DateTime end)
        {
            throw new NotImplementedException();
        }
    }
}