using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOE.Common.Models.Repositories
{
    public class ApproachRouteRepositoryFactory
    {
        private static IApproachRouteRepository approachRouteRepository;

        public static IApproachRouteRepository Create()
        {
            if (approachRouteRepository != null)
            {
                return approachRouteRepository;
            }
            return new ApproachRouteRepository();
        }

        public static void SetApproachRouteRepository(IApproachRouteRepository newRepository)
        {
            approachRouteRepository = newRepository;
        }
    }
    }

