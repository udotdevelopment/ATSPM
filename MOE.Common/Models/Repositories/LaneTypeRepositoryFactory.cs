using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOE.Common.Models.Repositories
{
   public class LaneTypeRepositoryFactory
   {
       private static ILaneTypeRepository laneTypeRepository;

       public static ILaneTypeRepository Create()
       {
           if (laneTypeRepository != null)
           {
               return laneTypeRepository;
           }
           return new LaneTypeRepository();
       }

       public static void SetLaneTypeRepository(ILaneTypeRepository newRepository)
       {
           laneTypeRepository = newRepository;
       }
   }
}
