using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace MOE.Common.Models
{
    public class ApproachSplitFailAggregation
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
        public int SplitFailures { get; set; }
        [Required]
        public int GapOuts { get; set; }
        [Required]
        public int ForceOffs { get; set; }
        [Required]
        public int MaxOuts { get; set; }
        [Required]
        public int UnknownTerminationTypes { get; set; }

        [Required]
        public bool IsProtectedPhase { get; set; }
    }
}
