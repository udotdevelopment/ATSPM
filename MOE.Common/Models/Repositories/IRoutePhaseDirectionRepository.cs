using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOE.Common.Models.Repositories
{
    public interface IRoutePhaseDirectionRepository
    {
        List<Models.RoutePhaseDirection> GetAll();
        Models.RoutePhaseDirection GetByID(int routeID);
        void DeleteByID(int id);
        void Update(RoutePhaseDirection routePhaseDirection);
        void Add(Models.RoutePhaseDirection newRRoutePhaseDirection);
    }
}
