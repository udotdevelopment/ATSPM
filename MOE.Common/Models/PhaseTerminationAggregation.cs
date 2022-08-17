using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CsvHelper.Configuration;

namespace MOE.Common.Models
{
    public class PhaseTerminationAggregation : Aggregation
    {

        [Key]
        [Required]
        [Column(Order = 0)]
        public override DateTime BinStartTime { get; set; }

        [Key]
        [Required]
        [StringLength(10)]
        [Column(Order = 1)]
        public string SignalId { get; set; }

        [Key]
        [Required]
        [Column(Order = 2)]
        public int PhaseNumber { get; set; }

        [Required]
        [Column(Order =3)]
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

        public sealed class PhaseTerminationAggregationClassMap : ClassMap<PhaseTerminationAggregation>
        {
            public PhaseTerminationAggregationClassMap()
            {
                Map(m => m.SignalId).Name("Signal Id");
                Map(m => m.PhaseNumber).Name("Phase Number");
                Map(m => m.BinStartTime).Name("Bin Start Time");
                Map(m => m.GapOuts).Name("Gap Outs");
                Map(m => m.MaxOuts).Name("Max Outs");
                Map(m => m.ForceOffs).Name("Force OFfs");
                Map(m => m.UnknownTerminationTypes).Name("Unknown Termination Types");
                //Map(m => m.PhaseSkipped).Name("Phase Skipped");
            }
        }
    }
}