using System.Collections.Generic;

namespace MOE.Common.Models.Repositories
{
    public interface ISPMMenuRepository
    {
        List<Menu> GetMenuItems();
    }
}