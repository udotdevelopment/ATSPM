using System.Collections.Generic;
using System.Linq;
using AtspmApi.Repositories;

namespace AtspmApi.Models
{
    public class MetricTypeRepository : IMetricTypeRepository
    {
        private readonly Models.AtspmApi db = new Models.AtspmApi();


        public List<MetricType> GetAllMetrics()
        {
            var results = (from r in db.MetricTypes
                           select r).ToList();
            return results;
        }

        //public List<MetricType> GetMetricTypesByMetricComment(MetricComment metricComment)
        //{
        //    var metricTypes = (from r in db.MetricTypes
        //        where metricComment.MetricTypeIDs.Contains(r.MetricID)
        //        select r).ToList();

        //    return metricTypes;
        //}

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
            var dt = (from d in db.DetectionTypes where d.DetectionTypeID == 1 select d).FirstOrDefault();
            return dt.MetricTypes.ToList();
        }

        public List<MetricType> GetMetricsByIDs(List<int> metricIDs)
        {
            return db.MetricTypes.Where(x => metricIDs.Contains(x.MetricID)).ToList();
        }

        public MetricType GetMetricsByID(int metricID)
        {
            var metricType = db.MetricTypes.Find(metricID);
            return metricType;
        }
    }
}