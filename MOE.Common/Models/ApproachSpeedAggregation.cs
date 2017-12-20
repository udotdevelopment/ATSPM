using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;
using CsvHelper.Configuration;

namespace MOE.Common.Models
{
    public class ApproachSpeedAggregation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public DateTime BinStartTime { get; set; }

        [Required]
        public int ApproachId { get; set; }

        public virtual Approach Approach { get; set; }

        [Required]
        public double SummedSpeed { get; set; }

        [Required]
        public double SpeedVolume { get; set; }

        [Required]
        public double Speed85Th { get; set; }

        [Required]
        public double Speed15Th { get; set; }

        [Required]
        public bool IsProtectedPhase { get; set; }

        public sealed class ApproachSpeedAggregationClassMap : ClassMap<ApproachSpeedAggregation>
        {
            public ApproachSpeedAggregationClassMap()
            {

                Map(m => m.Approach).Ignore();
                Map(m => m.Id).Name("Record Number");
                Map(m => m.BinStartTime).Name("Bin Start Time");
                Map(m => m.ApproachId).Name("Approach ID");
                Map(m => m.SummedSpeed).Name("Total speed for bin");
                Map(m => m.Speed85Th).Name("85th Percentile");
                Map(m => m.IsProtectedPhase).Name("Is Protected Phase");
            }
        }

    }
}
