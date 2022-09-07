using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ATSPM.Models
{
    public class ApproachYellowRedActivationAggregation : Aggregation
    {
        //[Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //public int Id { get; set; }

        [Required]
        [Column(Order = 0)]
        public override DateTime BinStartTime { get; set; }

        [Required]
        [Column(Order = 1)]
        public int ApproachId { get; set; }

        [Required]
        [Column(Order = 8)]
        [StringLength(10)]
        public string SignalId { get; set; }

        [Required]
        [Column(Order = 9)]
        public int PhaseNumber { get; set; }

        [Required]
        [Column(Order = 4)]
        public bool IsProtectedPhase { get; set; }

        [Required]
        [Column(Order = 2)]
        public int SevereRedLightViolations { get; set; }

        [Required]
        [Column(Order = 3)]
        public int TotalRedLightViolations { get; set; }


        [Required]
        [Column(Order = 5)]
        public int YellowActivations { get; set; }

        [Required]
        [Column(Order = 6)]
        public int ViolationTime { get; set; }

        [Required]
        [Column(Order = 7)]
        public int Cycles { get; set; }



    }
}