using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ATSPM.Models
{
    public class ApproachPcdAggregation : Aggregation
    {
        
        [Required]
        [Column(Order = 0)]
        public override DateTime BinStartTime { get; set; }

        [Required]
        [Column(Order = 7)]
        [StringLength(10)]
        public string SignalId { get; set; }

        [Required]
        [Column(Order = 8)]
        public int PhaseNumber { get; set; }

        [Required]
        [Column(Order = 5)]
        public bool IsProtectedPhase { get; set; }


        [Required]
        [Column(Order = 1)]
        public int ApproachId { get; set; }

        [Required]
        [Column(Order = 2)]
        public int ArrivalsOnGreen { get; set; }

        [Required]
        [Column(Order = 3)]
        public int ArrivalsOnRed { get; set; }

        [Required]
        [Column(Order = 4)]
        public int ArrivalsOnYellow { get; set; }


        [Required]
        [Column(Order = 6)]
        public int Volume { get; set; }



        [Required]
        [Column(Order = 9)]
        public int TotalDelay { get; set; }

       
    }
}