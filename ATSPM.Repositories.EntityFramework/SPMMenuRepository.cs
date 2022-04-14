using ATSPM.IRepositories;
using ATSPM.Application.Models;
using System.Collections.Generic;
using System.Linq;

namespace ATSPM.Infrastructure.Repositories.EntityFramework
{
    public class SPMMenuRepository : ISPMMenuRepository

    {
        private readonly MOEContext db;

        public SPMMenuRepository(MOEContext db)
        {
            this.db = db;
        }

        public List<Menu> GetMenuItems()
        {
            var items = (from r in db.Menus
                         select r).ToList();
            return items;
        }
    }
}