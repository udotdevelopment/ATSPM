using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CsvHelper.Configuration;

namespace MOE.Common.Models
{
    public class DetectorAggregation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column(Order = 3)]
        public int Id { get; set; }

        [Key]
        [Required]
        [Column(Order = 0)]
        public DateTime BinStartTime { get; set; }

        [Key]
        //[ForeignKey("Detector")]
        [Required]
        [Column(Order = 1)]
        public int DetectorPrimaryId { get; set; }
        //public virtual Detector Detector { get; set; }

        [Required]
        [Column(Order = 2)]
        public int Volume { get; set; }

        public sealed class DetectorAggregationClassMap : ClassMap<DetectorAggregation>
        {
            public DetectorAggregationClassMap()
            {
                //Map(m => m.Detector).Ignore();
                Map(m => m.Id).Name("Record Number");
                Map(m => m.BinStartTime).Name("Bin Start Time");
                Map(m => m.Volume).Name("Volume");
                Map(m => m.DetectorPrimaryId).Name("Detector Primary Id");
                Map(m => m.DetectorPrimaryId).Name("Detector ID");
            }
        }
    }
}