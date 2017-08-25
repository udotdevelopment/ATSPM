using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOE.Common.Models.Inrix.Repositories
{
    public class GroupRepository : IGroupRepository

    {
        private Models.Inrix.Inrix db = new Inrix();

        
        public List<Models.Inrix.Group> GetAll()
        {
            List<Models.Inrix.Group> grlist = (from r in db.Groups
                                              select r).ToList();

            return grlist;
        }

        public Models.Inrix.Group SelectGroupByName(string name)
        {
            var g = (from r in db.Groups
                                    where r.Group_Name == name
                                    select r).FirstOrDefault();

            return g;
        }

        public void Remove(Models.Inrix.Group group)
        {
            db.Groups.Remove(group);
            db.SaveChanges();
        }

        public Models.Inrix.Group SelectByID(int groupID)
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

        public void Add(Models.Inrix.Group group)
        {
            db.Groups.Add(group);
            db.SaveChanges();
        }

        public void Update(Models.Inrix.Group Group)
        {
            Models.Inrix.Group g = (from r in db.Groups
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
