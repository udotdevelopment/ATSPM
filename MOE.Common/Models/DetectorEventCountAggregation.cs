using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CsvHelper.Configuration;

namespace MOE.Common.Models
{
    //public class DetectorEventCountAggregation : Aggregation
    public class DetectorEventCountAggregation 
    {

        [Key]
        [Required]
        [Column(Order = 0)]
        public  DateTime BinStartTime { get; set; }

        [Required]
        [Column(Order = 1)]
        [StringLength(10)]
        public string SignalId { get; set; }


        [Key]
        [Required]
        [Column(Order = 2)]
        public int ApproachId { get; set; }

        [Key]
        [Required]
        [Column(Order = 3)]
        public int DetectorPrimaryId { get; set; }

        [Required]
        [Column(Order = 4)]
        public int EventCount { get; set; }
        
        public sealed class
            DetectorEventCountAggregationClassMap : ClassMap<DetectorEventCountAggregation>
        {
            public DetectorEventCountAggregationClassMap()
            {
        //        Map(m => m.Id).Name("Record Number");
                Map(m => m.BinStartTime).Name("Bin Start Time");
                Map(m => m.DetectorPrimaryId).Name("Detector Primary ID");
                Map(m => m.EventCount).Name("Event Count");
            }
        }
    }
}
