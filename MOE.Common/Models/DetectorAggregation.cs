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
        public int Id { get; set; }

        [Required]
        public DateTime BinStartTime { get; set; }

        [ForeignKey("Detector")]
        [Required]
        public int DetectorPrimaryId { get; set; }

        public virtual Detector Detector { get; set; }

        [Required]
        public int Volume { get; set; }

        [Required]
        public int DataPoints { get; set; }

        public sealed class DetectorAggregationClassMap : ClassMap<DetectorAggregation>
        {
            public DetectorAggregationClassMap()
            {
                Map(m => m.Detector).Ignore();
                Map(m => m.Id).Name("Record Number");
                Map(m => m.BinStartTime).Name("Bin Start Time");
                Map(m => m.DetectorPrimaryId).Name("Detector ID");
                Map(m => m.Volume).Name("Volume");
            }
        }
    }
}