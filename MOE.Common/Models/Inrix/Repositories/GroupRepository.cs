using System.Collections.Generic;
using System.Linq;

namespace MOE.Common.Models.Inrix.Repositories
{
    public class GroupRepository : IGroupRepository

    {
        private readonly Inrix db = new Inrix();


        public List<Group> GetAll()
        {
            var grlist = (from r in db.Groups
                select r).ToList();

            return grlist;
        }

        public Group SelectGroupByName(string name)
        {
            var g = (from r in db.Groups
                where r.Group_Name == name
                select r).FirstOrDefault();

            return g;
        }

        public void Remove(Group group)
        {
            db.Groups.Remove(group);
            db.SaveChanges();
        }

        public Group SelectByID(int groupID)
        {
            var g = (from r in db.Groups
                where r.Group_ID == groupID
                select r).FirstOrDefault();

            return g;
        }

        public void RemoveByID(int groupID)
        {
            var g = SelectByID(groupID);

            db.Groups.Remove(g);
            db.SaveChanges();
        }

        public void Add(Group group)
        {
            db.Groups.Add(group);
            db.SaveChanges();
        }

        public void Update(Group Group)
        {
            var g = (from r in db.Groups
                where r.Group_ID == Group.Group_ID
                select r).FirstOrDefault();
            if (g != null)
            {
                db.Entry(g).CurrentValues.SetValues(Group);
                db.SaveChanges();
            }
        }
    }
}