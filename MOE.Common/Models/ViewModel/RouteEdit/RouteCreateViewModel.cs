using MOE.Common.Models.ViewModel.Chart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOE.Common.Models.ViewModel.RouteEdit
{
    public class RouteCreateViewModel
    {        

        public MOE.Common.Models.Route Route { get; set; }
        public MOE.Common.Models.ViewModel.RouteEdit.RouteMapViewModel RouteMap { get; set; }
        public List<Tuple<string, string>> SignalSelectList { get; set; }

        public RouteCreateViewModel()
        {
            RouteMap = new RouteMapViewModel();
            SignalSelectList = new List<Tuple<string, string>>();
        }
    }
}
