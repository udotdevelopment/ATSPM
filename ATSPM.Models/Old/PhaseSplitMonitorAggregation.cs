using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ATSPM.Models
{
    public class PhaseSplitMonitorAggregation : Aggregation
    {

        [Required]
        [Column(Order = 0)]
        public override DateTime BinStartTime { get; set; }

        [Required]
        [Column(Order = 1)]
        public string SignalId { get; set; }

        [Required]
        [Column(Order = 2)]
        public int PhaseNumber { get; set; }

        [Required]
        [Column(Order = 3)]
        public int EightyFifthPercentileSplit { get; set; }

        [Required]
        [Column(Order = 4)]
        public int SkippedCount { get; set; }


    }
}