using ATSPM.Application.Models;
using System;
using System.Collections.Generic;

namespace MOE.Common.Models.Repositories
{
    public interface IRouteRepository
    {
        List<Route> GetAllRoutes();
        Route GetRouteByID(int routeID);
        Route GetRouteByName(string routeName);
        void DeleteByID(int routeID);
        void Update(Route route);
        void Add(Route newRoute);
        Route GetRouteByIDAndDate(int routeId, DateTime startDate);
    }
}