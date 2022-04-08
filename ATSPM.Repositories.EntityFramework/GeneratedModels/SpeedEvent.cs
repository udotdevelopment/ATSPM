using System;
using System.Collections.Generic;

#nullable disable

namespace ATSPM.Infrastructure.Repositories.EntityFramework.Repositories
{
    public partial class SpeedEvent
    {
        public string DetectorId { get; set; }
        public int Mph { get; set; }
        public int Kph { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
