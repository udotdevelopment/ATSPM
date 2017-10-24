using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOE.Common.Models.Repositories
{
    public class RoutePhaseDirectionRepositoryFactory
    {
        private static IRoutePhaseDirectionRepository approachRouteRepository;

        public static IRoutePhaseDirectionRepository Create()
        {
            if (approachRouteRepository != null)
            {
                return approachRouteRepository;
            }
            return new RoutePhaseDirectionRepository();
        }

        public static void SetApproachRouteRepository(IRoutePhaseDirectionRepository newRepository)
        {
            approachRouteRepository = newRepository;
        }
    }
    }

