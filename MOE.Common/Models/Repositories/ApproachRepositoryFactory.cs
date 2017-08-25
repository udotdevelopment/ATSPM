using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOE.Common.Models.Repositories
{
    public class ApproachRepositoryFactory
    {
        private static IApproachRepository approachRepository;

        public static IApproachRepository Create()
        {
            if (approachRepository != null)
            {
                return approachRepository;
            }
            return new ApproachRepository();
        }

        public static void SetApproachRepository(IApproachRepository newRepository)
        {
            approachRepository = newRepository;
        }
    }
}