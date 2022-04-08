using ATSPM.Application.Models;
using System.Collections.Generic;

namespace ATSPM.IRepositories
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