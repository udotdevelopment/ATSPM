using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOE.Common.Models.Repositories
{
    public interface IRouteRepository
    {
        List<Models.Route> SelectAllRoutes();

        Models.Route SelectByRouteID(int routeID);

        void InsertRoute(Models.Route route);
    }
}
