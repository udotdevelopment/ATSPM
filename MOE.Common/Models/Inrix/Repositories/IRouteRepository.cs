using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOE.Common.Models.Inrix.Repositories
{
    public interface IRouteRepository
    {
        void Add(Models.Inrix.Route route);
        
        List<Models.Inrix.Route> GetRoutesByGroupID(int groupID);

        Models.Inrix.Route GetRouteByName(string name);
    }
}
