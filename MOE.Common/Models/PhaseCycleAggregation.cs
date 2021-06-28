using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CsvHelper.Configuration;

namespace MOE.Common.Models
{
    public class PhaseCycleAggregation : Aggregation
    {

        [Key]
        [Required]
        [Column(Order = 0)]
        public override DateTime BinStartTime { get; set; }

        [Key]
        [Required]
        [Column(Order = 1)]
        [StringLength(10)]
        public string SignalId { get; set; }

        [Required]
        [Column(Order = 2)]
        public int ApproachId { get; set; }

        [Key]
        [Required]
        [Column(Order = 3)]
        public int PhaseNumber { get; set; }

        [Required]
        [Column(Order = 4)]
        public int RedTime { get; set; }

        [Required]
        [Column(Order = 5)]
        public int YellowTime { get; set; }

        [Required]
        [Column(Order = 6)]
        public int GreenTime { get; set; }

        [Required]
        [Column(Order = 7)]
        public int TotalRedToRedCycles { get; set; }

        [Required]
        [Column(Order = 8)]
        public int TotalGreenToGreenCycles { get; set; }

        public sealed class ApproachCycleAggregationClassMap : ClassMap<PhaseCycleAggregation>
        {
            public ApproachCycleAggregationClassMap()
            {
                Map(m => m.BinStartTime).Name("Bin Start Time");
                Map(m => m.ApproachId).Name("Approach ID");
                Map(m => m.RedTime).Name("RedTime");
                Map(m => m.YellowTime).Name("YellowTime");
                Map(m => m.GreenTime).Name("GreenTime");
                Map(m => m.TotalRedToRedCycles).Name("Total Red To Red Cycles");
                Map(m => m.TotalGreenToGreenCycles).Name("Total Green To Green Cycles");
            }
        }
    }
}