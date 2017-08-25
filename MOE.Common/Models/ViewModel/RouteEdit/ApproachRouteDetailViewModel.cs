using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOE.Common.Models.ViewModel.RouteEdit
{
    public class ApproachRouteDetailViewModel
    {
        public string RouteName { get; set; }
        public int RouteID { get; set; }
        public List<MOE.Common.Models.ApproachRouteDetail> Approaches { get; set; }
    }
}
