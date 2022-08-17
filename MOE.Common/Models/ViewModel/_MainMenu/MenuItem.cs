using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MOE.Common.Models.ViewModel._MainMenu
{
    public class MenuItem
    {
        public MenuItem(Menu menuItem)
        {
            IsAdmin = HttpContext.Current.User.IsInRole("Admin");
            IsTechnician = HttpContext.Current.User.IsInRole("Technician");
            IsData = HttpContext.Current.User.IsInRole("Data");
            IsConfiguration = HttpContext.Current.User.IsInRole("Configuration");
            IsRestrictedConfiguration = HttpContext.Current.User.IsInRole("Restricted Configuration");
            SubMenuItems = new List<MenuItem>();
            MenuObject = menuItem;

            var db = new SPM();

            var _children = (from m in db.Menus
                where m.ParentId == menuItem.MenuId
                orderby m.DisplayOrder
                select m).ToList();

            foreach (var m in _children)
                SubMenuItems.Add(new MenuItem(m));

            ExternalLinks = new List<ExternalLink>();
            if (MenuObject.MenuName == "Links")
                ExternalLinks = db.ExternalLinks.ToList();
        }

        public Menu MenuObject { get; set; }
        public List<MenuItem> SubMenuItems { get; set; }
        public List<ExternalLink> ExternalLinks { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsTechnician { get; set; }
        public bool IsData { get; set; }
        public bool IsConfiguration { get; set; }
        public bool IsRestrictedConfiguration { get; set; }
    }
}