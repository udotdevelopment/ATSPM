using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOE.Common.Models.Inrix.Repositories
{
    public class RouteRepository : IRouteRepository
    {
        Models.Inrix.Inrix db = new Inrix();

        public List<Models.Inrix.Route> GetRoutesByGroupID(int groupID)
        {
            
                
                List<Models.Inrix.Route> routes  = (from r in db.Routes 
                            join members in db.Group_Members on r.Route_ID 
                            equals members.Route_ID
                            where members.Group_ID == groupID
                            select r).ToList();

                return routes;
        }

    public void Add(Models.Inrix.Route route)
    {
        db.Routes.Add(route);
        db.SaveChanges();
    }

    public Models.Inrix.Route GetRouteByName(string name)
    {
        Models.Inrix.Route route = (from r in db.Routes
                                    where r.Route_Name == name
                                    select r).FirstOrDefault();

        return route;
    }



    }
}
