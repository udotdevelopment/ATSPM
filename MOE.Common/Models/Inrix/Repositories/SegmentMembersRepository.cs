using System.Linq;

namespace MOE.Common.Models.Inrix.Repositories
{
    public class SegmentMembersRepository : ISegmentMembersRepository
    {
        private readonly Inrix db = new Inrix();

        public void DeleteMembersBySegmentID(int segmentID)
        {
            var members = (from r in db.Segment_Members
                where r.Segment_ID == segmentID
                select r).ToList();

            db.Segment_Members.RemoveRange(members);

            db.SaveChanges();
        }

        public void InsertSegmentMembers(int segmentID, string TMC, int Order)
        {
            var m = new Segment_Members();

            m.Segment_ID = segmentID;
            m.TMC = TMC;
            m.Segment_Order = Order;
        }
    }
}