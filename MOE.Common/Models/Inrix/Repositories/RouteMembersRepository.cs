using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOE.Common.Models.Inrix.Repositories
{
    public class RouteMembersRepository
    {
        MOE.Common.Models.Inrix.Inrix db = new Inrix();

        public void AddRouteMember(Models.Inrix.Route_Members member)
        {
            db.Route_Members.Add(member);
            db.SaveChanges();

        }


        public void RemoveSegmentFromRoute(string segmentID)
        {
            MOE.Common.Models.Inrix.Route_Members rm = (from r in db.Route_Members
                                                       where r.Segment_ID == segmentID
                                                       select r).FirstOrDefault();

            if(rm != null)
            {
                db.Route_Members.Remove(rm);
            }

        }

    }
}
