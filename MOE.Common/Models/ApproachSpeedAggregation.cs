using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CsvHelper.Configuration;

namespace MOE.Common.Models
{
    public class ApproachSpeedAggregation : Aggregation
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
        [Column(Order = 2)]
        public int ApproachId { get; set; }

        [Required]
        [Column(Order = 3)]
        public int SummedSpeed { get; set; }

        [Required]
        [Column(Order = 4)]
        public int SpeedVolume { get; set; }

        [Required]
        [Column(Order = 5)]
        public int Speed85Th { get; set; }

        [Required]
        [Column(Order = 6)]
        public int Speed15Th { get; set; }

        

        public sealed class ApproachSpeedAggregationClassMap : ClassMap<ApproachSpeedAggregation>
        {
            public ApproachSpeedAggregationClassMap()
            {
                Map(m => m.BinStartTime).Name("Bin Start Time");
                Map(m => m.SignalId).Name("Signal ID");
                Map(m => m.ApproachId).Name("Approach ID");
                Map(m => m.SummedSpeed).Name("Total speed for bin");
                Map(m => m.SpeedVolume).Name("Total Volume for bin ");
                Map(m => m.Speed85Th).Name("85th Percentile");
                Map(m => m.Speed15Th).Name("15th Percentile");
            }
        }
    }
}