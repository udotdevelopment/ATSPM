using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOE.Common.Models.Repositories
{
    public interface IApproachRouteDetailRepository
    {
        List<Models.ApproachRouteDetail> GetAllRoutesDetails();
        List<Models.ApproachRouteDetail> GetByRouteID(int routeID);
        void DeleteByRouteID(int routeID);
        void UpdateByRouteAndApproachID(int routeID, string signalId, int newOrderNumber);
        void Add(Models.ApproachRouteDetail newRouteDetail);
    }

}