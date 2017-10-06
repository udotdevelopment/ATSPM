using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MOE.Common.Models;
using MOE.Common.Models.Repositories;

namespace MOE.CommonTests.Models
{
    class InMemoryApproachRouteDetailsRepository : IApproachRouteDetailRepository
    {


        public void Add(RouteSignal newRouteDetail)
        {
            throw new NotImplementedException();
        }

        public void DeleteByRouteID(int routeID)
        {
            throw new NotImplementedException();
        }

        public List<RouteSignal> GetAllRoutesDetails()
        {
            throw new NotImplementedException();
        }

        public List<RouteSignal> GetByRouteID(int routeID)
        {
            throw new NotImplementedException();
        }

        public void UpdateByRouteAndApproachID(int routeID, string signalId, int newOrderNumber)
        {
            throw new NotImplementedException();
        }
    }
}
