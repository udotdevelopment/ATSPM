using System.Collections.Generic;

namespace MOE.Common.Models.Repositories
{
    public interface ISPMWatchdogExclusionsRepository
    {
        List<SPMWatchdogExclusions> GetSPMWatchdogExclusions();
        void Add(SPMWatchdogExclusions exclusion);
        void Delete(int exclusion);
    }
}
