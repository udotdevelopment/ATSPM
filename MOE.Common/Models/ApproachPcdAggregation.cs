using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CsvHelper.Configuration;

namespace MOE.Common.Models
{
    public class ApproachPcdAggregation : Aggregation
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
        [Column(Order = 1)]
        public int ApproachId { get; set; }

        public virtual Approach Approach { get; set; }

        [Required]
        [Column(Order = 2)]
        public int ArrivalsOnGreen { get; set; }

        [Required]
        [Column(Order = 3)]
        public int ArrivalsOnRed { get; set; }

        [Required]
        [Column(Order = 4)]
        public int ArrivalsOnYellow { get; set; }

        [Key]
        [Required]
        [Column(Order = 5)]
        public bool IsProtectedPhase { get; set; }

        [Required]
        [Column(Order = 6)]
        public int Volume { get; set; }

        [Required]
        [Column(Order = 7)]
        public string SignalId { get; set; }

        [Required]
        [Column(Order = 8)]
        public int PhaseNumber { get; set; }

        [Required]
        [Column(Order = 9)]
        public double TotalDelay { get; set; }

        public sealed class ApproachPcdAggregationClassMap : ClassMap<ApproachPcdAggregation>
        {
            public ApproachPcdAggregationClassMap()
            {
                Map(m => m.Approach).Ignore();
        //        Map(m => m.Id).Name("Record Number");
                Map(m => m.BinStartTime).Name("Bin Start Time");
                Map(m => m.ApproachId).Name("Approach ID");
                Map(m => m.ArrivalsOnRed).Name("Arrivals On Red");
                Map(m => m.ArrivalsOnGreen).Name("Arrivals On Green");
                Map(m => m.ArrivalsOnYellow).Name("Arrivals On Yellow");
                Map(m => m.IsProtectedPhase).Name("Is Protected Phase");
                Map(m => m.Volume).Name("Volume");
                Map(m => m.SignalId).Name("Signal ID");
                Map(m => m.PhaseNumber).Name("Phase Number");
                Map(m => m.TotalDelay).Name("Wait for Green Time");
            }
        }
    }
}