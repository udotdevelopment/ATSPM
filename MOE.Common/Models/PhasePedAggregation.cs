using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CsvHelper.Configuration;

namespace MOE.Common.Models
{
    public class PhasePedAggregation : Aggregation
    {
        [Key]
        [Column(Order = 5)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Key]
        [Required]
        [Column(Order = 0)]
        public override DateTime BinStartTime { get; set; }

        //[Key]
        [Required]
        [Column(Order = 1)]
        [StringLength(10)]
        public string SignalId { get; set; }

        //[Key]
        [Required]
        [Column(Order = 2)]
        public int PhaseNumber { get; set; }

        [Required]
        [Column(Order = 3)]
        public int PedCount { get; set; }

        [Required]
        [Column(Order = 4)]
        public double PedDelay { get; set; }

        public sealed class PhasePedAggregationClassMap : ClassMap<PhasePedAggregation>
        {
            public PhasePedAggregationClassMap()
            {
                Map(m => m.Id).Name("Record Number");
                Map(m => m.SignalId).Name("Signal Id");
                Map(m => m.PhaseNumber).Name("Phase Number");
                Map(m => m.BinStartTime).Name("Bin Start Time");
                Map(m => m.PedCount).Name("Ped Count");
                Map(m => m.PedDelay).Name("Ped Delay");
            }
        }
    }
}