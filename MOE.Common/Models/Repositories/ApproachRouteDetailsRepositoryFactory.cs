using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOE.Common.Models.Repositories
{
    
public class ApproachRouteDetailRepositoryFactory
    {
        private static IApproachRouteDetailRepository approachRouteDetailRepository;

        public static IApproachRouteDetailRepository Create()
        {
            if (approachRouteDetailRepository != null)
            {
                return approachRouteDetailRepository;
            }
            return new ApproachRouteDetailRepository();
        }

        public static void SetApproachRouteDetailRepository(IApproachRouteDetailRepository newRepository)
        {
            approachRouteDetailRepository = newRepository;
        }
    }
}