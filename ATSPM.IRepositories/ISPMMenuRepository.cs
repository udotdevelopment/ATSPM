using ATSPM.Application.Models;
using System.Collections.Generic;

namespace ATSPM.IRepositories
{
    public interface ISPMMenuRepository
    {
        List<Menu> GetMenuItems();
    }
}