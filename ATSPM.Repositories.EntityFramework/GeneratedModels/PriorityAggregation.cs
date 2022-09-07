using System;
using System.Collections.Generic;

#nullable disable

namespace ATSPM.Infrastructure.Repositories.EntityFramework.Repositories
{
    public partial class PriorityAggregation
    {
        public DateTime BinStartTime { get; set; }
        public string SignalId { get; set; }
        public int PriorityNumber { get; set; }
        public int PriorityRequests { get; set; }
        public int PriorityServiceEarlyGreen { get; set; }
        public int PriorityServiceExtendedGreen { get; set; }
    }
}
