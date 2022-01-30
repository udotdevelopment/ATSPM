using System;
using System.Collections.Generic;

#nullable disable

namespace ATSPM.Infrastructure.Repositories.EntityFramework.Repositories
{
    public partial class ApproachYellowRedActivationAggregation
    {
        public DateTime BinStartTime { get; set; }
        public string SignalId { get; set; }
        public int ApproachId { get; set; }
        public int PhaseNumber { get; set; }
        public bool IsProtectedPhase { get; set; }
        public int SevereRedLightViolations { get; set; }
        public int TotalRedLightViolations { get; set; }
        public int YellowActivations { get; set; }
        public int ViolationTime { get; set; }
        public int Cycles { get; set; }
    }
}
