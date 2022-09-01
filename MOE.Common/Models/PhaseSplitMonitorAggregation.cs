using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CsvHelper.Configuration;

namespace MOE.Common.Models
{
    public class PhaseSplitMonitorAggregation : Aggregation
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
        [Column(Order =3)]
        public int EightyFifthPercentileSplit { get; set; }

        [Required]
        [Column(Order = 4)]
        public int SkippedCount { get; set; }

       

        public sealed class PhaseSplitMonitorAggregationClassMap : ClassMap<PhaseSplitMonitorAggregation>
        {
            public PhaseSplitMonitorAggregationClassMap()
            {
                Map(m => m.SignalId).Name("Signal Id");
                Map(m => m.PhaseNumber).Name("Phase Number");
                Map(m => m.BinStartTime).Name("Bin Start Time");
                Map(m => m.EightyFifthPercentileSplit).Name("Eighty Fifth Percentile Split");
                Map(m => m.SkippedCount).Name("Skipped Count");
            }
        }
    }
}