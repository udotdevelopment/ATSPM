using System.Collections.Generic;

namespace MOE.Common.Models.ViewModel.RouteEdit
{
    public class ApproachRouteDetailViewModel
    {
        public string RouteName { get; set; }
        public int RouteID { get; set; }
        public List<RouteSignal> RouteSignals { get; set; }
    }
}