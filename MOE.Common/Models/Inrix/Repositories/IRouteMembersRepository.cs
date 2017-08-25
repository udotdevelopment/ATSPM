using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOE.Common.Models.Inrix.Repositories
{
    public interface IRouteMembersRepository
    {
        void AddRouteMember();
        void RemoveSegmentFromRoute(int segmentID);
        
    }
}
