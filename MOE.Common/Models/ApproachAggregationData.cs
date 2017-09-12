using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace MOE.Common.Models
{
    public class ApproachAggregationData
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
        public double RedTime { get; set; }

        [Required]
        public double YellowTime { get; set; }

        [Required]
        public double GreenTime { get; set; }

        [Required]
        public int TotalCycles { get; set; }

        [Required]
        public int PedActuations { get; set; }

        [Required]
        public int SplitFailures { get; set; }
        
        [Required]
        public int ArrivalsOnGreen { get; set; }

        [Required]
        public int ArrivalsOnRed { get; set; }

        [Required]
        public int ArrivalsOnYellow { get; set; }

        [Required]
        public int SevereRedLightViolations { get; set; }

        [Required]
        public int TotalRedLightViolations { get; set; }
    }
}
