using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOE.Common.Models.Repositories
{
    public class LaneRepositoryFactory
    {
        private static ILaneRepository laneRepository;

        public static ILaneRepository Create()
        {
            if (laneRepository != null)
            {
                return laneRepository;
            }
            return new LaneRepository();
        }

        public static void SetLaneRepository(ILaneRepository newRepository)
        {
            laneRepository = newRepository;
        }
    }
}
