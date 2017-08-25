using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using System.Web.Security;
using System.Web;

namespace MOE.Common.Models.ViewModel._MainMenu
{
    public class MenuItem
    {
        public MOE.Common.Models.Menu MenuObject { get; set; }
        public List<MenuItem> SubMenuItems { get; set; }
        public List<ExternalLink> ExternalLinks { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsTechnician { get; set; }

        public MenuItem(MOE.Common.Models.Menu menuItem)
        {
            IsAdmin = HttpContext.Current.User.IsInRole("Admin");
            IsTechnician = HttpContext.Current.User.IsInRole("Technician");
            SubMenuItems = new List<MenuItem>();
            MenuObject = menuItem;

            MOE.Common.Models.SPM db = new SPM();

            List<MOE.Common.Models.Menu> _children = (from m in db.Menus
                           where m.ParentId == menuItem.MenuId
                           orderby m.DisplayOrder
                           select m).ToList();

            foreach(MOE.Common.Models.Menu m in _children)
            {
                SubMenuItems.Add(new MenuItem(m));
            }

            ExternalLinks = new List<ExternalLink>();
            if (MenuObject.MenuName == "Links")
            {
                ExternalLinks = db.ExternalLinks.ToList();
            }
        }
    }
}
