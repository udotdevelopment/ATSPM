using System.Linq;

namespace MOE.Common.Models.Inrix.Repositories
{
    public class RouteMembersRepository
    {
        private readonly Inrix db = new Inrix();

        public void AddRouteMember(Route_Members member)
        {
            db.Route_Members.Add(member);
            db.SaveChanges();
        }


        public void RemoveSegmentFromRoute(string segmentID)
        {
            var rm = (from r in db.Route_Members
                where r.Segment_ID == segmentID
                select r).FirstOrDefault();

            if (rm != null)
                db.Route_Members.Remove(rm);
        }
    }
}