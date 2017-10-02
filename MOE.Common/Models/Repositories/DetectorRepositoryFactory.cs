using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOE.Common.Models.Repositories
{
    public class DetectorRepositoryFactory
    {
       private static IDetectorRepository detectorRepository;

        public static IDetectorRepository Create()
        {
            if (detectorRepository != null)
            {
                return detectorRepository;
            }
            return new DetectorRepository();
        }
        public static IDetectorRepository Create(SPM context)
        {
            if (detectorRepository != null)
            {
                return detectorRepository;
            }
            return new DetectorRepository(context);
        }

        public static void SetArchivedMetricsRepository(IDetectorRepository newRepository)
        {
            detectorRepository = newRepository;
        }
    }
    
}
