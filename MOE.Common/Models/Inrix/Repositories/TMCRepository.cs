using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOE.Common.Models.Inrix.Repositories
{
    public class TMCRepository: ITMCRepository
    {
        Models.Inrix.Inrix db = new Inrix();

        public List<Models.Inrix.TMC> GetMembersBySegmentID(int segmentID)
        {
            List<Models.Inrix.TMC> members = (from tmc in db.TMCs
                                              join sm in db.Segment_Members on tmc.TMC1 equals sm.TMC
                                                          where sm.Segment_ID == segmentID
                                                          select tmc).ToList();

            return members;
        }


        public List<Models.Inrix.TMC> GetNonMembersBySegmentID(int segmentID)
        {
            List<Models.Inrix.TMC> nonMembers = (
                                                (from r in db.TMCs
                                                 select r).Except(
                                                 from tmc in db.TMCs
                                              join sm in db.Segment_Members on tmc.TMC1 equals sm.TMC
                                              where sm.Segment_ID == segmentID
                                              select tmc)
                                              ).ToList();

            return nonMembers;
        }


    }
}
