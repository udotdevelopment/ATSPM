using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ATSPM.Models
{
    public class SignalPlanAggregation
    {
        [Required]
        [Column(Order = 0)]
        public string SignalId { get; set; }

        [Required]
        [Column(Order = 1)]
        public DateTime Start { get; set; }

        [Required]
        [Column(Order = 2)]
        public DateTime End { get; set; }

        [Required]
        [Column(Order = 3)]
        public int PlanNumber { get; set; }


    }
}



