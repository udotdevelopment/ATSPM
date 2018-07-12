using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CsvHelper.Configuration;

namespace MOE.Common.Models
{
    public class ApproachCycleAggregation : Aggregation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public override int Id { get; set; }

        [Required]
        public override DateTime BinStartTime { get; set; }

        [Required]
        public int ApproachId { get; set; }

        public virtual Approach Approach { get; set; }

        [Required]
        public double RedTime { get; set; }

        [Required]
        public double YellowTime { get; set; }

        [Required]
        public double GreenTime { get; set; }

        [Required]
        public int TotalCycles { get; set; }

        [Required]
        public int PedActuations { get; set; }

        [Required]
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