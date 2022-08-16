using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ATSPM.Models
{
    //public class DetectorEventCountAggregation : Aggregation
    public class DetectorEventCountAggregation
    {

        [Required]
        [Column(Order = 0)]
        public DateTime BinStartTime { get; set; }

        [Required]
        [Column(Order = 1)]
        [StringLength(10)]
        public string SignalId { get; set; }

        [Required]
        [Column(Order = 2)]
        public int ApproachId { get; set; }

        [Required]
        [Column(Order = 3)]
        public int DetectorPrimaryId { get; set; }

        [Required]
        [Column(Order = 4)]
        public int EventCount { get; set; }

       
    }
}
