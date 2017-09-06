using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace MOE.Common.Models
{
    public class PriorityAggregationData
    {
        public PriorityAggregationData()
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
        public int PriorityRequests { get; set; }

        [Required]
        public int PriorityServiceEarlyGreen { get; set; }

        [Required]
        public int PriorityServiceExtendedGreen { get; set; }
    }
}
