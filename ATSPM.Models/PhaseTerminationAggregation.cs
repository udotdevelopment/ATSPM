using System;
using System.Collections.Generic;

#nullable disable

namespace ATSPM.Application.Models
{
    public partial class PhaseTerminationAggregation
    {
        public DateTime BinStartTime { get; set; }
        public string SignalId { get; set; }
        public int PhaseNumber { get; set; }
        public int GapOuts { get; set; }
        public int ForceOffs { get; set; }
        public int MaxOuts { get; set; }
        public int Unknown { get; set; }
    }
}
