using System;
using System.Collections.Generic;

#nullable disable

namespace ATSPM.Application.Models
{
    public partial class Route
    {
        public Route()
        {
            RouteSignals = new HashSet<RouteSignal>();
        }

        public int Id { get; set; }
        public string RouteName { get; set; }

        public virtual ICollection<RouteSignal> RouteSignals { get; set; }
    }
}
