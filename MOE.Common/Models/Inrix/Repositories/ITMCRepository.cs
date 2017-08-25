using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOE.Common.Models.Inrix.Repositories
{
    public interface ITMCRepository
    {
        List<Models.Inrix.TMC> GetMembersBySegmentID(int GroupID);
        List<Models.Inrix.TMC> GetNonMembersBySegmentID(int GroupID);
    }
}
