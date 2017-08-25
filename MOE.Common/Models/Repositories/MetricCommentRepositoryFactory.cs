
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOE.Common.Models.Repositories
{
    public class MetricCommentRepositoryFactory
    {
        private static IMetricCommentRepository metricCommentRepository;

        public static IMetricCommentRepository Create()
        {
            if (metricCommentRepository != null)
            {
                return metricCommentRepository;
            }
            return new MetricCommentRepository();
        }

        public static void SetMetricCommentRepository(IMetricCommentRepository newRepository)
        {
            metricCommentRepository = newRepository;
        }
    }
}
