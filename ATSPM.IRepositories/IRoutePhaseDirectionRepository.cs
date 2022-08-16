using ATSPM.Application.Models;
using System.Collections.Generic;

namespace ATSPM.IRepositories
{
    public interface IRoutePhaseDirectionRepository
    {
        List<RoutePhaseDirection> GetAll();
        RoutePhaseDirection GetByID(int routeID);
        void DeleteByID(int id);
        void Update(RoutePhaseDirection routePhaseDirection);
        void Add(RoutePhaseDirection newRRoutePhaseDirection);
    }
}