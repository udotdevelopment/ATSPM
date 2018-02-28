using System;
using System.Collections.Generic;
using System.Linq;

namespace MOE.Common.Models.Repositories
{
    public class DetectorEventCountAggregationRepository : IDetectorEventCountAggregationRepository
    {
        private readonly SPM _db;

        public DetectorEventCountAggregationRepository()
        {
            _db = new SPM();
        }

        public DetectorEventCountAggregationRepository(SPM context)
        {
            _db = context;
        }

        public int GetDetectorEventCountSumAggregationByDetectorIdAndDateRange(int detectorId, DateTime start,
            DateTime end)
        {
            var count = 0;
                if (_db.DetectorEventCountAggregations.Any(r => r.Id == detectorId 
                                                             && r.BinStartTime >= start && r.BinStartTime <= end))
                {
                    count = _db.DetectorEventCountAggregations.Where(r => r.Id == detectorId 
                                                                        && r.BinStartTime >= start && r.BinStartTime <= end)
                        .Sum(r => r.EventCount);
                }
               
            return count;
        }
        

        public List<DetectorEventCountAggregation> GetDetectorEventCountAggregationByDetectorIdAndDateRange(int detectorId, DateTime start,
            DateTime end)
        {
            if (_db.DetectorEventCountAggregations.Any(r => r.Id == detectorId 
                                                             && r.BinStartTime >= start && r.BinStartTime <= end))
                {
                    return _db.DetectorEventCountAggregations.Where(r => r.Id == detectorId 
                                                                      && r.BinStartTime >= start &&
                                                                      r.BinStartTime <= end).ToList();
                }
            return new List<DetectorEventCountAggregation>();
        }
    }

    
}