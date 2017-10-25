using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOE.Common.Models.Repositories
{
    public class DetectorRepositoryFactory
    {
       private static IDetectorRepository graphDetectorRepository;

        public static IDetectorRepository Create()
        {
            if (graphDetectorRepository != null)
            {
                return graphDetectorRepository;
            }
            return new DetectorRepository();
        }

        public static void SetArchivedMetricsRepository(IDetectorRepository newRepository)
        {
            graphDetectorRepository = newRepository;
        }
    }
    
}
