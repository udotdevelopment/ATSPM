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
            throw new NotImplementedException();
        }

        public void DeleteByID(int routeID)
        {
            throw new NotImplementedException();
        }

        public List<Route> GetAllRoutes()
        {
            throw new NotImplementedException();
        }

        public Route GetRouteByID(int routeID)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }
    }
}