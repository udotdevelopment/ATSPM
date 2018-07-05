using System.Collections.Generic;

namespace MOE.Common.Models.Inrix.Repositories
{
    public interface ITMCRepository
    {
        List<TMC> GetMembersBySegmentID(int GroupID);
        List<TMC> GetNonMembersBySegmentID(int GroupID);
    }
}