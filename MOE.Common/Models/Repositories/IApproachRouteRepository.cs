using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOE.Common.Models.Repositories
{
    public interface IApproachRouteRepository
    {
        List<Models.ApproachRoute> GetAllRoutes();
        Models.ApproachRoute GetRouteByID(int routeID);
        Models.ApproachRoute GetRouteByName(string routeName);
        void DeleteByID(int routeID);
        void UpdateByID(int routeID, string newDescription);
        void Add(Models.ApproachRoute newRoute);
    }
}
