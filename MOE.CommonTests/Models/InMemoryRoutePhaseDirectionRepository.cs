using System.Collections.Generic;
using MOE.Common.Models;
using MOE.Common.Models.Repositories;

namespace MOE.CommonTests.Models
{
    public class InMemoryRoutePhaseDirectionRepository: IRoutePhaseDirectionRepository
    {
        public InMemoryMOEDatabase _db;

        public InMemoryRoutePhaseDirectionRepository()
        {
            _db = new InMemoryMOEDatabase();
        }

        public InMemoryRoutePhaseDirectionRepository(InMemoryMOEDatabase context)
        {
            _db = context;
        }

        public void Add(RoutePhaseDirection newRRoutePhaseDirection)
        {
            throw new System.NotImplementedException();
        }

        public void DeleteByID(int id)
        {
            throw new System.NotImplementedException();
        }

        public List<RoutePhaseDirection> GetAll()
        {
            List<RoutePhaseDirection> list = _db.RoutePhaseDirection;
            return list;
        }

        public RoutePhaseDirection GetByID(int routeID)
        {
            RoutePhaseDirection pd = _db.RoutePhaseDirection.Find(r => r.Id == routeID);
            return pd;
        }

        public void Update(RoutePhaseDirection routePhaseDirection)
        {
            throw new System.NotImplementedException();
        }
    }
}