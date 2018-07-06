using System.Collections.Generic;
using System.Linq;

namespace MOE.Common.Models.Inrix.Repositories
{
    public class RouteRepository : IRouteRepository
    {
        private readonly Inrix db = new Inrix();

        public List<Route> GetRoutesByGroupID(int groupID)
        {
            var routes = (from r in db.Routes
                join members in db.Group_Members on r.Route_ID
                    equals members.Route_ID
                where members.Group_ID == groupID
                select r).ToList();

            return routes;
        }

        public void Add(Route route)
        {
            db.Routes.Add(route);
            db.SaveChanges();
        }

        public Route GetRouteByName(string name)
        {
            var route = (from r in db.Routes
                where r.Route_Name == name
                select r).FirstOrDefault();

            return route;
        }
    }
}