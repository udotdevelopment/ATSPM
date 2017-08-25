using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOE.Common.Models.Repositories
{
    public class RouteDetectorRepositoryFactory
    {
        private static IRouteDetectorsRepository routeDetectorRepository;

        public static IRouteDetectorsRepository Create()
        {
            if (routeDetectorRepository != null)
            {
                return routeDetectorRepository;
            }
            return new RouteDetectorRepository();
        }

        public static void SetRouteDetectorRepository(IRouteDetectorsRepository newRepository)
        {
            routeDetectorRepository = newRepository;
        }
    }
}
