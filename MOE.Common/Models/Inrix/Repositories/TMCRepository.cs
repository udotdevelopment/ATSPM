using System.Collections.Generic;
using System.Linq;

namespace MOE.Common.Models.Inrix.Repositories
{
    public class TMCRepository : ITMCRepository
    {
        private readonly Inrix db = new Inrix();

        public List<TMC> GetMembersBySegmentID(int segmentID)
        {
            var members = (from tmc in db.TMCs
                join sm in db.Segment_Members on tmc.TMC1 equals sm.TMC
                where sm.Segment_ID == segmentID
                select tmc).ToList();

            return members;
        }


        public List<TMC> GetNonMembersBySegmentID(int segmentID)
        {
            var nonMembers = (from r in db.TMCs
                select r).Except(
                from tmc in db.TMCs
                join sm in db.Segment_Members on tmc.TMC1 equals sm.TMC
                where sm.Segment_ID == segmentID
                select tmc).ToList();

            return nonMembers;
        }
    }
}