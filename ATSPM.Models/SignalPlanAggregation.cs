using System;
using System.Collections.Generic;

#nullable disable

namespace ATSPM.Application.Models
{
    public partial class SignalPlanAggregation
    {
        public string SignalId { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public int PlanNumber { get; set; }
    }
}
