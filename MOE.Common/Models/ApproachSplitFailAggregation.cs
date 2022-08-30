using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AjaxControlToolkit;
using CsvHelper.Configuration;

namespace MOE.Common.Models
{
    public class ApproachSplitFailAggregation : Aggregation
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

        [Key]
        [Required]
        [Column(Order = 10)]
        public int PhaseNumber { get; set; }

        [Key]
        [Required]
        [Column(Order = 2)]
        public int ApproachId { get; set; }

        [Required]
        [Column(Order = 3)]
        public int SplitFailures { get; set; }

        [Key]
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

        public sealed class ApproachSplitFailAggregationClassMap : ClassMap<ApproachSplitFailAggregation>
        {
            public ApproachSplitFailAggregationClassMap()
            {
                Map(m => m.BinStartTime).Name("Bin Start Time");
                Map(m => m.SignalId).Name("Signal ID");
                Map(m => m.ApproachId).Name("Approach ID");
                Map(m => m.SplitFailures).Name("Split Failures");
                Map(m => m.IsProtectedPhase).Name("Is Protected Phase");
                Map(m => m.GreenOccupancySum).Name("Green Occupancy Sum");
                Map(m => m.RedOccupancySum).Name("Red Occupancy Sum");
                Map(m => m.GreenTimeSum).Name("Green Detection Sum");
                Map(m => m.RedTimeSum).Name("Red Detection Sum");
                Map(m => m.Cycles).Name("Cycles");
            }
        }
    }
}