using System.Collections.Generic;
using AtspmApi.Models;

namespace AtspmApi.Repositories
{
    public interface IRegionsRepository
    {
        List<Region> GetAllRegions();
    }
}