using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ATSPM.Models
{
    public class ApproachSpeedAggregation : Aggregation
    {

        [Required]
        [Column(Order = 0)]
        public override DateTime BinStartTime { get; set; }

        [Required]
        [Column(Order = 1)]
        [StringLength(10)]
        public string SignalId { get; set; }

        [Required]
        [Column(Order = 2)]
        public int ApproachId { get; set; }

        [Required]
        [Column(Order = 3)]
        public int SummedSpeed { get; set; }

        [Required]
        [Column(Order = 4)]
        public int SpeedVolume { get; set; }

        [Required]
        [Column(Order = 5)]
        public int Speed85Th { get; set; }

        [Required]
        [Column(Order = 6)]
        public int Speed15Th { get; set; }



       
    }
}