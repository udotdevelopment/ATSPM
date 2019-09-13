using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AjaxControlToolkit;
using CsvHelper.Configuration;

namespace MOE.Common.Models
{
    public class ApproachSplitFailAggregation : Aggregation
    {
        [Key]
        [Column(Order = 5)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Key]
        [Required]
        [Column(Order = 0)]
        public override DateTime BinStartTime { get; set; }

        [Key]
        [Required]
        [Column(Order = 1)]
        public int ApproachId { get; set; }

        
        [Column(Order= 2)]
        public virtual Approach Approach { get; set; }

        [Required]
        [Column(Order = 3)]
        public int SplitFailures { get; set; }

        [Key]
        [Required]
        [Column(Order = 4)]
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
                Map(m => m.IsProtectedPhase).Name("Is Protected Phase");
            }
        }
    }
}