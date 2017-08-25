using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOE.Common.Models.Inrix.Repositories
{
    public interface IGroupMemberRepository
    {
         void Add(Models.Inrix.Group_Members groupMember);
         void DeleteByGroupID(int ID);
    }
}
