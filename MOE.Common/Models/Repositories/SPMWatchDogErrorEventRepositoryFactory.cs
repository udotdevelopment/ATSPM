using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOE.Common.Models.Repositories
{
    public class SPMWatchDogErrorEventRepositoryFactory
    {
        private static ISPMWatchDogErrorEventRepository SPMWatchDogErrorEventRepository;

        public static ISPMWatchDogErrorEventRepository Create()
        {
            if (SPMWatchDogErrorEventRepository != null)
            {
                return SPMWatchDogErrorEventRepository;
            }
            return new SPMWatchDogErrorEventRepository();
        }

        public static void SetSPMWatchDogErrorEventRepository(ISPMWatchDogErrorEventRepository newRepository)
        {
            SPMWatchDogErrorEventRepository = newRepository;
        }
    }
}