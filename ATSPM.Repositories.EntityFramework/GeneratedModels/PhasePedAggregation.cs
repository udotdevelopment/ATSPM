using System;
using System.Collections.Generic;

#nullable disable

namespace ATSPM.Infrastructure.Repositories.EntityFramework.Repositories
{
    public partial class PhasePedAggregation
    {
        public DateTime BinStartTime { get; set; }
        public string SignalId { get; set; }
        public int ApproachId { get; set; }
        public int PhaseNumber { get; set; }
        public int PedCycles { get; set; }
        public int PedDelaySum { get; set; }
        public int MinPedDelay { get; set; }
        public int MaxPedDelay { get; set; }
        public int PedActuations { get; set; }
    }
}
