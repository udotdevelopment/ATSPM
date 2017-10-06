using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MOE.Common.Models;
using MOE.Common.Models.Repositories;

namespace MOE.CommonTests.Models
{
    class InMemoryApproachRouteRepository : IApproachRouteRepository
    {


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

        public Route GetRouteByName(string routeName)
        {
            throw new NotImplementedException();
        }

        public void UpdateByID(int routeID, string newDescription)
        {
            throw new NotImplementedException();
        }
    }
}
