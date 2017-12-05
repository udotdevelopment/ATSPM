using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOE.Common.Models.Repositories
{
    public class AggregationMetricTypeRepository : IAggregationMetricTypeRepository
    {
        Models.SPM db = new SPM();


        public List<Models.AggregationMetricType> GetAllMetrics()
        {
            List<Models.AggregationMetricType> results = (from r in db.AggregationMetricTypes
                                           select r).ToList();
            return results;
        }



        public List<AggregationMetricType> GetAllToDisplayMetrics()
        {
            List<Models.AggregationMetricType> results = (from r in db.AggregationMetricTypes
                                               where r.ShowOnWebsite == true
                                               select r).ToList();
            return results;
        }



        public List<AggregationMetricType> GetMetricsByIDs(List<int> metricIDs)
        {
            return db.AggregationMetricTypes.Where(x => metricIDs.Contains(x.MetricID)).ToList();            
        }

        public AggregationMetricType GetMetricsByID(int metricID)
        {
            var metricType = db.AggregationMetricTypes.Find(metricID);
            return metricType;
        }

        
    }
}
