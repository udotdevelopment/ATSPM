using System;
using System.Collections.Generic;

namespace MOE.Common.Models.ViewModel.RouteEdit
{
    public class RouteCreateViewModel
    {
        public RouteCreateViewModel()
        {
            RouteMap = new RouteMapViewModel();
            SignalSelectList = new List<Tuple<string, string>>();
        }

        public Route Route { get; set; }
        public RouteMapViewModel RouteMap { get; set; }
        public List<Tuple<string, string>> SignalSelectList { get; set; }
    }
}