using ATSPM.Application.Models;
using System.Collections.Generic;

namespace ATSPM.IRepositories
{
    public interface IRegionsRepository
    {
        List<Region> GetAllRegions();
    }
}