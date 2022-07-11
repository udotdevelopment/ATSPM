using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CsvHelper.Configuration;

namespace MOE.Common.Models
{
    public class PhasePedAggregation : Aggregation
    {
        [Key]
        [Required]
        [Column(Order = 0)]
        public override DateTime BinStartTime { get; set; }

        [Key]
        [Required]
        [Column(Order = 1)]
        public string SignalId { get; set; }

        [Key]
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
        public int PedRequests { get; set; }

        [Required]
        public int ImputedPedCallsRegistered { get; set; }

        [Required]
        public int UniquePedDetections { get; set; }

        [Required]
        public int PedBeginWalkCount { get; set; }

        [Required]
        public int PedCallsRegisteredCount { get; set; }

        public sealed class PhasePedAggregationClassMap : ClassMap<PhasePedAggregation>
        {
            public PhasePedAggregationClassMap()
            {
                Map(m => m.SignalId).Name("Signal Id");
                Map(m => m.PhaseNumber).Name("Phase Number");
                Map(m => m.BinStartTime).Name("Bin Start Time");
                Map(m => m.PedCycles).Name("Ped Cycles");
                Map(m => m.PedDelaySum).Name("Ped Delay Sum");
                Map(m => m.MinPedDelay).Name("Min Ped Delay");
                Map(m => m.MaxPedDelay).Name("Max Ped Delay");
                Map(m => m.PedRequests).Name("Ped Requests");
                Map(m => m.ImputedPedCallsRegistered).Name("Imputed Ped Calls Registered");
                Map(m => m.UniquePedDetections).Name("Unique Ped Detections");
                Map(m => m.PedBeginWalkCount).Name("Ped Begin Walk Count");
                Map(m => m.PedCallsRegisteredCount).Name("Ped Calls Registered Count");
            }
        }
    }
}
