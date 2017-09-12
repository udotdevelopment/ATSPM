using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace MOE.Common.Models
{
    public class ApproachSpeedAggregationData
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        public DateTime BinStartTime { get; set; }

        [Required]
        public int ApproachID { get; set; }

        public virtual Approach Approach { get; set; }

        [Required]
        public double SummedSpeed { get; set; }

        [Required]
        public double SpeedVolume { get; set; }

        [Required]
        public double Speed85th { get; set; }

        [Required]
        public double Speed15th { get; set; }
        
    }
}
