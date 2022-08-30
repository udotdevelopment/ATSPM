using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CsvHelper.Configuration;

namespace MOE.Common.Models
{
    public class SignalEventCountAggregation : Aggregation
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

        [Required]
        [Column(Order = 2)]
        public int EventCount { get; set; }
        
        public sealed class
            EventCountAggregationClassMap : ClassMap<SignalEventCountAggregation>
        {
            public EventCountAggregationClassMap()
            {
                Map(m => m.BinStartTime).Name("Bin Start Time");
                Map(m => m.SignalId).Name("Signal ID");
                Map(m => m.EventCount).Name("Event Count");

            }
        }
    }
}



