using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CsvHelper.Configuration;

namespace MOE.Common.Models
{
    public class DetectorAggregation
    {
        [Key]
        [Required]
        [Column(Order = 0)]
        public string SignalId { get; set; }

        [Key]
        [Required]
        [Column(Order = 1)]
        public int ApproachId { get; set; }

        [Key]
        [Required]
        [Column(Order = 2)]
        public DateTime BinStartTime { get; set; }

        [Key]
        //[ForeignKey("Detector")]
        [Required]
        [Column(Order = 3)]
        public int DetectorPrimaryId { get; set; }
        //public virtual Detector Detector { get; set; }

        [Required]
        [Column(Order = 4)]
        public int Volume { get; set; }

        [Required]
        [Column(Order = 5)]
        public int MovementTypeId { get; set; }

        [Required]
        [Column(Order = 6)]
        public int DirectionTypeId { get; set; }

        public sealed class DetectorAggregationClassMap : ClassMap<DetectorAggregation>
        {
            public DetectorAggregationClassMap()
            {
                Map(m => m.SignalId).Name("Signal Id");
                Map(m => m.ApproachId).Name("Approach Id");
                Map(m => m.BinStartTime).Name("Bin Start Time");
                Map(m => m.Volume).Name("Volume");
                Map(m => m.DetectorPrimaryId).Name("Detector Primary Id");
                Map(m => m.MovementTypeId).Name("Movement Type ID");
                Map(m => m.DirectionTypeId).Name("Direction Type ID");
            }
        }
    }
}