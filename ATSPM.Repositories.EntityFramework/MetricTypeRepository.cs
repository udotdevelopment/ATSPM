using ATSPM.IRepositories;
using ATSPM.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ATSPM.Infrastructure.Repositories.EntityFramework
{
    public class MetricTypeRepository : IMetricTypeRepository
    {
        private readonly MOEContext db;

        public MetricTypeRepository(MOEContext db)
        {
            this.db = db;
        }

        public List<MetricType> GetAllMetrics()
        {
            var results = (from r in db.MetricTypes
                           select r).ToList();
            return results;
        }

        public List<MetricType> GetMetricTypesByMetricComment(MetricComment metricComment)
        {

            throw new NotImplementedException();
            //var metricTypes = (from r in db.MetricTypes
            //                   where metricComment.MetricTypeIDs.Contains(r.MetricID)
            //                   select r).ToList();

            //return metricTypes;
        }

        public List<MetricType> GetAllToDisplayMetrics()
        {
            var results = (from r in db.MetricTypes
                           where r.ShowOnWebsite
                           select r).ToList();
            return results;
        }

        public List<MetricType> GetAllToAggregateMetrics()
        {
            var results = (from r in db.MetricTypes
                           where r.ShowOnAggregationSite
                           select r).ToList();
            return results;
        }

        public List<MetricType> GetBasicMetrics()
        {
            var dt = (from d in db.DetectionTypes where d.DetectionTypeId == 1 select d).FirstOrDefault();
            return dt.DetectionTypeMetricTypes.Select(m => m.MetricTypeMetric).ToList();
        }

        public List<MetricType> GetMetricsByIDs(List<int> metricIDs)
        {
            return db.MetricTypes.Where(x => metricIDs.Contains(x.MetricId)).ToList();
        }

        public MetricType GetMetricsByID(int metricID)
        {
            var metricType = db.MetricTypes.Find(metricID);
            return metricType;
        }
    }
}