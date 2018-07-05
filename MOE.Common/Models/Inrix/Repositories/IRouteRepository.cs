using System.Collections.Generic;

namespace MOE.Common.Models.Inrix.Repositories
{
    public interface IRouteRepository
    {
        void Add(Route route);

        List<Route> GetRoutesByGroupID(int groupID);

        Route GetRouteByName(string name);
    }
}