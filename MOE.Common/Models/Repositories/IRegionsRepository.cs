using System.Collections.Generic;

namespace MOE.Common.Models.Repositories
{
    public interface IRegionsRepository
    {
        List<Region> GetAllRegions();
    }
}