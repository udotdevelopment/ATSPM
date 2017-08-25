using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOE.Common.Models.Repositories
{
    public class DetectionHardwareRepositoryFactory
    {
        private static IDetectionHardwareRepository detectionHardwareRepository;

        public static IDetectionHardwareRepository Create()
        {
            if (detectionHardwareRepository != null)
            {
                return detectionHardwareRepository;
            }
            return new DetectionHardwareRepository();
        }

        public static void SetDetectionHardwareRepository(IDetectionHardwareRepository newRepository)
        {
            detectionHardwareRepository = newRepository;
        }
    }
}
