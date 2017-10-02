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
        public MOE.Common.Models.ApproachRoute ApproachRoute { get; set; }
        public MOE.Common.Models.ViewModel.Chart.SignalSearchViewModel SignalSearch { get; set; }

        public RouteCreateViewModel()
        {
            SignalSearch = new SignalSearchViewModel();
        }
    }
}
