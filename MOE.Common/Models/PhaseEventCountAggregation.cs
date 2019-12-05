using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CsvHelper.Configuration;

namespace MOE.Common.Models
{
    public class ApproachEventCountAggregation : Aggregation
    {
        //[Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //public int Id { get; set; }

        [Key]
        [Required]
        [Column(Order = 0)]
        public override DateTime BinStartTime { get; set; }

        [Key]
        [Required]
        [Column(Order = 1)]
        public int ApproachId { get; set; }

        [Required]
        [Column (Order = 2)]
        public int EventCount { get; set; }

        [Key]
        [Required]
        [Column(Order = 3)]
        public bool IsProtectedPhase { get; set; }

        public sealed class
            ApproachCountAggregationClassMap : ClassMap<ApproachEventCountAggregation>
        {
            public ApproachCountAggregationClassMap()
            {
                //Map(m => m.Id).Name("Record Number");
                Map(m => m.BinStartTime).Name("Bin Start Time");
                Map(m => m.ApproachId).Name("Approach ID");
                Map(m => m.EventCount).Name("Event Count");
                Map(m => m.IsProtectedPhase).Name("Is Protected Phase");
            }
        }
    }
}
