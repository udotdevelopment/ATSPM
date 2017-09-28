using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace MOE.Common.Models
{
    public class ApproachCycleAggregation
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
        public double RedTime { get; set; }

        [Required]
        public double YellowTime { get; set; }

        [Required]
        public double GreenTime { get; set; }

        [Required]
        public int TotalCycles { get; set; }

        [Required]
        public int PedActuations { get; set; }
    }
}
