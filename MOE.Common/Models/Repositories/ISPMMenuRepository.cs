using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOE.Common.Models.Repositories
{
    public interface ISPMMenuRepository
    {
        List<MOE.Common.Models.Menu> GetMenuItems();
    }
}
