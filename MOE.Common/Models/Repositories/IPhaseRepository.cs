using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOE.Common.Models.Repositories
{
    public interface IPhaseRepository
    {
        List<Models.Phase> GetAllPhases();
        Models.Phase GetPhaseByPhaseID(int phaseID);
        void Update(MOE.Common.Models.Phase phase);
        void Add(MOE.Common.Models.Phase phase);
        void Remove(MOE.Common.Models.Phase phase);
        
    }
}
