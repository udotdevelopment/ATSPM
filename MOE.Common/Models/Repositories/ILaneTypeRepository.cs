using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOE.Common.Models.Repositories
{
    public interface ILaneTypeRepository
    {
        List<Models.LaneType> GetAllLaneTypes();
        Models.LaneType GetLaneTypeByLaneTypeID(int laneTypeID);
        void Update(MOE.Common.Models.LaneType laneType);
        void Add(MOE.Common.Models.LaneType laneType);
        void Remove(MOE.Common.Models.LaneType laneType);
    }
}
