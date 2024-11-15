﻿using System.Collections.Generic;

namespace MOE.Common.Models.Repositories
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