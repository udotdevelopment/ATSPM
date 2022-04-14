using System;
using System.Collections.Generic;

#nullable disable

namespace ATSPM.Infrastructure.Repositories.EntityFramework.Repositories
{
    public partial class SignalEventCountAggregation
    {
        public DateTime BinStartTime { get; set; }
        public string SignalId { get; set; }
        public int EventCount { get; set; }
    }
}
