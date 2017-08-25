using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOE.Common.Models.Repositories
{
    public interface IApproachRepository
    {
         List<Models.Approach> GetAllApproaches();
         Models.Approach GetApproachByApproachID(int approachID);
         void AddOrUpdate(MOE.Common.Models.Approach approach);
         //Approach Add(MOE.Common.Models.Approach approach);
         void Remove(MOE.Common.Models.Approach approach);
         void Remove(int approachID);
    }
}
