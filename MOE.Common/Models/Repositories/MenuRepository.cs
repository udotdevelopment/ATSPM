using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOE.Common.Models.Repositories
{
    public class MenuRepository : IMenuRepository
    {
        Models.SPM db = new SPM();

        public List<Models.Menu> GetAll(string Application)
        {
            return db.Menus.ToList();
        }

        public Models.Menu GetMenuItembyID(int id)
        {
            return db.Menus.Where(m => m.MenuId == id).First();
        }

        public void Add(Models.Menu menuItem)
        {
            db.Menus.Add(menuItem);
            db.SaveChanges();
        }

        public void Remove(int id)
        {
            Menu menu = GetMenuItembyID(id);
            if (menu != null)
            {
                db.Menus.Remove(menu);
                db.SaveChanges();
            }
            else
            {
                throw new Exception("Menu Not Found");
            }
        }

        public void Update(Models.Menu menuItem)
        {
            Menu menuFromDatabase = GetMenuItembyID(menuItem.MenuId);
            if (menuFromDatabase != null)
            {
                db.Entry(menuFromDatabase).CurrentValues.SetValues(menuItem);
                db.SaveChanges();
            }
            else
            {
                throw new Exception("Menu Not Found");
            }
        }

        public List<Menu> GetTopLevelMenuItems(string Application)
        {
            List<Menu> menu = new List<Menu>();
            try
            {

                menu = (from m in db.Menus
                            where m.Application == Application &&
                            m.ParentId == 0
                            orderby m.DisplayOrder
                            select m).ToList();
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
            catch(Exception ex)
            {

            }

            return menu;
        }
    }
}
