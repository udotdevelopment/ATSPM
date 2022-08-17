using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ATSPM.Models
{
    public class PreemptionAggregation : Aggregation
    {

        [Required]
        [Column(Order = 0)]
        public override DateTime BinStartTime { get; set; }

        [Required]
        [StringLength(10)]
        [Column(Order = 1)]
        public string SignalId { get; set; }

        [Required]
        [Column(Order = 3)]
        public int PreemptNumber { get; set; }

        [Required]
        [Column(Order = 4)]
        public int PreemptRequests { get; set; }

        [Required]
        [Column(Order = 5)]
        public int PreemptServices { get; set; }

       
    }
}