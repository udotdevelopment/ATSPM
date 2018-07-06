using System;
using System.Collections.Generic;

namespace MOE.Common.Models.ViewModel.RouteEdit
{
    public class ApproachesForRouteViewModel
    {
        public List<Tuple<RoutePhaseDirection, RoutePhaseDirection>> PairedApproaches { get; set; } =
            new List<Tuple<RoutePhaseDirection, RoutePhaseDirection>>();
    }
}