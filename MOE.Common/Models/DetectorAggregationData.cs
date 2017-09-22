using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace MOE.Common.Models
{
    public class DetectorAggregationData
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public DateTime BinStartTime { get; set; }
        [ForeignKey("Id")]
        public virtual Detector Detector { get; set; }
        [Required]
        [StringLength(10)]
        public string DetectorId { get; set; }


        [Required]
        public int Volume { get; set; }

    }
}
