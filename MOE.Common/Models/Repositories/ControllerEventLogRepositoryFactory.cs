using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOE.Common.Models.Repositories
{
    public class ControllerEventLogRepositoryFactory
    {
        private static IControllerEventLogRepository controllerEventLogRepository;

        public static IControllerEventLogRepository Create()
        {
            if (controllerEventLogRepository != null)
            {
                return controllerEventLogRepository;
            }
            return new ControllerEventLogRepository();
        }

        public static void SetDetectorCommentRepository(IControllerEventLogRepository newRepository)
        {
            controllerEventLogRepository = newRepository;
        }
    }
}
