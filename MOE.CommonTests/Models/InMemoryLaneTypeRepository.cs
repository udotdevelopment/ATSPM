using System.Collections.Generic;
using System.Linq;
using MOE.Common.Models;
using MOE.Common.Models.Repositories;

namespace MOE.CommonTests.Models
{
    public class InMemoryLaneTypeRepository : ILaneTypeRepository
    {
        private InMemoryMOEDatabase _db;

        public InMemoryLaneTypeRepository()
        {
            _db = new InMemoryMOEDatabase();

        }

        public InMemoryLaneTypeRepository(InMemoryMOEDatabase db)
        {
            _db = db;

        }

        public void Add(LaneType laneType)
        {
            throw new System.NotImplementedException();
        }

        public List<LaneType> GetAllLaneTypes()
        {
            
            List<LaneType> laneTypes = (from r in _db.LaneTypes
                select r).ToList();
            return laneTypes;
        }

        public LaneType GetLaneTypeByLaneTypeID(int laneTypeID)
        {
            var laneType = (from r in _db.LaneTypes
                where r.LaneTypeID == laneTypeID
                select r);
            return laneType.FirstOrDefault();
        }

        public LaneType GetLaneTypeByLaneDesc(string desc)
        {
            throw new System.NotImplementedException();
        }

        public void Remove(LaneType laneType)
        {
            throw new System.NotImplementedException();
        }

        public void Update(LaneType laneType)
        {
            throw new System.NotImplementedException();
        }
    }
}