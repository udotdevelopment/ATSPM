using ATSPM.Application.Models;
using System.Collections.Generic;

namespace ATSPM.IRepositories
{
    public interface IMenuRepository
    {
        List<Menu> GetAll(string Application);
        Menu GetMenuItembyID(int id);
        void Add(Menu menuItem);
        void Remove(int id);
        void Update(Menu menuItem);
        List<Menu> GetTopLevelMenuItems(string Application);
    }
}