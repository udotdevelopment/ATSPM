using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ATSPM.Models
{
    public class PhaseCycleAggregation : Aggregation
    {

        [Required]
        [Column(Order = 0)]
        public override DateTime BinStartTime { get; set; }

        [Required]
        [Column(Order = 1)]
        [StringLength(10)]
        public string SignalId { get; set; }

        [Required]
        [Column(Order = 2)]
        public int ApproachId { get; set; }

        [Required]
        [Column(Order = 3)]
        public int PhaseNumber { get; set; }

        [Required]
        [Column(Order = 4)]
        public int RedTime { get; set; }

        [Required]
        [Column(Order = 5)]
        public int YellowTime { get; set; }

        [Required]
        [Column(Order = 6)]
        public int GreenTime { get; set; }

        [Required]
        [Column(Order = 7)]
        public int TotalRedToRedCycles { get; set; }

        [Required]
        [Column(Order = 8)]
        public int TotalGreenToGreenCycles { get; set; }

    }
}