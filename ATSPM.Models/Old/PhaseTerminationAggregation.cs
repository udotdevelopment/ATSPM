using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ATSPM.Models
{
    public class PhaseTerminationAggregation : Aggregation
    {

        [Required]
        [Column(Order = 0)]
        public override DateTime BinStartTime { get; set; }

        [Required]
        [StringLength(10)]
        [Column(Order = 1)]
        public string SignalId { get; set; }

        [Required]
        [Column(Order = 2)]
        public int PhaseNumber { get; set; }

        [Required]
        [Column(Order = 3)]
        public int GapOuts { get; set; }

        [Required]
        [Column(Order = 4)]
        public int ForceOffs { get; set; }

        [Required]
        [Column(Order = 5)]
        public int MaxOuts { get; set; }

        [Required]
        [Column(Order = 6)]
        public int UnknownTerminationTypes { get; set; }


    }
}