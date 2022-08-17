using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ATSPM.Models
{
    public class PhasePedAggregation : Aggregation
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
        public int PedCycles { get; set; }

        [Required]
        [Column(Order = 4)]
        public int PedDelaySum { get; set; }

        [Required]
        [Column(Order = 5)]
        public int MinPedDelay { get; set; }

        [Required]
        [Column(Order = 6)]
        public int MaxPedDelay { get; set; }

        [Required]
        [Column(Order = 7)]
        public int PedActuations { get; set; }


    }
}