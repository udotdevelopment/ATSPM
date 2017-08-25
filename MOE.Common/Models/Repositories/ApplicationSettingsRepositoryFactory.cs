using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOE.Common.Models.Repositories
{
    public class ApplicationSettingsRepositoryFactory
    {
        private static IApplicationSettingsRepository genericRepository;

        public static IApplicationSettingsRepository Create()
        {
            if (genericRepository != null)
            {
                return genericRepository;
            }
            return new ApplicationSettingsRepository();
        }

        public static void SetMetricsRepository(IApplicationSettingsRepository newRepository)
        {
            genericRepository = newRepository;
        }
    }
}
