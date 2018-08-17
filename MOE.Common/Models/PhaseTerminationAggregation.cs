using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CsvHelper.Configuration;

namespace MOE.Common.Models
{
    public class PhaseTerminationAggregation : Aggregation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public override long Id { get; set; }

        [Required]
        public override DateTime BinStartTime { get; set; }

        [Required]
        public string SignalId { get; set; }

        [Required]
        public int PhaseNumber { get; set; }

        [Required]
        public int GapOuts { get; set; }

        [Required]
        public int ForceOffs { get; set; }

        [Required]
        public int MaxOuts { get; set; }

        [Required]
        public int UnknownTerminationTypes { get; set; }

        public sealed class PhaseTerminationAggregationClassMap : ClassMap<PhaseTerminationAggregation>
        {
            public PhaseTerminationAggregationClassMap()
            {
                Map(m => m.Id).Name("Record Number");
                Map(m => m.PhaseNumber).Name("Signal Id");
                Map(m => m.PhaseNumber).Name("Phase Number");
                Map(m => m.BinStartTime).Name("Bin Start Time");
                Map(m => m.GapOuts).Name("Gap Outs");
                Map(m => m.MaxOuts).Name("Max Outs");
                Map(m => m.ForceOffs).Name("Force OFfs");
                Map(m => m.UnknownTerminationTypes).Name("Unknown Termination Types");
            }
        }
    }
}