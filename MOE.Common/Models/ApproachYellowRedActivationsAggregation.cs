using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CsvHelper.Configuration;

namespace MOE.Common.Models
{
    public class ApproachYellowRedActivationAggregation : Aggregation
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
        [Column(Order= 1)]
        public int ApproachId { get; set; }

        [Key]
        [Required]
        [Column(Order = 8)]
        [StringLength(10)]
        public string SignalId { get; set; }

        [Key]
        [Required]
        [Column(Order = 9)]
        public int PhaseNumber { get; set; }

        [Key]
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


        public sealed class
            ApproachYellowRedActivationAggregationClassMap : ClassMap<ApproachYellowRedActivationAggregation>
        {
            public ApproachYellowRedActivationAggregationClassMap()
            {
                //Map(m => m.Id).Name("Record Number");
                Map(m => m.BinStartTime).Name("Bin Start Time");
                Map(m => m.ApproachId).Name("Approach ID");
                Map(m => m.SevereRedLightViolations).Name("Severe Red Light Violations");
                Map(m => m.TotalRedLightViolations).Name("Total Red Light Violations");
                Map(m => m.IsProtectedPhase).Name("Is Protected Phase");
                Map(m => m.YellowActivations).Name("Yellow Activations");
                Map(m => m.ViolationTime).Name("Violation Time");
                Map(m => m.Cycles).Name("Cycles");
                Map(m => m.SignalId).Name("Signal ID");
                Map(m => m.PhaseNumber).Name("Signal ID");
            }
        }
    }
}