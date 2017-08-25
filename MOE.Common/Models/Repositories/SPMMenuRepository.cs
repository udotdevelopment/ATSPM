using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOE.Common.Models.Repositories
{
    public class SPMMenuRepository : ISPMMenuRepository

    {
        MOE.Common.Models.SPM db = new SPM ();
        public List<MOE.Common.Models.Menu> GetMenuItems()
        {
             List<MOE.Common.Models.Menu> items = (from r in db.Menus
                                                  select r).ToList();
                 return  items;

        }
    }
}
