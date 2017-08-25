using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOE.Common.Models.Repositories
{
    public class ArchivedMetricsRepositoryFactory
    {
        private static IArchivedMetricsRepository archivedMetricsRepository;

        public  static IArchivedMetricsRepository Create()
        {
            if(archivedMetricsRepository != null)
            {
                return archivedMetricsRepository;
            }
            return new ArchivedMetricsRepository();
        }

        public static void SetArchivedMetricsRepository(IArchivedMetricsRepository newRepository)
        {
            archivedMetricsRepository = newRepository;
        }
    }
}
