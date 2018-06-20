using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CsvHelper.Configuration;

namespace MOE.Common.Models
{
    public class PhasePedAggregation : Aggregation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public override int Id { get; set; }

        [Required]
        public override DateTime BinStartTime { get; set; }

        [Required]
        public string SignalId { get; set; }

        [Required]
        public int PhaseNumber { get; set; }

        [Required]
        public int PedCount { get; set; }

        [Required]
        public double PedDelay { get; set; }

        public sealed class PhasePedAggregationClassMap : ClassMap<PhasePedAggregation>
        {
            public PhasePedAggregationClassMap()
            {
                Map(m => m.Id).Name("Record Number");
                Map(m => m.PhaseNumber).Name("Signal Id");
                Map(m => m.PhaseNumber).Name("Phase Number");
                Map(m => m.BinStartTime).Name("Bin Start Time");
                Map(m => m.PedCount).Name("Ped Count");
                Map(m => m.PedDelay).Name("Ped Delay");
            }
        }
    }
}