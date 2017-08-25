using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOE.Common.Models.Repositories
{
    public class RouteRepositoryFactory
    {
        private static IRouteRepository routeRepository;

        public static IRouteRepository Create()
        {
            if (routeRepository != null)
            {
                return routeRepository;
            }
            return new RouteRepository();
        }

        public static void SetRouteRepository(IRouteRepository newRepository)
        {
            routeRepository = newRepository;
        }
    }
}
