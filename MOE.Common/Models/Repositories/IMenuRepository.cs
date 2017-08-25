using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOE.Common.Models.Repositories
{
    public interface IMenuRepository
    {
        List<Models.Menu> GetAll(string Application);
        Models.Menu GetMenuItembyID(int id);
        void Add(Models.Menu menuItem);
        void Remove(int id);
        void Update(Models.Menu menuItem);
        List<Menu> GetTopLevelMenuItems(string Application);
    }
}
