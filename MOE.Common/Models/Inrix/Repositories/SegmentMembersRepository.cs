using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOE.Common.Models.Inrix.Repositories
{
    public class SegmentMembersRepository: ISegmentMembersRepository
    {
        MOE.Common.Models.Inrix.Inrix db = new Inrix();

        public void DeleteMembersBySegmentID(int segmentID)
        {
            List<MOE.Common.Models.Inrix.Segment_Members> members = (from r in db.Segment_Members 
                                                                    where r.Segment_ID == segmentID 
                                                                    select r).ToList();

                db.Segment_Members.RemoveRange(members);

                db.SaveChanges();
            

        }

        public void InsertSegmentMembers(int segmentID, string TMC, int Order)
        {
            Models.Inrix.Segment_Members m = new Segment_Members();

            m.Segment_ID = segmentID;
            m.TMC = TMC;
            m.Segment_Order = Order;

        }



   
    }
}
