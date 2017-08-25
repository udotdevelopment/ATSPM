using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOE.Common.Models.Repositories
{
    public class MetricTypeRepository : IMetricTypeRepository
    {
        Models.SPM db = new SPM();


        public List<Models.MetricType> GetAllMetrics()
        {
            List<Models.MetricType> results = (from r in db.MetricTypes
                                           select r).ToList();
            return results;
        }

        public List<Models.MetricType> GetMetricTypesByMetricComment(Models.MetricComment metricComment)
        {
            var metricTypes = (from r in db.MetricTypes
                               where metricComment.MetricTypeIDs.Contains(r.MetricID)
                               select r).ToList();

            return metricTypes;
        }

        public List<MetricType> GetAllToDisplayMetrics()
        {
            List<Models.MetricType> results = (from r in db.MetricTypes
                                               where r.ShowOnWebsite == true
                                               select r).ToList();
            return results;
        }

        public List<MetricType> GetBasicMetrics()
        {
            Models.DetectionType dt = (from d in db.DetectionTypes where d.DetectionTypeID == 1 select d).FirstOrDefault();


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
