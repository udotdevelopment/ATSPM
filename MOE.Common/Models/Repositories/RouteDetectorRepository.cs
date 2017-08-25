using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOE.Common.Models.Repositories
{
    public class RouteDetectorRepository : IRouteDetectorsRepository
    {
        MOE.Common.Models.SPM db = new SPM();

        public List<Models.Route_Detectors> SelectByRegion(int regionID)
        {
            var routeDetectors = (from r in db.Routes
                                  join rd in db.Route_Detectors on r.RouteID equals rd.RouteID
                                  where r.Region == regionID
                                  select rd).ToList();
            return routeDetectors;
        }
    }
}
