using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CsvHelper.Configuration;

namespace MOE.Common.Models
{
    public class ApproachCycleAggregation : Aggregation
    {
        //[Key]
        [Column(Order = 8)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Key]
        [Required]
        [Column(Order = 0)]
        public override DateTime BinStartTime { get; set; }

        [Key]
        [Required][Column(Order = 1)]
        public int ApproachId { get; set; }
       
        public virtual Approach Approach { get; set; }

        [Required]
        [Column(Order = 2)]
        public double RedTime { get; set; }

        [Required]
        [Column(Order = 3)]
        public double YellowTime { get; set; }

        [Required]
        [Column(Order = 4)]
        public double GreenTime { get; set; }

        [Required]
        [Column(Order = 5)]
        public int TotalCycles { get; set; }

        [Required]
        [Column(Order = 6)]
        public int PedActuations { get; set; }

        [Key]
        [Required]
        [Column(Order = 7)]
        public bool IsProtectedPhase { get; set; }

        public sealed class ApproachCycleAggregationClassMap : ClassMap<ApproachCycleAggregation>
        {
            public ApproachCycleAggregationClassMap()
            {
                Map(m => m.Approach).Ignore();
                Map(m => m.Id).Name("Record Number");
                Map(m => m.BinStartTime).Name("Bin Start Time");
                Map(m => m.ApproachId).Name("Approach ID");
                Map(m => m.RedTime).Name("RedTime");
                Map(m => m.YellowTime).Name("YellowTime");
                Map(m => m.GreenTime).Name("GreenTime");
                Map(m => m.TotalCycles).Name("Total Cycles");
                Map(m => m.PedActuations).Name("Ped Actuations");
                Map(m => m.IsProtectedPhase).Name("Is Protected Phase");
            }
        }
    }
}