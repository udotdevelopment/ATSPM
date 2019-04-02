using System.Collections.Generic;
using System.Linq;

namespace MOE.Common.Models.Repositories
{
    public class SPMMenuRepository : ISPMMenuRepository

    {
        private readonly SPM db = new SPM();

        public List<Menu> GetMenuItems()
        {
            var items = (from r in db.Menus
                select r).ToList();
            return items;
        }
    }
}