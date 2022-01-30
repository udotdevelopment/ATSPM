using ATSPM.Application.Models;
using System.Collections.Generic;

namespace ATSPM.IRepositories
{
    public interface IRouteSignalsRepository
    {
        List<RouteSignal> GetAllRoutesDetails();
        List<RouteSignal> GetByRouteID(int routeID);
        void DeleteByRouteID(int routeID);
        void DeleteById(int id);
        void UpdateByRouteAndApproachID(int routeID, string signalId, int newOrderNumber);
        void Add(RouteSignal newRouteDetail);
        RouteSignal GetByRouteSignalId(int id,
            IDetectionHardwareRepository detectionHardwareRepository,
            IDetectionTypeRepository detectionTypeRepository);
        void MoveRouteSignalUp(int routeId, int routeSignalId);
        void MoveRouteSignalDown(int routeId, int routeSignalId);
    }
}