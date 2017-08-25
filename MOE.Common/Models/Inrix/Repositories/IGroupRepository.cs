using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOE.Common.Models.Inrix.Repositories
{
    public interface IGroupRepository
    {
        List<Models.Inrix.Group> GetAll();
        void Add(Models.Inrix.Group group);
        void Update(Models.Inrix.Group group);
        void Remove(Models.Inrix.Group group);
        void RemoveByID(int groupID);

        Models.Inrix.Group SelectByID(int groupID);

        Models.Inrix.Group SelectGroupByName(string name);
    }
}
