using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOE.Common.Models.Repositories
{
    public class RegionsRepositoryFactory
    {
        private static IRegionsRepository regionRepository;

        public static IRegionsRepository Create()
        {
            if (regionRepository != null)
            {
                return regionRepository;
            }
            return new RegionsRepository();
        }

        public static void SetArchivedMetricsRepository(IRegionsRepository newRepository)
        {
            regionRepository = newRepository;
        }
    }
}
