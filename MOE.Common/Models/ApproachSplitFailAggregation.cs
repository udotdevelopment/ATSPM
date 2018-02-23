using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CsvHelper.Configuration;

namespace MOE.Common.Models
{
    public class ApproachSplitFailAggregation : Aggregation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public override int Id { get; set; }

        [Required]
        public override DateTime BinStartTime { get; set; }

        [Required]
        public int ApproachId { get; set; }

        public virtual Approach Approach { get; set; }

        [Required]
        public int SplitFailures { get; set; }

        [Required]
        public int GapOuts { get; set; }

        [Required]
        public int ForceOffs { get; set; }

        [Required]
        public int MaxOuts { get; set; }

        [Required]
        public int UnknownTerminationTypes { get; set; }

        [Required]
        public bool IsProtectedPhase { get; set; }

        public sealed class ApproachSplitFailAggregationClassMap : ClassMap<ApproachSplitFailAggregation>
        {
            public ApproachSplitFailAggregationClassMap()
            {
                Map(m => m.Approach).Ignore();
                Map(m => m.Id).Name("Record Number");
                Map(m => m.BinStartTime).Name("Bin Start Time");
                Map(m => m.ApproachId).Name("Approach ID");
                Map(m => m.SplitFailures).Name("Split Failures");
                Map(m => m.GapOuts).Name("Gap Outs");
                Map(m => m.MaxOuts).Name("Max Outs");
                Map(m => m.UnknownTerminationTypes).Name("Unknown Termination Types");
                Map(m => m.IsProtectedPhase).Name("Is Protected Phase");
            }
        }
    }
}