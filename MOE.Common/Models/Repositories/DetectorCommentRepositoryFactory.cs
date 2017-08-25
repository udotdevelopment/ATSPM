using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOE.Common.Models.Repositories
{
    public class DetectorCommentRepositoryFactory
    {
        private static IDetectorCommentRepository detectorCommentRepository;

        public static IDetectorCommentRepository Create()
        {
            if (detectorCommentRepository != null)
            {
                return detectorCommentRepository;
            }
            return new DetectorCommentRepository();
        }

        public static void SetDetectorCommentRepository(IDetectorCommentRepository newRepository)
        {
            detectorCommentRepository = newRepository;
        }
    }
}
