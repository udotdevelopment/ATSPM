using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MOE.Common.Models.ViewModel.RouteEdit
{
   public class RouteListViewModel
    {
        public List<Models.ApproachRoute> FillRoutes()
        {

            MOE.Common.Models.Repositories.IApproachRouteRepository arr = MOE.Common.Models.Repositories.ApproachRouteRepositoryFactory.Create();

            List<Models.ApproachRoute> list = arr.GetAllRoutes();

            return list;
        }

        public List<Models.RouteSignal> GetRouteDetails(Models.ApproachRoute Route)
        {
            MOE.Common.Models.Repositories.IApproachRouteDetailRepository ardr = MOE.Common.Models.Repositories.ApproachRouteDetailRepositoryFactory.Create();

            return ardr.GetByRouteID(Route.Id);
        }
    }
}
