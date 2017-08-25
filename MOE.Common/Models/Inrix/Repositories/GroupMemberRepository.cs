using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOE.Common.Models.Inrix.Repositories
{
    class GroupMemberRepository : IGroupMemberRepository
    {

        Models.Inrix.Inrix db = new Inrix();

        public void Add(Models.Inrix.Group_Members groupMember)
        {
            db.Group_Members.Add(groupMember);
            db.SaveChanges();
        }

        public void DeleteByGroupID(int GroupID)
        {
            var ToDelete = from r in db.Group_Members
                           where r.Group_ID == GroupID
                           select r;

            db.Group_Members.RemoveRange(ToDelete);
            db.SaveChanges();
        }

    }
}
