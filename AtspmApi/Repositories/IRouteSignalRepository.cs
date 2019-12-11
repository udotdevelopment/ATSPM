using System.Collections.Generic;
using AtspmApi.Models;

namespace AtspmApi.Repositories
{
    public interface IRouteSignalsRepository
    {
        List<RouteSignal> GetAllRoutesDetails();
        List<RouteSignal> GetByRouteID(int routeID);
        void DeleteByRouteID(int routeID);
        void DeleteById(int id);
        void UpdateByRouteAndApproachID(int routeID, string signalId, int newOrderNumber);
        void Add(RouteSignal newRouteDetail);
        RouteSignal GetByRouteSignalId(int id);
        void MoveRouteSignalUp(int routeId, int routeSignalId);
        void MoveRouteSignalDown(int routeId, int routeSignalId);
    }
}