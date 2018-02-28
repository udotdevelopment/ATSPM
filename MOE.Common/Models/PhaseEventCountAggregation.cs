using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CsvHelper.Configuration;

namespace MOE.Common.Models
{
    public class PhaseEventCountAggregation : Aggregation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public override int Id { get; set; }

        [Required]
        public override DateTime BinStartTime { get; set; }

        [Required]
        public int ApproachId { get; set; }

        [Required]
        public int EventCount { get; set; }

        [Required]
        public bool IsProtectedPhase { get; set; }


        public sealed class
            ApproachCountAggregationClassMap : ClassMap<PhaseEventCountAggregation>
        {
            public ApproachCountAggregationClassMap()
            {
                Map(m => m.Id).Name("Record Number");
                Map(m => m.BinStartTime).Name("Bin Start Time");
                Map(m => m.ApproachId).Name("Approach ID");
                Map(m => m.EventCount).Name("Event Count");
            }
        }
    }
}
