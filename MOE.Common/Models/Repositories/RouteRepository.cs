using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOE.Common.Models.Repositories
{
    public class RouteRepository : IRouteRepository
    {
        MOE.Common.Models.SPM db = new SPM();

        public List<Models.Route> SelectAllRoutes()
        {
            var routes = (from r in db.Routes
                         select r).ToList();
            return routes;
        }

        public Models.Route SelectByRouteID(int routeID)
        {
            var route = (from r in db.Routes
                             where r.RouteID == routeID
                             select r).FirstOrDefault();
            return route;
        }

        public void InsertRoute(Models.Route route)
        {
            db.Routes.Add(route);
            db.SaveChanges();
        }
    }
}
