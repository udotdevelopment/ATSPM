using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOE.Common.Models.Repositories
{
    public class SignalsRepositoryFactory
    {
        private static ISignalsRepository signalsRepository;

        public static ISignalsRepository Create()
        {
            if (signalsRepository != null)
            {
                return signalsRepository;
            }
            return new SignalsRepository();
        }

        public static void SetSignalsRepository(ISignalsRepository newRepository)
        {
            signalsRepository = newRepository;
        }
    }
}
