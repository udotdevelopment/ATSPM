using MOE.Common.Models;
using MOE.Common.Models.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MOE.CommonTests.Models
{
    public class InMemoryMetricTypeRepository : IMetricTypeRepository
    {
        private InMemoryMOEDatabase _db;

        public InMemoryMetricTypeRepository()
        {
            _db = new InMemoryMOEDatabase();
        }

        InMemoryMetricTypeRepository(InMemoryMOEDatabase dB)
        {
            _db = dB;
        }

        public List<MetricType> GetAllMetrics()
        {
            throw new NotImplementedException();
        }

        public List<MetricType> GetAllToDisplayMetrics()
        {
            List<Common.Models.MetricType> results = (from r in _db.MetricTypes
                where r.ShowOnWebsite == true
                select r).ToList();
            return results;
        }

        public List<MetricType> GetBasicMetrics()
        {
            throw new NotImplementedException();
        }

        public MetricType GetMetricsByID(int metricID)
        {
            throw new NotImplementedException();
        }

        public List<MetricType> GetMetricsByIDs(List<int> metricIDs)
        {
            throw new NotImplementedException();
        }

        public List<MetricType> GetMetricTypesByMetricComment(MetricComment metricComment)
        {
            throw new NotImplementedException();
        }
    }

}