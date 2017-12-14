using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOE.Common.Models.ViewModel.RouteEdit
{
    public class ApproachesForRouteViewModel
    {
        public List<Tuple<RoutePhaseDirection, RoutePhaseDirection>> PairedApproaches { get; set; } =
            new List<Tuple<RoutePhaseDirection, RoutePhaseDirection>>();
    }
}
