using System.Linq;

namespace MOE.Common.Models.Inrix.Repositories
{
    internal class GroupMemberRepository : IGroupMemberRepository
    {
        private readonly Inrix db = new Inrix();

        public void Add(Group_Members groupMember)
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