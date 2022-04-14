using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ATSPM.Models
{
    public class PhaseLeftTurnGapAggregation : Aggregation
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
        public int PhaseNumber { get; set; }

        [Required]
        [Column(Order = 3)]
        public int ApproachId { get; set; }

        [Required]
        [Column(Order = 4)]
        public int GapCount1 { get; set; }

        [Required]
        [Column(Order = 5)]
        public int GapCount2 { get; set; }

        [Required]
        [Column(Order = 6)]
        public int GapCount3 { get; set; }


        [Required]
        [Column(Order = 7)]
        public int GapCount4 { get; set; }


        [Required]
        [Column(Order = 8)]
        public int GapCount5 { get; set; }


        [Required]
        [Column(Order = 9)]
        public int GapCount6 { get; set; }


        [Required]
        [Column(Order = 10)]
        public int GapCount7 { get; set; }


        [Required]
        [Column(Order = 11)]
        public int GapCount8 { get; set; }


        [Required]
        [Column(Order = 12)]
        public int GapCount9 { get; set; }


        [Required]
        [Column(Order = 13)]
        public int GapCount10 { get; set; }


        [Required]
        [Column(Order = 14)]
        public int GapCount11 { get; set; }


        [Required]
        [Column(Order = 15)]
        public double SumGapDuration1 { get; set; }

        [Required]
        [Column(Order = 16)]
        public double SumGapDuration2 { get; set; }

        [Required]
        [Column(Order = 17)]
        public double SumGapDuration3 { get; set; }

        [Required]
        [Column(Order = 18)]
        public double SumGreenTime { get; set; }

       
    }
}