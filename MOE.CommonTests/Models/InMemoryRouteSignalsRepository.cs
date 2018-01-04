
using System.Collections.Generic;
using MOE.Common.Models;
using MOE.Common.Models.Repositories;

namespace MOE.CommonTests.Models
{
    public class InMemoryRouteSignalsRepository : IRouteSignalsRepository
    {
        public InMemoryMOEDatabase _db;

        public InMemoryRouteSignalsRepository()
        {
            _db = new InMemoryMOEDatabase();
        }

        public InMemoryRouteSignalsRepository(InMemoryMOEDatabase context)
        {
            _db = context;
        }

        public void Add(RouteSignal newRouteDetail)
        {
            throw new System.NotImplementedException();
        }

        public void AddRouteMember()
        {
            throw new System.NotImplementedException();
        }

        public void DeleteById(int id)
        {
            throw new System.NotImplementedException();
        }

        public void DeleteByRouteID(int routeID)
        {
            throw new System.NotImplementedException();
        }

        public List<RouteSignal> GetAllRoutesDetails()
        {
            throw new System.NotImplementedException();
        }

        public List<RouteSignal> GetByRouteID(int routeID)
        {
            throw new System.NotImplementedException();
        }

        public RouteSignal GetByRouteSignalId(int id)
        {
            throw new System.NotImplementedException();
        }

        public void MoveRouteSignalDown(int routeId, int routeSignalId)
        {
            throw new System.NotImplementedException();
        }

        public void MoveRouteSignalUp(int routeId, int routeSignalId)
        {
            throw new System.NotImplementedException();
        }

        public void RemoveSegmentFromRoute(int segmentID)
        {
            throw new System.NotImplementedException();
        }

        public void UpdateByRouteAndApproachID(int routeID, string signalId, int newOrderNumber)
        {
            throw new System.NotImplementedException();
        }
    }
}