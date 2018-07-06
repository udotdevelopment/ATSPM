using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CsvHelper.Configuration;

namespace MOE.Common.Models
{
    public class ApproachPcdAggregation : Aggregation
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
        public int ArrivalsOnGreen { get; set; }

        [Required]
        public int ArrivalsOnRed { get; set; }

        [Required]
        public int ArrivalsOnYellow { get; set; }

        [Required]
        public bool IsProtectedPhase { get; set; }
        [Required]
        public int Volume { get; set; }

        public sealed class ApproachPcdAggregationClassMap : ClassMap<ApproachPcdAggregation>
        {
            public ApproachPcdAggregationClassMap()
            {
                Map(m => m.Approach).Ignore();
                Map(m => m.Id).Name("Record Number");
                Map(m => m.BinStartTime).Name("Bin Start Time");
                Map(m => m.ApproachId).Name("Approach ID");
                Map(m => m.ArrivalsOnRed).Name("Arrivals On Red");
                Map(m => m.ArrivalsOnGreen).Name("Arrivals On Green");
                Map(m => m.ArrivalsOnYellow).Name("Arrivals On Yellow");
                Map(m => m.IsProtectedPhase).Name("Is Protected Phase");
            }
        }
    }
}