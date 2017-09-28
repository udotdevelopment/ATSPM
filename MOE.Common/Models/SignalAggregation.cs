using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace MOE.Common.Models
{
    public class SignalAggregation
    {
        public SignalAggregation()
        {
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        public DateTime BinStartTime { get; set; }

        [Required]
        [StringLength(10)]
        public string SignalID { get; set; }

        public virtual Signal Signal { get; set; }

        [Required]
        public int TotalCycles { get; set; }

        [Required]
        public int AddCyclesInTransition { get; set; }

        [Required]
        public int SubtractCyclesInTransition { get; set; }

        [Required]
        public int DwellCyclesInTransition { get; set; }

    }
}
