using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ATSPM.Models
{
    public class SignalEventCountAggregation : Aggregation
    {

        [Required]
        [Column(Order = 0)]
        public override DateTime BinStartTime { get; set; }

        [Required]
        [StringLength(10)]
        [Column(Order = 1)]
        public string SignalId { get; set; }

        [Required]
        [Column(Order = 2)]
        public int EventCount { get; set; }


    }
}



