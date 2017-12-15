using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOE.Common.Models.Repositories
{
    public interface IRouteSignalsRepository
    {
        List<Models.RouteSignal> GetAllRoutesDetails();
        List<Models.RouteSignal> GetByRouteID(int routeID);
        void DeleteByRouteID(int routeID);
        void DeleteById(int id);
        void UpdateByRouteAndApproachID(int routeID, string signalId, int newOrderNumber);
        void Add(Models.RouteSignal newRouteDetail);
        Models.RouteSignal GetByRouteSignalId(int id);
    }

}