using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOE.Common.Models.Repositories
{
    public interface IRouteRepository
    {
        List<Models.Route> GetAllRoutes();
        Models.Route GetRouteByID(int routeID);
        Models.Route GetRouteByName(string routeName);
        void DeleteByID(int routeID);
        void Update(Route route);
        void Add(Models.Route newRoute);
        Route GetRouteByIDAndDate(int routeId, DateTime startDate);
    }
}
