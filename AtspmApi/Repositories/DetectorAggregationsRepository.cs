using System;
using System.Collections.Generic;
using System.Linq;
using AtspmApi.Models;

namespace AtspmApi.Repositories
{
    public class DetectorAggregationsRepository : IDetectorAggregationsRepository
    {
        private readonly Models.AtspmApi _db;


        public DetectorAggregationsRepository()
        {
            _db = new Models.AtspmApi();
        }

        public DetectorAggregationsRepository(Models.AtspmApi context)
        {
            _db = context;
        }

        public DetectorAggregation Add(DetectorAggregation DetectorAggregation)
        {
            throw new NotImplementedException();
        }

        public List<DetectorAggregation> GetActivationsByDetectorIDandDateRange(int detectorPrimaryId, DateTime start,
            DateTime end)
        {
            var activationsList = (from r in _db.DetectorAggregations
                where r.DetectorPrimaryId == detectorPrimaryId
                      && r.BinStartTime >= start && r.BinStartTime < end
                select r).ToList();

            return activationsList;
        }

        public void Remove(DetectorAggregation DetectorAggregation)
        {
            throw new NotImplementedException();
        }

        public List<DetectorAggregation> GetDetectorAggregationByApproachIdAndDateRange(int detectorId,
            DateTime startDate, DateTime endDate)
        {
            return _db.DetectorAggregations.Where(r => r.DetectorPrimaryId == detectorId
                                                       && r.BinStartTime >= startDate &&
                                                       r.BinStartTime < endDate).ToList();
        }

        public void Update(DetectorAggregation DetectorAggregation)
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