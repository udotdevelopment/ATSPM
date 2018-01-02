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
            throw new System.NotImplementedException();
        }

        public RoutePhaseDirection GetByID(int routeID)
        {
            throw new System.NotImplementedException();
        }

        public void Update(RoutePhaseDirection routePhaseDirection)
        {
            throw new System.NotImplementedException();
        }
    }
}