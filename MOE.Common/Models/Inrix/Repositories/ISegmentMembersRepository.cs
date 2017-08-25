using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOE.Common.Models.Inrix.Repositories
{
    public interface ISegmentMembersRepository
    {
         void DeleteMembersBySegmentID(int segmentID);
         void InsertSegmentMembers(int segmentID, string TMC, int Order);



    }
}
