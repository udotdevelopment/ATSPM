using System.Collections.Generic;

namespace MOE.Common.Models.Inrix.Repositories
{
    public interface IGroupRepository
    {
        List<Group> GetAll();
        void Add(Group group);
        void Update(Group group);
        void Remove(Group group);
        void RemoveByID(int groupID);

        Group SelectByID(int groupID);

        Group SelectGroupByName(string name);
    }
}