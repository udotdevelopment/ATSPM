using System;
using System.Collections.Generic;
using MOE.Common.Models;
using MOE.Common.Models.Repositories;

namespace MOE.CommonTests.Models
{
    public class InMemoryRouteRepository : IRouteRepository
    {
       

        public InMemoryMOEDatabase _db;
        
        public InMemoryRouteRepository()
        {
            _db = new InMemoryMOEDatabase();
        }

        public InMemoryRouteRepository(InMemoryMOEDatabase context)
        {
            _db = context;
        }


        public void Add(Route newRoute)
        {
            _db.Routes.Add(newRoute);
        }

        public void DeleteByID(int routeID)
        {
            Route route = _db.Routes.Find(r => r.Id == routeID);

            _db.Routes.Remove(route);
        }

        public List<Route> GetAllRoutes()
        {
            List<Route> routes = _db.Routes;
            return routes;
        }

        public Route GetRouteByID(int routeID)
        {
            Route route = _db.Routes.Find(r => r.Id == routeID);

            return route;
        }

        public Route GetRouteByIDAndDate(int routeId, DateTime startDate)
        {
            throw new NotImplementedException();
        }

        public Route GetRouteByName(string routeName)
        {
            throw new NotImplementedException();
        }



        public void Update(Route route)
        {
            var checkRoute = _db.Routes.Find(r => r.Id == route.Id);
            if (checkRoute != null)
            {
                checkRoute.RouteName = route.RouteName;
                if (route.RouteSignals != null)
                {
                    checkRoute.RouteSignals = route.RouteSignals;
                }
            }
        }
    }
}