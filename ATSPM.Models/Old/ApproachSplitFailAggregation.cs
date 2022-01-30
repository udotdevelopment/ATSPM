using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ATSPM.Models
{
    public class ApproachSplitFailAggregation : Aggregation
    {
        
        [Required]
        [Column(Order = 0)]
        public override DateTime BinStartTime { get; set; }
                
        [Required]
        [Column(Order = 1)]
        [StringLength(10)]
        public string SignalId { get; set; }

        [Required]
        [Column(Order = 10)]
        public int PhaseNumber { get; set; }

        [Required]
        [Column(Order = 2)]
        public int ApproachId { get; set; }

        [Required]
        [Column(Order = 3)]
        public int SplitFailures { get; set; }

        [Required]
        [Column(Order = 4)]
        public bool IsProtectedPhase { get; set; }

        [Required]
        [Column(Order = 5)]
        public int GreenOccupancySum { get; set; }

        [Required]
        [Column(Order = 6)]
        public int RedOccupancySum { get; set; }

        [Required]
        [Column(Order = 7)]
        public int GreenTimeSum { get; set; }

        [Required]
        [Column(Order = 8)]
        public int RedTimeSum { get; set; }

        [Required]
        [Column(Order = 9)]
        public int Cycles { get; set; }

    }
}