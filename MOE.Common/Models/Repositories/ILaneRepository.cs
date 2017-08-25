using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOE.Common.Models.Repositories
{
    public interface ILaneRepository
    {
        List<Models.Lane> GetAllLanes();
        Models.Lane GetLaneByLaneID(int laneID);
        void Update(MOE.Common.Models.Lane lane);
        Lane Add(MOE.Common.Models.Lane lane);
        void Remove(MOE.Common.Models.Lane lane);
        void Remove(int id);

    }
}
