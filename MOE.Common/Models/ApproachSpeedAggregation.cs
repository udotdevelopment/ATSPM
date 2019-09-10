using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CsvHelper.Configuration;

namespace MOE.Common.Models
{
    public class ApproachSpeedAggregation : Aggregation
    {
        [Key]
        [Column(Order = 7)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }


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
        public double SummedSpeed { get; set; }

        [Required]
        [Column(Order = 3)]
        public double SpeedVolume { get; set; }

        [Required]
        [Column(Order = 4)]
        public double Speed85Th { get; set; }

        [Required]
        [Column(Order = 5)]
        public double Speed15Th { get; set; }

        [Key]
        [Required]
        [Column(Order = 6)]
        public bool IsProtectedPhase { get; set; }
        

        public sealed class ApproachSpeedAggregationClassMap : ClassMap<ApproachSpeedAggregation>
        {
            public ApproachSpeedAggregationClassMap()
            {
               // Map(m => m.Approach).Ignore();
                Map(m => m.Id).Name("Record Number");
                Map(m => m.BinStartTime).Name("Bin Start Time");
                Map(m => m.ApproachId).Name("Approach ID");
                Map(m => m.SummedSpeed).Name("Total speed for bin");
                Map(m => m.SpeedVolume).Name("Total Volume for bin ");
                Map(m => m.Speed85Th).Name("85th Percentile");
                Map(m => m.Speed15Th).Name("15th Percentile");
                Map(m => m.IsProtectedPhase).Name("Is Protected Phase");
            }
        }
    }
}