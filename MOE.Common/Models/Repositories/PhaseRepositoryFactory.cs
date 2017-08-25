using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOE.Common.Models.Repositories
{
    public class PhaseRepositoryFactory 
    {
        private static IPhaseRepository phaseRepository;

        public static IPhaseRepository CreatePhaseRepository()
        {
            if (phaseRepository != null)
            {
                return phaseRepository;
            }
            return new PhaseRepository();
        }

        public static void SetPhaseRepository(IPhaseRepository newRepository)
        {
            phaseRepository = newRepository;
        }
    }
}
