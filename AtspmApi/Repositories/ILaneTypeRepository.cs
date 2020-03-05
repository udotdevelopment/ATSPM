using System.Collections.Generic;
using AtspmApi.Models;

namespace AtspmApi.Repositories
{
    public interface ILaneTypeRepository
    {
        List<LaneType> GetAllLaneTypes();
        LaneType GetLaneTypeByLaneTypeID(int laneTypeID);
        void Update(LaneType laneType);
        void Add(LaneType laneType);
        void Remove(LaneType laneType);
    }
}