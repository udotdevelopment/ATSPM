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


        public void Add(ApproachRouteDetail newRouteDetail)
        {
            throw new NotImplementedException();
        }

        public void DeleteByRouteID(int routeID)
        {
            throw new NotImplementedException();
        }

        public List<ApproachRouteDetail> GetAllRoutesDetails()
        {
            throw new NotImplementedException();
        }

        public List<ApproachRouteDetail> GetByRouteID(int routeID)
        {
            throw new NotImplementedException();
        }

        public void UpdateByRouteAndApproachID(int routeID, string signalId, int newOrderNumber)
        {
            throw new NotImplementedException();
        }
    }
}
